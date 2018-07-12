using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Helpers;
using Server.Api.Models.View.Realms;
using Server.Data.Services.Abstraction;
using Server.Models;
using Server.Models.Realms.Input;

namespace Server.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RealmsController : Controller
    {
        private readonly IRealmsService _realmsService;
        private readonly IMapper _mapper;

        public RealmsController(IRealmsService realmsService, IMapper mapper)
        {
            _realmsService = realmsService;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetRealm")]
        public async Task<IActionResult> GetRealm(int id)
        {
            var realm = await this._realmsService.GetRealm(id);

            var realmToReturn = _mapper.Map<RealmDetailedDto>(realm);

            return Ok(realmToReturn);
        }

        /// <summary>
        /// Get a list of all realms in the server.
        /// </summary>
        /// <remarks>
        /// Remarks can be used to put some description to the request
        /// It can be multiline description
        /// </remarks>
        /// <param name="queryParams"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="200">Returns an collection of all realms in the server</response>
        /// <response code="400">If some of the queryParams are invalid</response>
        [HttpGet]
        public async Task<IActionResult> GetRealmsList(QueryParams queryParams)
        {
            var realms = await this._realmsService.GetRealms(queryParams);

            var realmsToReturn = _mapper.Map<IEnumerable<RealmListItemDto>>(realms);

            Response.AddPagination(realms.CurrentPage, realms.PageSize, realms.TotalCount, realms.TotalPages);

            return Ok(realmsToReturn);
        }

        [HttpPut("{realmId}/users/{userId}/updateCurrentRealm")]
        public async Task<IActionResult> UpdateCurrentRealm(int realmId, int userId)
        {
            // TODO: Add support for admins to call this method
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            await this._realmsService.UpdateCurrentRealm(userId, realmId);

            return Ok();
        }

        [HttpGet("{realmId}/users/{userId}/avatar")]
        [ProducesResponseType(200, Type = typeof(AvatarDetailedDto))]
        public async Task<IActionResult> GetUserAvatarForRealm(int realmId, int userId)
        {
            var avatar = await this._realmsService.GetUserAvatarForRealm(realmId, userId);

            var avatarToReturn = _mapper.Map<AvatarDetailedDto>(avatar);

            return Ok(avatarToReturn);
        }

        [HttpPost("{realmId}/users/{userId}/avatar")]
        [ProducesResponseType(200, Type = typeof(AvatarDetailedDto))]
        public async Task<IActionResult> CreateHeroOrAvatarWithHero(int realmId, int userId, [FromBody] AvatarWithHeroCreationDto input)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var avatar = await this._realmsService.CreateHeroOrAvatarWithHero(realmId, userId, input);

            if (avatar != null)
            {
                var avatarToReturn = _mapper.Map<AvatarDetailedDto>(avatar);

                return Ok(avatarToReturn);
            }
            else
            {
                return BadRequest("Hero name is already taken!");
            }
        }

    }
}
