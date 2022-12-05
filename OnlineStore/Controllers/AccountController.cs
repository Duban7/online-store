using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BLL.AccountService;
using OnlineStore.BLL.AccountService.implementation;
using OnlineStore.BLL.AccountService.Model;
using OnlineStore.Domain.Models;

namespace OnlineStore.Controllers
{

    [ApiController]
    [Route("")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IValidator<Account> _accountValidator;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, IValidator<Account> accountValidator, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _accountValidator = accountValidator;
            _logger = logger;
        }

        [HttpPost]
        [Route("clients/registration")]
        public async Task<ActionResult<object>> CreateAccount([FromBody] Account newAccount)
        {
            ValidationResult result = await _accountValidator.ValidateAsync(newAccount);

            if (!result.IsValid)
            {
                _logger.LogError("Invalid account");
                foreach (var error in result.Errors) _logger.LogError(error.ErrorMessage);
                return BadRequest(result.Errors);
            }

            _logger.LogInformation("Account is valid");

            User? newUser = await _accountService.CreateAccount(newAccount);

           // if (newUser == null) return BadRequest();

            var response = new
            {
                access_token = _accountService.GenerateJwtToken(newUser),
                user = newUser
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("clients/authorisation")]
        public async Task<ActionResult<object>> LogIn([FromBody] RegUser regUser)
        {
            User? foundUser = await _accountService.LogIn(regUser);

           // if (foundUser == null) return Unauthorized();

            var response = new
            {
                access_token = _accountService.GenerateJwtToken(foundUser),
                user = foundUser
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("clients/{id}")]
        [Authorize]
        public async Task<ActionResult> GetUser(string id)
        {
            Account foundAccount = await _accountService.GetUser(id);

            return foundAccount.User==null || foundAccount.RegUser==null || foundAccount==null?BadRequest():Ok(foundAccount);
        }

        [HttpPut]
        [Route("clients/update")]
        [Authorize]
        public async Task<ActionResult> UpdateAccount([FromBody] Account account)
        {
            ValidationResult result = await _accountValidator.ValidateAsync(account);

            if (!result.IsValid)
            {
                _logger.LogError("Invalid account");
                foreach (var error in result.Errors) _logger.LogError(error.ErrorMessage);
                return BadRequest(result.Errors);
            }

            _logger.LogInformation("Account is valid");

            await _accountService.UpdateAccount(account);
            return NoContent();
        }


        [HttpDelete]
        [Route("clients/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteAccount(string id)
        {
            await _accountService.DeleteUser(id);
            return NoContent();

        }

    }
}
