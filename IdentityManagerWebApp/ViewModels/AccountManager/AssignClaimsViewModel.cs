using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IdentityManagerWebApp.Dtos;
using IdentityManagerWebApp.Infrastructure.Mapper;

namespace IdentityManagerWebApp.ViewModels.AccountManager
{
    public class AssignClaimsViewModel : IMapFrom<UserDetailsDto>
    {
        public string Id { get; set; }

        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        public IEnumerable<ClaimDto> Claims { get; set; }

        public IEnumerable<string> ClaimsList { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [Display(Name = "Claim")]
        public string SelectedClaim { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [Display(Name = "Valor")]
        public string SelectedClaimValue { get; set; }

    }
}