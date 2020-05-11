using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

        public GamesController()
        {

        }
    }
}
