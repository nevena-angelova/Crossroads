using Crossroads.Data;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Crossroads.Web.Infrastructure.Account
{
    public class MyIdentity : IIdentity
    {
        public string AuthenticationType
        {
            get
            {
                return "default";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return IsAuthenticetedCheck();
            }
        }

        public string Name
        {
            get
            {
                return GetName();
            }
        }

        private bool IsAuthenticetedCheck()
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string name = ticket.Name;

                CrossroadsDbContext dbContext = new CrossroadsDbContext();
                if (dbContext.Users.Where(u => u.UserName == name).Any())
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        private string GetName()
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                return null;
            }

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

            return ticket.Name;
        }
    }
}