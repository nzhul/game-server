using App.Data.Repositories;
using App.Models;
using App.Models.Heroes;
using App.Models.Towns;
using App.Models.Units;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data
{
    public class UnitOfWork : IUnitOfWork
    {

        private DbContext context;
        private IDictionary<Type, object> repositories;

        public UnitOfWork()
            : this(new GameDbContext())
        {
        }

        public UnitOfWork(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<AppUser> Users
        {
            get { return this.GetRepository<AppUser>(); }
        }

        public IRepository<Unit> Units
        {
            get { return this.GetRepository<Unit>(); }
        }

        public IRepository<Hero> Heroes
        {
            get { return this.GetRepository<Hero>(); }
        }

        public IRepository<Town> Towns
        {
            get { return this.GetRepository<Town>(); }
        }

        public IRepository<Building> Buildings
        {
            get { return this.GetRepository<Building>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfRepository = typeof(T);
            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepository = Activator.CreateInstance(typeof(EFRepository<T>), context);
                this.repositories.Add(typeOfRepository, newRepository);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }
    }
}
