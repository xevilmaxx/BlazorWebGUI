using AuthenticationProtos;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHttpGRPC.CustomAuth;

namespace BlazorWebGUI.GrpcServices
{
    public class AuthenticationService : AuthenticationProto.AuthenticationProtoBase
    {

        public override Task<UserCredsAnswer> Authenticate(UserCreds request, ServerCallContext context)
        {

            var token = new AuthManager().AuthenticationAPI(request.Username, request.Password);

            var result = new UserCredsAnswer()
            {
                Token = "-1"
            };

            if (token == null)
                return Task.FromResult(result);

            result.Token = token;

            return Task.FromResult(result);

        }

    }
}
