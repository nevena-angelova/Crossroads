using System.Collections.Generic;
using System.Web.Mvc;

namespace Crossroads.Web.Infrastructure.Populators
{
    public interface IDropDownListPopulator
    {
        IEnumerable<SelectListItem> GetCategories();

        IEnumerable<SelectListItem> GetTowns();

        IEnumerable<SelectListItem> GetInterests();

        IEnumerable<SelectListItem> GetMusicGenres();

        IEnumerable<SelectListItem> GetRoles();
    }
}
