using MvcPaging;
using System;

namespace Crossroads.Web.ViewModels.UsersViewModels
{
    public class CurrentFilterViewModel
    {
        public IPagedList<UserViewModel> Users { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? TownId { get; set; }

        public bool? IsMale { get; set; }

        public int? StartAge { get; set; }

        public int? EndAge { get; set; }

        public int[] InterestsIds { get; set; }

        public int[] MusicGenresIds { get; set; }

        public bool? IsOnline { get; set; }
    }
}