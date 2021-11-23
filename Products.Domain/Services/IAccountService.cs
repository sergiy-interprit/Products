using System.Collections.Generic;
using System.Threading.Tasks;
using Products.Domain.Models;

namespace Products.Domain.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAccounts(bool includeProducts);
        Task<Account> GetAccountById(int id, bool includeProducts);
        Task<Account> CreateAccount(Account newAccount);
        Task UpdateAccount(Account accountToBeUpdated, Account account);
        Task DeleteAccount(Account account);
    }
}