using Crossroads.Common;
using Crossroads.Data;
using Crossroads.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Crossroads.Web.Areas.Administration.ViewModels.ForumViewModels.Categories;
using Crossroads.Web.Infrastructure.Constants;
using System.Net;
using Crossroads.Models.Forum;
using AutoMapper;
using Crossroads.Web.Infrastructure.Caching;

namespace Crossroads.Web.Areas.Administration.Controllers.Forum
{
    [Authorize(Roles = GlobalConstants.AdminRole + "," + GlobalConstants.ModeratorRole)]
    public class CategoriesController : BaseController
    {
        private readonly ICacheService service;

        public CategoriesController(ICrossroadsData data, ICacheService service)
            : base(data)
        {
            this.service = service;
        }

        public ActionResult ListCategories(string orderBy)
        {
            IList<CategoryOutputViewModel> categories = this.Data.Categories.All()
                    .OrderBy(orderBy ?? "Id")
                    .Project()
                    .To<CategoryOutputViewModel>()
                    .ToList();

            return this.View(Views.ListCategories, categories);
        }

        public ActionResult AddCategory()
        {
            return this.PartialView(Partials.AddCategory);
        }

        [HttpPost]
        public ActionResult AddCategory(CategoryInputViewModel category)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (category != null && ModelState.IsValid)
            {
                Category dbCategory = Mapper.Map<Category>(category);
                this.Data.Categories.Add(dbCategory);
                this.Data.SaveChanges();

                this.service.Clear("categories");

                CategoryOutputViewModel newCategory = Mapper.Map<CategoryOutputViewModel>(dbCategory);

                return this.PartialView(Partials.Category, newCategory);
            }

            return this.PartialView(Partials.AddCategory);
        }

        public ActionResult EditCategory(int id)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            Category dbCategory = this.Data.Categories.GetById(id);
            if (dbCategory == null)
            {
                return HttpNotFound("Category not found!");
            }

            CategoryInputViewModel category = Mapper.Map<CategoryInputViewModel>(dbCategory);

            return this.PartialView(Partials.EditCategory, category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(CategoryInputViewModel category)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (category != null && ModelState.IsValid)
            {
                Category dbCategory = this.Data.Categories.GetById(category.Id);
                if (dbCategory == null)
                {
                    return HttpNotFound("Category not found!");
                }

                dbCategory.Name = category.Name;
                dbCategory.Description = category.Description;
                this.Data.SaveChanges();

                this.service.Clear("categories");

                CategoryOutputViewModel newCategory = Mapper.Map<CategoryOutputViewModel>(dbCategory);

                return this.PartialView(Partials.Category, newCategory);
            }

            return this.PartialView(Partials.EditInterest, category);
        }

        [HttpDelete]
        public ActionResult DeleteCategory(int id)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            Category category = this.Data.Categories.GetById(id);
            if (category == null)
            {
                return HttpNotFound("Category not found!");
            }

            var topicIds = category.Topics.Select(t => t.Id).ToList();

            foreach (var topicId in topicIds)
            {
                var topicVoteIds = this.Data.TopicVotes.All()
                    .Where(v => v.TopicId == topicId)
                    .Select(v => v.Id)
                    .ToList();

                foreach (var topicVoteId in topicVoteIds)
                {
                    this.Data.TopicVotes.Delete(topicVoteId);
                }

                var answerIds = this.Data.Answers.All()
                    .Where(a => a.TopicId == topicId)
                    .Select(a => a.Id)
                    .ToList();

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

                this.Data.Topics.Delete(topicId);
            }

            this.Data.Categories.Delete(category);
            this.Data.SaveChanges();

            this.service.Clear("categories");

            return this.Content(String.Empty);
        }
    }
}