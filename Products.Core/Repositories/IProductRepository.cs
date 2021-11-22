using System.Collections.Generic;
using System.Threading.Tasks;
using Products.Core.Models;

namespace Products.Core.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllWithAccountAsync();
        Task<Product> GetWithAccountByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllWithAccountByAccountIdAsync(int accounttId);
    }
}