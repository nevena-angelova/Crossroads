using AutoMapper;
using Crossroads.Models;
using Crossroads.Models.Profile;
using Crossroads.Web.Infrastructure.Constants;
using Crossroads.Web.Infrastructure.Mappings;
using Crossroads.Web.ViewModels.ValidationAttributes;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Users
{
    public class DisplayUserViewModel : IMapFrom<UserProfile>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Town { get; set; }

        public string Bands { get; set; }

        public bool? IsMale { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Image { get; set; }

        public bool IsUserOnline { get; set; }

        public ICollection<MusicGenre> MusicGenres { get; set; }

        public ICollection<ProfileInterest> Interests { get; set; }

        public string About { get; set; }

        public DateTime DateCreated { get; set; }

        public int ForumPoints { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime? LastActionTime { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UserProfile, DisplayUserViewModel>()
                .ForMember(m => m.Town, opt => opt.MapFrom(p => p.Town.Name))
                .ForMember(m => m.UserName, opt => opt.MapFrom(p => p.ProfileUser.UserName))
                .ForMember(m => m.Email, opt => opt.MapFrom(p => p.ProfileUser.Email))
                .ForMember(m => m.IsUserOnline, opt =>
                    opt.MapFrom(p => DbFunctions.DiffMinutes(p.ProfileUser.LastActionTime, DateTime.Now) < Constants.MaxMinutesFromAcction ? true : false))
                .ForMember(m => m.LastActionTime, opt => opt.MapFrom(p => p.ProfileUser.LastActionTime));
        }
    }
}