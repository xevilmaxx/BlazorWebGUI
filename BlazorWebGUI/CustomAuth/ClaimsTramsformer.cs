using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

//https://stackoverflow.com/questions/52732431/how-to-custom-override-user-isinrole-in-asp-net-core
//https://question-it.com/questions/690059/kak-nastroit-pereopredelit-userisinrole-v-aspnet-core
namespace BlazorWebGUI.CustomAuth
{
    public class ClaimsTramsformer : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var customPrincipal = new CustomClaimsPrincipal(principal) as ClaimsPrincipal;
            return Task.FromResult(customPrincipal);
        }
    }
}
