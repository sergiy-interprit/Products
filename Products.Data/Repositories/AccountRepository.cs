using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Products.Core.Models;
using Products.Core.Repositories;

namespace Products.Data.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(ProductsDbContext context) 
            : base(context)
        { }

        public async Task<IEnumerable<Account>> GetAllAsync(bool includeProducts)
        {
            if (includeProducts)
            {
                return await ProductsDbContext.Accounts
                    .Include(a => a.Products)
                    .ToListAsync();
            }

            return await ProductsDbContext.Accounts
                .ToListAsync();
        }

        public Task<Account> GetByIdAsync(int id, bool includeProducts)
        {
            if (includeProducts)
            {
                return ProductsDbContext.Accounts
                    .Include(a => a.Products)
                    .SingleOrDefaultAsync(a => a.Id == id);
            }

            return ProductsDbContext.Accounts
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        private ProductsDbContext ProductsDbContext
        {
            get { return Context as ProductsDbContext; }
        }
    }
}