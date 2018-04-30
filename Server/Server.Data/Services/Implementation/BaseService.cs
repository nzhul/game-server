using System.Threading.Tasks;
using Server.Data.Services.Abstraction;

namespace Server.Data.Services.Implementation
{
    public class BaseService : IService
    {
        protected readonly DataContext _context;

        public BaseService(DataContext context){
            this._context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}