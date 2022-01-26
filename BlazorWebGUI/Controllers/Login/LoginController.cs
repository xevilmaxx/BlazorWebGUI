using BlazorWebGUI.CustomAuth;
using BlazorWebGUI.DTO;
using BlazorWebGUI.DTO.Enums;
using BlazorWebGUI.SignalR;
using BlazorWebGUI.SignalR.Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebGUI.Controllers.Login
{
    public class LoginController : ComponentBase
    {

        private HubConnection _hubConnection;

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        protected Toolbelt.Blazor.I18nText.I18nText I18nText { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        //[CascadingParameter]
        //private Task<AuthenticationState> authenticationStateTask { get; set; }

        public I18nText.Login.Login Lang = new I18nText.Login.Login();

        protected List<string> _receivedMessages = new List<string>();

        public LoginDTO model = new LoginDTO();
        private bool success;

        protected override async Task OnInitializedAsync()
        {

            Lang = await I18nText.GetTextTableAsync<I18nText.Login.Login>(this);

            //base.OnInitialized();
            //.WithUrl($"{this.Navigation.BaseUri}/signalr")
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(@$"{Navigation.BaseUri}{SignalREndpoints.InternalEndpoint}")
                .Build();

            _hubConnection.On<string>("MessageReceived", message =>
            {
                _receivedMessages.Add(message);

                this.StateHasChanged();
            });

            await _hubConnection.StartAsync();

            await Task.Run(() =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(5000);
                    TrigSignalRServer();
                }
            });


        }

        protected void TrigSignalR()
        {
            _hubConnection.SendAsync("SendMessageAsync", "Alive - " + new Random().Next());
        }

        protected void TrigSignalRServer()
        {
            //This is using local connection
            //_=SignalrHub0.Instance.SendMessageAsync("hello");

            //This is using server connection
            //Those are same, but in different variants
            //SignalrServerSide.HubContext.Clients.All.SendAsync("MessageReceived", "Server Alive - " + new Random().Next());
            _ = new SignalrHub0(SignalrServerSide.HubContext).SendMessageAsync("Server Alive - " + new Random().Next());
        }

        public ValueTask DisposeAsync()
        {
            return _hubConnection.DisposeAsync();
        }

        /// <summary>
        /// Login Button Pressed, and data are syntattically valid
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnValidatedLoginPress(EditContext context)
        {
            success = true;

            await ((CustomAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(model.Username, model.Password);

            Navigation.NavigateTo("/");
            //StateHasChanged();
        }

    }
}
