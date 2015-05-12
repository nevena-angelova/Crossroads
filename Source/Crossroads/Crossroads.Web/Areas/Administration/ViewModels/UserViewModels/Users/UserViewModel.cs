using System;
namespace Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Users
{
    public class UserViewModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime? LastActionTime { get; set; }
    }
}