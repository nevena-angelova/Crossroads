using AutoMapper;
using Crossroads.Models.Profile;
using Crossroads.Web.Infrastructure.Mappings;

namespace Crossroads.Web.ViewModels.ForumViewModels
{
    public class TopUserViewModel : IMapFrom<UserProfile>, IHaveCustomMappings
    {
        public string UserName { get; set; }

        public string Image { get; set; }

        public int ForumPoints { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UserProfile, TopUserViewModel>()
                .ForMember(m => m.UserName, opt => opt.MapFrom(p => p.ProfileUser.UserName));
        }

    }
}