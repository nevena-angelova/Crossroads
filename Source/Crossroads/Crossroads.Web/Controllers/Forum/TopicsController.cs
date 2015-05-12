using Crossroads.Data;
using Crossroads.Web.ViewModels.ForumViewModels.Topics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crossroads.Web.Infrastructure.AuthorizeAttributes;
using System.Net;
using Crossroads.Models.Forum;
using Crossroads.Web.Infrastructure;
using Crossroads.Web.Infrastructure.Sanitizer;
using Crossroads.Web.Infrastructure.Constants;
using MvcPaging;
using Crossroads.Web.ViewModels.ForumViewModels.Answers;
using System.Linq.Expressions;

namespace Crossroads.Web.Controllers.Forum
{
    public class TopicsController : BaseController
    {
        private const int DefaultPageSize = 8;

        private readonly ISanitizer sanitizer;

        public TopicsController(ICrossroadsData data, ISanitizer sanitizer)
            : base(data)
        {
            this.sanitizer = sanitizer;
        }

        public ActionResult DisplayTopics(int categoryId, string categoryName, int? page, string orderBy)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            IPagedList<TopicViewModel> topics = this.Data.Topics.All()
                .Where(t => t.Category.Id == categoryId)
                .OrderBy(orderBy ?? "DateUpdated descending")
                .Project()
                .To<TopicViewModel>()
                .ToPagedList(currentPageIndex, DefaultPageSize);

            ViewBag.OrderBy = orderBy;
            ViewBag.CategoryName = categoryName;
            ViewBag.CategoryId = categoryId;

            return this.View(Views.DisplayTopics, topics);
        }

        public ActionResult TopTopics()
        {
            List<TopTopicViewModel> topics = this.Data.Topics.All()
                .OrderByDescending(t => t.Votes)
                .Take(30)
                .Project()
                .To<TopTopicViewModel>()
                .ToList();

            return this.View(Views.TopTopics, topics);
        }

        [Authorize]
        public ActionResult SearchTopics(int? page, string searchString, string searchProperty)
        {
            Expression<Func<Topic, bool>> WhereExpression = t => true;

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                if (searchProperty == "UserName")
                {
                    WhereExpression = t => t.AuthorProfile.ProfileUser.UserName.ToLower().Contains(searchString.ToLower());
                }

                if (searchProperty == "Title")
                {
                    WhereExpression = t => t.Title.ToLower().Contains(searchString.ToLower());
                }
            }

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            IPagedList<TopicViewModel> topics = this.Data.Topics.All()
                .Where(WhereExpression)
                .Project()
                .To<TopicViewModel>()
                .ToList()
                .ToPagedList(currentPageIndex, DefaultPageSize);

            if (!topics.Any())
            {
                ViewBag.NothingFound = "Няма намерени теми";
            }

            ViewBag.SearchString = searchString;
            ViewBag.SearchProperty = searchProperty;

