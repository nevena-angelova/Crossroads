using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;
using System.Collections.Generic;

namespace Crossroads.Web.ViewModels.ForumViewModels
{
    public class TopUserViewModel : IMapFrom<User>
    {
        public string UserName { get; set; }

        public string Image { get; set; }

        public int ForumPoints { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}