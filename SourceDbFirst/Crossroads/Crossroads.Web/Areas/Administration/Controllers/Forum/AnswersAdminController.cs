using Crossroads.Data;
using Crossroads.Web.Areas.Administration.ViewModels.ForumViewModels.Answers;
using Crossroads.Web.Controllers;
using Crossroads.Web.Infrastructure.Sanitizer;
using MvcPaging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crossroads.Web.Infrastructure.Constants;
using System.Net;

namespace Crossroads.Web.Areas.Administration.Controllers.Forum
{
   [Authorize(Roles = UserRoles.AdminRole + "," + UserRoles.ModeratorRole)]
    public class AnswersAdminController : BaseController
    {
        private readonly ISanitizer sanitizer;

        private const int DefaultPageSize = 4;

        public AnswersAdminController(ICrossroadsData data, ISanitizer sanitizer)
            : base(data)
        {
            this.sanitizer = sanitizer;
        }

        public ActionResult ListAnswers(string orderBy, string searchValue, int? page, string searchProperty)
        {
            Expression<Func<Answer, bool>> WhereExpression = a => true;

            if (!String.IsNullOrWhiteSpace(searchValue))
            {
                if (searchProperty == "UserName")
                {
                    WhereExpression = a => a.User.UserName.ToLower().Contains(searchValue.ToLower());
                }

                if (searchProperty == "Topic")
                {
                    WhereExpression = a => a.Topic.Title.ToLower().Contains(searchValue.ToLower());
                }
            }

            if (searchProperty == "Flagged")
            {
                WhereExpression = a => a.Flags > 0;
            }

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            IPagedList<AdminAnswerViewModel> answers = this.Data.Answers.All()
                .Where(WhereExpression)
                .OrderBy(orderBy ?? "Id")
                .Project()
                .To<AdminAnswerViewModel>()
                .ToPagedList(currentPageIndex, DefaultPageSize);

            if (!answers.Any())
            {
                ViewBag.NothingFound = "Няма намерени отговори";
            }

            ViewBag.OrderBy = orderBy;
            ViewBag.SearchValue = searchValue;
            ViewBag.SearchProperty = searchProperty;

            return this.View(Views.ListAnswers, answers);
        }

        public ActionResult AdminEditAnswer(int id, bool? refuse)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            Answer dbAnswer = this.Data.Answers.GetById(id);
            if (dbAnswer == null)
            {
                return HttpNotFound("Answer not found!");
            }

            AdminEditAnswerViewModel answer = Mapper.Map<AdminEditAnswerViewModel>(dbAnswer);

            if (refuse == true)
            {
                return this.Content(answer.Content);
            }

            return this.PartialView(Partials.AdminEditAnswer, answer);
        }

        [HttpPost]
        public ActionResult AdminEditAnswer(AdminEditAnswerViewModel answer)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (answer != null && ModelState.IsValid)
            {
                Answer dbAnswer = this.Data.Answers.GetById(answer.Id);
                dbAnswer.Content = sanitizer.Sanitize(answer.Content);

                if (answer.RemoveFlags && dbAnswer.AnswerFlags.Count > 0)
                {
                    dbAnswer.Flags = 0;
                    dbAnswer.AnswerFlags.Clear();
                }

                this.Data.SaveChanges();

                return this.Content(answer.Content);
            }

            return this.PartialView(Partials.AdminEditAnswer, answer);
        }

        public ActionResult AdminDeleteAnswer(int id)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            Answer answer = this.Data.Answers.GetById(id);
            if (answer == null)
            {
                return HttpNotFound("Answer not found!");
            }

            var answerVoteIds = answer.AnswerVotes.Select(v => v.Id).ToList();

            foreach (var answerVoteId in answerVoteIds)
            {
                this.Data.AnswerVotes.Delete(answerVoteId);
            }

            var commentIds = answer.Comments.Select(c => c.Id).ToList();

            foreach (var commentId in commentIds)
            {
                this.Data.Comments.Delete(commentId);
            }

            this.Data.Answers.Delete(answer);
            this.Data.SaveChanges();

            return this.Content(String.Empty);
        }
    }
}