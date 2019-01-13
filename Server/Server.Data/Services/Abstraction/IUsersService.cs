using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Models.Pagination;
using Server.Models.Users;

namespace Server.Data.Services.Abstraction
{
    public interface IUsersService : IService
    {
        Task<PagedList<User>> GetUsers(UserParams userParams);

        Task<User> GetUser(int id);

        /// <summary>
        /// Creates new Friendship entity and set its status to None
        /// </summary>
        /// <param name="senderId">The one who sends the friend request</param>
        /// <param name="recieverUsernameOrEmail">The one who will recieve the friend request</param>
        /// <returns>Null if success. Error message on fail</returns>
        Task<string> SendFriendRequest(int senderId, string recieverUsernameOrEmail);

        /// <summary>
        /// Sets the status of the request to Approved
        /// </summary>
        /// <param name="senderId">The id of the Friendship.</param>
        /// <param name="recieverId">The id of the one who is recieving the friend request. Will be used for validation.</param>
        /// <returns>Null if success. Error message on fail</returns>
        Task<string> ApproveFriendRequest(int senderId, int recieverId);

        /// <summary>
        /// Set the status of the friendship to Rejected
        /// </summary>
        /// <param name="requestId">The id of the Friendship.</param>
        /// <param name="recieverId">The id of the one who is recieving the friend request. Will be used for validation.</param>
        /// <returns>Null if success. Error message on fail</returns>
        Task<string> RejectFriendRequest(int requestId, int recieverId);

        /// <summary>
        /// Will create new Friendship with status Blocked.
        /// </summary>
        /// <param name="blockerId">The one who blocks.</param>
        /// <param name="blockedId">The one who is getting blocked.</param>
        /// <returns>Null if success. Error message on fail</returns>
        Task<string> BlockUser(int blockerId, int blockedId);

        Task<IEnumerable<User>> GetFriends(int userId);
    }
}