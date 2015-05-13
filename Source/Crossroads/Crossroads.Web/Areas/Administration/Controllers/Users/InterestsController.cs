using Crossroads.Common;
using Crossroads.Data;
using Crossroads.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Interests;
using Crossroads.Web.Infrastructure.Constants;
using System.Net;
using Crossroads.Models.Profile;
using System.Linq.Expressions;
using AutoMapper;
using Crossroads.Web.Infrastructure.Caching;

namespace Crossroads.Web.Areas.Administration.Controllers.Users
{
    [Authorize(Roles = GlobalConstants.AdminRole)]
    public class InterestsController : BaseController
    {
        private readonly ICacheService service;

        public InterestsController(ICrossroadsData data, ICacheService service)
            : base(data)
        {
            this.service = service;
        }

        public ActionResult ListInterests(bool? ordered)
        {
            IList<InterestOutputViewModel> interests = null;

            if (ordered == true)
            {
                interests = this.Data.Interests.All()
                    .OrderBy(i => i.Name)
                    .Project()
                    .To<InterestOutputViewModel>()
                    .ToList();
            }
            else
            {
                interests = this.Data.Interests.All()
                    .Project()
                    .To<InterestOutputViewModel>()
                    .ToList();
            }

            return this.View(Views.ListInterests, interests);
        }

        public ActionResult AddInterest()
        {
            return this.PartialView(Partials.AddInterest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddInterest(InterestInputViewModel interest)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (interest != null && ModelState.IsValid)
            {
                ProfileInterest dbInterest = Mapper.Map<ProfileInterest>(interest);
                this.Data.Interests.Add(dbInterest);
                this.Data.SaveChanges();

                this.service.Clear("interests");

                InterestOutputViewModel newInterest = Mapper.Map<InterestOutputViewModel>(dbInterest);

                return this.PartialView(Partials.Interest, newInterest);
            }

            return this.PartialView(Partials.AddInterest);
        }

        public ActionResult EditInterest(int id, bool? refuse)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            ProfileInterest dbInterest = this.Data.Interests.GetById(id);
            if (dbInterest == null)
            {
                return HttpNotFound("Interest not found!");
            }

            InterestInputViewModel interest = Mapper.Map<InterestInputViewModel>(dbInterest);

            if (refuse == true)
            {
                return this.Content(interest.Name);
            }

            return this.PartialView(Partials.EditInterest, interest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInterest(InterestInputViewModel interest)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (interest != null && ModelState.IsValid)
            {
                ProfileInterest dbInterest = this.Data.Interests.GetById(interest.Id);
                if (dbInterest == null)
                {
                    return HttpNotFound("Interest not found!");
                }

                dbInterest.Name = interest.Name;

                this.Data.SaveChanges();

                this.service.Clear("interests");

                return this.Content(interest.Name);
            }

            return this.PartialView(Partials.EditInterest, interest);
        }

        public ActionResult DeleteInterest(int id)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            ProfileInterest interest = this.Data.Interests.GetById(id);

            if (interest == null)
            {
                return HttpNotFound("Interest not found!");
            }

            this.Data.Interests.Delete(interest);
            this.Data.SaveChanges();

            this.service.Clear("interests");

            return this.Content(String.Empty);
        }
    }
}