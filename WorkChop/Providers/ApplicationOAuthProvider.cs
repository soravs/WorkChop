using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using WorkChop.BusinessService.IBusinessService;
using WorkChop.BusinessService.BusinessService;

namespace WorkChop.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        private readonly IUserService _userService;
        
        public ApplicationOAuthProvider()
        {
            _userService = new UserService();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var userManager = _userService.GetByQuery(context.UserName);
            //var userId=userManager.UserID.TimeStamp
            if (userManager == null)
            {
                context.SetError("invalid_UserName");
                return;
            }
            if (!userManager.Password.Equals(context.Password))
            {
                context.SetError("invalid_Password");
                return;
            }
            var props = new AuthenticationProperties(new Dictionary<string, string>());

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Teacher"));
       
           
            props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "UserId", userManager.UserID.ToString()
                    },
                    {
                        "Name", userManager.FirstName + " " + userManager.LastName
                    },
                    {
                        "Email",userManager.Email
                    },
                    {
                        "UserStatus", userManager.IsActive.ToString()
                    }
                   
                  
                  });


            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}