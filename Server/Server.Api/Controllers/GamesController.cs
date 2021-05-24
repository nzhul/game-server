using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Data.Services.Abstraction;

namespace Server.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gamesService;

        public GamesController(IMapper mapper, IGameService gameService)
        {
            _gamesService = gameService;
        }

        [HttpPut("{id}/{winnerid}/end")]
        public async Task<IActionResult> EndGame(int id, int winnerId)
        {
            try
            {
                await _gamesService.EndGame(id, winnerId);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
