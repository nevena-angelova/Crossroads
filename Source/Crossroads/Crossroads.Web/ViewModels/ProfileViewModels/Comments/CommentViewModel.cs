using AutoMapper;
using Crossroads.Models.Profile;
using Crossroads.Web.Infrastructure.Mappings;
using System;
using System.Linq;

namespace Crossroads.Web.ViewModels.ProfileViewModels.Comments
{
    public class CommentViewModel : IMapFrom<ProfileComment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string AuthorName { get; set; }

        public string ProfileUserName { get; set; }

        public DateTime DateCreated { get; set; }

        public string Image { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProfileComment, CommentViewModel>()
                .ForMember(c => c.AuthorName, opt => opt.MapFrom(c => c.AuthorProfile.ProfileUser.UserName))
                .ForMember(c => c.ProfileUserName, opt => opt.MapFrom(c => c.Profile.ProfileUser.UserName))
                .ForMember(c => c.Image, opt => opt.MapFrom(c => c.AuthorProfile.Image))
                .ReverseMap();
        }
    }
}