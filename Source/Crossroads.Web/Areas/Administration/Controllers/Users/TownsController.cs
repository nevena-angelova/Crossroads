using Crossroads.Common;
using Crossroads.Data;
using Crossroads.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Towns;
using Crossroads.Web.Infrastructure.Constants;
using System.Net;
using Crossroads.Models.Profile;
using System.Linq.Expressions;
using AutoMapper;
using Crossroads.Web.Infrastructure.Caching;

namespace Crossroads.Web.Areas.Administration.Controllers.Users
{
    [Authorize(Roles = GlobalConstants.AdminRole)]
    public class TownsController : BaseController
    {
        private readonly ICacheService service;

        public TownsController(ICrossroadsData data, ICacheService service)
            : base(data)
        {
            this.service = service;
        }

        public ActionResult ListTowns(bool? ordered)
        {
            IList<TownOutputViewModel> towns = null;

            if (ordered == true)
            {
                towns = this.Data.Towns.All()
                    .OrderBy(i => i.Name)
                    .Project()
                    .To<TownOutputViewModel>()
                    .ToList();
            }
            else
            {
                towns = this.Data.Towns.All()
                    .Project()
                    .To<TownOutputViewModel>()
                    .ToList();
            }

            return this.View(Views.ListTowns, towns);
        }

        public ActionResult AddTown()
        {
            return this.PartialView(Partials.AddTown);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTown(TownInputViewModel town)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (town != null && ModelState.IsValid)
            {
                Town dbTown = Mapper.Map<Town>(town);
                this.Data.Towns.Add(dbTown);
                this.Data.SaveChanges();

                this.service.Clear("towns");

                TownOutputViewModel newTown = Mapper.Map<TownOutputViewModel>(dbTown);

                return this.PartialView(Partials.Town, newTown);
            }

            return this.PartialView(Partials.AddTown);
        }

        public ActionResult EditTown(int id, bool? refuse)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            Town dbTown = this.Data.Towns.GetById(id);
            if (dbTown == null)
            {
                return HttpNotFound("Town not found!");
            }

            TownInputViewModel town = Mapper.Map<TownInputViewModel>(dbTown);

            if (refuse == true)
            {
                return this.Content(town.Name);
            }

            return this.PartialView(Partials.EditTown, town);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTown(TownInputViewModel town)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (town != null && ModelState.IsValid)
            {
                Town dbTown = this.Data.Towns.GetById(town.Id);
                dbTown.Name = town.Name;

                this.Data.SaveChanges();

                this.service.Clear("towns");

                return this.Content(town.Name);
            }

            return this.PartialView(Partials.EditTown, town);
        }

        public ActionResult DeleteTown(int id)
        {
            Town town = this.Data.Towns.GetById(id);

            if (town == null)
            {
                return HttpNotFound("Town not found!");
            }

            this.Data.Towns.Delete(town);
            this.Data.SaveChanges();

            this.service.Clear("towns");

            return this.Content(String.Empty);
        }
    }
}