using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models.View.Avatars;
using Server.Data.Services.Abstraction;

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
            _avatarsService = _avatarsService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a list of all dwellings of the avatar. Including not-owned
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/dwellings", Name = "GetDwellings")]
        public async Task<IActionResult> GetDwellings(int id)
        {
            var dwellings = await this._avatarsService.GetAvatarDwellings(id);

            var realmToReturn = _mapper.Map<DwellingDetailedDto>(dwellings);

            return Ok(realmToReturn);
        }
    }
}