            return this.View(Views.DisplayFoundTopics, topics);
        }

        public ActionResult DisplayTopic(int topicId, int? page)
        {
            Topic dbTopic = this.Data.Topics.GetById(topicId);
            if (dbTopic == null)
            {
                return HttpNotFound("Topic not found!");
            }

            dbTopic.Views++;

            Category category = dbTopic.Category;
            dbTopic.Category = category;

            this.Data.SaveChanges();

            TopicContentViewModel topic = Mapper.Map<TopicContentViewModel>(dbTopic);

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            IPagedList<DisplayAnswerViewModel> answers = this.Data.Answers.All()
                .Where(a => a.TopicId == topicId)
                .OrderBy(a => a.DateCreated)
                .Project()
                .To<DisplayAnswerViewModel>()
                .ToPagedList(currentPageIndex, DefaultPageSize);

            DisplayTopicViewModel topicModel = new DisplayTopicViewModel
            {
                Topic = topic,
                Answers = answers
            };

            ViewBag.CategoryId = dbTopic.Category.Id;
            ViewBag.TopicId = dbTopic.Id;

            return this.View(Views.DisplayTopic, topicModel);
        }

        [Authorize]
        public ActionResult AddTopic(int categoryId)
        {
            AddTopicViewModel topic = new AddTopicViewModel();
            topic.CategoryId = categoryId;
            return this.View(Views.AddTopic, topic);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddTopic(AddTopicViewModel topic)
        {
            if (topic != null && ModelState.IsValid)
            {
                Topic dbTopic = new Topic
                {
                    CategoryId = topic.CategoryId,
                    Title = topic.Title,
                    Content = sanitizer.Sanitize(topic.Content),
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                };

                dbTopic.AuthorProfile = this.Data.Profiles.All()
                    .Where(p => p.ProfileUser.UserName == User.Identity.Name)
                    .FirstOrDefault();

                this.Data.Topics.Add(dbTopic);
                this.Data.SaveChanges();

                return this.RedirectToAction("DisplayTopic", new { topicId = dbTopic.Id });
            }

            return this.View(Views.AddTopic, topic);
        }

        [MyOrInRoleAuthorizeAttribute]
        public ActionResult EditTopic(int topicId, string userName)
        {
            EditTopicViewModel topic = this.Data.Topics.All()
                .Where(t => t.Id == topicId).Project()
                .To<EditTopicViewModel>()
                .FirstOrDefault();

            return this.View(Views.EditTopic, topic);
        }

        [HttpPost]
        [MyOrInRoleAuthorizeAttribute]
        [ValidateAntiForgeryToken]
        public ActionResult EditTopic(EditTopicViewModel topic)
        {
            if (topic != null && ModelState.IsValid)
            {
                Topic dbTopic = this.Data.Topics.GetById(topic.Id);

                if (dbTopic == null)
                {
                    return HttpNotFound("Topic not found!");
                }

                dbTopic.Content = sanitizer.Sanitize(topic.Content);
                dbTopic.Title = topic.Title;
                dbTopic.DateEdited = DateTime.Now;

                this.Data.SaveChanges();

                return this.RedirectToAction("DisplayTopic", new { topicId = dbTopic.Id });
            }

            return this.View(Views.EditTopic, topic);
        }

        [HttpPost]
        [AjaxAuthorize]
        public ActionResult TopicFlag(int topicId)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call!");
            }

            Topic topic = this.Data.Topics.GetById(topicId);
            if (topic == null)
            {
                return HttpNotFound("Topic not found!");
            }

            if (!(topic.AuthorProfile.ProfileUser.Id == this.CurrentUser.Id))
            {
                bool isUserInCollection = false;

                foreach (var flag in topic.UserFlags)
                {
                    if (flag.UserId == this.CurrentUser.Id)
                    {
                        if (flag.Flagged == true)
                        {
                            topic.Flags--;
                            flag.Flagged = false;
                        }
                        else
                        {
                            topic.Flags++;
                            flag.Flagged = true;
                        }

                        isUserInCollection = true;
                        break;
                    }
                }

                if (!isUserInCollection)
                {
                    TopicFlag newFlag = new TopicFlag
                    {
                        User = this.CurrentUser,
                    };

                    topic.Flags++;
                    topic.UserFlags.Add(newFlag);
                }

                this.Data.SaveChanges();

                return this.Content(topic.Flags.ToString());
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [AjaxAuthorize]
        public ActionResult TopicVote(int topicId, bool isVotePositive)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call!");
            }

            Topic topic = this.Data.Topics.GetById(topicId);
            if (topic == null)
            {
                return HttpNotFound("Topic not found!");
            }

            if (!(topic.AuthorProfile.ProfileUser.Id == this.CurrentUser.Id))
            {
                bool isUserInCollection = false;

                if (topic.UserVotes.Count > 0)
                {
                    foreach (var vote in topic.UserVotes)
                    {
                        if (vote.UserId == this.CurrentUser.Id)
                        {
                            if (vote.IsVotePositive == isVotePositive)
                            {
                                return this.Content("<strong class=\"vote-error\">Не може да давате повече от един глас на тема!<a href=\"javascript:void(0)\">OK</a></strong>" + topic.Votes.ToString());
                            }
                            else if (vote.IsVotePositive == null)
                            {
                                vote.IsVotePositive = isVotePositive;
                            }
                            else
                            {
                                vote.IsVotePositive = null;
                            }

                            isUserInCollection = true;
                            break;
                        }
                    }
                }

                if (!isUserInCollection)
                {
                    TopicVote newVote = new TopicVote
                    {
                        User = this.CurrentUser,
                        IsVotePositive = isVotePositive
                    };

                    topic.UserVotes.Add(newVote);
                }

                if (isVotePositive)
                {
                    topic.Votes++;
                    topic.AuthorProfile.ForumPoints += 2;
                }
                else
                {
                    topic.Votes--;
                    topic.AuthorProfile.ForumPoints -= 2;
                }

                this.Data.SaveChanges();

                return this.Content(topic.Votes.ToString());
            }

            return this.Content("<strong class=\"vote-error\">Не може да гласувате за собствените си теми!<a href=\"javascript:void(0)\">OK</a></strong>" + topic.Votes.ToString());
        }
    }
}