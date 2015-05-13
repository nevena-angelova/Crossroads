using Crossroads.Data;
using Crossroads.Web.ViewModels.ProfilesViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Net;
using Crossroads.Models.Profile;
using MvcPaging;
using Crossroads.Web.Infrastructure.Constants;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Text.RegularExpressions;
using Crossroads.Web.Infrastructure.Populators;

namespace Crossroads.Web.Controllers
{
    public class HomeController : BaseController
    {
        private IDropDownListPopulator populator;
        private const int DefaultPageSize = 21;

        public HomeController(ICrossroadsData data, IDropDownListPopulator populator)
            : base(data)
        {
            this.populator = populator;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        } 

        //[OutputCache(Duration = 60 * 30)]
        public ActionResult Display(int? page)
        {
            IList<ProfileViewModel> profiles = this.Data.Profiles.All()
                .OrderByDescending(p => p.DateCreated)
                .Project()
                .To<ProfileViewModel>()
                .ToList();

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return this.PartialView(Partials.DisplayProfiles, profiles.ToPagedList(currentPageIndex, DefaultPageSize));
        }

        public ActionResult Search(int? page, string searchString, string searchProperty)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            Expression<Func<UserProfile, bool>> WhereExpression = p => true;

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                if (searchProperty == "UserName")
                {
                    WhereExpression = p => p.ProfileUser.UserName.ToLower().Contains(searchString.ToLower());
                }

                if (searchProperty == "About")
                {
                    WhereExpression = p => p.About.ToLower().Contains(searchString.ToLower());
                }

                if (searchProperty == "Band")
                {
                    WhereExpression = p => p.Bands.ToLower().Contains(searchString.ToLower());
                }
            }

            ViewBag.SearchString = searchString;
            ViewBag.SearchProperty = searchProperty;

            IList<ProfileViewModel> profiles = this.Data.Profiles.All()
                .Where(WhereExpression)
                .OrderByDescending(p => p.DateCreated)
                .Project()
                .To<ProfileViewModel>()
                .ToList();

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return this.PartialView(Partials.SearchProfiles, profiles.ToPagedList(currentPageIndex, DefaultPageSize));
        }

        public ActionResult FilterProfiles()
        {
            ViewBag.Age = PopulateAge();

            ViewBag.Towns = this.populator.GetTowns();
            ViewBag.AllInterests = this.populator.GetInterests();
            ViewBag.AllMusicGenres = this.populator.GetMusicGenres();

            return PartialView(Partials.Filter);
        }

        [HttpPost]
        public ActionResult FilterProfiles(FilterViewModel filter)
        {
            if (filter != null && ModelState.IsValid)
            {
                if (!Request.IsAjaxRequest())
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return this.Content("This action can be invoke only by AJAX call");
                }

                var filteredProfilesModel = new CurrentFilterViewModel();
                filteredProfilesModel.FirstName = filter.FirstName ?? null;
                filteredProfilesModel.LastName = filter.LastName ?? null;
                filteredProfilesModel.TownId = filter.TownId ?? null;
                filteredProfilesModel.IsMale = filter.IsMale ?? null;
                filteredProfilesModel.StartAge = filter.StartAge ?? null;
                filteredProfilesModel.EndAge = filter.EndAge ?? null;
                filteredProfilesModel.InterestsIds = filter.InterestsIds ?? new int[0];
                filteredProfilesModel.MusicGenresIds = filter.MusicGenresIds ?? new int[0];
                filteredProfilesModel.IsOnline = filter.IsOnline ?? null;

                int currentPageIndex = filter.Page.HasValue ? filter.Page.Value - 1 : 0;

                filteredProfilesModel.Profiles = this.Data.Profiles.All()
                    .Where(Predicate(filter))
                    .OrderByDescending(p => p.DateCreated).AsQueryable()
                    .Project()
                    .To<ProfileViewModel>()
                    .ToPagedList(currentPageIndex, DefaultPageSize);

                return this.PartialView(Partials.FilterProfiles, filteredProfilesModel);
            }

            ViewBag.AllInterests = this.populator.GetInterests();
            ViewBag.AllMusicGenres = this.populator.GetMusicGenres();
            ViewBag.Towns = this.populator.GetTowns();

            return PartialView(Partials.Filter);
        }

        private List<SelectListItem> PopulateAge()
        {
            List<SelectListItem> numbers = new List<SelectListItem>();

            for (int i = 1; i <= 100; i++)
            {
                numbers.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }

            return numbers;
        }

        private static Expression<Func<UserProfile, bool>> Predicate(FilterViewModel f)
        {
            return CompareFilter(f);
        }

        // checks wether all the filter properties are null or match the profile ones. Returns true only if all the statments are true.
        private static Expression<Func<UserProfile, bool>> CompareFilter(FilterViewModel filter)
        {
            //convert the int[] to List because:
            //"Cannot compare elements of type 'System.Int32[]'. Only primitive types, enumeration types and entity types are supported."
            List<int> filterMusicGenresIds = new List<int>();
            List<int> filterInterestsIds = new List<int>();

            if (filter.MusicGenresIds != null)
            {
                filterMusicGenresIds = filter.MusicGenresIds.ToList();
            }

            if (filter.InterestsIds != null)
            {
                filterInterestsIds = filter.InterestsIds.ToList();
            }

            return p => (filter.FirstName == null || filter.FirstName == p.FirstName) &&
                        (filter.LastName == null || filter.LastName == p.LastName) &&
                        (filter.IsMale == null || filter.IsMale == p.IsMale) &&
                        (filter.TownId == null || filter.TownId == p.TownId) &&
                        ((filter.StartAge == null && filter.EndAge == null) ||
                            ((DbFunctions.DiffYears(p.BirthDate, DateTime.Now) >= filter.StartAge)) &&
                            ((DbFunctions.DiffYears(p.BirthDate, DateTime.Now) <= filter.EndAge))) &&
                        (!filterInterestsIds.Any() || p.Interests.Any(i => filterInterestsIds.Contains(i.Id))) &&
                        (!filterMusicGenresIds.Any() || p.MusicGenres.Any(i => filterMusicGenresIds.Contains(i.Id))) &&
                        (filter.IsOnline == null ||
                            DbFunctions.DiffMinutes(p.ProfileUser.LastActionTime, DateTime.Now) < Constants.MaxMinutesFromAcction);
        }
    }
}