using BlazorWebGUI.SignalR;
using BlazorWebGUI.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using System.Text;
using TestHttpGRPC.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BlazorWebGUI.GrpcServices;
using BlazorWebGUI.DTO.Enums;
using Microsoft.AspNetCore.Http.Connections;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorWebGUI.CustomAuth;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

namespace BlazorWebGUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddRazorPages();
            services.AddServerSideBlazor();

            //for MudBlazor GUI
            services.AddMudServices();

            //for signalr usage
            services.AddHostedService<SignalrServerSide>();
            services.AddHostedService<SignalrPublicSide>();

            services.AddGrpc();

            #region Testing_Purpose

            //optional (needed mainly for easy debugging)
            services.AddGrpcReflection();

            #endregion

            #region AspNetCore_Specials

            //services.AddControllers()
            //.ConfigureApiBehaviorOptions(options =>
            //{
            //    options.SuppressConsumesConstraintForFormFileParameters = true;
            //    options.SuppressInferBindingSourcesForParameters = true;
            //    options.SuppressModelStateInvalidFilter = true;
            //    options.SuppressMapClientErrors = true;
            //    options.ClientErrorMapping[StatusCodes.Status404NotFound].Link =
            //        "https://httpstatuses.com/404";
            //});

            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            }));

            //Response time optimization
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            #endregion

            #region HTTP

            //To expose GRPC in HTTP format
            services.AddGrpcHttpApi();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "gRPC HTTP API", Version = "v1 [Alpha]" });
            });
            services.AddGrpcSwagger();

            #endregion

            services.AddProtectedBrowserStorage();

            #region JWT

            //Security mechanism to authenticate API user

            var key = Encoding.ASCII.GetBytes(RunCfgs.ServerKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        //to avoid token expiration
                        RequireExpirationTime = false
                    };
                });

            //https://chrissainty.com/securing-your-blazor-apps-configuring-policy-based-authorization-with-blazor/
            services.AddAuthorization(config =>
            {
                config.AddPolicy("Test0", policy => policy.RequireClaim("role", "Admin", "User"));
            });


            //services.AddTransient<IClaimsTransformation, ClaimsTramsformer>();

            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddRoles<IdentityRole>();

            //simple login, session wide
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            #endregion

            //MultiLanguage Support
            services.AddI18nText();
            services.Configure<RequestLocalizationOptions>(options => {
                var supportedCultures = new[] { "en", "ja" };
                options.DefaultRequestCulture = new RequestCulture("en");
                options.AddSupportedCultures(supportedCultures);
                options.AddSupportedUICultures(supportedCultures);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //uncoment for SSL support
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            // must be added after UseRouting and before UseEndpoints 
            //we will set default to true, to enable defualt GRPC-WEB interface on each GRPC.addService
            app.UseGrpcWeb(new GrpcWebOptions() { DefaultEnabled = true });

            app.UseCors();

            #region HTTP

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "gRPC HTTP API V1 [Alpha feature]");
            });

            #endregion

            #region JWT

            app.UseAuthentication();
            app.UseAuthorization();

            #endregion

            app.UseRequestLocalization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");

                #region SignalR_Hubs_Mapping
                endpoints.MapHub<SignalrHub0>($"/{SignalREndpoints.InternalEndpoint}", options =>
                {
                    options.Transports =
                        HttpTransportType.WebSockets |
                        HttpTransportType.ServerSentEvents;
                });
                endpoints.MapHub<SignalrPublicHub0>($"/{SignalREndpoints.PublicEndpoint}", options =>
                {
                    options.Transports =
                        HttpTransportType.WebSockets |
                        HttpTransportType.ServerSentEvents;
                });
                #endregion

                endpoints.MapGrpcService<BlazorWebGUI.GrpcServices.AuthenticationService>().RequireCors("AllowAll");
                endpoints.MapGrpcService<GenericApiService>().RequireCors("AllowAll");

                #region Testing_Purpose
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    endpoints.MapGrpcReflectionService();
                }
                #endregion

            });
        }
    }
}
