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
        private readonly IMapper _mapper;

        public GamesController(IMapper mapper, IGameService gameService)
        {
            _mapper = mapper;
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

        //[HttpPost]
        //public async Task<IActionResult> CreateGame([FromBody] GameParams input)
        //{
        //    try
        //    {
        //        input.MapTemplate = MapTemplate.Small; // TODO: remove this! For testing only!

        //        var game = await _gamesService.CreateGameAsync(input);
        //        var gameDto = _mapper.Map<GameDetailedDto>(game);
        //        return Ok(gameDto);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex);
        //    }
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetGame(int id)
        //{
        //    try
        //    {
        //        var game = await _gamesService.GetGameAsync(id);
        //        var gameDto = _mapper.Map<GameDetailedDto>(game);
        //        return Ok(gameDto);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex);
        //    }
        //}

        //[HttpPut("{id}/{userId}/leave")]
        //public async Task<IActionResult> LeaveGame(int id, int userId)
        //{
        //    try
        //    {
        //        await _gamesService.LeaveGame(id, userId);
        //        return Ok();
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex);
        //    }
        //}
    }
}
