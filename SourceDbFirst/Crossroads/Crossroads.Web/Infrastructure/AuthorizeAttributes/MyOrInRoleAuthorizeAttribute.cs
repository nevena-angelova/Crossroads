using Crossroads.Web.Infrastructure.Constants;
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

            return identityName.Equals(userName) || httpContext.User.IsInRole(UserRoles.AdminRole) || httpContext.User.IsInRole(UserRoles.ModeratorRole);
        }
    }
}