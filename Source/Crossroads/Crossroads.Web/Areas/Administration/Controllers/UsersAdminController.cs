using Crossroads.Common;
using Crossroads.Data;
using Crossroads.Web.Areas.Administration.ViewModels.Users;
using Crossroads.Web.Controllers;
using Crossroads.Web.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcPaging;
using System.Linq.Expressions;
using Crossroads.Models;
using System.Net;
using Microsoft.AspNet.Identity;
using Crossroads.Models.Profile;
using Crossroads.Web.Infrastructure.Sanitizer;
using System.IO;
using Crossroads.Web.Infrastructure.Services;
using System.Drawing;
using System.Drawing.Imaging;

namespace Crossroads.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = GlobalConstants.AdminRole)]
    public class UsersAdminController : BaseController
    {
        private readonly ISanitizer sanitizer;

        private const int DefaultPageSize = 4;

        public UsersAdminController(ICrossroadsData data, ISanitizer sanitizer)
            : base(data)
        {
            this.sanitizer = sanitizer;
        }

        public ActionResult ListUsers(int? page, string searchString, string orderBy)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            IPagedList<UserViewModel> users;

            Expression<Func<User, bool>> WhereExpression = u => true;

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                WhereExpression = u => u.UserName.ToLower().Contains(searchString.ToLower());
            }

            if (orderBy == "LastActionTime")
            {
                users = this.Data.Users.All()
                    .Where(WhereExpression)
                    .OrderByDescending(u => u.LastActionTime)
                    .Select(u => new UserViewModel
                    {
                        UserName = u.UserName,
                        Email = u.Email,
                        LastActionTime = u.LastActionTime
                    })
                    .ToPagedList(currentPageIndex, DefaultPageSize);
            }
            else
            {
                Expression<Func<User, Object>> OrderByExpression = null;

                switch (orderBy)
                {
                    case "UserName":
                        OrderByExpression = u => u.UserName;
                        break;
                    default:
                        OrderByExpression = u => u.Id;
                        break;
                }

                users = this.Data.Users.All()
                    .Where(WhereExpression)
                    .OrderBy(OrderByExpression)
                    .Select(u => new UserViewModel
                    {
                        UserName = u.UserName,
                        Email = u.Email,
                        LastActionTime = u.LastActionTime
                    })
                    .ToPagedList(currentPageIndex, DefaultPageSize);
            }

            ViewBag.OrderBy = orderBy;
            ViewBag.SearchString = searchString;

            return View(users);
        }

        public ActionResult DisplayUser(string userName)
        {
            DisplayUserViewModel user = this.Data.Profiles.All()
                .Where(p => p.ProfileUser.UserName == userName)
                .Project()
                .To<DisplayUserViewModel>()
                .FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound("User not found!");
            }

            return this.View(user);
        }

        public ActionResult EditUser(string userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EditUserViewModel profile = this.Data.Profiles.All()
                .Where(p => p.ProfileUser.UserName == userName)
                .Project()
                .To<EditUserViewModel>()
                .FirstOrDefault();

            if (profile == null)
            {
                return HttpNotFound("Profile not found!");
            }

            ViewBag.Towns = this.Data.Towns.All()
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                });

            if (profile.Interests != null)
            {
                profile.InterestsIds = profile.Interests.Select(i => i.Id).ToArray();
            }

            if (profile.MusicGenres != null)
            {
                profile.MusicGenresIds = profile.MusicGenres.Select(i => i.Id).ToArray();
            }

            ViewBag.AllInterests = this.Data.Interests.All()
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.Name
                });


            ViewBag.AllMusicGenres = this.Data.MusicGenres.All()
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.Name
                });

            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(EditUserViewModel user)
        {
            if (user != null && ModelState.IsValid)
            {
                User dbUser = this.Data.Users.All()
                    .Where(u => u.Id == user.ProfileUserId)
                    .FirstOrDefault();

                if (dbUser == null)
                {
                    return HttpNotFound("User not found!");
                }

                bool IsUserNameOccupied = this.Data.Users.All()
                    .Where(u => u.UserName == user.UserName)
                    .Any();

                if (IsUserNameOccupied && dbUser.UserName != user.UserName)
                {
                    TempData["UserNameError"] = "Потребителското име е заето!";
                    return this.RedirectToAction("DisplayUser", new { userName = dbUser.UserName });
                }

                PasswordHasher passwordHasher = new PasswordHasher();

                dbUser.UserName = user.UserName;
                dbUser.Email = user.Email;
                dbUser.PasswordHash = passwordHasher.HashPassword(user.Password);

                UserProfile dbProfile = this.Data.Profiles.GetById(user.Id);

                if (dbProfile == null)
                {
                    return HttpNotFound("Profile not found!");
                }

                dbProfile.FirstName = user.FirstName;
                dbProfile.LastName = user.LastName;
                dbProfile.TownId = user.TownId;
                dbProfile.Bands = user.Bands;
                dbProfile.IsMale = user.IsMale;
                dbProfile.BirthDate = user.BirthDate;
                dbProfile.About = sanitizer.Sanitize(user.About);

                dbProfile.Interests.Clear();

                if (user.InterestsIds != null)
                {
                    List<ProfileInterest> newInterests = this.Data.Interests.All()
                    .Where(i => user.InterestsIds.Contains(i.Id))
                    .ToList();

                    dbProfile.Interests = newInterests;
                }

                dbProfile.MusicGenres.Clear();

                if (user.MusicGenresIds != null)
                {
                    List<MusicGenre> newMusicGenres = this.Data.MusicGenres.All()
                    .Where(g => user.MusicGenresIds.Contains(g.Id))
                    .ToList();

                    dbProfile.MusicGenres = newMusicGenres;
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

                        dbProfile.Image = image;
                    }
                    else
                    {
                        throw new ArgumentException("Only jpg, jpeg, png, bmp and gif formats are supported!");
                    }
                }

                this.Data.SaveChanges();

                return RedirectToAction("DisplayUser", new { userName = user.UserName });

            }

            ViewBag.AllInterests = this.Data.Interests.All().Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Name
            });

            ViewBag.AllMusicGenres = this.Data.Interests.All().Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Name
            });

            ViewBag.Towns = this.Data.Towns.All()
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                });

            return View(user);
        }

        public ActionResult DeleteUser(string userName)
        {
            User user = this.Data.Users.All()
                .Where(u => u.UserName == userName)
                .FirstOrDefault();

            UserProfile profile = this.Data.Profiles.All()
                .Where(p => p.ProfileUserId == user.Id)
                .FirstOrDefault();

            var commentIds = profile.ProfileComments.Select(c => c.Id).ToList();

            foreach (var commentId in commentIds)
            {
                this.Data.ProfileComments.Delete(commentId);
            }

            var messageIds = profile.Messages.Select(m => m.Id).ToList();

            foreach (var messageId in messageIds)
            {
                this.Data.Messages.Delete(messageId);
            }

            this.Data.Profiles.Delete(profile);
            this.Data.Users.Delete(user);

            this.Data.SaveChanges();

            return this.RedirectToAction("ListUsers");
        }

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
    }
}