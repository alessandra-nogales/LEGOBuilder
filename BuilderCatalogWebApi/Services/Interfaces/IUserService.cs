using BuilderCatalogWebApi.Models;

namespace BuilderCatalogWebApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserAsync(string username);
        Task<User> GetUserDetailsAsync(Guid userId);

    }
}
