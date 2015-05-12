using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crossroads.Data;
using Crossroads.Models.UserProfile;
using Crossroads.Web.ViewModels.ProfileViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crossroads.Web.Controllers
{
    public class ProfileCommentsController : BaseController
    {
        public ProfileCommentsController(ICrossroadsData data)
            :base(data)
        {

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult PostComment(AddCommentViewModel comment)
        {
            if (comment != null && ModelState.IsValid)
            {
                var dbComment = Mapper.Map<ProfileComment>(comment);

                var autorProfile = this.Data.Profiles.All()
                    .Where(p => p.ProfileUser.UserName == this.CurrentUser.UserName)
                    .FirstOrDefault();

                dbComment.AuthorProfile = autorProfile;
                dbComment.DateCreated = DateTime.Now;

                var profile = this.Data.Profiles.GetById(comment.ProfileId);
                if (profile == null)
                {
                    throw new HttpException(404, "Profile not found");
                }

                profile.Comments.Add(dbComment);
                this.Data.SaveChanges();

                var viewModel = Mapper.Map<CommentViewModel>(dbComment);

                return PartialView("~/Views/Profile/_CommentPartial.cshtml", viewModel);
            }

            throw new HttpException(400, "Invalid comment");
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteComment(int id)
        {
            ProfileComment comment = this.Data.Comments.GetById(id);
            this.Data.Comments.Delete(comment);
            this.Data.SaveChanges();

            return Content("");
        }

    }
}