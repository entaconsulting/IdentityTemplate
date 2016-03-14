using AutoMapper;

namespace IdentityManagerWebApp.Infrastructure.Mapper
{
    public interface IHaveCustomMappings
    {
        void CreateMappings(IConfiguration configuration);
    }
}
