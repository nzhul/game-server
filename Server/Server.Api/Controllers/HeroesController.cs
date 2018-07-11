using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data.Services.Abstraction;

namespace Server.Api.Controllers
{
    [Authorize]
    [Route("api/realms")]
    public class HeroesController : Controller
    {
        private readonly IHeroesService _heroesService;
        private readonly IMapper _mapper;

        public HeroesController(IHeroesService heroesService, IMapper mapper)
        {
            _heroesService = heroesService;
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
    }
}
