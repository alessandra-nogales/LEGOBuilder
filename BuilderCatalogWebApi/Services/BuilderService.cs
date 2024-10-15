using BuilderCatalogWebApi.Models;
using BuilderCatalogWebApi.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace BuilderCatalogWebApi.Services
{
    /// <summary>
    /// Bulk of the work done in this service: Calls the other services to query and process the data.
    /// </summary>
    public class BuilderService : IBuilderService
    {
        private IUserService _userService;
        private ISetService _setService;
        private IMemoryCache _cache;
        private MemoryCacheEntryOptions options = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(600000)
        };

        public BuilderService(IUserService u, ISetService s, IMemoryCache m)
        {
            _userService = u;
            _setService = s;
            _cache = m;
        }

        /// <summary>
        /// Queries the user's pieces, and pieces for each set that the user COULD build (total bricks < user's brick count)
        /// </summary>
        /// <param name="username">User's username to look up</param>
        /// <returns>A list of viable sets the given user can build.</returns>
        public async Task<IEnumerable<SetDto>> GetUserSets(string username) {
            var u = await _userService.GetUserAsync(username);
            // look up username's inventory, if we have the cached value, return it.
            // else look up from scratch
            if (_cache.TryGetValue(username, out object? value))
                return (List<SetDto>)value;

            // get all the user's LEGO pieces and create a dictionary for lookup:
            var userDetails = await _userService.GetUserDetailsAsync(u.Id);
            var userPartLookup = userDetails.Collection.ToDictionary(x => x.PieceId,         
                y => y.Variants.ToDictionary(part => part.Color, part => part.Count));

            // get all sets but limit the check to only those sets where the user has at least as many pieces:
            var collection = await _setService.GetAllLEGOSetsAsync();
            var setIds = collection.Sets.Where(x => x.TotalPieces <= userDetails.BrickCount)
                .Select(x => x.Id).ToList(); // limit memory used here to the Guids

            var sets = await ParseUserPieces(setIds, userPartLookup);

            // cache the user's available sets (for maybe 10 minutes? or when the user's brick count changes)
            // so we don't have to do the comparison each time
            var result = sets.Select(x => new SetDto { SetNumber = x.SetNumber, Name = x.Name }).ToList();
            _cache.Set(username, result, options);
            return result;
        }


        /// <summary>
        /// Compares the set's pieces to the user's pieces (limited by color) using a lookup on the user's piece count.
        /// </summary>
        /// <param name="setIds">The unique set GUIDs to query details for</param>
        /// <param name="userPartLookup">Dictionary lookup of user's pieces and color count</param>
        /// <returns></returns>
        private async Task<List<LEGOSet>> ParseUserPieces(List<Guid> setIds, Dictionary<string, Dictionary<int, int>> userPartLookup) {
            List<LEGOSet> sets = new List<LEGOSet>();
            // the double foreach is not great -- considered a hash set instead but would result in different hashes if the
            // user had MORE than the number of required parts, so that wouldn't work.
            foreach (var setId in setIds)
            {
                bool validSet = true;
                // query each set and compare the pieces to the user's
                var setDetails = await _setService.GetLEGOSetDetailsAsync(setId);
                foreach (var part in setDetails.Pieces)
                {
                    // if user doesn't have the piece OR has the piece but not in the right color OR has the piece
                    // but not enough, then set valid to false and break
                    if (!userPartLookup.TryGetValue(part.Part.DesignID, out Dictionary<int, int> variants)
                        || !variants.TryGetValue(part.Part.Material, out int userPartCount)
                        || userPartCount < part.Quantity)
                    {
                        validSet = false;
                        break;
                    }
                }

                if (validSet)
                    sets.Add(setDetails);
            }
            return sets;
        }
    }
}
