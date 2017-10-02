using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Web.Infrastructure.Mappings;
using AutoMapper;
using Crossroads.Web.ViewModels.ProfileViewModels.Comments;
using Crossroads.Web.ViewModels.ProfileViewModels.Messages;
using System.Data.Entity;
using Crossroads.Web.Infrastructure.Constants;
using Crossroads.Data;

namespace Crossroads.Web.ViewModels.ProfileViewModels
{
    public class DisplayUserViewModel : IMapFrom<User>, IHaveCustomMappings
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

        public bool IsUserOnline { get; set; }

        public int ForumPoints { get; set; }

        public ICollection<MusicGenre> MusicGenres { get; set; }

        public ICollection<ProfileInterest> ProfileInterests { get; set; }

        public ICollection<CommentViewModel> ProfileComments { get; set; }

        public ICollection<MessageContentViewModel> Messages { get; set; }

        public DateTime DateCreated { get; set; }

        public int UnreadMessages { get; set; }

        public string Skype { get; set; }

        public string Facebook { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, DisplayUserViewModel>()
                .ForMember(m => m.Town, opt => opt.MapFrom(u => u.Town.Name))
                .ForMember(m => m.IsUserOnline, opt =>
                    opt.MapFrom(u => DbFunctions.DiffMinutes(u.LastActionTime, DateTime.Now) < Constants.MaxMinutesFromAcction ? true : false))
                .ForMember(m => m.UnreadMessages, opt => opt.MapFrom(p => p.ProfileMessages.Where(m => m.IsRead == false).Count()))
                .ReverseMap();
        }
    }
}