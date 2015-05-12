using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crossroads.Web.ViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Потребителското име е задължително")]
        [Display(Name = "Потребителско име:")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна")]
        [DataType(DataType.Password)]
        [Display(Name = "Парола:")]
        public string Password { get; set; }

        [Display(Name = "Запомни ме?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [RegularExpression("^[a-zA-Z]([/._]?[a-zA-Z0-9]+)+$", ErrorMessage = "Потребителското име може да съдържа само малки и главни латински букви, цифри и знаците точка и долна черта. Потребителското име трябва да започва с буква и да завършва с буква или цифра.")]
        [Required(ErrorMessage = "Потребителското име е задължително")]
        [Display(Name = "Потребителско име")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "Потребителското име трябва да е от {2} до {1} символа.")]
        public string UserName { get; set; }

        [RegularExpression("^[A-Za-z0-9]+[\\._A-Za-z0-9-]+@([A-Za-z0-9]+[-\\.]?[A-Za-z0-9]+)+(\\.[A-Za-z0-9]+[-\\.]?[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$",
            ErrorMessage = "Моля въведете валиден имейл адрес.")]
        [Required(ErrorMessage = "E-mail-ът е задължителен")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна")]
        [StringLength(100, ErrorMessage = "Паролата трябва да е от {2} до {1} символа.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потрвърди паролата")]
        [Compare("Password", ErrorMessage = "Паролата и потвърдената парола не съвпадат.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "E-mail-ът е задължителен")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "E-mail-ът е задължителен")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
