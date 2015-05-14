using System;
using System.Web;
using Crossroads.Web.Infrastructure.Mappings;
using AutoMapper;
using Crossroads.Models.Profile;
using System.ComponentModel.DataAnnotations;
using Crossroads.Web.ViewModels.ValidationAttributes;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace Crossroads.Web.ViewModels.ProfileViewModels
{
    public class EditProfileViewModel : IMapFrom<UserProfile>, IHaveCustomMappings
    {
        // IMapFrom<Profile> - automatically the id is mapped because of it also all other 
        // properties that match
        public int Id { get; set; }

        public string UserName { get; set; }

        [RegularExpression("^$|^[а-яА-Я]{1,}[-]?[а-яА-Я]{1,}$", ErrorMessage = "Името може да съдържа само букви от българската азбука и знака тире. Името трябва да започва и да завършва с буква.")]
        [Display(Name = "Име на български:")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Името трябва да е от {2} до {1} символа.")]
        public string FirstName { get; set; }

        [RegularExpression("^$|^[а-яА-Я]{1,}[-]?[а-яА-Я]{1,}$", ErrorMessage = "Фамилията може да съдържа само букви от българската азбука и знака тире. Фамилията трябва да започва и да завършва с буква.")]
        [Display(Name = "Фамилия на български:")]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "Фамилията трябва да е от {2} до {1} символа.")]
        public string LastName { get; set; }

        [Display(Name = "Град:")]
        public int? TownId { get; set; }

        [Display(Name = "Любими групи:")]
        [StringLength(600, ErrorMessage = "Дължината на изброените групи трябва да е до {1} символа.")]
        public string Bands { get; set; }

        [Display(Name = "Пол:")]
        public bool? IsMale { get; set; }

        [Display(Name = "Рожденна дата:")]
        [DateRange("01/01/1910", "01/01/2015")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        public string Image { get; set; }

        public HttpPostedFileBase ProfileImage { get; set; }

        [Display(Name = "Интереси:")]
        public int[] InterestsIds { get; set; }

        public List<ProfileInterest> Interests { get; set; }

        [Display(Name = "Музикални стилове:")]
        public int[] MusicGenresIds { get; set; }

        public List<MusicGenre> MusicGenres { get; set; }

        [Display(Name = "За мен:")]
        [StringLength(1500, ErrorMessage = "Описанието трябва да е до {1} символа.")]
        public string About { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UserProfile, EditProfileViewModel>()
                .ForMember(m => m.UserName, opt => opt.MapFrom(p => p.ProfileUser.UserName));
        }
    }
}