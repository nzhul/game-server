using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models.View.Realms;
using Server.Data.Services.Abstraction;
using Server.Models.Armies;

namespace Server.Api.Controllers
{
    [Authorize]
    [Route("api/realms")]
    public class HeroesController : ControllerBase
    {
        private readonly IArmiesService _heroesService;
        private readonly IMapper _mapper;

        public HeroesController(IArmiesService heroesService, IMapper mapper)
        {
            _heroesService = heroesService;
            _mapper = mapper;
        }

        [HttpDelete("{realmId}/users/{userId}/avatar/{avatarId}/heroes/{heroId}")]
        public async Task<IActionResult> DeleteHero(int realmId, int userId, int avatarId, int heroId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            bool result = await _heroesService.DeleteHero(heroId);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        // TODO: change this route to somehting that use UpdateParams class
        [HttpPut("heroes/{regionId}/{heroId}/{x}/{y}")]
        public async Task<IActionResult> UpdateArmyPosition(int gameId, int armyId, int x, int y)
        {
            //TODO: update the region also!
            Army dbArmy = await _heroesService.GetArmy(armyId);

            if (dbArmy != null)
            {
                await _heroesService.UpdateArmyPosition(dbArmy, x, y, gameId);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("heroes/{heroId}")]
        [ProducesResponseType(200, Type = typeof(ArmyDetailedDto))]
        public async Task<IActionResult> GetArmy(int armyId)
        {
            var dbArmy = await _heroesService.GetArmy(armyId);

            if (dbArmy != null)
            {
                var armyToReturn = _mapper.Map<ArmyDetailedDto>(dbArmy);
                return Ok(armyToReturn);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
