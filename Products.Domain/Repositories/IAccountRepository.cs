using System.Collections.Generic;
using System.Threading.Tasks;
using Products.Domain.Models;

namespace Products.Domain.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<IEnumerable<Account>> GetAllAsync(bool includeProducts);

        Task<Account> GetByIdAsync(int id, bool includeProducts);
    }
}