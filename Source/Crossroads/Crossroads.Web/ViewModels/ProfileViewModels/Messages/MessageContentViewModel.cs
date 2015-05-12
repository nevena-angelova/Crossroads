using AutoMapper;
using Crossroads.Models.Profile;
using Crossroads.Web.Infrastructure.Mappings;
using System;
using System.Linq;

namespace Crossroads.Web.ViewModels.ProfileViewModels.Messages
{
    public class MessageContentViewModel : IMapFrom<ProfileMessage>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string AuthorName { get; set; }

        public DateTime DateCreated { get; set; }

        public string Image { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProfileMessage, MessageContentViewModel>()
                .ForMember(c => c.AuthorName, opt => opt.MapFrom(c => c.AuthorProfile.ProfileUser.UserName))
                .ForMember(c => c.Image, opt => opt.MapFrom(c => c.AuthorProfile.Image))
                .ReverseMap();
        }
    }
}