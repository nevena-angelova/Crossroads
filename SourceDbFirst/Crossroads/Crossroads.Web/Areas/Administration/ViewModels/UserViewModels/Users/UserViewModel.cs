using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;
using System;
using System.Collections.Generic;

namespace Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Users
{
    public class UserViewModel : IMapFrom<User>
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public ICollection<Role> Roles { get; set; }

        public DateTime? LastActionTime { get; set; }
    }
}