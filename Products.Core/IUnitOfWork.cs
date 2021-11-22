using System;
using System.Threading.Tasks;
using Products.Core.Repositories;

namespace Products.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IAccountRepository Accounts { get; }
        Task<int> CommitAsync();
    }
}