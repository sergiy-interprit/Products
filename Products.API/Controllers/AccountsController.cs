using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Products.API.Models;
using Products.Core.Models;
using Products.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Products.API.Validations;

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
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccounts();

            var accountDtos = _mapper.Map<IEnumerable<Account>, IEnumerable<AccountDto>>(accounts);

            return Ok(accountDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetAccountById(int id)
        {
            var account = await _accountService.GetAccountById(id);

            var accountDto = _mapper.Map<Account, AccountDto>(account);

            return Ok(accountDto);
        }

        [HttpPost("")]
        public async Task<ActionResult<AccountDto>> CreateAccount([FromBody] SaveAccountDto saveAccountDto)
        {
            var validator = new SaveAccountDtoValidator();
            var validationResult = await validator.ValidateAsync(saveAccountDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // this needs refining, but for demo it is ok

            var accountToCreate = _mapper.Map<SaveAccountDto, Account>(saveAccountDto);

            var newAccount = await _accountService.CreateAccount(accountToCreate);

            var account = await _accountService.GetAccountById(newAccount.Id);

            var accountDto = _mapper.Map<Account, AccountDto>(account);

            return Ok(accountDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AccountDto>> UpdateAccount(int id, [FromBody] SaveAccountDto saveAccountDto)
        {
            var validator = new SaveAccountDtoValidator();
            var validationResult = await validator.ValidateAsync(saveAccountDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // this needs refining, but for demo it is ok

            var accountToBeUpdated = await _accountService.GetAccountById(id);

            if (accountToBeUpdated == null)
                return NotFound();

            var account = _mapper.Map<SaveAccountDto, Account>(saveAccountDto);

            await _accountService.UpdateAccount(accountToBeUpdated, account);

            var updatedAccount = await _accountService.GetAccountById(id);

            var updatedAccountDto = _mapper.Map<Account, AccountDto>(updatedAccount);

            return Ok(updatedAccountDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _accountService.GetAccountById(id);

            await _accountService.DeleteAccount(account);

            return NoContent();
        }
    }
}