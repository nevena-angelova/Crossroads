using Crossroads.Data;
using Crossroads.Data.Common.Repository;
using Crossroads.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crossroads.Web.Controllers
{
    public class UsersController : Controller
    {
        private IRepository<ApplicationUser> users;

        public UsersController(IRepository<ApplicationUser> users)
        {
            this.users = users;
        }

        // GET: /Users/
        public ActionResult Users()
        {
            var users = this.users.All();
            return View(users);
        }
    }
}