using Crossroads.Data;
using Crossroads.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;

namespace Crossroads.Web.Controllers
{
    [HandleError]

    public class BaseController : Controller
    {
        protected ICrossroadsData Data { get; private set; }

        protected User CurrentUser { get; private set; }

        public BaseController(ICrossroadsData data)
        {
            this.Data = data;
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            this.CurrentUser = this.Data.Users.All()
                                       .Where(u => u.UserName == requestContext.HttpContext.User.Identity.Name)
                                       .FirstOrDefault();

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}