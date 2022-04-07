using ShopOnline.Core;
using ShopOnline.Core.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Data.Repositories.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext _context;

        public ProductRepository(MyDbContext context)
        {
            _context = context;
        }

        public IQueryable<ProductEntity> Get()
        {
            return _context.Products.Where(x => !x.IsDeleted);
        }

        public void Add(ProductEntity productEntity)
        {
            _context.Products.Add(productEntity);
        }

        public void Update(ProductEntity productEntity)
        {
            _context.Products.Update(productEntity);
        }

        public void SoftDelete(ProductEntity productEntity)
        {
            productEntity.IsDeleted = true;
            _context.Products.Update(productEntity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
