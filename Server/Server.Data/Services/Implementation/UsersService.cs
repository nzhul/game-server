using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Data.Services.Abstraction;
using Server.Models;
using Server.Models.Pagination;
using Server.Models.Users;

namespace Server.Data.Services.Implementation
{
    public class UsersService : BaseService, IUsersService
    {
        public UsersService(DataContext context) : base(context)
        {
        }

        public async Task<string> ApproveFriendRequest(int senderId, int recieverId)
        {
            Friendship friendship = await _context.Friendships.FirstOrDefaultAsync(x => x.SenderId == senderId && x.RecieverId == recieverId);
            if (friendship != null)
            {
                friendship.State = FriendshipState.Approved;
                await _context.SaveChangesAsync();

                return null;
            }

            return "Cannot find friend request!";
        }

        public async Task<string> BlockUser(int senderId, int recieverId)
        {
            Friendship friendship = await _context.Friendships
                .FirstOrDefaultAsync(x => (x.SenderId == senderId && x.RecieverId == recieverId) || (x.SenderId == recieverId && x.RecieverId == senderId));

            if (friendship == null)
            {
                User sender = await GetUser(senderId);
                User reciever = await GetUser(recieverId);

                if (reciever == null)
                {
                    return $"Cannot find user with id: {recieverId}";
                }

                friendship = new Friendship
                {
                    SenderId = sender.Id,
                    Sender = sender,
                    Reciever = reciever,
                    RecieverId = reciever.Id,
                    State = FriendshipState.Blocked,
                    RequestTime = DateTime.UtcNow
                };

                sender.SendFriendRequests.Add(friendship);
                reciever.RecievedFriendRequests.Add(friendship);
                _context.Friendships.Add(friendship);
            }

            friendship.State = FriendshipState.Blocked;
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<IEnumerable<User>> GetFriends(int userId)
        {
            IEnumerable<User> friends = null;

            User dbUser = await GetUser(userId);
            if (dbUser != null)
            {
                friends = _context.Users
                    .Where(u => u.RecievedFriendRequests.Any(f => f.SenderId == userId && f.RecieverId == u.Id && f.State == FriendshipState.Approved) ||
                           u.SendFriendRequests.Any(f => f.SenderId == u.Id && f.RecieverId == userId && f.State == FriendshipState.Approved))
                    .ToList();
            }

            return friends;
        }

        public async Task<User> GetUser(int id)
        {
            // TODO: i should not use this on all places because includes the photos.
            return await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUser(string usernameOrEmail)
        {
            User dbUser = null;

            if (Utilities.IsEmail(usernameOrEmail))
            {
                dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == usernameOrEmail);
            }
            else
            {
                dbUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == usernameOrEmail);
            }

            return dbUser;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            IQueryable<User> users = _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);

            if (!string.IsNullOrEmpty(userParams.Gender) && userParams.Gender != "undefined")
            {
                users = users.Where(u => u.Gender == userParams.Gender);
            }

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                DateTime min = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                DateTime max = DateTime.Today.AddYears(-userParams.MinAge);

                users = users.Where(u => u.DateOfBirth >= min && u.DateOfBirth <= max);
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.CreatedAt);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<string> RejectFriendRequest(int requestId, int recieverId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SendFriendRequest(int senderId, string usernameOrEmail)
        {
            User sender = await GetUser(senderId);
            User reciever = await GetUser(usernameOrEmail);

            if (reciever == null)
            {
                return $"Cannot find user: {usernameOrEmail}";
            }

            Friendship newFriendship = new Friendship
            {
                SenderId = sender.Id,
                Sender = sender,
                Reciever = reciever,
                RecieverId = reciever.Id,
                State = FriendshipState.Pending,
                RequestTime = DateTime.UtcNow
            };

            sender.SendFriendRequests.Add(newFriendship);
            reciever.RecievedFriendRequests.Add(newFriendship);
            _context.Friendships.Add(newFriendship);
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<string> SetOffline(int userId)
        {
            User dbUser = await GetUser(userId);

            if (dbUser == null)
            {
                return $"Cannot find user with Id: {userId}";
            }

            dbUser.ActiveConnection = -1;
            dbUser.OnlineStatus = 0;
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<string> SetOnline(int userId, int connectionId)
        {
            User dbUser = await GetUser(userId);

            if (dbUser == null)
            {
                return $"Cannot find user with Id: {userId}";
            }

            dbUser.ActiveConnection = connectionId;
            dbUser.OnlineStatus = 1;
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task ClearBattle(int userId)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (dbUser != null)
            {
                dbUser.BattleId = null;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearAllBattles()
        {
            var users = _context.Users.Where(x => x.BattleId != null);
            foreach (var user in users)
            {
                user.BattleId = null;
            }

            await _context.SaveChangesAsync();
        }
    }
}