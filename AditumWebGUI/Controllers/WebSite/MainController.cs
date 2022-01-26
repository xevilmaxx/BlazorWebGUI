using AditumWebGUI.CustomAuth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.ProtectedBrowserStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AditumWebGUI.Controllers.WebSite
{

    public class MainController : ComponentBase
    {

		private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

		//same as @inject in .razor page
		[Inject]
        protected ProtectedSessionStorage SessionStorage { get; set; }

		[Inject]
		protected Toolbelt.Blazor.I18nText.I18nText I18nText { get; set; }

		[CascadingParameter]
		private Task<AuthenticationState> authenticationStateTask { get; set; }

		public int MyCounter = 0;

		public I18nText.Main.Main Lang = new I18nText.Main.Main();

		public async Task ClcikedHello()
		{
			MyCounter++;
			await SessionStorage.SetAsync("tmp", MyCounter);
		}


		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender == false)
			{
				return;
			}

			//dont do Get before render async as in server prerendered mode you will have issues
			var result = await SessionStorage.GetAsync<int>("tmp");
			if (result.Success)
			{
				MyCounter = result.Value;
				StateHasChanged();
			}
		}
		
        protected override async Task OnInitializedAsync()
        {
			Lang = await I18nText.GetTextTableAsync<I18nText.Main.Main>(this);
			//StateHasChanged();

			//var claimsPrincipal = new CustomClaimsPrincipal(((await authenticationStateTask).User));
			var claimsPrincipal = (await authenticationStateTask).User;

			log.Debug($"Name: {claimsPrincipal?.Identity?.Name}, AuthState: {claimsPrincipal?.Identity?.IsAuthenticated}");
			var xxx = claimsPrincipal?.IsInRole("Admin");
			var xxy = claimsPrincipal?.IsInRole("User");
			//Claims.Where(x => x.Type == ClaimType.)

		}

    }
}
