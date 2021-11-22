using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Products.API.Models;
using Products.Core.Models;
using Products.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Products.API.Validations;
using Microsoft.AspNetCore.JsonPatch;

namespace Products.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountsController(IAccountService accountService, IMapper mapper)
        {
            this._accountService = accountService;
            this._mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllAccounts(bool includeProducts = false)
        {
            var accounts = await _accountService.GetAllAccounts(includeProducts);

            if (includeProducts)
            {
                var accountDtos = _mapper.Map<IEnumerable<Account>, IEnumerable<AccountDto>>(accounts);
                return Ok(accountDtos);
            }

            var accountWithoutProductsDtos = _mapper.Map<IEnumerable<Account>, IEnumerable<AccountWithoutProductsDto>>(accounts);
            return Ok(accountWithoutProductsDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetAccountById(int id, bool includeProducts = false)
        {
            var account = await _accountService.GetAccountById(id, includeProducts);

            if (account == null)
                return NotFound();

            if(includeProducts)
            {
                var accountDto = _mapper.Map<Account, AccountDto>(account);
                return Ok(accountDto);
            }

            var accountWithoutProductsDto = _mapper.Map<Account, AccountWithoutProductsDto>(account);
            return Ok(accountWithoutProductsDto);
        }

        [HttpPost("")]
        public async Task<ActionResult<AccountDto>> CreateAccount([FromBody] SaveAccountDto saveAccountDto)
        {
            var validator = new SaveAccountDtoValidator();
            var validationResult = await validator.ValidateAsync(saveAccountDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var accountToCreate = _mapper.Map<SaveAccountDto, Account>(saveAccountDto);
            var newAccount = await _accountService.CreateAccount(accountToCreate);

            var account = await _accountService.GetAccountById(newAccount.Id, true);
            var accountDto = _mapper.Map<Account, AccountDto>(account);

            return Ok(accountDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AccountDto>> UpdateAccount(int id, [FromBody] SaveAccountDto saveAccountDto)
        {
            var validator = new SaveAccountDtoValidator();
            var validationResult = await validator.ValidateAsync(saveAccountDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var accountToBeUpdated = await _accountService.GetAccountById(id, false);

            if (accountToBeUpdated == null)
                return NotFound();

            var account = _mapper.Map<SaveAccountDto, Account>(saveAccountDto);
            await _accountService.UpdateAccount(accountToBeUpdated, account);

            //var updatedAccount = await _accountService.GetAccountById(id, false);
            //var updatedAccountDto = _mapper.Map<Account, AccountDto>(updatedAccount);
            //return Ok(updatedAccountDto);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<AccountDto>> PartiallyUpdateAccount(int id,
             [FromBody] JsonPatchDocument<SaveAccountDto> patchDoc)
        {
            var accountToBeUpdated = await _accountService.GetAccountById(id, false);

            if (accountToBeUpdated == null)
                return NotFound();

            var accountToPatch = _mapper.Map<SaveAccountDto>(accountToBeUpdated);

            patchDoc.ApplyTo(accountToPatch, ModelState);

            var validator = new SaveAccountDtoValidator();
            var validationResult = await validator.ValidateAsync(accountToPatch);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var account = _mapper.Map<SaveAccountDto, Account>(accountToPatch);
            await _accountService.UpdateAccount(accountToBeUpdated, account);

            //var updatedAccount = await _accountService.GetAccountById(id, false);
            //var updatedAccountDto = _mapper.Map<Account, AccountDto>(updatedAccount);
            //return Ok(updatedAccountDto);
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _accountService.GetAccountById(id, false);

            if (account == null)
            {
                return NotFound();
            }

            await _accountService.DeleteAccount(account);

            return NoContent();
        }
    }
}