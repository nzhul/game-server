using System.Threading.Tasks;
using Server.Models.Pagination;
using Server.Models.Users;

namespace Server.Data.Services.Abstraction
{
    public interface IUsersService : IService
    {
        Task<PagedList<User>> GetUsers(UserParams userParams);

        Task<User> GetUser(int id);
    }
}