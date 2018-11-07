using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data.Services.Abstraction;
using Server.Models.Heroes;

namespace Server.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ValidationController : ControllerBase
    {
        private readonly IValidationService _validationService;

        private readonly IHeroesService _heroesService;

        public ValidationController(IValidationService validationService, IHeroesService heroesService)
        {
            _validationService = validationService;
            _heroesService = heroesService;
        }

        [HttpPut("heroes/{heroId}/{x}/{y}")]
        public async Task<IActionResult> UpdateHeroPosition(int heroId, int x, int y)
        {
            Hero dbHero = await _heroesService.GetHeroWithRegion(heroId); // TODO: consider adding all active players regions into memory cache!

            if (_validationService.IsValidPosition(dbHero.Region, x, y))
            {
                await _heroesService.UpdateHeroPosition(dbHero, x, y);
                return Ok();
            }
            else
            {
                return Forbid();
            }
        }
    }
}
