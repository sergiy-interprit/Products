using System.Collections.Generic;
using System.Threading.Tasks;
using Products.Core.Models;

namespace Products.Core.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<IEnumerable<Account>> GetAllAsync(bool includeProducts);

        Task<Account> GetByIdAsync(int id, bool includeProducts);
    }
}