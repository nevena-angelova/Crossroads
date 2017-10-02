using AutoMapper;
using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;
using System;

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
                .ForMember(c => c.AuthorName, opt => opt.MapFrom(c => c.User1.UserName))
                .ForMember(c => c.Image, opt => opt.MapFrom(c => c.User1.Image))
                .ReverseMap();
        }
    }
}