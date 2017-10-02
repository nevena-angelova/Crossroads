using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;

namespace Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Roles
{
    public class RoleOutputViewModel : IMapFrom<Role>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}