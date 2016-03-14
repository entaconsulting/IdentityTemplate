using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentityManagerWebApp.ViewModels.AccountManager
{
    public class CreateSuccessViewModel
    {
        [Display(Name = "Usuario creado")]
        public string CreatedUserName { get; set; }

        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }
}