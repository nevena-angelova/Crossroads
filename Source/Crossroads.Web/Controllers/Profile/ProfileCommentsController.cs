using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crossroads.Data;
using Crossroads.Models.Profile;
using Crossroads.Web.ViewModels.ProfileViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Crossroads.Web.Infrastructure.Constants;
using Crossroads.Web.Infrastructure.Sanitizer;
using Crossroads.Web.Infrastructure.AuthorizeAttributes;

namespace Crossroads.Web.Controllers
{
    public class ProfileCommentsController : BaseController
    {
        private readonly ISanitizer sanitizer;

        public ProfileCommentsController(ICrossroadsData data, ISanitizer sanitizer)
            : base(data)
        {
            this.sanitizer = sanitizer;
        }

        [Authorize]
        public ActionResult PostComment(int profileId)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            AddCommentViewModel comment = new AddCommentViewModel
            {
                ProfileId = profileId
            };

            return this.PartialView(Partials.PostComment, comment);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult PostComment(AddCommentViewModel comment)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (comment != null && ModelState.IsValid)
            {
                var autorProfile = this.Data.Profiles.All()
                    .Where(p => p.ProfileUser.UserName == this.CurrentUser.UserName)
                    .FirstOrDefault();

                if (autorProfile == null)
                {
                    return HttpNotFound("Profile not found!");
                }

                ProfileComment dbComment = new ProfileComment
                {
                    AuthorProfile = autorProfile,
                    Content = sanitizer.Sanitize(comment.Content)
                };

                var profile = this.Data.Profiles.GetById(comment.ProfileId);
                if (profile == null)
                {
                    return HttpNotFound("Profile not found!");
                }

                profile.ProfileComments.Add(dbComment);
                this.Data.SaveChanges();

                var viewModel = Mapper.Map<CommentViewModel>(dbComment);

                return PartialView(Partials.ProfileComment, viewModel);
            }

            throw new HttpException(400, "Comment not valid!");
        }

        [MyAuthorize]
        public ActionResult EditComment(int commentId, string userName)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            EditCommentViewModel comment = this.Data.ProfileComments.All()
                .Where(c => c.Id == commentId).Project()
                .To<EditCommentViewModel>()
                .FirstOrDefault();

            return this.PartialView(Partials.EditProfileComment, comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorize]
        public ActionResult EditComment(EditCommentViewModel comment)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (comment != null && ModelState.IsValid)
            {
                ProfileComment dbComment = this.Data.ProfileComments.GetById(comment.Id);

                if (dbComment == null)
                {
                    return HttpNotFound("Comment not found!");
                }

                dbComment.Content = sanitizer.Sanitize(comment.Content);
                this.Data.SaveChanges();

                CommentViewModel commentModel = Mapper.Map<CommentViewModel>(dbComment);

                return this.PartialView(Partials.ProfileComment, commentModel);
            }

            throw new HttpException(400, "Comment not valid!");
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteComment(int commentId)
        {
            ProfileComment comment = this.Data.ProfileComments.GetById(commentId);
            if (comment == null)
            {
                return HttpNotFound("Comment not found!");
            }

            this.Data.ProfileComments.Delete(comment);
            this.Data.SaveChanges();

            return Content("");
        }
    }
}