using System.ComponentModel.DataAnnotations;

namespace Crossroads.Web.ViewModels.AccountViewModels
{
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
}