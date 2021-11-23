using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Products.API.Dto;
using Products.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Products.Services.Validators;
using Microsoft.AspNetCore.JsonPatch;
using Products.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Products.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<ActionResult> GetAllAccounts(bool includeProducts = false)
        {
            if (includeProducts)
            {
                var accountDtos = await _accountService.GetAllAccounts();
                return Ok(accountDtos);
            }

            var accountWithoutProductsDtos = await _accountService.GetAllAccountsWithoutProducts();
            return Ok(accountWithoutProductsDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id, bool includeProducts = false)
        {
            var accountWithoutProductsDtos = await _accountService.GetAccountWithoutProductsById(id);

            if (accountWithoutProductsDtos == null)
                return NotFound();

            if(includeProducts)
            {
                var accountDto = await _accountService.GetAccountById(id);
                return Ok(accountDto);
            }

            var accountWithoutProductsDto = await _accountService.GetAccountWithoutProductsById(id);
            return Ok(accountWithoutProductsDto);
        }

        [HttpPost("")]
        public async Task<ActionResult> CreateAccount([FromBody] SaveAccountDto saveAccountDto)
        {
            var result = await _accountService.CreateAccount(saveAccountDto);

            if (!result.Item1.IsValid)
                return BadRequest(result.Item1.Errors);

            var accountWithoutProductsDto = await _accountService.GetAccountWithoutProductsById(result.Item2);
            return Ok(accountWithoutProductsDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAccount(int id, [FromBody] SaveAccountDto saveAccountDto)
        {
            var result = await _accountService.UpdateAccount(id, saveAccountDto);
            
            if (!result.Item1.IsValid)
                return BadRequest(result.Item1.Errors);

            if (!result.Item2)
                return NotFound();

            //var updatedAccountDto = await _accountService.GetAccountWithoutProductsById(id);
            //return Ok(updatedAccountDto);

            return NoContent();
        }

        // TODO: Move validation logic to Services layer!
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
            var result = await _accountService.DeleteAccount(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}