using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentityManagerWebApp.ViewModels.AccountManager
{
    public class CreateViewModel
    {
        [Required(ErrorMessage = "Requerido")]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}