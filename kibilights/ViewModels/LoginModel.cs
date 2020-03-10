using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KibiLights.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "EmailMissing")]
        [EmailAddress(ErrorMessage = "EmailMissing")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PasswordMissing")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
