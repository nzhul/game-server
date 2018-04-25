using System.Threading.Tasks;
using Server.Models.Users;

namespace Server.Data.Services.Abstraction
{
    public interface IAuthService
    {
        Task<User> Register(User user, string password);

        Task<User> Login(string username, string password);

        Task<bool> UserExists(string username);
    }
}
