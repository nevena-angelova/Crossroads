using AutoMapper;
using Crossroads.Models.Profile;
using Crossroads.Web.Infrastructure.Mappings;
using Crossroads.Web.Infrastructure.Constants;
using System;
using System.Linq;
using System.Data.Entity;

namespace Crossroads.Web.ViewModels.ProfilesViewModels
{
    public class ProfileViewModel : IMapFrom<UserProfile>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Image { get; set; }

        public bool IsUserOnline { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UserProfile, ProfileViewModel>()
                .ForMember(m => m.UserName, opt => opt.MapFrom(p => p.ProfileUser.UserName))
                .ForMember(m => m.IsUserOnline, opt =>
                    opt.MapFrom(p => DbFunctions.DiffMinutes(p.ProfileUser.LastActionTime, DateTime.Now) < Constants.MaxMinutesFromAcction ? true : false));
        }
    }
}