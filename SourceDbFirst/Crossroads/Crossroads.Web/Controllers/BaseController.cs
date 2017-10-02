using Crossroads.Data;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using Crossroads.Web.Infrastructure.Account;

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
            MyPrincipal User = new MyPrincipal();

            this.CurrentUser = this.Data.Users.All()
                                       .Where(u => u.UserName == User.Identity.Name)
                                       .FirstOrDefault();

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}