using AutoMapper;
using Crossroads.Data;
using Crossroads.Web.Infrastructure.AuthorizeAttributes;
using Crossroads.Web.Infrastructure.Constants;
using Crossroads.Web.Infrastructure.Sanitizer;
using Crossroads.Web.ViewModels.ForumViewModels.Comments;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Crossroads.Web.Controllers.Forum
{
    public class CommentsController : BaseController
    {
        private readonly ISanitizer sanitizer;

        public CommentsController(ICrossroadsData data, ISanitizer sanitizer)
            : base(data)
        {
            this.sanitizer = sanitizer;
        }

        [AjaxAuthorize]
        public ActionResult AddComment(int answerId)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            AddCommentViewModel comment = new AddCommentViewModel();
            comment.AnswerId = answerId;
            return this.PartialView(Partials.AddComment, comment);
        }

        [HttpPost]
        [AjaxAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(AddCommentViewModel comment)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (comment != null && ModelState.IsValid)
            {
                User author = this.Data.Users.All()
                                    .Where(u => u.UserName == User.Identity.Name)
                                    .FirstOrDefault();

                Comment dbComment = new Comment
                {
                    DateCreated = DateTime.Now,
                    Content = sanitizer.Sanitize(comment.Content),
                    AnswerId = comment.AnswerId,
                    User = author
                };

                this.Data.Comments.Add(dbComment);
                this.Data.SaveChanges();

                DisplayCommentViewModel commentModel = Mapper.Map<DisplayCommentViewModel>(dbComment);

                return this.PartialView(Partials.DisplayComment, commentModel);
            }

            throw new HttpException(400, "Comment not valid!");
        }

        [MyOrInRoleAuthorize]
        public ActionResult EditComment(int commentId, string userName)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            Comment dbComment = this.Data.Comments.GetById(commentId);
            if (dbComment == null)
            {
                return HttpNotFound("Comment not found!");
            }

            EditCommentViewModel comment = Mapper.Map<EditCommentViewModel>(dbComment);

            return this.PartialView(Partials.EditComment, comment);
        }

        [HttpPost]
        [MyOrInRoleAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment(EditCommentViewModel comment)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (comment != null && ModelState.IsValid)
            {
                Comment dbComment = this.Data.Comments.GetById(comment.Id);
                if (dbComment == null)
                {
                    return HttpNotFound("Comment not found!");
                }

                dbComment.Content = sanitizer.Sanitize(comment.Content);
                dbComment.DateEdited = DateTime.Now;

                this.Data.SaveChanges();

                DisplayCommentViewModel commentModel = Mapper.Map<DisplayCommentViewModel>(dbComment);

                return this.PartialView(Partials.DisplayComment, commentModel);
            }

            throw new HttpException(400, "Comment not valid!");
        }

        [HttpPost]
        [AjaxAuthorize]
        public ActionResult CommentFlag(int commentId)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call!");
            }

            Comment comment = this.Data.Comments.GetById(commentId);
            if (comment == null)
            {
                return HttpNotFound("Comment not found!");
            }

            if (!(comment.User.Id == this.CurrentUser.Id))
            {
                bool isUserInCollection = false;

                foreach (var flag in comment.CommentFlags)
                {
                    if (flag.UserId == this.CurrentUser.Id)
                    {
                        if (flag.Flagged == true || flag.Flagged == null)
                        {
                            comment.Flags--;
                            flag.Flagged = false;
                        }
                        else
                        {
                            comment.Flags++;
                            flag.Flagged = true;
                        }

                        isUserInCollection = true;
                        break;
                    }
                }

                if (!isUserInCollection)
                {
                    CommentFlag newFlag = new CommentFlag
                    {
                        User = this.CurrentUser
                    };

                    comment.Flags++;
                    comment.CommentFlags.Add(newFlag);
                }

                this.Data.SaveChanges();

                return this.Content(comment.Flags.ToString());
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}