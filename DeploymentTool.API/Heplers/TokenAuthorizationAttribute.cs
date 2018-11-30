using DeploymentTool.API.Services;
using DeploymentTool.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DeploymentTool.API.Helpers
{
    public class TokenAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase context)
        {
            IEnumerable<string> header = context.Request.Headers.GetValues("Authorization");

            if (header == null)
            {
                return false;
            }

            Token token = TokenService.GetToken(header.FirstOrDefault());

            if (token == null || token.IsExpired)
            {
                return false;
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult("Authentification failed");
        }
    }
}