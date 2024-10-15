using BuilderCatalogWebApi.Models;

namespace BuilderCatalogWebApi.Services.Interfaces
{
    public interface IBuilderService
    {
        Task<IEnumerable<SetDto>> GetUserSets(string username);
    }
}
