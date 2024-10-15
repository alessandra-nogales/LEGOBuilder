using BuilderCatalogWebApi.Models;
using BuilderCatalogWebApi.Services.Interfaces;

namespace BuilderCatalogWebApi.Services
{
    /// <summary>
    /// Requests and returns User info.
    /// </summary>
    public class UserService : IUserService
    {   
        private IApiService _apiService;
        private ApiOptions _options;

        public UserService(IConfiguration config, IApiService a ) { 
            _apiService = a;
            _options = config.GetSection("Endpoints").Get<ApiOptions>() ?? new ApiOptions();
        }

        /// <summary>
        /// Gets User info based on username
        /// </summary>
        /// <param name="username">given user's username</param>
        /// <returns>User class requested</returns>
        public async Task<User> GetUserAsync(string username) {
            User u = await _apiService.HttpGet<User>(_options.GetUserByUsername, username);
            return u;
        }

        /// <summary>
        /// Gets User info based on User ID
        /// </summary>
        /// <param name="userId">given user's unique GUID identifier</param>
        /// <returns>User class requested</returns>
        public async Task<User> GetUserDetailsAsync(Guid userId)
        {
            User u = await _apiService.HttpGet<User>(_options.GetUserByUserId, userId.ToString());
            return u;
        }
    }
}
