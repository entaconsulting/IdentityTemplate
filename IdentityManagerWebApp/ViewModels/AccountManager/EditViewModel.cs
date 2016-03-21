using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using IdentityManagerWebApp.Dtos;
using IdentityManagerWebApp.Infrastructure.Mapper;

namespace IdentityManagerWebApp.ViewModels.AccountManager
{
    public class EditViewModel : IMapFrom<UserDetailsDto>
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}