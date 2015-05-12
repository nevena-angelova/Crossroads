using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crossroads.Web.Areas.Administration.Controllers
{
    public class MusicGenresController : Controller
    {
        // GET: Administration/MusicGenres
        public ActionResult Index()
        {
            return View();
        }
    }
}