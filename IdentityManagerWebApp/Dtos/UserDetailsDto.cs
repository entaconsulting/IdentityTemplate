using System.Collections.Generic;

namespace IdentityManagerWebApp.Dtos
{
    public class UserDetailsDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public IEnumerable<ClaimDto> Claims { get; set; }
        public IEnumerable<string> Roles { get; set; }

    }

    public class ClaimDto
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
