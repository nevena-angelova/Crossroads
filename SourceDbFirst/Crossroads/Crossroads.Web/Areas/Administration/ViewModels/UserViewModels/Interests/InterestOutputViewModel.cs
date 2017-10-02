using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;

namespace Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Interests
{
    public class InterestOutputViewModel : IMapFrom<ProfileInterest>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}