using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.ViewModels
{
    public class UserViewModel
    {

        [Required(ErrorMessage ="Kullanıcı adınızı boş geçemezsiniz.")]
        [Display(Name ="Kullanıcı Adı")]
        public string UserName { get; set; }

        
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email adresinizi boş geçemezsiniz.")]
        [Display(Name = "Email Adresi")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanını boş geçemezsiniz.")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
