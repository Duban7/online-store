using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.IO;
using OnlineStore.BLL.AccountService;
using OnlineStore.BLL.AccountService.Model;
using OnlineStore.DAL;
using OnlineStore.Domain.Models;
using OnlineStore.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnlineStore.Controllers
{
 
    [ApiController]
    [Route("")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("clients/registration")]
        public async Task<ActionResult<User>> CreateAccount([FromBody] Account newAccount)
        {
            User newUser = await _accountService.CreateAccount(newAccount);

            return newUser == null ? BadRequest() : Ok(newUser);
        }

        [HttpPost]
        [Route("clients/authorisation")]
        public async Task<ActionResult<object>> LogIn([FromBody] RegUser regUser)
        {
            User foundUser = await _accountService.LogIn(regUser);

            if (foundUser == null) return Unauthorized();

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, foundUser.Name),
                new Claim(ClaimTypes.Email, foundUser.Email),
                new Claim(ClaimTypes.MobilePhone, foundUser.Phone)
            };
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: JwtOptions.ISSUER,
                audience: JwtOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(JwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                user = foundUser
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("clients/{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            return await _accountService.GetUser(id);
        }

        [HttpPut]
        [Route("clients/{id}")]
        [Authorize]
        public async Task<ActionResult<User>> UpdateAccount([FromBody] Account account)
        {
            User updatedUser = await _accountService.UpdateAccount(account);

            return updatedUser == null ? BadRequest() : Ok(updatedUser);
        }

        [HttpDelete]
        [Route("clients/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteAccount(string id) =>
            await _accountService.DeleteUser(id) == false ? BadRequest() : NoContent();

    }
}
