using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Text;
using System.Threading;
using WatsonWebsocket;
using Websocket;
using Websocket.Client;

namespace WebSocketTester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //var exitEvent = new ManualResetEvent(false);
            //var url = new Uri("https://localhost:44367/InternalSignalR");

            //using (var client = new WebsocketClient(url))
            //{
            //    client.ReconnectTimeout = TimeSpan.FromSeconds(5);
            //    client.ReconnectionHappened.Subscribe(info =>
            //        Console.WriteLine($"Reconnection happened, type: {info.Type}"));

            //    client.MessageReceived.Subscribe(msg => Console.WriteLine($"Message received: {msg.Text}"));

            //    client.Start().Wait();

            //    //Task.Run(() => client.Send("{ message }"));

            //    exitEvent.WaitOne();
            //}

            //#########################################################################################
            //#########################################################################################
            //#########################################################################################

            //using (WatsonWsClient wsc = new WatsonWsClient(new Uri("ws://localhost:44367/InternalSignalR")))
            //{
            //    wsc.ServerConnected += (s, e) => Console.WriteLine("Client connected to server");
            //    wsc.ServerDisconnected += (s, e) => Console.WriteLine("Client disconnected from server");
            //    wsc.MessageReceived += (s, e) => Console.WriteLine("Client received message from server: " + Encoding.UTF8.GetString(e.Data));
            //    wsc.Start();

            //    Console.WriteLine("Connected");
            //    Console.ReadLine();
            //}

            //#########################################################################################
            //#########################################################################################
            //#########################################################################################

            //The only working way for SignalR/Websocket

            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44367/InternalSignalR")
                .WithAutomaticReconnect()
                .Build();

            connection.Reconnected += async (client) =>
            {
                Console.WriteLine("Reconnected: " + client);
            };

            connection.Closed += async (client) =>
            {
                Console.WriteLine("Disconnected: " + client);
            };

            connection.On<string>("MessageReceived", message =>
            {
                Console.WriteLine(message);
            });

            connection.StartAsync();

            Console.ReadLine();
        }
    }
}
