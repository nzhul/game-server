using System.Threading.Tasks;
using Server.Models;
using Server.Models.Pagination;
using Server.Models.Realms;
using Server.Models.Realms.Input;
using Server.Models.Users;

namespace Server.Data.Services.Abstraction
{
    public interface IRealmsService : IService
    {
        Task<PagedList<Realm>> GetRealms(QueryParams queryParams);

        Task<Realm> GetRealm(int id);

        Task UpdateCurrentRealm(int userId, int realmId);

        Task<Avatar> GetUserAvatarForRealm(int realmId, int userId);

        Task<Avatar> CreateHeroOrAvatarWithHero(int realmId, int userId, AvatarWithHeroCreationDto input);

        Task<Realm> CreateRealm(string name);
    }
}
