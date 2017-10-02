using Crossroads.Data;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Crossroads.Web.Infrastructure.Account
{
    public class MyPrincipal : IPrincipal
    {
        public IIdentity Identity
        {
            get
            {
                return new MyIdentity();
            }
        }

        public bool IsInRole(string role)
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string name = ticket.Name;

                CrossroadsDbContext db = new CrossroadsDbContext();
                if (db.Users.Where(u => u.UserName == name && u.Roles.Select(r => r.Name).Contains(role)).Any())
                {
                    return true;
                }

                return false;
            }

            return false;

        }
    }
}