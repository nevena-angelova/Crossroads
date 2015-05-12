using Crossroads.Data;
using Crossroads.Web.ViewModels.ProfileViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crossroads.Models.UserProfile;
using System.IO;
using System.Net;
using Crossroads.Web.Infrastructure.AuthorizeAttributes;

namespace Crossroads.Web.Controllers
{
    public class ProfileController : BaseController
    {

        public ProfileController(ICrossroadsData data)
            : base(data)
        { }

        [Authorize]
        public ActionResult Display(string userName)
        {
            DisplayProfileViewModel userProfile = this.Data
                     .Profiles
                     .All()
                     .Where(p => p.ProfileUser.UserName == userName)
                     .Project()
                     .To<DisplayProfileViewModel>()
                     .FirstOrDefault();

            if (userProfile == null)
            {
                throw new HttpException(404, "Няма намерен профил.");
            }

            return View(userProfile);
        }

        [Authorize]
        public ActionResult CreateProfile()
        {
            UserProfile dbUserProfile = new UserProfile()
            {
                ProfileUser = this.CurrentUser,
                DateCreated = DateTime.Now
            };

            this.Data.Profiles.Add(dbUserProfile);
            this.Data.SaveChanges();

            return RedirectToAction("Display", new { userName = this.CurrentUser.UserName });
        }

        // TODO: Add Caching
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
                return HttpNotFound();
            }

            if (profile.Interests != null)
            {
                profile.InterestsIds = profile.Interests.Select(i => i.Id).ToArray();
            }

            if (profile.MusicGenres != null)
            {
                profile.MusicGenresIds = profile.MusicGenres.Select(i => i.Id).ToArray();
            }

            ViewBag.AllInterests = this.Data.Interests.All().Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Name
            });


            ViewBag.AllMusicGenres = this.Data.MusicGenres.All().Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Name
            });

            return View(profile);
        }

        [HttpPost]
        [MyAuthorizeAttribute]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(EditProfileViewModel profile)
        {

            if (profile != null && ModelState.IsValid)
            {
                var dbProfile = Mapper.Map<UserProfile>(profile);

                if (profile.ProfileImage != null)
                {
                    using (var memory = new MemoryStream())
                    {
                        profile.ProfileImage.InputStream.CopyTo(memory);
                        var content = memory.GetBuffer();

                        ProfileImage newImage = new ProfileImage
                        {
                            Content = content,
                            FileExtension = profile.ProfileImage.FileName.Split(new[] { '.' }).Last()
                        };

                        dbProfile.Image = newImage;
                        this.Data.Images.Add(newImage);
                        dbProfile.Image = newImage;
                    }
                }

                this.Data.Profiles.Update(dbProfile);
                this.Data.SaveChanges();
                this.Data.Profiles.Detach(dbProfile); // without it .Clear() does nothing.

                var dbProfile2 = this.Data.Profiles.GetById(profile.Id);
                dbProfile2.Interests.Clear();
                dbProfile2.MusicGenres.Clear();

                if (profile.InterestsIds != null)
                {
                    dbProfile2.Interests = this.Data.Interests.All()
                        .Where(i => profile.InterestsIds.Contains(i.Id))
                        .ToList();

                    this.Data.Profiles.Update(dbProfile2);
                    this.Data.SaveChanges();
                }

                if (profile.MusicGenresIds != null)
                {
                    dbProfile2.MusicGenres = this.Data.MusicGenres.All()
                        .Where(i => profile.MusicGenresIds.Contains(i.Id))
                        .ToList();

                    this.Data.Profiles.Update(dbProfile2);
                    this.Data.SaveChanges();
                }

                return RedirectToAction("Display", new { userName = profile.UserName });
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

            return View(profile);
        }

        public ActionResult Image(int id)
        {
            var image = this.Data.Images.GetById(id);
            if (image == null)
            {
                throw new HttpException(404, "Няма намерена снимка.");
            }

            return File(image.Content, "image/" + image.FileExtension);
        }
    }
}