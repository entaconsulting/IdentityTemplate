namespace IdentityManagerWebApp.Dtos
{
    public class EditUserDto
    {
        public int? CollectEntityCode { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        // TODO: Refactorear a bool
        public string IsAdmin { get; set; }
        public string UserName { get; set; }
    }
}
