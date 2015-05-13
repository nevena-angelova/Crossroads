using Crossroads.Common;
using Crossroads.Data;
using Crossroads.Web.Areas.Administration.ViewModels.ForumViewModels.Topics;
using Crossroads.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crossroads.Web.Infrastructure.Constants;
using Crossroads.Web.Infrastructure.Sanitizer;
using MvcPaging;
using System.Linq.Expressions;
using Crossroads.Models.Forum;
using System.Linq.Dynamic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Net;
using Crossroads.Web.Infrastructure.Populators;

namespace Crossroads.Web.Areas.Administration.Controllers.Forum
{
    [Authorize(Roles = GlobalConstants.AdminRole + "," + GlobalConstants.ModeratorRole)]
    public class TopicsAdminController : BaseController
    {
        private IDropDownListPopulator populator;

        private readonly ISanitizer sanitizer;

        private const int DefaultPageSize = 4;

        public TopicsAdminController(ICrossroadsData data, ISanitizer sanitizer, IDropDownListPopulator populator)
            : base(data)
        {
            this.sanitizer = sanitizer;
            this.populator = populator;
        }

        public ActionResult ListTopics(string orderBy, string searchValue, int? page, string searchProperty)
        {
            Expression<Func<Topic, bool>> WhereExpression = t => true;

            if (!String.IsNullOrWhiteSpace(searchValue))
            {
                if (searchProperty == "Title")
                {
                    WhereExpression = t => t.Title.ToLower().Contains(searchValue.ToString().ToLower());
                }

                if (searchProperty == "UserName")
                {
                    WhereExpression = t => t.AuthorProfile.ProfileUser.UserName.ToLower().Contains(searchValue.ToString().ToLower());
                }

                if (searchProperty == "Category")
                {
                    int categoryId = int.Parse(searchValue);

                    WhereExpression = t => t.CategoryId == categoryId;
                }
            }

            if (searchProperty == "Flagged")
            {
                WhereExpression = t => t.Flags > 0;
            }

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            IPagedList<AdminTopicViewModel> topics = this.Data.Topics.All()
                .Where(WhereExpression)
                .OrderBy(orderBy ?? "Id")
                .Project()
                .To<AdminTopicViewModel>()
                .ToPagedList(currentPageIndex, DefaultPageSize);

            if (!topics.Any())
            {
                ViewBag.NothingFound = "Няма намерени теми";
            }

            ViewBag.OrderBy = orderBy;
            ViewBag.SearchValue = searchValue;
            ViewBag.SearchProperty = searchProperty;
            ViewBag.Categories = this.populator.GetCategories();

            return this.View(Views.ListTopics, topics);
        }

        public ActionResult AdminDisplayTopic(int id)
        {
            Topic dbTopic = this.Data.Topics.GetById(id);
            if (dbTopic == null)
            {
                return HttpNotFound("Topic not found!");
            }

            AdminTopicDetailsViewModel topic = Mapper.Map<AdminTopicDetailsViewModel>(dbTopic);

            return this.View(Views.AdminDisplayTopic, topic);
        }

        public ActionResult AdminEditTopic(int id)
        {
            Topic dbTopic = this.Data.Topics.GetById(id);
            if (dbTopic == null)
            {
                return HttpNotFound("Topic not found!");
            }

            ViewBag.Categories = this.populator.GetCategories();

            AdminEditTopicViewModel topic = Mapper.Map<AdminEditTopicViewModel>(dbTopic);

            return this.View(Views.AdminEditTopic, topic);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AdminEditTopic(AdminEditTopicViewModel topic)
        {
            if (topic != null && ModelState.IsValid)
            {
                Topic dbTopic = this.Data.Topics.GetById(topic.Id);
                if (dbTopic == null)
                {
                    return HttpNotFound("Topic not found!");
                }

                dbTopic.CategoryId = topic.CategoryId;
                dbTopic.Title = topic.Title;
                dbTopic.Content = sanitizer.Sanitize(topic.Content);
                this.Data.SaveChanges();

                AdminTopicDetailsViewModel newTopic = Mapper.Map<AdminTopicDetailsViewModel>(dbTopic);

                return this.View(Views.AdminDisplayTopic, newTopic);
            }

            ViewBag.Categories = this.populator.GetCategories();

            return this.View(Views.AdminEditTopic, topic);
        }

        public ActionResult AdminDeleteTopic(int id)
        {
            Topic topic = this.Data.Topics.GetById(id);
            if (topic == null)
            {
                return HttpNotFound("Topic not found!");
            }

            var topicVoteIds = topic.UserVotes.Select(v => v.Id).ToList();

            foreach (var topicVoteId in topicVoteIds)
            {
                this.Data.TopicVotes.Delete(topicVoteId);
            }

            var answerIds = topic.Answers.Select(a => a.Id).ToList();

            foreach (var answerId in answerIds)
            {
                var answerVoteIds = this.Data.AnswerVotes.All()
                    .Where(v => v.AnswerId == answerId)
                    .Select(v => v.Id)
                    .ToList();

                foreach (var answerVoteId in answerVoteIds)
                {
                    this.Data.AnswerVotes.Delete(answerVoteId);
                }

                var commentIds = this.Data.Comments.All()
                    .Where(c => c.AnswerId == answerId)
                    .Select(c => c.Id)
                    .ToList();

                foreach (var commentId in commentIds)
                {
                    this.Data.Comments.Delete(commentId);
                }

                this.Data.Answers.Delete(answerId);
            }

            this.Data.Topics.Delete(topic);
            this.Data.SaveChanges();

            return this.RedirectToAction("ListTopics");
        }
    }
}