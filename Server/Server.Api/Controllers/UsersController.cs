using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Helpers;
using Server.Api.Models.View;
using Server.Data.Services.Abstraction;
using Server.Models.Users;

namespace Server.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public UsersController(IUsersService usersService, IMapper mapper)
        {
            _usersService = usersService;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            User user = await _usersService.GetUser(id);

            UserForDetailedDto userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(UserParams userParams)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // var dbUser = await _usersService.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            // if (string.IsNullOrEmpty(userParams.Gender))
            // {
            //     userParams.Gender = dbUser.Gender == "male" ? "female" : "male";
            // }

            Server.Models.Pagination.PagedList<User> users = await _usersService.GetUsers(userParams);

            IEnumerable<UserForListDto> usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpPost("addfriend/{usernameOrEmail}")]
        public async Task<IActionResult> SendFriendRequest(string usernameOrEmail)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            string result = await _usersService.SendFriendRequest(currentUserId, usernameOrEmail);

            if (string.IsNullOrEmpty(result))
            {
                return Ok();
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("approvefriend/{senderId}")]
        public async Task<IActionResult> ApproveFriendRequest(int senderId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); // currentUserId is the reciever

            int recieverId = currentUserId;

            string result = await _usersService.ApproveFriendRequest(senderId, recieverId);

            if (string.IsNullOrEmpty(result))
            {
                return Ok();
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("block/{userId}")]
        public async Task<IActionResult> BlockUser(int userId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            string result = await _usersService.BlockUser(currentUserId, userId);

            if (string.IsNullOrEmpty(result))
            {
                return Ok();
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("{userId}/friends")]
        public async Task<IActionResult> GetFriends(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            IEnumerable<User> friends = await _usersService.GetFriends(userId);

            if (friends != null)
            {
                IEnumerable<UserForListDto> usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(friends);
                return Ok(usersToReturn);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPut("{userId}/setoffline")]
        public async Task<IActionResult> SetOffline(int userId)
        {
            string result = await _usersService.SetOffline(userId);

            if (string.IsNullOrEmpty(result))
            {
                return Ok();
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("{userId}/setonline/{connectionId}")]
        public async Task<IActionResult> SetOnline(int userId, int connectionId)
        {
            string result = await _usersService.SetOnline(userId, connectionId);

            if (string.IsNullOrEmpty(result))
            {
                return Ok();
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("{userId}/clearbattle")]
        public async Task<IActionResult> ClearBattle(int userId)
        {
            await _usersService.ClearBattle(userId);
            return Ok();
        }

        [HttpPut("clearallbattles")]
        public async Task<IActionResult> ClearAllBattles()
        {
            await _usersService.ClearAllBattles();
            return Ok();
        }
    }
}