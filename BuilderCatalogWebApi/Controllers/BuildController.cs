using BuilderCatalogWebApi.Models;
using BuilderCatalogWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BuilderCatalogWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildController : ControllerBase
    {

        private IBuilderService _builderService;
        public BuildController(IBuilderService s) {
           _builderService = s;
        }

        [HttpGet]
        public async Task<IEnumerable<SetDto>> GetSets(string username)
        {
            var result = await _builderService.GetUserSets(username);
            return result;
        }


    }
}
