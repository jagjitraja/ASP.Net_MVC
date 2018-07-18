using System.Linq;
using MyShop.Core.Models;

namespace MyShop.Core.Contracts
{
    public interface IStorageRepository<T> where T : BaseObjectEntity
    {
        void Commit();
        void Delete(string id);
        T FindItem(string id);
        IQueryable<T> GetItems();
        void InsertItem(T t);
        void Update(T t);
    }
}