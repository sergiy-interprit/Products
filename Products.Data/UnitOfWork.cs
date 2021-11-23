using System.Threading.Tasks;
using Products.Domain;
using Products.Domain.Repositories;
using Products.Data.Repositories;
using Products.Data.Contexts;

namespace Products.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductsDbContext _context;

        private ProductRepository _productRepository;
        private AccountRepository _accountRepository;

        public UnitOfWork(ProductsDbContext context)
        {
            this._context = context;
        }

        public IAccountRepository Accounts => _accountRepository = _accountRepository ?? new AccountRepository(_context);

        public IProductRepository Products => _productRepository = _productRepository ?? new ProductRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}