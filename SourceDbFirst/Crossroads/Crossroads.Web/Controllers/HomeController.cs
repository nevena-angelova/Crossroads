using Crossroads.Data;
using Crossroads.Web.ViewModels.UsersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using System.Net;
using MvcPaging;
using Crossroads.Web.Infrastructure.Constants;
using System.Linq.Expressions;
using System.Data.Entity;
using Crossroads.Web.Infrastructure.Populators;

namespace Crossroads.Web.Controllers
{
    public class HomeController : BaseController
    {
        private IDropDownListPopulator populator;
        private const int DefaultPageSize = 24;

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
            IList<UserViewModel> users = this.Data.Users.All()
                .OrderByDescending(u => u.DateCreated)
                .Project()
                .To<UserViewModel>()
                .ToList();

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return this.PartialView(Partials.DisplayUsers, users.ToPagedList(currentPageIndex, DefaultPageSize));
        }

        public ActionResult Search(int? page, string searchString, string searchProperty)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            Expression<Func<User, bool>> WhereExpression = p => true;

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                if (searchProperty == "UserName")
                {
                    WhereExpression = u => u.UserName.ToLower().Contains(searchString.ToLower());
                }

                if (searchProperty == "About")
                {
                    WhereExpression = u => u.About.ToLower().Contains(searchString.ToLower());
                }

                if (searchProperty == "Band")
                {
                    WhereExpression = u => u.Bands.ToLower().Contains(searchString.ToLower());
                }
            }

            ViewBag.SearchString = searchString;
            ViewBag.SearchProperty = searchProperty;

            IList<UserViewModel> profiles = this.Data.Users.All()
                .Where(WhereExpression)
                .OrderByDescending(u => u.DateCreated)
                .Project()
                .To<UserViewModel>()
                .ToList();

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return this.PartialView(Partials.SearchUsers, profiles.ToPagedList(currentPageIndex, DefaultPageSize));
        }

        public ActionResult FilterUsers()
        {
            ViewBag.Age = PopulateAge();

            ViewBag.Towns = this.populator.GetTowns();
            ViewBag.AllInterests = this.populator.GetInterests();
            ViewBag.AllMusicGenres = this.populator.GetMusicGenres();

            return PartialView(Partials.Filter);
        }

        [HttpPost]
        public ActionResult FilterUsers(FilterViewModel filter)
        {
            if (filter != null && ModelState.IsValid)
            {
                if (!Request.IsAjaxRequest())
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return this.Content("This action can be invoke only by AJAX call");
                }

                var filteredUsersModel = new CurrentFilterViewModel();
                filteredUsersModel.FirstName = filter.FirstName ?? null;
                filteredUsersModel.LastName = filter.LastName ?? null;
                filteredUsersModel.TownId = filter.TownId ?? null;
                filteredUsersModel.IsMale = filter.IsMale ?? null;
                filteredUsersModel.StartAge = filter.StartAge ?? null;
                filteredUsersModel.EndAge = filter.EndAge ?? null;
                filteredUsersModel.InterestsIds = filter.InterestsIds ?? new int[0];
                filteredUsersModel.MusicGenresIds = filter.MusicGenresIds ?? new int[0];
                filteredUsersModel.IsOnline = filter.IsOnline ?? null;

                int currentPageIndex = filter.Page.HasValue ? filter.Page.Value - 1 : 0;

                filteredUsersModel.Users = this.Data.Users.All()
                    .Where(Predicate(filter))
                    .OrderByDescending(p => p.DateCreated).AsQueryable()
                    .Project()
                    .To<UserViewModel>()
                    .ToPagedList(currentPageIndex, DefaultPageSize);

                return this.PartialView(Partials.FilterUsers, filteredUsersModel);
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

        private static Expression<Func<User, bool>> Predicate(FilterViewModel f)
        {
            return CompareFilter(f);
        }

        // checks wether all the filter properties are null or match the profile ones. Returns true only if all the statments are true.
        private static Expression<Func<User, bool>> CompareFilter(FilterViewModel filter)
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

            return u => (filter.FirstName == null || filter.FirstName == u.FirstName) &&
                        (filter.LastName == null || filter.LastName == u.LastName) &&
                        (filter.IsMale == null || filter.IsMale == u.IsMale) &&
                        (filter.TownId == null || filter.TownId == u.Town.Id) &&
                        ((filter.StartAge == null && filter.EndAge == null) ||
                            ((DbFunctions.DiffYears(u.BirthDate, DateTime.Now) >= filter.StartAge)) &&
                            ((DbFunctions.DiffYears(u.BirthDate, DateTime.Now) <= filter.EndAge))) &&
                        (!filterInterestsIds.Any() || u.ProfileInterests.Any(i => filterInterestsIds.Contains(i.Id))) &&
                        (!filterMusicGenresIds.Any() || u.MusicGenres.Any(i => filterMusicGenresIds.Contains(i.Id))) &&
                        (filter.IsOnline == null ||
                            DbFunctions.DiffMinutes(u.LastActionTime, DateTime.Now) < Constants.MaxMinutesFromAcction);
        }
    }
}