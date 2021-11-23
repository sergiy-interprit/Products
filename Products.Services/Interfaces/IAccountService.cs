using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation.Results;
using Products.API.Dto;
using Products.Domain.Models;

namespace Products.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAllAccounts();
        Task<IEnumerable<AccountWithoutProductsDto>> GetAllAccountsWithoutProducts();

        Task<AccountDto> GetAccountById(int id);

        Task<Account> GetAccountById(int id, bool includeProducts);  // TODO: Remove later after moving PATCH

        Task<AccountWithoutProductsDto> GetAccountWithoutProductsById(int id);

        Task<(ValidationResult, int)> CreateAccount(SaveAccountDto saveAccountDto);
        Task<(ValidationResult, bool)> UpdateAccount(int id, SaveAccountDto saveAccountDto);

        Task UpdateAccount(Account accountToBeUpdated, Account account); // TODO: Remove later after moving PATCH

        Task<bool> DeleteAccount(int id);
    }
}