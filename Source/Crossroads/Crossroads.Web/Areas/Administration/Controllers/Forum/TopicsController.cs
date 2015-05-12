using Crossroads.Common;
using Crossroads.Data;
using Crossroads.Web.Areas.Administration.ViewModels.ForumViewModels.Topics;
using Crossroads.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Crossroads.Web.Infrastructure.Constants;
using Crossroads.Web.Infrastructure.Sanitizer;
using MvcPaging;

namespace Crossroads.Web.Areas.Administration.Controllers.Forum
{
    [Authorize(Roles = GlobalConstants.AdminRole)]
    public class TopicsController : BaseController
    {
        private readonly ISanitizer sanitizer;

        private const int DefaultPageSize = 4;

        public TopicsController(ICrossroadsData data, ISanitizer sanitizer)
            : base(data)
        {
            this.sanitizer = sanitizer;
        }

        public ActionResult ListTopics(bool? ordered, int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            IPagedList<TopicOutputViewModel> topics = null;

            if (ordered == true)
            {
                topics = this.Data.Topics.All()
                    .OrderBy(t => t.Title)
                    .Project()
                    .To<TopicOutputViewModel>()
                    .ToPagedList(currentPageIndex, DefaultPageSize);
            }
            else
            {
                topics = this.Data.Topics.All()
                    .OrderBy(t => t.Id)
                    .Project()
                    .To<TopicOutputViewModel>()
                    .ToPagedList(currentPageIndex, DefaultPageSize);
            }

            ViewBag.Ordered = ordered;

            return this.View(Views.ListTopics, topics);
        }
    }
}