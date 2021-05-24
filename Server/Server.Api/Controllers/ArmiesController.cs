//using System.Security.Claims;
//using System.Threading.Tasks;
//using AutoMapper;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Server.Data.Services.Abstraction;
//using Server.Models.Armies;

//namespace Server.Api.Controllers
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    public class ArmiesController : ControllerBase
//    {
//        private readonly IArmiesService _armiesService;
//        private readonly IMapper _mapper;

//        public ArmiesController(IArmiesService armiesService, IMapper mapper)
//        {
//            _armiesService = armiesService;
//            _mapper = mapper;
//        }

//        //[HttpDelete("{realmId}/users/{userId}/avatar/{avatarId}/heroes/{heroId}")]
//        //public async Task<IActionResult> DeleteHero(int realmId, int userId, int avatarId, int heroId)
//        //{
//        //    if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
//        //    {
//        //        return Unauthorized();
//        //    }

//        //    bool result = await _armiesService.DeleteHero(heroId);

//        //    if (result)
//        //    {
//        //        return NoContent();
//        //    }
//        //    else
//        //    {
//        //        return NotFound();
//        //    }
//        //}

//        // TODO: change this route to somehting that use UpdateParams class
//        [HttpPut("{armyId}/{x}/{y}")]
//        public async Task<IActionResult> UpdateArmyPosition(int armyId, int x, int y)
//        {
//            if (await _armiesService.UpdateArmyPosition(armyId, x, y))
//            {
//                return Ok();
//            }
//            else
//            {
//                return NotFound();
//            }
//        }

//        //[HttpGet("heroes/{heroId}")]
//        //[ProducesResponseType(200, Type = typeof(ArmyDetailedDto))]
//        //public async Task<IActionResult> GetArmy(int armyId)
//        //{
//        //    var dbArmy = await _heroesService.GetArmy(armyId);

//        //    if (dbArmy != null)
//        //    {
//        //        var armyToReturn = _mapper.Map<ArmyDetailedDto>(dbArmy);
//        //        return Ok(armyToReturn);
//        //    }
//        //    else
//        //    {
//        //        return NotFound();
//        //    }
//        //}
//    }
//}
