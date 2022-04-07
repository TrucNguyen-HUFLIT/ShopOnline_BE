using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Data.Repositories
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> Get();
        void SaveChanges();
        Task SaveChangesAsync();
        void SoftDelete(T entity);
        void Update(T entity);
        void Add(T entity);

    }
}
