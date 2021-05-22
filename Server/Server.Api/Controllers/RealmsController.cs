//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Server.Api.Helpers;
//using Server.Api.Models.View.Avatars;
//using Server.Api.Models.View.Realms;
//using Server.Data.Services.Abstraction;
//using Server.Models;
//using Server.Models.MapEntities;
//using Server.Models.Realms;
//using Server.Models.Realms.Input;
//using Server.Models.Users;

//namespace Server.Api.Controllers
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    public class RealmsController : ControllerBase
//    {
//        private readonly IRealmsService _realmsService;
//        private readonly IMapper _mapper;

//        public RealmsController(IRealmsService realmsService, IMapper mapper)
//        {
//            _realmsService = realmsService;
//            _mapper = mapper;
//        }

//        [Authorize(Policy = "RequireAdmin")]
//        [HttpPost("CreateRealm")]
//        public async Task<IActionResult> CreateRealm()
//        {
//            Realm createdRealm = await this._realmsService.CreateRealm("GenerateRandomNameHere"); //TODO: generate random name
//            var realmToReturn = _mapper.Map<RealmDetailedDto>(createdRealm);

//            return CreatedAtRoute("GetRealm", new { controller = "Realms", id = createdRealm.Id }, realmToReturn);
//        }

//        [HttpGet("{id}", Name = "GetRealm")]
//        public async Task<IActionResult> GetRealm(int id)
//        {
//            var realm = await this._realmsService.GetRealm(id);

//            var realmToReturn = _mapper.Map<RealmDetailedDto>(realm);

//            return Ok(realmToReturn);
//        }

//        /// <summary>
//        /// Get a list of all realms in the server.
//        /// </summary>
//        /// <remarks>
//        /// Remarks can be used to put some description to the request
//        /// It can be multiline description
//        /// </remarks>
//        /// <param name="queryParams"></param>
//        /// <returns>A newly created TodoItem</returns>
//        /// <response code="200">Returns an collection of all realms in the server</response>
//        /// <response code="400">If some of the queryParams are invalid</response>
//        [HttpGet]
//        public async Task<IActionResult> GetRealmsList(QueryParams queryParams)
//        {
//            var realms = await this._realmsService.GetRealms(queryParams);

//            var realmsToReturn = _mapper.Map<IEnumerable<RealmListItemDto>>(realms);

//            Response.AddPagination(realms.CurrentPage, realms.PageSize, realms.TotalCount, realms.TotalPages);

//            return Ok(realmsToReturn);
//        }

//        [HttpPut("{realmId}/users/{userId}/updateCurrentRealm")]
//        public async Task<IActionResult> UpdateCurrentRealm(int realmId, int userId)
//        {
//            // TODO: Add support for admins to call this method
//            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
//            {
//                return Unauthorized();
//            }

//            await this._realmsService.UpdateCurrentRealm(userId, realmId);

//            return Ok();
//        }

//        //TODO: move to AvatarsController.cs
//        [HttpGet("{realmId}/users/{userId}/avatar")]
//        [ProducesResponseType(200, Type = typeof(AvatarDetailedDto))]
//        public async Task<IActionResult> GetUserAvatarForRealm(int realmId, int userId)
//        {
//            var avatar = await this._realmsService.GetUserAvatarForRealm(realmId, userId);

//            var avatarToReturn = _mapper.Map<AvatarDetailedDto>(avatar);

//            if (avatarToReturn != null)
//            {
//                ManualMapDwellings(avatarToReturn, avatar.AvatarDwellings);
//            }

//            return Ok(avatarToReturn);
//        }

//        private void ManualMapDwellings(AvatarDetailedDto avatarToReturn, ICollection<AvatarDwelling> avatarDwellings)
//        {
//            avatarToReturn.Dwellings = new List<DwellingDetailedDto>();
//            avatarToReturn.Waypoints = new List<WaypointDto>();
//            foreach (var ad in avatarDwellings)
//            {
//                // Map dwellings
//                var dwellingDto = _mapper.Map<DwellingDetailedDto>(ad.Dwelling);
//                avatarToReturn.Dwellings.Add(dwellingDto);

//                // Map Waypoints
//                if (ad.Dwelling.Type == DwellingType.Waypoint)
//                {
//                    var waypointDto = _mapper.Map<WaypointDto>(ad.Dwelling);
//                    avatarToReturn.Waypoints.Add(waypointDto);
//                }
//            }
            
            
//        }

//        //TODO: move to AvatarsController.cs
//        [HttpPost("{realmId}/users/{userId}/avatar")]
//        [ProducesResponseType(200, Type = typeof(AvatarDetailedDto))]
//        public async Task<IActionResult> CreateHeroOrAvatarWithHero(int realmId, int userId, [FromBody] AvatarWithHeroCreationDto input)
//        {
//            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
//            {
//                return Unauthorized();
//            }

//            var avatar = await this._realmsService.CreateHeroOrAvatarWithHero(realmId, userId, input);

//            if (avatar != null)
//            {
//                var avatarToReturn = _mapper.Map<AvatarDetailedDto>(avatar);

//                return Ok(avatarToReturn);
//            }
//            else
//            {
//                return BadRequest("Hero name is already taken!");
//            }
//        }

//        /// <summary>
//        /// Returns a list of regions with detailed information.
//        /// For querying more than one region use this syntax:
//        /// /api/realms/{realmId}/regions?regionIds=1&regionIds=2&regionIds=3 
//        /// </summary>
//        /// <param name="realmId">Id the the realm</param>
//        /// <param name="regionIds"> collection of regions -> /api/realms/{realmId}/regions?regionIds=1&regionIds=2&regionIds=3</param>
//        /// <returns></returns>
//        [HttpGet("{realmId}/regions")]
//        public async Task<IActionResult> GetRegions(int realmId, [FromQuery] int[] regionIds)
//        {
//            var regions = this._realmsService.GetRegions(regionIds);

//            if (regions != null)
//            {
//                var regionsToReturn = regions.ProjectTo<RegionDetailedDto>(_mapper.ConfigurationProvider);

//                return Ok(regionsToReturn);
//            }
//            else
//            {
//                return BadRequest("Cannot query specified regions!");
//            }
//        }
//    }
//}
