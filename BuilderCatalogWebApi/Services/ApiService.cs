using BuilderCatalogWebApi.Models;
using BuilderCatalogWebApi.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;

namespace BuilderCatalogWebApi.Services
{
    /// <summary>
    /// Generic API service whose only job is to reach out for HTTP requests with the expected relative path.
    /// </summary>
    public class ApiService : IApiService
    {
        private ApiOptions _options;
        private ILogger _logger;
        public ApiService(IConfiguration config, ILogger<ApiService> l)
        {
            _options = config.GetSection("Endpoints").Get<ApiOptions>() ?? new ApiOptions();
            _logger = l;
        }

        /// <summary>
        /// Performs a simple GET to the third party API and converts the returned content 
        /// into the generic class provided by the caller.
        /// </summary>
        /// <typeparam name="T">Generic return type</typeparam>
        /// <param name="relativePath">Relative path of the URL</param>
        /// <param name="param">Optional params</param>
        /// <returns></returns>
        public async Task<T> HttpGet<T>(string relativePath, string param = null) {
            try
            {
                using (HttpClient client = new())
                {
                    var uri = new Uri(new Uri(_options.BaseUrl), relativePath);
                    if (!String.IsNullOrEmpty(param)) { 
                        uri = new Uri(uri, param);
                    }
                    HttpResponseMessage response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        T? s = JsonConvert.DeserializeObject<T>(content);
                        return s;
                    }
                    else {
                        var msg = $"Error returned {response.StatusCode} from {uri.ToString()}";
                        throw new Exception(msg);
                    }
                }
            }
            catch (Exception ex) {
                //  Log & throw
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }


    }
}
