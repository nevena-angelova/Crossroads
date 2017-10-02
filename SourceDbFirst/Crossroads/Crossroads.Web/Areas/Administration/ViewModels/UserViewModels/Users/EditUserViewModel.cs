using AutoMapper;
using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;
using Crossroads.Web.ViewModels.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Users
{
    public class EditUserViewModel : IMapFrom<User>
    {
        [Key]
        public int Id { get; set; }

        [RegularExpression("^[a-zA-Z]([/._-]?[a-zA-Z0-9]+)+$", ErrorMessage = "Потребителското име може да съдържа само малки и главни латински букви, цифри и знаците точка и долна черта и тире. Потребителското име трябва да започва с буква и да завършва с буква или цифра.")]
        [Required(ErrorMessage = "Потребителското име е задължително")]
        [Display(Name = "Потребителско име")]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "Потребителското име трябва да е от {2} до {1} символа.")]
        public string UserName { get; set; }

        [RegularExpression("^[A-Za-z0-9]+[\\._A-Za-z0-9-]+@([A-Za-z0-9]+[-\\.]?[A-Za-z0-9]+)+(\\.[A-Za-z0-9]+[-\\.]?[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$",
    ErrorMessage = "Моля въведете валиден имейл адрес.")]
        [Required(ErrorMessage = "E-mail-ът е задължителен.")]
        [EmailAddress(ErrorMessage = "Невалиден E-mail.")]
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

        [Display(Name = "Снимка:")]
        public HttpPostedFileBase ProfileImage { get; set; }

        [Display(Name = "Интереси:")]
        public int[] InterestsIds { get; set; }

        public List<ProfileInterest> ProfileInterests { get; set; }

        [Display(Name = "Музикални стилове:")]
        public int[] MusicGenresIds { get; set; }

        public List<MusicGenre> MusicGenres { get; set; }

        [Display(Name = "За мен:")]
        [StringLength(1500, MinimumLength = 2, ErrorMessage = "Описанието трябва да е от {2} до {1} символа.")]
        public string About { get; set; }

        [Display(Name = "Форум точки:")]
        public int ForumPoints { get; set; }

        [Display(Name = "Права:")]
        public int[] RolesIds { get; set; }

        public List<Role> Roles { get; set; }
    }
}