using BlazorWebGUI.SignalR.Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace BlazorWebGUI.SignalR
{
    /// <summary>
    /// Will be populated by Blazor injection on runtime
    /// </summary>
    public class SignalrServerSide : IHostedService, IDisposable
    {

        public static IHubContext<SignalrHub0> HubContext;

        public SignalrServerSide(IHubContext<SignalrHub0> Context)
        {
            HubContext = Context;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //TODO: your start logic, some timers, singletons, etc
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //TODO: your stop logic
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            
        }

    }
}
