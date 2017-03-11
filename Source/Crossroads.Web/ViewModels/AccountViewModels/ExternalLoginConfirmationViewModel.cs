using System.ComponentModel.DataAnnotations;

namespace Crossroads.Web.ViewModels.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}