using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityManagerWebApp.Models
{
    public class Roles
    {
        public const string BackendAdmin = "ADMIN";
        public const string Rol1 = "ROL1";
        public const string Rol2 = "ROL2";
        public const string Rol3 = "ROL3";

        public static IEnumerable<string> GetRoles()
        {
            return new List<string>()
            {
                Roles.BackendAdmin,
                Roles.Rol1,
                Roles.Rol2,
                Roles.Rol3,

            };
        }
    }
}