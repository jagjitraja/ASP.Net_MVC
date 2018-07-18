using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepository<T> : IStorageRepository<T> where T : BaseObjectEntity
    {

        internal DataContext dataContext;
        internal DbSet<T> dbset;

        public SQLRepository(DataContext context)
        {
            dataContext = context;
            dbset = context.Set<T>();
        }

        public void Commit()
        {
            dataContext.SaveChanges();
        }

        public void Delete(string id)
        {
            T TtoDelete = FindItem(id);

            if (dataContext.Entry(TtoDelete).State == EntityState.Detached)
                dbset.Attach(TtoDelete);

            dbset.Remove(TtoDelete);
        }

        public T FindItem(string id)
        {
            return dbset.Find(id);
        }

        public IQueryable<T> GetItems()
        {
            return dbset;
        }

        public void InsertItem(T t)
        {
            dbset.Add(t);
        }

        public void Update(T t)
        {
            dbset.Attach(t);
            dataContext.Entry(t).State = EntityState.Modified;
        }
    }
}
