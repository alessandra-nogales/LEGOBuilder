using BuilderCatalogWebApi.Models;
using BuilderCatalogWebApi.Services.Interfaces;

namespace BuilderCatalogWebApi.Services
{
    /// <summary>
    /// Requests and returns Set information
    /// </summary>
    public class SetService : ISetService
    {
        private ApiOptions _options;
        private IApiService _apiService;
        public SetService(IConfiguration config, IApiService apiService)
        {
            _options = config.GetSection("Endpoints").Get<ApiOptions>() ?? new ApiOptions();
            _apiService = apiService;
        }

        /// <summary>
        /// Requests all Lego sets.
        /// </summary>
        /// <returns>All possible sets info</returns>
        public async Task<LEGOCollection> GetAllLEGOSetsAsync() {
            return await _apiService.HttpGet<LEGOCollection>(_options.GetAllSets);
        }

        /// <summary>
        /// Requests a particular set's detailed info.
        /// </summary>
        /// <param name="id">Unique set GUID to query</param>
        /// <returns>A unique set including piece details</returns>
        public async Task<LEGOSet> GetLEGOSetDetailsAsync(Guid id) {
            return await _apiService.HttpGet<LEGOSet>(_options.GetSetById, id.ToString());
        }
    }
}
