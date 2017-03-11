using Crossroads.Data;
using Crossroads.Models;
using System;
using System.Web.Mvc;
using System.Linq;

namespace Crossroads.Web.Infrastructure.ActionFilters
{
    public class LogUserActionTimeFilter : IActionFilter
    {
        private CrossroadsDbContext data;

        public LogUserActionTimeFilter()
        {
            this.data = new CrossroadsDbContext();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            User currentUser = this.data.Users
                    .Where(u => u.UserName == filterContext.HttpContext.User.Identity.Name)
                    .FirstOrDefault();

            if (currentUser != null)
            {
                currentUser.LastActionTime = DateTime.Now;
                this.data.SaveChanges();
            }

        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}