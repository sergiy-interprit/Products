using System.Collections.Generic;
using System.Threading.Tasks;
using Products.Domain.Models;

namespace Products.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllWithAccountAsync();
        Task<Product> GetWithAccountByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllWithAccountByAccountIdAsync(int accountId);
    }
}