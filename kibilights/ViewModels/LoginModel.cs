using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KibiLights.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Enter an e-mail")]
        [EmailAddress(ErrorMessage = "Incorrect e-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
