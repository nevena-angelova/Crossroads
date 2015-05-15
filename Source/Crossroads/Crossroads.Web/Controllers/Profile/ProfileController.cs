using Crossroads.Data;
using Crossroads.Web.ViewModels.ProfileViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crossroads.Models.Profile;
using System.IO;
using System.Net;
using Crossroads.Web.Infrastructure.AuthorizeAttributes;
using Crossroads.Web.Infrastructure.Constants;
using Crossroads.Web.Infrastructure.Services;
using System.Drawing;
using System.Drawing.Imaging;
using Crossroads.Web.Infrastructure.Populators;

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

        public ActionResult DisplayProfile(string userName)
        {
            if (userName == null)
            {
                userName = this.CurrentUser.UserName;
            }

            DisplayProfileViewModel userProfile = this.Data
                     .Profiles
                     .All()
                     .Where(p => p.ProfileUser.UserName == userName)
                     .Project()
                     .To<DisplayProfileViewModel>()
                     .FirstOrDefault();

            if (userProfile == null)
            {
                return HttpNotFound("Profile not found!");
            }

            return View(userProfile);
        }

        public ActionResult CreateProfile()
        {
            UserProfile dbUserProfile = new UserProfile
            {
                ProfileUser = this.CurrentUser,
                ForumPoints = 0,
                DateCreated = DateTime.Now
            };

            this.Data.Profiles.Add(dbUserProfile);
            this.Data.SaveChanges();

            return RedirectToAction("DisplayProfile", new { userName = this.CurrentUser.UserName });
        }

        [MyAuthorizeAttribute]
        public ActionResult EditProfile(string userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EditProfileViewModel profile = this.Data.Profiles
                .All().Where(p => p.ProfileUser.UserName == userName)
                .Project().To<EditProfileViewModel>().FirstOrDefault();

            if (profile == null)
            {
                return HttpNotFound("Profile not found!");
            }

            if (profile.Interests != null)
            {
                profile.InterestsIds = profile.Interests.Select(i => i.Id).ToArray();
            }

            if (profile.MusicGenres != null)
            {
                profile.MusicGenresIds = profile.MusicGenres.Select(i => i.Id).ToArray();
            }

            ViewBag.Towns = this.populator.GetTowns();
            ViewBag.AllInterests = this.populator.GetInterests();
            ViewBag.AllMusicGenres = this.populator.GetMusicGenres();

            return View(profile);
        }

        [HttpPost]
        [MyAuthorizeAttribute]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(EditProfileViewModel profile)
        {
            if (profile != null && ModelState.IsValid)
            {
                UserProfile dbProfile = this.Data.Profiles.GetById(profile.Id);

                if (dbProfile == null)
                {
                    return HttpNotFound("Profile not found!");
                }

                dbProfile.FirstName = profile.FirstName;
                dbProfile.LastName = profile.LastName;
                dbProfile.TownId = profile.TownId;
                dbProfile.Bands = profile.Bands;
                dbProfile.IsMale = profile.IsMale;
                dbProfile.BirthDate = profile.BirthDate;
                dbProfile.About = profile.About;

                dbProfile.Interests.Clear();

                if (profile.InterestsIds != null)
                {
                    List<ProfileInterest> newInterests = this.Data.Interests.All()
                    .Where(i => profile.InterestsIds.Contains(i.Id))
                    .ToList();

                    dbProfile.Interests = newInterests;
                }

                dbProfile.MusicGenres.Clear();

                if (profile.MusicGenresIds != null)
                {
                    List<MusicGenre> newMusicGenres = this.Data.MusicGenres.All()
                    .Where(g => profile.MusicGenresIds.Contains(g.Id))
                    .ToList();

                    dbProfile.MusicGenres = newMusicGenres;
                }

                if (profile.ProfileImage != null)
                {
                    Random generator = new Random();
                    int randNum = generator.Next(0, 100);

                    HttpPostedFileBase file = profile.ProfileImage;
                    string fileName = file.FileName.Split(new[] { '.' }).First();
                    string image = fileName + "-" + profile.Id + randNum + ".png";

                    if (HttpPostedFileBaseExtensions.IsImage(file))
                    {
                        Image img = Image.FromStream(file.InputStream);
                        Bitmap bitmap = this.ResizeImage(img, 300);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            bitmap.Save(ms, ImageFormat.Png);
                            DropboxHandler.UploadFile(ms.GetBuffer(), image);
                        }

                        if (dbProfile.Image != null)
                        {
                            DropboxHandler.Delete(dbProfile.Image);
                        }

                        dbProfile.Image = image;
                    }
                    else
                    {
                        throw new ArgumentException("Only jpg, jpeg, png, bmp and gif formats are supported!");
                    }
                }

                this.Data.SaveChanges();

                return RedirectToAction("DisplayProfile", new { userName = this.CurrentUser.UserName });
            }

            ViewBag.AllInterests = this.populator.GetInterests();
            ViewBag.AllMusicGenres = this.populator.GetMusicGenres();
            ViewBag.Towns = this.populator.GetTowns();

            return View(profile);
        }

        [AllowAnonymous]
        [OutputCache(Duration = 60 * 60 * 168, VaryByParam = "image; limitWidth")]
        public ActionResult GetImage(string image, int? limitWidth)
        {
            Image img;
            using (MemoryStream stream = new MemoryStream(DropboxHandler.GetBytes(image)))
            {
                img = Image.FromStream(stream);
            }

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