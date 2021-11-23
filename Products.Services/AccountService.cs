using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Products.API.Dto;
using Products.Domain;
using Products.Domain.Models;
using Products.Services.Interfaces;
using Products.Services.Validators;

namespace Products.Services
{
    // TODO: Finalise PATCH!
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccounts()
        {
            var accounts = await _unitOfWork.Accounts.GetAllAsync(true);
            var accountDtos = _mapper.Map<IEnumerable<Account>, IEnumerable<AccountDto>>(accounts);
            return accountDtos;
        }

        public async Task<IEnumerable<AccountWithoutProductsDto>> GetAllAccountsWithoutProducts()
        {
            var accounts = await _unitOfWork.Accounts.GetAllAsync(false);
            var accountWithoutProductsDtos = _mapper.Map<IEnumerable<Account>, IEnumerable<AccountWithoutProductsDto>>(accounts);
            return accountWithoutProductsDtos;
        }

        public async Task<AccountDto> GetAccountById(int id)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(id, true);
            var accountDto = _mapper.Map<Account, AccountDto>(account);
            return accountDto;
        }

        // TODO: Remove later after migrating PATCH!
        public async Task<Account> GetAccountById(int id, bool includeProducts)
        {
            return await _unitOfWork.Accounts.GetByIdAsync(id, includeProducts);
        }

        public async Task<AccountWithoutProductsDto> GetAccountWithoutProductsById(int id)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(id, false);
            var accountWithoutProductsDto = _mapper.Map<Account, AccountWithoutProductsDto>(account);
            return accountWithoutProductsDto;
        }

        public async Task<(ValidationResult,int)> CreateAccount(SaveAccountDto saveAccountDto)
        {
            var validator = new SaveAccountDtoValidator();
            var validationResult = await validator.ValidateAsync(saveAccountDto);

            if (!validationResult.IsValid)
                return (validationResult, 0);

            var newAccount = _mapper.Map<SaveAccountDto, Account>(saveAccountDto);

            await _unitOfWork.Accounts.AddAsync(newAccount);
            await _unitOfWork.CommitAsync();

            return (validationResult, newAccount.Id);
        }

        public async Task<(ValidationResult,bool)> UpdateAccount(int id, SaveAccountDto saveAccountDto)
        {
            var validator = new SaveAccountDtoValidator();
            var validationResult = await validator.ValidateAsync(saveAccountDto);

            if (!validationResult.IsValid)
                return (validationResult, false);

            var accountToBeUpdated = await _unitOfWork.Accounts.GetByIdAsync(id, false);

            if (accountToBeUpdated == null)
                return (validationResult, false);

            _mapper.Map(saveAccountDto, accountToBeUpdated);

            await _unitOfWork.CommitAsync();

            return (validationResult, true);
        }

        // TODO: Remove later after migrating PATCH!
        public async Task UpdateAccount(Account accountToBeUpdated, Account account)
        {
            accountToBeUpdated.Name = account.Name;
            accountToBeUpdated.Description = account.Description;

            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> DeleteAccount(int id)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(id, false);

            if (account == null)
            {
                return false;
            }

            _unitOfWork.Accounts.Remove(account);

            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
