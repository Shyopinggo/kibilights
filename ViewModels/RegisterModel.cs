using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KibiLights.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "EmailMissing")]
        [EmailAddress(ErrorMessage = "EmailMissing")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PasswordMissing")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "ConfirmMissing")]
        [Display(Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
