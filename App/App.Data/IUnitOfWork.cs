using App.Data.Repositories;
using App.Models;
using App.Models.Heroes;
using App.Models.Towns;
using App.Models.Units;

namespace App.Data
{
	public interface IUnitOfWork
    {
        IRepository<AppUser> Users { get; }
        IRepository<Unit> Units { get; }
        IRepository<Hero> Heroes { get; }
        IRepository<Town> Towns { get; }
        IRepository<Building> Buildings { get; }

        int SaveChanges();
    }
}
