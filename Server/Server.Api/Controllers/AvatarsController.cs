using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models.View.Avatars;
using Server.Data.Services.Abstraction;
using Server.Models.MapEntities;

namespace Server.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AvatarsController : ControllerBase
    {
        private readonly IAvatarsService _avatarsService;
        private readonly IMapper _mapper;

        public AvatarsController(IAvatarsService avatarsService, IMapper mapper)
        {
            _avatarsService = avatarsService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a list of all dwellings of the avatar. Including not-owned
        /// </summary>
        /// <param name="id">Id of the avatar</param>
        /// <returns></returns>
        [HttpGet("{id}/dwellings", Name = "GetDwellings")]
        public async Task<IActionResult> GetDwellings(int id)
        {
            var dwellings = await this._avatarsService.GetAvatarDwellings(id);

            var realmToReturn = _mapper.Map<DwellingDetailedDto>(dwellings);

            return Ok(realmToReturn);
        }

        /// <summary>
        /// Explores all waypoints in his REALM.
        /// </summary>
        /// <param name="avatarId"></param>
        /// <param name="dwellingType">Type of the dwelling we wish to explore</param>
        /// <returns></returns>
        [HttpPut("{avatarId}/explore")]
        public async Task<IActionResult> Explore(int avatarId,[FromBody] ExploreParams exploreParams)
        {
            var exploredDwellings = await this._avatarsService.Explore(avatarId, exploreParams.Type, exploreParams.RegionIds);

            return Ok(exploredDwellings);
        }

        public class ExploreParams
        {
            public DwellingType Type { get; set; }

            public int[] RegionIds { get; set; }
        }
    }
}
