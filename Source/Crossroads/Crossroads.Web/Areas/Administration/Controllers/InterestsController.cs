using Crossroads.Common;
using Crossroads.Data;
using Crossroads.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Crossroads.Web.Areas.Administration.ViewModels.Interests;
using Crossroads.Web.Infrastructure.Constants;
using System.Net;
using Crossroads.Models.Profile;
using System.Linq.Expressions;

namespace Crossroads.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = GlobalConstants.AdminRole)]
    public class InterestsController : BaseController
    {
        public InterestsController(ICrossroadsData data)
            : base(data)
        {
        }

        public ActionResult ListInterests(bool? ordered)
        {
            IList<InterestViewModel> interests = null;

            if (ordered == true)
            {
                interests = this.Data.Interests.All()
                    .OrderBy(i => i.Name)
                    .Project()
                    .To<InterestViewModel>()
                    .ToList();
            }
            else
            {
                interests = this.Data.Interests.All()
                    .Project()
                    .To<InterestViewModel>()
                    .ToList();
            }

            return this.View(interests);
        }

        public ActionResult EditInterest(int id, bool? refuse)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            EditInterestViewModel interest = this.Data.Interests.All()
                .Where(i => i.Id == id)
                .Project()
                .To<EditInterestViewModel>()
                .FirstOrDefault();

            if (refuse == true)
            {
                return this.Content(interest.Name);
            }

            return this.PartialView(Partials.EditInterest, interest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInterest(EditInterestViewModel interest)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (interest != null && ModelState.IsValid)
            {
                ProfileInterest dbInterest = this.Data.Interests.GetById(interest.Id);
                dbInterest.Name = interest.Name;

                this.Data.SaveChanges();

                return this.Content(interest.Name);
            }

            return this.PartialView(Partials.EditInterest, interest);
        }

        public ActionResult AddInterest()
        {
            AddInterestViewModel interest = new AddInterestViewModel();

            return this.PartialView(Partials.AddInterest, interest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddInterest(AddInterestViewModel interest)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (interest != null && ModelState.IsValid)
            {
                ProfileInterest dbInterest = new ProfileInterest { Name = interest.Name };
                this.Data.Interests.Add(dbInterest);
                this.Data.SaveChanges();

                InterestViewModel newInterest = new InterestViewModel
                {
                    Id = dbInterest.Id,
                    Name = interest.Name
                };

                return this.PartialView(Partials.Interest, newInterest);
            }

            return this.PartialView(Partials.AddInterest, interest);
        }

        public ActionResult DeleteInterest(int id)
        {
            ProfileInterest interest = this.Data.Interests.GetById(id);

            if (interest == null)
            {
                return HttpNotFound("Interest not found!");
            }

            this.Data.Interests.Delete(interest);
            this.Data.SaveChanges();

            return this.Content(String.Empty);
        }
    }
}