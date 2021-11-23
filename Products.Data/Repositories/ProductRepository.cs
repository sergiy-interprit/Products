using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Products.Data.Contexts;
using Products.Domain.Models;
using Products.Domain.Repositories;

namespace Products.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ProductsDbContext context) 
            : base(context)
        { }

        public async Task<IEnumerable<Product>> GetAllWithAccountAsync()
        {
            return await ProductsDbContext.Products
                .Include(m => m.Account)
                .ToListAsync();
        }

        public async Task<Product> GetWithAccountByIdAsync(int id)
        {
            return await ProductsDbContext.Products
                .Include(m => m.Account)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllWithAccountByAccountIdAsync(int accountId)
        {
            return await ProductsDbContext.Products
                .Include(m => m.Account)
                .Where(m => m.AccountId == accountId)
                .ToListAsync();
        }
        
        private ProductsDbContext ProductsDbContext
        {
            get { return Context as ProductsDbContext; }
        }
    }
}