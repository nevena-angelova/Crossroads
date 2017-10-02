using Crossroads.Data;
using Crossroads.Web.Infrastructure.Caching;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Crossroads.Web.Infrastructure.Populators
{
    public class DropDownListPopulator : IDropDownListPopulator
    {
        private ICrossroadsData data;
        private ICacheService cache;

        public DropDownListPopulator(ICrossroadsData data, ICacheService cache)
        {
            this.data = data;
            this.cache = cache;
        }

        public IEnumerable<SelectListItem> GetCategories()
        {
            var categories = this.cache.Get<IEnumerable<SelectListItem>>("categories",
                () =>
                {
                    return this.data.Categories
                       .All()
                       .Select(c => new SelectListItem
                       {
                           Value = c.Id.ToString(),
                           Text = c.Name
                       })
                       .ToList();
                });

            return categories;
        }

        public IEnumerable<SelectListItem> GetTowns()
        {
            var towns = this.cache.Get<IEnumerable<SelectListItem>>("towns",
                () =>
                {
                    return this.data.Towns
                       .All()
                       .OrderBy(t => t.Name)
                       .Select(c => new SelectListItem
                       {
                           Value = c.Id.ToString(),
                           Text = c.Name
                       })
                       .ToList();
                });

            return towns;
        }

        public IEnumerable<SelectListItem> GetInterests()
        {
            var interests = this.cache.Get<IEnumerable<SelectListItem>>("interests",
                () =>
                {
                    return this.data.Interests
                       .All()
                       .Select(c => new SelectListItem
                       {
                           Value = c.Id.ToString(),
                           Text = c.Name
                       })
                       .ToList();
                });

            return interests;
        }

        public IEnumerable<SelectListItem> GetMusicGenres()
        {
            var genres = this.cache.Get<IEnumerable<SelectListItem>>("genres",
                () =>
                {
                    return this.data.MusicGenres
                       .All()
                       .Select(c => new SelectListItem
                       {
                           Value = c.Id.ToString(),
                           Text = c.Name
                       })
                       .ToList();
                });

            return genres;
        }

        public IEnumerable<SelectListItem> GetRoles()
        {
            var roles = this.cache.Get<IEnumerable<SelectListItem>>("roles",
                () =>
                {
                    return this.data.Roles
                       .All()
                       .Select(c => new SelectListItem
                       {
                           Value = c.Id.ToString(),
                           Text = c.Name
                       })
                       .ToList();
                });

            return roles;
        }
    }
}