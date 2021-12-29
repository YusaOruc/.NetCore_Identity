using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.ViewModels
{
    public class PasswordResetViewModel
    {
        [Required(ErrorMessage = "Email adresinizi boş geçemezsiniz.")]
        [Display(Name = "Email adresinizi")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Yeni şifrenizi boş geçemezsiniz.")]
        [Display(Name = "Yeni şifreniz")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakter olmalıdır.")]
        public string Password { get; set; }
    }
}
