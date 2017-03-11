using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Models.Profile;
using Crossroads.Web.Infrastructure.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Web.Security;
using Crossroads.Web.ViewModels.ProfileViewModels.Comments;
using System.ComponentModel.DataAnnotations;
using Crossroads.Web.ViewModels.ProfileViewModels.Messages;
using System.Data.Entity;
using Crossroads.Web.Infrastructure.Constants;

namespace Crossroads.Web.ViewModels.ProfileViewModels
{
    public class DisplayProfileViewModel : IMapFrom<UserProfile>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Town { get; set; }

        public string Bands { get; set; }

        public bool? IsMale { get; set; }

        public DateTime? BirthDate { get; set; }

        public string About { get; set; }

        public string Image { get; set; }

        public int ForumPoints { get; set; }

        public ICollection<MusicGenre> MusicGenres { get; set; }

        public ICollection<ProfileInterest> Interests { get; set; }

        public ICollection<CommentViewModel> ProfileComments { get; set; }

        public ICollection<MessageContentViewModel> Messages { get; set; }

        public DateTime DateCreated { get; set; }

        public int UnreadMessages { get; set; }

        public string Skype { get; set; }

        public string Facebook { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UserProfile, DisplayProfileViewModel>()
                .ForMember(m => m.UserName, opt => opt.MapFrom(p => p.ProfileUser.UserName))
                .ForMember(m => m.Email, opt => opt.MapFrom(p => p.ProfileUser.Email))
                .ForMember(m => m.Town, opt => opt.MapFrom(p => p.Town.Name))
                .ForMember(m => m.UnreadMessages, opt => opt.MapFrom(p => p.Messages.Where(m => m.IsRead == false).Count()))
                .ReverseMap();
        }
    }
}