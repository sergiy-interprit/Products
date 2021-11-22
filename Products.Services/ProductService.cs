using System.Collections.Generic;
using System.Threading.Tasks;
using Products.Core;
using Products.Core.Models;
using Products.Core.Services;

namespace Products.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetAllWithAccount()
        {
            return await _unitOfWork.Products.GetAllWithAccountAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _unitOfWork.Products.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProductsByAccountId(int accountId)
        {
            return await _unitOfWork.Products.GetAllWithAccountByAccountIdAsync(accountId);
        }

        public async Task<Product> CreateProduct(Product newProduct)
        {
            await _unitOfWork.Products.AddAsync(newProduct);
            
            await _unitOfWork.CommitAsync();
            
            return newProduct;
        }

        public async Task UpdateProduct(Product productToBeUpdated, Product product)
        {
            productToBeUpdated.Name = product.Name;
            productToBeUpdated.AccountId = product.AccountId;

            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteProduct(Product product)
        {
            _unitOfWork.Products.Remove(product);

            await _unitOfWork.CommitAsync();
        }
    }
}