using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models.View.Games;
using Server.Data.Services.Abstraction;
using Server.Models.Realms.Input;

namespace Server.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gamesService;
        private readonly IMapper _mapper;

        public GamesController(IMapper mapper, IGameService gameService)
        {
            _mapper = mapper;
            _gamesService = gameService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] GameParams input)
        {
            try
            {
                var game = await _gamesService.CreateGameAsync(input);
                var gameDto = _mapper.Map<GameDetailedDto>(game);
                return Ok(gameDto);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
