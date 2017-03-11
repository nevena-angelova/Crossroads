using System.ComponentModel.DataAnnotations;

namespace Crossroads.Web.ViewModels.AccountViewModels
{
    public class RegisterViewModel
    {
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

        [Required(ErrorMessage = "Паролата е задължителна")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Паролата трябва да е от {2} до {1} символа.")]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потрвърди паролата")]
        [Compare("Password", ErrorMessage = "Паролата и потвърдената парола не съвпадат.")]
        public string ConfirmPassword { get; set; }
    }
}