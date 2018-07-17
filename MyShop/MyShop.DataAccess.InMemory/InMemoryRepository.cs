using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IStorageRepository<T> where T : BaseObjectEntity
    {
        private string memoryStorageName;
        ObjectCache cache = MemoryCache.Default;
        List<T> items;


        public InMemoryRepository()
        {
            memoryStorageName = typeof(T).Name;
            items = cache[memoryStorageName] as List<T>;
            if (items == null)
            {
                items = new List<T>();
            }
        }

        public void Commit()
        {
            cache[memoryStorageName] = items;
        }

        public void InsertItem(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T TtoUpdate = items.Find(i => i.Id == t.Id);
            if (TtoUpdate != null)
            {
                TtoUpdate = t;
            }
            else
            {
                throw new Exception("Item Not Found");
            }
        }

        public T FindItem(string id)
        {
            T t = items.Find(i => i.Id == id);
            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception("Item Not Found");
            }
        }
        public void Delete(T t)
        {
            T TtoDelete = items.Find(i => i.Id == t.Id);
            if (TtoDelete != null)
            {
                items.Remove(TtoDelete);
            }
            else
            {
                throw new Exception("Item Not Found");
            }
        }
        public IQueryable<T> GetItems()
        {
            return items.AsQueryable();
        }
    }
}
