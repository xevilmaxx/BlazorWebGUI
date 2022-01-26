using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestHttpGRPC.DTO;

namespace TestHttpGRPC.CustomAuth
{
    public class AuthManager
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        private readonly string username = "pi";
        private readonly string password = "pwd";
        private readonly string key;

        public AuthManager()
        {
            this.key = RunCfgs.ServerKey;
        }

        public string AuthenticationAPI(string username, string password)
        {

            log.Debug($"AuthenticationAPI Invoked! {username}, {password}");

            if (!(this.username.Equals(username) || this.password.Equals(password)))
            {
                return null;
            }

            // 1. Create Security Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 2. Create Private Key to Encrypted
            var tokenKey = Encoding.ASCII.GetBytes(key);

            //3. Create JETdescriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim(ClaimTypes.Role, "User")
                    }),
                Expires = null,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            //4. Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 5. Return Token from method
            return tokenHandler.WriteToken(token);
        }

        //public AuthenticationState AuthenticationLogin(string username, string password)
        //{

        //    log.Debug($"AuthenticationLogin Invoked! {username}, {password}");

        //    if (!(this.username.Equals(username) || this.password.Equals(password)))
        //    {
        //        return null;
        //    }

        //    var identity = new ClaimsIdentity(new[]
        //    {
        //        new Claim(ClaimTypes.Name, username),
        //        new Claim(ClaimTypes.Role, "Admin"),
        //        new Claim(ClaimTypes.Role, "User")
        //        //new Claim("IsUserEmployedBefore1990", IsUserEmployedBefore1990(user))
        //    }, "login_auth_type");

        //    var claimsPrincipal = new ClaimsPrincipal(identity);

        //    return new AuthenticationState(claimsPrincipal);

        //}

        public JwtSecurityToken DecodeToken(string JwtToken)
        {

            log.Debug("DecodeToken Invoked!");

            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.CanReadToken(JwtToken) == true)
            {
                return tokenHandler.ReadJwtToken(JwtToken);
            }
            else
            {
                return null;
            }
        }

        public AuthenticationState DecodeTokenAuthenticationState(string JwtToken)
        {

            log.Debug($"DecodeTokenAuthenticationState Invoked! With: {JwtToken}");

            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.CanReadToken(JwtToken) == true)
            {

                log.Debug("Token is decodable");

                var token = tokenHandler.ReadJwtToken(JwtToken);

                var identities = new List<ClaimsIdentity>();

                foreach(var claim in token.Claims)
                {
                    if(claim.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase))
                    {
                        log.Trace($"Found Role: {claim.Value}");
                        identities.Add(new ClaimsIdentity(
                            token.Claims,
                            "apiauth_type",
                            claim.Type,
                            claim.Value
                        ));
                    }
                }

                //!!!WARNING!!!
                //population of authenticationType in ClaimsIdentity is really important in order to make user authenticated
                return new AuthenticationState(
                    new ClaimsPrincipal(
                        identities
                        )
                    );

            }
            else
            {
                log.Debug("Invalid Token");
                return new AuthenticationState(
                    new ClaimsPrincipal(
                        new List<ClaimsIdentity>()
                        {
                            new ClaimsIdentity()
                        }
                        )
                    );
            }
        }

    }
}
