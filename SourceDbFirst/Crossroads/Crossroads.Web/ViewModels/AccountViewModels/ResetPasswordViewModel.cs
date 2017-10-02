using System.ComponentModel.DataAnnotations;

namespace Crossroads.Web.ViewModels.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [RegularExpression("^[A-Za-z0-9]+[\\._A-Za-z0-9-]+@([A-Za-z0-9]+[-\\.]?[A-Za-z0-9]+)+(\\.[A-Za-z0-9]+[-\\.]?[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$",
    ErrorMessage = "Моля въведете валиден имейл адрес.")]
        [Required(ErrorMessage = "E-mail-ът е задължителен.")]
        [EmailAddress(ErrorMessage = "Невалиден E-mail.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}