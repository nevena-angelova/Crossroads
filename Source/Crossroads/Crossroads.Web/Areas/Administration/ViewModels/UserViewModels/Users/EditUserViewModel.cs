using AutoMapper;
using Crossroads.Models;
using Crossroads.Models.Profile;
using Crossroads.Web.Infrastructure.Mappings;
using Crossroads.Web.ViewModels.ValidationAttributes;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Users
{
    public class EditUserViewModel : IMapFrom<UserProfile>, IHaveCustomMappings
    {
        [Key]
        public int Id { get; set; }

        public string ProfileUserId { get; set; }

        [Display(Name = "Име:")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Името трябва да е от {2} до {1} символа.")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия:")]
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
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public string Image { get; set; }

        [Display(Name = "Снимка:")]
        public HttpPostedFileBase ProfileImage { get; set; }

        [Display(Name = "Интереси:")]
        public int[] InterestsIds { get; set; }

        public List<ProfileInterest> Interests { get; set; }

        [Display(Name = "Музикални стилове:")]
        public int[] MusicGenresIds { get; set; }

        public List<MusicGenre> MusicGenres { get; set; }

        [Display(Name = "За мен:")]
        [StringLength(1500, MinimumLength = 2, ErrorMessage = "Описанието трябва да е от {2} до {1} символа.")]
        public string About { get; set; }

        [Display(Name = "Форум точки:")]
        public int ForumPoints { get; set; }

        [Required(ErrorMessage = "Потребителското име е задължително")]
        [Display(Name = "Потребителско име")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "Потребителското име трябва да е от {2} до {1} символа.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-mail-ът е задължителен")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна")]
        [StringLength(100, ErrorMessage = "Паролата трябва да е от {2} до {1} символа.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<UserProfile, EditUserViewModel>()
                .ForMember(m => m.UserName, opt => opt.MapFrom(p => p.ProfileUser.UserName))
                .ForMember(m => m.Email, opt => opt.MapFrom(p => p.ProfileUser.Email))
                .ForMember(m => m.Password, opt => opt.MapFrom(p => p.ProfileUser.PasswordHash));
        }
    }
}