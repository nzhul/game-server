using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Helpers;
using Server.Api.Models.Input.Realms;
using Server.Api.Models.View.Realms;
using Server.Data.Services.Abstraction;
using Server.Models;

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
        /// Create user account will create new avatar for the user
        /// create new hero and assign random starting world position.
        /// </summary>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> CreateAvatarWithHero([FromBody] AvatarWithHeroCreationDto input)
        {
            if (input.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> GetRealmsList(QueryParams queryParams)
        {
            var realms = await this._realmsService.GetRealms(queryParams);

            var realmsToReturn = _mapper.Map<IEnumerable<RealmListItemDto>>(realms);

            Response.AddPagination(realms.CurrentPage, realms.PageSize, realms.TotalCount, realms.TotalPages);

            return Ok(realmsToReturn);
        }
    }
}
