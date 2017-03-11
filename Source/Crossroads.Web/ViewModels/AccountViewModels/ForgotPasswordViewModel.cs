using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Crossroads.Web.ViewModels.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "E-mail-ът е задължителен")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}