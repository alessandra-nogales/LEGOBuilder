using BuilderCatalogWebApi.Models;

namespace BuilderCatalogWebApi.Services.Interfaces
{
    public interface IApiService
    {
        Task<T> HttpGet<T>(string relativePath, string param = null);
    }
}
