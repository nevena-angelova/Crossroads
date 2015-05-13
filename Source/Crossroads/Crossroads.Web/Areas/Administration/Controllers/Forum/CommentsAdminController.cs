using Crossroads.Common;
using Crossroads.Data;
using Crossroads.Models.Forum;
using Crossroads.Web.Controllers;
using Crossroads.Web.Infrastructure.Sanitizer;
using MvcPaging;
using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using Crossroads.Web.Areas.Administration.ViewModels.ForumViewModels.Comments;
using Crossroads.Web.Infrastructure.Constants;
using System.Net;

namespace Crossroads.Web.Areas.Administration.Controllers.Forum
{
    [Authorize(Roles = GlobalConstants.AdminRole + "," + GlobalConstants.ModeratorRole)]
    public class CommentsAdminController : BaseController
    {
        private readonly ISanitizer sanitizer;

        private const int DefaultPageSize = 4;

        public CommentsAdminController(ICrossroadsData data, ISanitizer sanitizer)
            : base(data)
        {
            this.sanitizer = sanitizer;
        }

        public ActionResult ListComments(string orderBy, string searchValue, int? page, string searchProperty)
        {
            Expression<Func<Comment, bool>> WhereExpression = c => true;

            if (!String.IsNullOrWhiteSpace(searchValue))
            {
                if (searchProperty == "UserName")
                {
                    WhereExpression = c => c.AuthorProfile.ProfileUser.UserName.ToLower().Contains(searchValue.ToLower());
                }
            }

            if (searchProperty == "Flagged")
            {
                WhereExpression = a => a.Flags > 0;
            }

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            IPagedList<AdminCommentViewModel> comments = this.Data.Comments.All()
                .Where(WhereExpression)
                .OrderBy(orderBy ?? "Id")
                .Project()
                .To<AdminCommentViewModel>()
                .ToPagedList(currentPageIndex, DefaultPageSize);

            if (!comments.Any())
            {
                ViewBag.NothingFound = "Няма намерени коментари.";
            }

            ViewBag.OrderBy = orderBy;
            ViewBag.SearchValue = searchValue;
            ViewBag.SearchProperty = searchProperty;

            return this.View(Views.ListComments, comments);
        }

        public ActionResult AdminEditComment(int id, bool? refuse)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            Comment dbComment = this.Data.Comments.GetById(id);
            if (dbComment == null)
            {
                return HttpNotFound("Comment not found!");
            }

            AdminEditCommentViewModel comment = Mapper.Map<AdminEditCommentViewModel>(dbComment);

            if (refuse == true)
            {
                return this.Content(comment.Content);
            }

            return this.PartialView(Partials.AdminEditComment, comment);
        }

        [HttpPost]
        public ActionResult AdminEditComment(AdminEditCommentViewModel comment)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (comment != null && ModelState.IsValid)
            {
                Comment dbComment = this.Data.Comments.GetById(comment.Id);
                dbComment.Content = sanitizer.Sanitize(comment.Content);

                this.Data.SaveChanges();

                return this.Content(comment.Content);
            }

            return this.PartialView(Partials.AdminEditComment, comment);
        }

        [HttpDelete]
        public ActionResult AdminDeleteComment(int id)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            Comment comment = this.Data.Comments.GetById(id);
            if (comment == null)
            {
                return HttpNotFound("Comment not found!");
            }

            this.Data.Comments.Delete(comment);
            this.Data.SaveChanges();

            return this.Content(String.Empty);
        }
    }
}