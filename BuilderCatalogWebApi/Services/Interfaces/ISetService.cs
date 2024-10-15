using BuilderCatalogWebApi.Models;

namespace BuilderCatalogWebApi.Services.Interfaces
{
    public interface ISetService
    {
        Task<LEGOCollection> GetAllLEGOSetsAsync();
        Task<LEGOSet> GetLEGOSetDetailsAsync(Guid id);
    }
}
