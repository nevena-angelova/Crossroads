using Crossroads.Models.Profile;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Crossroads.Web.ViewModels.ProfilesViewModels
{
    public class FilterViewModel
    {
        public int? Page { get; set; }

        [Display(Name = "Име:")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия:")]
        public string LastName { get; set; }

        [Display(Name = "Град:")]
        public int? TownId { get; set; }

        [Display(Name = "Пол:")]
        public bool? IsMale { get; set; }

        public int? StartAge { get; set; }

        public int? EndAge { get; set; }

        [Display(Name = "Интереси:")]
        public int[] InterestsIds { get; set; }

        [Display(Name = "Музикални стилове:")]
        public int[] MusicGenresIds { get; set; }

        [Display(Name = "Online:")]
        public bool? IsOnline { get; set; }
    }
}