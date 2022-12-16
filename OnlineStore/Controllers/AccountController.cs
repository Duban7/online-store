using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BLL.AccountService;
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

        /// <summary>
        /// Creates a new account
        /// </summary>
        /// <param name="newAccount"></param>
        /// <returns>jwt access token and newly created user model</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /clients/new
        ///     {
        ///         regUSer: {id:"null", password: "somePassword123", login: "someLogin"},
        ///         user: {id:"null", name: "someName", email: "someEmail@gmail.com", phone: "375258793215"}
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns newly created user model</response>
        /// <response code="400">Returns array of validation errors if validation fails</response>
        /// <response code="422">If user already exists</response>
        [HttpPost]
        [Route("clients/new")]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status422UnprocessableEntity)]
        [Produces("application/json")]
        public async Task<ActionResult<object>> CreateAccount([FromBody] Account newAccount)
        {
            ValidationResult result = await _accountValidator.ValidateAsync(newAccount);

            if (!result.IsValid)
            {
                List<string> errors = new List<string>();

                _logger.LogError("Invalid account");

                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.ErrorMessage);
                    errors.Add(error.ErrorMessage);    
                }

                return BadRequest(errors);
            }

            _logger.LogInformation("Account is valid");

            User? newUser = await _accountService.CreateAccount(newAccount);

            var response = new
            {
                access_token = _accountService.GenerateJwtToken(newUser),
                user = newUser
            };

            return Created(Request.Host.ToString()+"/"+newUser.Id, response);
        }

        /// <summary>
        /// Log in to account
        /// </summary>
        /// <param name="regUser"></param>
        /// <returns>jwt access token and found user model</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /clients/log-in
        ///     {
        ///        id:"null",
        ///        login: "someLogin",
        ///        password: "somePassword123"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns found user model</response>
        /// <response code="401">If login or password is incorrect</response>
        /// <response code="500">If the data in the database is corrupted</response>
        [HttpPost]
        [Route("clients/log-in")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<ActionResult<object>> LogIn([FromBody] RegUser regUser)
        {
            User? foundUser = await _accountService.LogIn(regUser);

            var response = new
            {
                access_token = _accountService.GenerateJwtToken(foundUser),
                user = foundUser
            };

            return Ok(response);
        }

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>found account model</returns>
        /// <remarks>
        /// !!!Requset needs an authorization
        /// 
        /// Sample request:
        ///
        ///     GET /clients/{id}
        ///
        /// </remarks>
        /// <response code="200">Returns found account model</response>
        /// <response code="400">If user doesn't exists</response>
        [HttpGet]
        [Route("clients/{id}")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Authorize]
        public async Task<ActionResult> GetUser(string id)
        {
            Account foundAccount = await _accountService.GetUser(id);

            return foundAccount.User==null || foundAccount.RegUser==null || foundAccount==null?BadRequest():Ok(foundAccount);
        }

        /// <summary>
        /// Updates an account
        /// </summary>
        /// <param name="account"></param>
        /// <param name="id"></param>
        /// <remarks>
        /// Sample request:
        /// !!!Requset needs an authorization
        /// 
        ///     PUT /clients/{id}
        ///     {
        ///         regUSer: {id: "someId", password: "somePassword123", login: "someLogin"},
        ///         user: {id: "someID", name: "someName", email: "someEmail@gmail.com", phone: "375258765849"}
        ///     }
        ///
        /// </remarks>
        /// <response code="204">No content</response>
        /// <response code="400">Returns array of validation errors if validation fails</response>
        /// <response code="401">If account is missing</response>
        [HttpPut]
        [Route("clients/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult> UpdateAccount([FromBody] Account account, string id)
        {
            ValidationResult result = await _accountValidator.ValidateAsync(account);

            if (!result.IsValid)
            {
                _logger.LogError("Invalid account");
                foreach (var error in result.Errors) _logger.LogError(error.ErrorMessage);

                return BadRequest(result.Errors);
            }

            _logger.LogInformation("Account is valid");

            await _accountService.UpdateAccount(account, id);

            return NoContent();
        }

        /// <summary>
        /// Deletes an account
        /// </summary>
        /// <param name="id"></param>
        /// <returns>found account model</returns>
        /// <remarks>
        /// !!!Requset needs an authorization
        /// 
        /// Sample request:
        ///
        ///     GET /clients/{id}
        ///
        /// </remarks>
        /// <response code="204">No content</response>
        /// <response code="401">If account is missing</response>
        [HttpDelete]
        [Route("clients/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult> DeleteAccount(string id)
        {
            await _accountService.DeleteUser(id);

            return NoContent();
        }

    }
}
