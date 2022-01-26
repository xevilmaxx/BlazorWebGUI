using GenericApiProtos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AditumWebGUI.GrpcServices
{

    [Authorize]
    public class GenericApiService : GenericApiProto.GenericApiProtoBase
    {

        public override Task<Reply> PostMethod0(Message request, ServerCallContext context)
        {

            var user = context.GetHttpContext().User.Identity.Name;

            _ = new SignalR.Hubs.SignalrHub0(SignalR.SignalrServerSide.HubContext).SendMessageAsync($"{user} -> {request.Data}");

            return Task.FromResult(new Reply() { });

        }

        public override Task<Reply> GetMethod0(Message request, ServerCallContext context)
        {

            var user = context.GetHttpContext().User.Identity.Name;

            return Task.FromResult(new Reply() { });

        }

    }
}
