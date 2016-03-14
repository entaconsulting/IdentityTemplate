using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IdentityManagerWebApp.Dtos;
using IdentityManagerWebApp.Infrastructure.Mapper;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityManagerWebApp.ViewModels.AccountManager
{
    public class AssignRolesViewModel : IMapFrom<UserDetailsDto>
    {
        public string Id { get; set; }


        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public IEnumerable<string> Roles { get; set; }

        [Display(Name = "Roles")]
        public IEnumerable<string> RolesList { get; set; }
    }
}