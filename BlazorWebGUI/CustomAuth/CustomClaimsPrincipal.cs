using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

//https://stackoverflow.com/questions/52732431/how-to-custom-override-user-isinrole-in-asp-net-core
//https://question-it.com/questions/690059/kak-nastroit-pereopredelit-userisinrole-v-aspnet-core
namespace BlazorWebGUI.CustomAuth
{
    public class CustomClaimsPrincipal : ClaimsPrincipal
    {
        public CustomClaimsPrincipal(IPrincipal claimsPrincipal) : base(claimsPrincipal)
        {

        }

        public override bool IsInRole(string role)
        {
            // ...
            return base.IsInRole(role);
        }
    }
}
