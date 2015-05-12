using AutoMapper;
using Crossroads.Models.UserProfile;
using Crossroads.Web.Infrastructure.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crossroads.Web.ViewModels.ProfileViewModels
{
    public class ProfileCommentViewModel: IMapFrom<ProfileComment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string AutorName { get; set; }

        public string Content { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProfileComment, ProfileCommentViewModel>()
                .ForMember(m => m.AutorName, opt => opt.MapFrom(c => c.Profile.ProfileUser.UserName))
                .ReverseMap();
        }
    }
}