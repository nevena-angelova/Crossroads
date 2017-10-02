﻿using AutoMapper;
using Crossroads.Data;
using Crossroads.Web.Infrastructure.Constants;
using Crossroads.Web.Infrastructure.Mappings;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Users
{
    public class DisplayUserViewModel : IMapFrom<User>, IHaveCustomMappings
    {
        public DisplayUserViewModel()
        {
            this.Interests = new List<ProfileInterest>();
        }

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

        public ICollection<Role> Roles { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, DisplayUserViewModel>()
                .ForMember(m => m.Town, opt => opt.MapFrom(u => u.Town.Name))
                .ForMember(m => m.UserName, opt => opt.MapFrom(u => u.UserName))
                .ForMember(m => m.Email, opt => opt.MapFrom(u => u.Email))
                .ForMember(m => m.IsUserOnline, opt =>
                    opt.MapFrom(u => DbFunctions.DiffMinutes(u.LastActionTime, DateTime.Now) < Constants.MaxMinutesFromAcction ? true : false))
                .ForMember(m => m.LastActionTime, opt => opt.MapFrom(u => u.LastActionTime));
        }
    }
}