using Crossroads.Data;
using Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Users;
using Crossroads.Web.Controllers;
using System;
using System.Collections.Generic;
using AutoMapper.QueryableExtensions;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Net;
using System.IO;
using Crossroads.Web.Infrastructure.Services;
using System.Drawing;
using System.Drawing.Imaging;
using Crossroads.Web.Infrastructure.Constants;
using Crossroads.Web.Infrastructure.Populators;
using System.Web.Helpers;
using System.Net.Mail;

namespace Crossroads.Web.Areas.Administration.Controllers.Users
{
    [Authorize(Roles = UserRoles.AdminRole)]
    public class UsersAdminController : BaseController
    {
        private IDropDownListPopulator populator;

        private const int DefaultPageSize = 15;

        public UsersAdminController(ICrossroadsData data, IDropDownListPopulator populator)
            : base(data)
        {
            this.populator = populator;
        }

        public ActionResult ListUsers(int? page, string searchString, string orderBy)
        {
            Expression<Func<User, bool>> WhereExpression = u => true;

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                WhereExpression = u => u.UserName.ToLower().Contains(searchString.ToLower());
            }

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            IPagedList<UserViewModel> users = this.Data.Users.All()
                   .Where(WhereExpression)
                   .OrderBy(orderBy ?? "Id")
                   .Project()
                   .To<UserViewModel>()
                   .ToPagedList(currentPageIndex, DefaultPageSize);

            ViewBag.OrderBy = orderBy;
            ViewBag.SearchString = searchString;

            return View(Views.ListUsers, users);
        }

        public ActionResult DisplayUser(string userName)
        {
            DisplayUserViewModel user = this.Data.Users.All()
                .Where(u => u.UserName == userName)
                .Project()
                .To<DisplayUserViewModel>()
                .FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound("User not found!");
            }

