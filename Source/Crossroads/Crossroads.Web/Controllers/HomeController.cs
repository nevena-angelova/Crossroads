using Crossroads.Data.Common.Repository;
using Crossroads.Data.Models;
using Crossroads.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;

namespace Crossroads.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IDeletableEntityRepository<Post> posts;

        public HomeController(IDeletableEntityRepository<Post> posts)
        {
            this.posts = posts;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            this.posts.Delete(7);
            this.posts.SaveChanges();

            var model = this.posts.All().Project().To<PostViewModel>();

            return View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}