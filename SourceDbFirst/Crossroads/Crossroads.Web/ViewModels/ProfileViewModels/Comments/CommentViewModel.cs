using AutoMapper;
using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;
using System;

namespace Crossroads.Web.ViewModels.ProfileViewModels.Comments
{
    public class CommentViewModel : IMapFrom<ProfileComment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string AuthorName { get; set; }

        public string UserName { get; set; }

        public DateTime DateCreated { get; set; }

        public string Image { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProfileComment, CommentViewModel>()
                .ForMember(c => c.AuthorName, opt => opt.MapFrom(c => c.User1.UserName))
                .ForMember(c => c.UserName, opt => opt.MapFrom(c => c.User.UserName))
                .ForMember(c => c.Image, opt => opt.MapFrom(c => c.User1.Image))
                .ReverseMap();
        }
    }
}