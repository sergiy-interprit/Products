using System;
using System.Threading.Tasks;
using Products.Domain.Repositories;

namespace Products.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IAccountRepository Accounts { get; }
        Task<int> CommitAsync();
    }
}