using System.Threading.Tasks;
using Server.Models;
using Server.Models.Pagination;
using Server.Models.Realms;

namespace Server.Data.Services.Abstraction
{
    public interface IRealmsService : IService
    {
        Task<PagedList<Realm>> GetRealms(QueryParams queryParams);

        Task<Realm> GetRealm(int id);
    }
}
