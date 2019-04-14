using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models.View.Realms;
using Server.Data.Services.Abstraction;
using Server.Models.Heroes;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Server.Api.Controllers
{
    [Authorize]
    [Route("api/realms")]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroesService _heroesService;
        private readonly IMapper _mapper;

        public HeroesController(IHeroesService heroesService, IMapper mapper)
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
        public async Task<IActionResult> UpdateHeroPosition(int regionId, int heroId, int x, int y)
        {
            //TODO: update the region also!
            Hero dbHero = await _heroesService.GetHero(heroId);

            if (dbHero != null)
            {
                await _heroesService.UpdateHeroPosition(dbHero, x, y, regionId);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("heroes/{heroId}")]
        [ProducesResponseType(200, Type = typeof(HeroDetailedDto))]
        public async Task<IActionResult> GetHero(int heroId)
        {
            Hero dbHero = await _heroesService.GetHero(heroId);

            if (dbHero != null)
            {
                var heroToReturn = _mapper.Map<HeroDetailedDto>(dbHero);
                return Ok(heroToReturn);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
