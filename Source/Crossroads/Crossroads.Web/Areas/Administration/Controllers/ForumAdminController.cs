using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crossroads.Web.Areas.Administration.Controllers
{
    public class ForumAdminController : Controller
    {
        // GET: Administration/Forum
        public ActionResult Index()
        {
            return View("~/Areas/Administration/Views/Forum/ForumTopics.cshtml");
        }
    }
}