            return this.View(Views.DisplayUserAdmin, user);
        }

        public ActionResult EditUser(string userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EditUserViewModel user = this.Data.Users.All()
                .Where(u => u.UserName == userName)
                .Project()
                .To<EditUserViewModel>()
                .FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound("User not found!");
            }

            if (user.ProfileInterests != null)
            {
                user.InterestsIds = user.ProfileInterests.Select(i => i.Id).ToArray();
            }

            if (user.MusicGenres != null)
            {
                user.MusicGenresIds = user.MusicGenres.Select(mg => mg.Id).ToArray();
            }

            if (user.Roles != null)
            {
                user.RolesIds = user.Roles.Select(r => r.Id).ToArray();
            }

            ViewBag.AllRoles = this.populator.GetRoles();
            ViewBag.Towns = this.populator.GetTowns();
            ViewBag.AllInterests = this.populator.GetInterests();
            ViewBag.AllMusicGenres = this.populator.GetMusicGenres();

            return View(Views.EditUserAdmin, user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(EditUserViewModel user)
        {
            if (user != null && ModelState.IsValid)
            {
                User dbUser = this.Data.Users.GetById(user.Id);
                if (dbUser == null)
                {
                    return HttpNotFound("User not found!");
                }

                bool IsUserNameOccupied = this.Data.Users.All()
                    .Where(u => u.UserName == user.UserName)
                    .Any();

                if (IsUserNameOccupied && dbUser.UserName != user.UserName)
                {
                    ModelState.AddModelError("", "Потребителското име е заето!");

                    ViewBag.AllRoles = this.populator.GetRoles();
                    ViewBag.Towns = this.populator.GetTowns();
                    ViewBag.AllInterests = this.populator.GetInterests();
                    ViewBag.AllMusicGenres = this.populator.GetMusicGenres();

                    return View(Views.EditUserAdmin, user);
                }

                if (!String.IsNullOrWhiteSpace(user.Password))
                {
                    dbUser.PasswordHash = Crypto.SHA1(user.UserName.Substring(3) + user.Password);
                }

                dbUser.UserName = user.UserName;
                dbUser.Email = user.Email;
                dbUser.FirstName = user.FirstName;
                dbUser.LastName = user.LastName;
                dbUser.TownId = user.TownId;
                dbUser.Bands = user.Bands;
                dbUser.IsMale = user.IsMale;
                dbUser.BirthDate = user.BirthDate;
                dbUser.About = user.About;

                dbUser.ProfileInterests.Clear();

                if (user.InterestsIds != null)
                {
                    List<ProfileInterest> newInterests = this.Data.Interests.All()
                    .Where(i => user.InterestsIds.Contains(i.Id))
                    .ToList();

                    dbUser.ProfileInterests = newInterests;
                }

                dbUser.MusicGenres.Clear();

                if (user.MusicGenresIds != null)
                {
                    List<MusicGenre> newMusicGenres = this.Data.MusicGenres.All()
                    .Where(g => user.MusicGenresIds.Contains(g.Id))
                    .ToList();

                    dbUser.MusicGenres = newMusicGenres;
                }

                dbUser.Roles.Clear();

                if (user.RolesIds != null)
                {
                    List<Role> newRoles = this.Data.Roles.All()
                    .Where(i => user.RolesIds.Contains(i.Id))
                    .ToList();

                    dbUser.Roles = newRoles;
                }

                if (user.ProfileImage != null)
                {
                    Random generator = new Random();
                    int randNum = generator.Next(0, 100);

                    HttpPostedFileBase file = user.ProfileImage;
                    string fileName = file.FileName.Split(new[] { '.' }).First();
                    string fileExtension = file.FileName.Split(new[] { '.' }).Last();
                    string image = fileName + "-" + user.Id + randNum + "." + fileExtension;
                    string url = Path.Combine(HttpContext.Server.MapPath("~/App_Data/Avatars/"), image);

                    if (HttpPostedFileBaseExtensions.IsImage(file))
                    {
                        Image img = Image.FromStream(file.InputStream);
                        Bitmap bitmap = this.ResizeImage(img, 300);
                        bitmap.Save(url);

                        dbUser.Image = image;
                    }
                    else
                    {
                        throw new ArgumentException("Only jpg, jpeg, png, bmp and gif formats are supported!");
                    }
                }

                this.Data.SaveChanges();

                return RedirectToAction("DisplayUser", new { userName = user.UserName });
            }

            ViewBag.AllRoles = this.populator.GetRoles();
            ViewBag.Towns = this.populator.GetTowns();
            ViewBag.AllInterests = this.populator.GetInterests();
            ViewBag.AllMusicGenres = this.populator.GetMusicGenres();

            return View(Views.EditUserAdmin, user);
        }

        public ActionResult DeleteUser(string userName)
        {
            User user = this.Data.Users.All()
                .Where(u => u.UserName == userName)
                .FirstOrDefault();

            var profileCommentToIds = user.ProfileComments1.Select(c => c.Id).ToList();
            foreach (var profileCommentToId in profileCommentToIds)
            {
                this.Data.ProfileComments.Delete(profileCommentToId);
            }

            var profileCommentIds = user.ProfileComments.Select(c => c.Id).ToList();
            foreach (var profileCommentId in profileCommentIds)
            {
                this.Data.ProfileComments.Delete(profileCommentId);
            }

            var messageToIds = user.ProfileMessages.Select(m => m.Id).ToList();
            foreach (var messageToId in messageToIds)
            {
                this.Data.Messages.Delete(messageToId);
            }

            var messageIds = user.ProfileMessages1.Select(m => m.Id).ToList();
            foreach (var messageId in messageIds)
            {
                this.Data.Messages.Delete(messageId);
            }

            var topicsIds = user.Topics.Select(t => t.Id).ToList();
            foreach (var topicId in topicsIds)
            {
                this.Data.Topics.Delete(topicId);
            }

            var answersIds = user.Answers.Select(a => a.Id).ToList();

            foreach (var answerId in answersIds)
            {
                this.Data.Answers.Delete(answerId);
            }

            var commentsIds = user.Comments.Select(c => c.Id).ToList();

            foreach (var commentId in commentsIds)
            {
                this.Data.Comments.Delete(commentId);
            }

            var answerFlagIds = user.AnswerFlags.Select(af => af.Id).ToList();
            foreach (var answerFlagId in answerFlagIds)
            {
                this.Data.AnswerFlags.Delete(answerFlagId);
            }

            var answerVoteIds = user.AnswerVotes.Select(av => av.Id).ToList();
            foreach (var answerVoteId in answerVoteIds)
            {
                this.Data.AnswerVotes.Delete(answerVoteId);
            }

            var commentFlagIds = user.CommentFlags.Select(cf => cf.Id).ToList();
            foreach (var commentFlagId in commentFlagIds)
            {
                this.Data.AnswerVotes.Delete(commentFlagId);
            }

            var topicFlagIds = user.TopicFlags.Select(tf => tf.Id).ToList();
            foreach (var topicFlagId in topicFlagIds)
            {
                this.Data.TopicFlags.Delete(topicFlagId);
            }

            var topicVoteIds = user.TopicVotes.Select(tv => tv.Id).ToList();
            foreach (var topicVoteId in topicVoteIds)
            {
                this.Data.TopicVotes.Delete(topicVoteId);
            }

            this.Data.Users.Delete(user);

            this.Data.SaveChanges();

            return this.RedirectToAction("ListUsers");
        }

        [OutputCache(Duration = 60 * 60 * 168, VaryByParam = "image; limitWidth")]
        public ActionResult GetImage(string image, int? limitWidth)
        {
            string imageUrl = Path.Combine(HttpContext.Server.MapPath("~/App_Data/Avatars/"), image);
            Image img = Image.FromFile(imageUrl);
            if (img == null)
            {
                return HttpNotFound("Image not found!");
            }

            Bitmap bitmap = this.ResizeImage(img, limitWidth);

            using (var outputStream = new MemoryStream())
            {
                bitmap.Save(outputStream, ImageFormat.Png);
                return File(outputStream.GetBuffer(), "image/png");
            }
        }

        private Bitmap ResizeImage(Image img, int? limitWidth)
        {
            int height = img.Height;
            int width = img.Width;
            if (limitWidth != null && img.Width > limitWidth)
            {
                height = (int)Math.Round((double)(limitWidth * height) / width);
                width = (int)limitWidth;
            }

            return new Bitmap(img, width, height);
        }

        public ActionResult AddMessageToAll()
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            MsgToAllViewModel msg = new MsgToAllViewModel();
            return this.PartialView(Partials.MsgToAll, msg);

        }

        [HttpPost]
        public ActionResult AddMessageToAll(MsgToAllViewModel model)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            SmtpClient smtpServer = new SmtpClient("hefes.icnhost.net");
            smtpServer.Credentials = new NetworkCredential(Constants.AdminEmail, Constants.AdminEmailPass);
            smtpServer.Port = 25;

            var allUsers = this.Data.Users.All().ToList();

            foreach (var user in allUsers)
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Constants.AdminEmail);
                mail.To.Add(user.Email);
                mail.Subject = model.Title;
                mail.Body = model.Content;
                mail.IsBodyHtml = true;

                smtpServer.Send(mail);
            }

            return this.Content("Съобщенията са изпратени успешно!");
        }
    }
}