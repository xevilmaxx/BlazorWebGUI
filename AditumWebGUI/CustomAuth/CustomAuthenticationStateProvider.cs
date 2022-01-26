using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using TestHttpGRPC.CustomAuth;

namespace AditumWebGUI.CustomAuth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public ProtectedSessionStorage _localStorageService { get; }

        public CustomAuthenticationStateProvider(ProtectedSessionStorage protectedSessionStorageService)
        {
            _localStorageService = protectedSessionStorageService;
        }

        //Invoked on each manual refresh of browser page
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            log.Debug("GetAuthenticationStateAsync Invoked!");

            var accessToken = await _localStorageService.GetAsync<string>("AccessToken");

            AuthenticationState result = null;
            if (accessToken.Success == false)
            {
                log.Debug("Token got from local storage is: NULL");
                result = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            else
            {
                log.Debug("Token got from local storage: SUCCESS");
                result = new AuthManager().DecodeTokenAuthenticationState(accessToken.Value);
            }

            return await Task.FromResult(result);
        }

        public async Task MarkUserAsAuthenticated(string Username, string Password)
        {

            log.Debug($"MarkUserAsAuthenticated Invoked! With: {Username} : {Password}");

            var identity = new AuthManager().AuthenticationAPI(Username, Password);

            await _localStorageService.SetAsync("AccessToken", identity);

            log.Debug($"Generated Identity: {identity}");

            var authState = new AuthManager().DecodeTokenAuthenticationState(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(authState));

        }

        public async Task MarkUserAsLoggedOut()
        {

            log.Debug($"MarkUserAsLoggedOut Invoked!");

            await _localStorageService.DeleteAsync("AccessToken");

            var emptyUser = new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity()
                )
            );

            NotifyAuthenticationStateChanged(Task.FromResult(emptyUser));
        }

    }
}
