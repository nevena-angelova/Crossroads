using Crossroads.Common;
using System.Web;
using System.Web.Mvc;

namespace Crossroads.Web.Infrastructure.AuthorizeAttributes
{
    public class MyOrInRoleAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            var identityName = httpContext.User.Identity.Name;
            var userName = httpContext.Request.Params["userName"];

            return identityName.Equals(userName) || httpContext.User.IsInRole(GlobalConstants.AdminRole) || httpContext.User.IsInRole(GlobalConstants.ModeratorRole);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var result = new ViewResult();
            result.ViewName = "Error";
            result.MasterName = "_Layout";
            filterContext.Result = result;
        }
    }
}