using System.Collections.Generic;
using System.Threading.Tasks;
using Products.Domain;
using Products.Domain.Models;
using Products.Domain.Services;

namespace Products.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Account>> GetAllAccounts(bool includeProducts)
        {
            return await _unitOfWork.Accounts.GetAllAsync(includeProducts);
        }

        public async Task<Account> GetAccountById(int id, bool includeProducts)
        {
            return await _unitOfWork.Accounts.GetByIdAsync(id, includeProducts);
        }

        public async Task<Account> CreateAccount(Account newAccount)
        {
            await _unitOfWork.Accounts.AddAsync(newAccount);

            await _unitOfWork.CommitAsync();                    
            
            return newAccount;
        }

        public async Task UpdateAccount(Account accountToBeUpdated, Account account)
        {
            accountToBeUpdated.Name = account.Name;
            accountToBeUpdated.Description = account.Description;

            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAccount(Account account)
        {
            _unitOfWork.Accounts.Remove(account);

            await _unitOfWork.CommitAsync();
        }
    }
}
