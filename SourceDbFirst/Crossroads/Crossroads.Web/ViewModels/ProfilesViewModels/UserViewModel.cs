using AutoMapper;
using Crossroads.Web.Infrastructure.Mappings;
using Crossroads.Web.Infrastructure.Constants;
using System;
using System.Data.Entity;
using Crossroads.Data;

namespace Crossroads.Web.ViewModels.UsersViewModels
{
    public class UserViewModel : IMapFrom<User>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Image { get; set; }

        public bool IsUserOnline { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, UserViewModel>()
                .ForMember(m => m.IsUserOnline, opt =>
                    opt.MapFrom(u => DbFunctions.DiffMinutes(u.LastActionTime, DateTime.Now) < Constants.MaxMinutesFromAcction ? true : false));
        }
    }
}