using AutoMapper;
using Crossroads.Models.Profile;
using Crossroads.Web.Infrastructure.Mappings;

namespace Crossroads.Web.ViewModels.ProfileViewModels.Messages
{
    public class MessageViewModel : IMapFrom<ProfileMessage>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string Title { get; set; }

        public bool IsRead { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProfileMessage, MessageViewModel>()
                .ForMember(c => c.AuthorId, opt => opt.MapFrom(c => c.AuthorProfile.Id))
                .ForMember(c => c.AuthorName, opt => opt.MapFrom(c => c.AuthorProfile.ProfileUser.UserName))
                .ReverseMap();
        }

    }
}