using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crossroads.Data;
using Crossroads.Web.ViewModels.ForumViewModels;
using MvcPaging;
using Crossroads.Web.ViewModels.ForumViewModels.Topics;

namespace Crossroads.Web.Controllers.Forum
{
    public class ForumController : BaseController
    {
        public ForumController(ICrossroadsData data)
            : base(data)
        { }

        private const int DefaultPageSize = 6;

        public ActionResult DisplayCategories()
        {
            IList<DisplayCategoryViewModel> categories = this.Data.Categories.All()
                .Project()
                .To<DisplayCategoryViewModel>()
                .ToList();

            return this.View(categories);
        }

        public ActionResult TopUsers()
        {
            List<TopUserViewModel> users = this.Data.Profiles.All()
                .OrderByDescending(p => p.ForumPoints)
                .Take(60)
                .Project()
                .To<TopUserViewModel>()
                .ToList();

            return this.View(users);
        }
    }
}