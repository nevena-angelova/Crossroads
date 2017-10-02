using System;
using System.Web;
using Crossroads.Web.Infrastructure.Mappings;
using System.ComponentModel.DataAnnotations;
using Crossroads.Web.ViewModels.ValidationAttributes;
using System.Collections.Generic;
using Crossroads.Data;
using AutoMapper;

namespace Crossroads.Web.ViewModels.ProfileViewModels
{
    public class EditUserViewModel : IMapFrom<User>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        [RegularExpression("^[A-Za-z0-9]+[\\._A-Za-z0-9-]+@([A-Za-z0-9]+[-\\.]?[A-Za-z0-9]+)+(\\.[A-Za-z0-9]+[-\\.]?[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$",
    ErrorMessage = "Моля въведете валиден имейл адрес.")]
        [Required(ErrorMessage = "E-mail-ът е задължителен")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Паролата трябва да е от {2} до {1} символа.")]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потрвърди паролата")]
        [Compare("Password", ErrorMessage = "Паролата и потвърдената парола не съвпадат.")]
        public string ConfirmPassword { get; set; }

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
        [DateRange("01/01/1910")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public string Image { get; set; }

        public HttpPostedFileBase ProfileImage { get; set; }

        [Display(Name = "Интереси:")]
        public int[] InterestsIds { get; set; }

        public List<ProfileInterest> ProfileInterests { get; set; }

        [Display(Name = "Музикални стилове:")]
        public int[] MusicGenresIds { get; set; }

        public List<MusicGenre> MusicGenres { get; set; }

        [Display(Name = "За мен:")]
        [StringLength(1500, ErrorMessage = "Описанието трябва да е до {1} символа.")]
        public string About { get; set; }

        [Display(Name = "Skype потребител:")]
        [RegularExpression("^$|[a-zA-Z][a-zA-Z0-9_\\-\\,\\.]{5,31}", ErrorMessage = "Skype името може да съдържа само латински букви, цифри и знацитe долна черта, тире, запетая и точка. Минимална дължина - 6 символа. Максимална дължина - 32 символа.")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Skype името трябва да е от {2} до {1} символа.")]
        public string Skype { get; set; }

        [Display(Name = "Facebook профил:")]
        [RegularExpression("(?i)^$|^((http|https)?(\\://)?(www\\.)?facebook.com/)(.)+$", ErrorMessage = "Въведете адрес, който се намира домейна \"facebook.com\"")]
        [StringLength(90, ErrorMessage = "Facebook адреса трябва да е до {1} символа.")]
        public string Facebook { get; set; }

        [Display(Name = "Email при ново съобщение:")]
        public bool EmailMsgNotify { get; set; }

        // It is possible to use the automapper in both directions - ReverseMap().
        // If some property needs to be updated manually it has to be disconected from the mapping with Ignore():
        // ForMember(x => x.SomeProp, opt => opt.Ignore())
        // But if this needs to happen in only one direction (edit/update), ForMember is used after ReverseMap().
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, EditUserViewModel>()
                .ForMember(m => m.EmailMsgNotify, opt => opt.MapFrom(u => u.EmailMsgNotify == null ? false : u.EmailMsgNotify.Value));
        }
    }
}