using Crossroads.Models.Profile;
using Crossroads.Web.Infrastructure.Mappings;

namespace Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Towns
{
    public class TownOutputViewModel : IMapFrom<Town>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}