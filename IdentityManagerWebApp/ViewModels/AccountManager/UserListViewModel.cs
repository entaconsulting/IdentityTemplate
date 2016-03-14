using IdentityManagerWebApp.Dtos;
using IdentityManagerWebApp.Infrastructure.Mapper;

namespace IdentityManagerWebApp.ViewModels.AccountManager
{
    public class UserListViewModel : IMapFrom<UserHeaderDto>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }
}