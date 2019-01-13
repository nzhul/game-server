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
            this._usersService = usersService;
            this._mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await this._usersService.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // var dbUser = await _usersService.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            // if (string.IsNullOrEmpty(userParams.Gender))
            // {
            //     userParams.Gender = dbUser.Gender == "male" ? "female" : "male";
            // }

            var users = await this._usersService.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpPost("addfriend/{usernameOrEmail}")]
        public async Task<IActionResult> SendFriendRequest(string usernameOrEmail)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

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
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); // currentUserId is the reciever

            var recieverId = currentUserId;

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

        [HttpGet("{userId}/friends")]
        public async Task<IActionResult> GetFriends(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var friends = await _usersService.GetFriends(userId);

            if (friends != null)
            {
                var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(friends);
                return Ok(usersToReturn);
            }
            else
            {
                return NoContent();
            }
        }
    }
}