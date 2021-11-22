using System.Collections.Generic;
using System.Threading.Tasks;
using Products.Core.Models;

namespace Products.Core.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllWithAccount();
        Task<Product> GetProductById(int id);
        Task<IEnumerable<Product>> GetProductsByAccountId(int accountId);
        Task<Product> CreateProduct(Product newProduct);
        Task UpdateProduct(Product productToBeUpdated, Product product);
        Task DeleteProduct(Product product);
    }
}