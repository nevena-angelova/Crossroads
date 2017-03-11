using AutoMapper;
using Crossroads.Models.Profile;
using Crossroads.Web.Infrastructure.Mappings;

namespace Crossroads.Web.ViewModels.ProfilesViewModels
{
    public class ProfileViewModel : IMapFrom<UserProfile>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Image { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UserProfile, ProfileViewModel>()
                .ForMember(m => m.UserName, opt => opt.MapFrom(p => p.ProfileUser.UserName));
        }
    }
}