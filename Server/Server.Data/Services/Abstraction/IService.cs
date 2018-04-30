using System.Threading.Tasks;

namespace Server.Data.Services.Abstraction
{
    public interface IService
    {
        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAll();
    }
}