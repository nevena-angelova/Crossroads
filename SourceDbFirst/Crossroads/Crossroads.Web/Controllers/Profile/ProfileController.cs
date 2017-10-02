using Crossroads.Data;
using Crossroads.Web.ViewModels.ProfileViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using System.IO;
using System.Net;
using Crossroads.Web.Infrastructure.Services;
using System.Drawing;
using System.Drawing.Imaging;
using Crossroads.Web.Infrastructure.Populators;
using Crossroads.Web.Infrastructure.AuthorizeAttributes;
using System.Web.Helpers;

namespace Crossroads.Web.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        private IDropDownListPopulator populator;

        public ProfileController(ICrossroadsData data, IDropDownListPopulator populator)
            : base(data)
        {
            this.populator = populator;
        }

        public ActionResult DisplayUser(string userName)
        {
            if (userName == null)
            {
                userName = this.CurrentUser.UserName;
            }

            DisplayUserViewModel user = this.Data
                     .Users
                     .All()
                     .Where(u => u.UserName == userName)
                     .Project()
                     .To<DisplayUserViewModel>()
                     .FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound("User not found!");
            }

            return View(user);
        }

        [MyAuthorizeAttribute]
        public ActionResult EditUser(string userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EditUserViewModel user = this.Data.Users
                .All().Where(u => u.UserName == userName)
                .Project().To<EditUserViewModel>().FirstOrDefault();

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
                user.MusicGenresIds = user.MusicGenres.Select(i => i.Id).ToArray();
            }

            ViewBag.Towns = this.populator.GetTowns();
            ViewBag.AllInterests = this.populator.GetInterests();
            ViewBag.AllMusicGenres = this.populator.GetMusicGenres();

            return View(user);
        }

        [HttpPost]
        [MyAuthorizeAttribute]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(EditUserViewModel user)
        {
            if (user != null && ModelState.IsValid)
            {
                User dbUser = this.Data.Users.All()
                    .Where(u => u.UserName == user.UserName)
                    .FirstOrDefault();

                if (dbUser == null)
                {
                    return HttpNotFound("User not found!");
                }

                dbUser.FirstName = user.FirstName;
                dbUser.LastName = user.LastName;
                dbUser.Email = user.Email;
                dbUser.TownId = user.TownId;
                dbUser.Bands = user.Bands;
                dbUser.IsMale = user.IsMale;
                dbUser.BirthDate = user.BirthDate;
                dbUser.About = user.About;
                dbUser.Skype = user.Skype;
                dbUser.Facebook = user.Facebook;
                dbUser.EmailMsgNotify = user.EmailMsgNotify;

                if (!String.IsNullOrWhiteSpace(user.Password))
                {
                    dbUser.PasswordHash = Crypto.SHA1(user.UserName.Substring(3) + user.Password);
                }

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

                return RedirectToAction("DisplayUser", new { userName = this.CurrentUser.UserName });
            }

            ViewBag.AllInterests = this.populator.GetInterests();
            ViewBag.AllMusicGenres = this.populator.GetMusicGenres();
            ViewBag.Towns = this.populator.GetTowns();

            return View(user);
        }

        [AllowAnonymous]
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

        [ChildActionOnly]
        public int GetAge(DateTime birthday)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthday.Year;
            if (now < birthday.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
}