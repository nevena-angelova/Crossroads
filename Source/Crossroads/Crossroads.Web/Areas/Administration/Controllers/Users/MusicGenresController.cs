using Crossroads.Common;
using Crossroads.Data;
using Crossroads.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Crossroads.Web.Infrastructure.Constants;
using System.Net;
using Crossroads.Models.Profile;
using System.Linq.Expressions;
using Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.MusicGenres;
using AutoMapper;
using Crossroads.Web.Infrastructure.Caching;

namespace Crossroads.Web.Areas.Administration.Controllers.Users
{
    [Authorize(Roles = GlobalConstants.AdminRole)]
    public class MusicGenresController : BaseController
    {
        private readonly ICacheService service;

        public MusicGenresController(ICrossroadsData data, ICacheService service)
            : base(data)
        {
            this.service = service;
        }

        public ActionResult ListGenres(bool? ordered)
        {
            IList<GenreOutputViewModel> genres = null;

            if (ordered == true)
            {
                genres = this.Data.MusicGenres.All()
                    .OrderBy(i => i.Name)
                    .Project()
                    .To<GenreOutputViewModel>()
                    .ToList();
            }
            else
            {
                genres = this.Data.MusicGenres.All()
                    .Project()
                    .To<GenreOutputViewModel>()
                    .ToList();
            }

            return this.View(Views.ListGenres, genres);
        }

        public ActionResult AddGenre()
        {
            return this.PartialView(Partials.AddGenre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGenre(GenreInputViewModel genre)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (genre != null && ModelState.IsValid)
            {
                MusicGenre dbGenre = Mapper.Map<MusicGenre>(genre);
                this.Data.MusicGenres.Add(dbGenre);
                this.Data.SaveChanges();

                this.service.Clear("genres");

                GenreOutputViewModel newGenre = Mapper.Map<GenreOutputViewModel>(dbGenre);

                return this.PartialView(Partials.Genre, newGenre);
            }

            return this.PartialView(Partials.AddGenre);
        }

        public ActionResult EditGenre(int id, bool? refuse)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            MusicGenre dbGenre = this.Data.MusicGenres.GetById(id);
            if (dbGenre == null)
            {
                return HttpNotFound("Genre not found!");
            }

            GenreInputViewModel genre = Mapper.Map<GenreInputViewModel>(dbGenre);

            if (refuse == true)
            {
                return this.Content(genre.Name);
            }

            return this.PartialView(Partials.EditGenre, genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGenre(GenreInputViewModel genre)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (genre != null && ModelState.IsValid)
            {
                MusicGenre dbGenre = this.Data.MusicGenres.GetById(genre.Id);
                if (dbGenre == null)
                {
                    return HttpNotFound("Genre not found!");
                }

                dbGenre.Name = genre.Name;

                this.Data.SaveChanges();

                this.service.Clear("genres");

                return this.Content(genre.Name);
            }

            return this.PartialView(Partials.EditGenre, genre);
        }

        public ActionResult DeleteGenre(int id)
        {
            MusicGenre genre = this.Data.MusicGenres.GetById(id);

            if (genre == null)
            {
                return HttpNotFound("Genre not found!");
            }

            this.Data.MusicGenres.Delete(genre);
            this.Data.SaveChanges();

            this.service.Clear("genres");

            return this.Content(String.Empty);
        }
    }
}