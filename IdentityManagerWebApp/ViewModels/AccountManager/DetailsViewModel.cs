using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IdentityManagerWebApp.Dtos;
using IdentityManagerWebApp.Infrastructure.Mapper;

namespace IdentityManagerWebApp.ViewModels.AccountManager
{
    public class DetailsViewModel : IMapFrom<UserDetailsDto>
    {
        public string Id { get; set; }

        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Claims")]
        public IList<ClaimListViewModel> ClaimsList { get; set; }
        public IEnumerable<string> Roles { get; set; }


    }
}