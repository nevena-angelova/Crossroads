﻿using Crossroads.Models.Profile;
using Crossroads.Web.Infrastructure.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Interests
{
    public class InterestInputViewModel : IMapFrom<ProfileInterest>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Дължината трябва да е от {2} до {1} символа.")]
        public string Name { get; set; }
    }
}