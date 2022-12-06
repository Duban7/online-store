using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using OnlineStore.BLL.AccountService.Model;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Domain.Models;
using OnlineStore.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnlineStore.BLL.AccountService.implementation
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRegUserRepository _regUserRepository;
        private readonly IBasketRepository _basketRepository;

        private readonly JwtOptions _jwtOptions;

        public readonly ILogger<AccountService> _logger;

        public AccountService(IUserRepository userRepository,
                            IRegUserRepository regUserRepository,
                            IBasketRepository basketRepository,
                            IOptions<JwtOptions> jwtOptions,
                            ILogger<AccountService> logger)
        {
            _userRepository = userRepository;
            _regUserRepository = regUserRepository;
            _basketRepository = basketRepository;
            _jwtOptions = jwtOptions.Value;
            _logger = logger;
        }
        public async Task<User> CreateAccount(Account newAccount)
        {
            var user = await _regUserRepository.GetOneByLoginAsync(newAccount.RegUser.Login);

            if (user != null)
            {
                _logger.LogError("User is alread exists: id-" + user.Id);
                throw new Exception("User is alread exists: id-" + user.Id);
            }

            string id = _regUserRepository.GenerateObjectID();
            RegUser newRegUser = new RegUser()
            {
                Id = id,
                Login = newAccount.RegUser.Login,
                Password = newAccount.RegUser.Password
            };
            User newUser = new User()
            {
                Id = id,
                Name = newAccount.User.Name,
                Email = newAccount.User.Email,
                Phone = newAccount.User.Phone
            };
            Basket basket = new Basket()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                IdUser = id,
                Products = new List<Product>(),
                TotalSum = 0
            };
            await _regUserRepository.CreateAsync(newRegUser);
            await _userRepository.CreateAsync(newUser);
            await _basketRepository.CreateAsync(basket);

            return newUser;
        }

        public async Task UpdateAccount(Account account)
        {
            string id = account.RegUser.Id;
            RegUser? regUser = await _regUserRepository.GetOneByIdAsync(id);
            User? user = await _userRepository.GetOneByIdAsync(id);

            if (user == null || regUser == null)
            {
                _logger.LogError("User doesn't exist");
                throw new Exception("User doesn't exist");
            }

            await _regUserRepository.UpdateAsync(account.RegUser);
            await _userRepository.UpdateAsync(account.User);
        }

        public async Task DeleteUser(string id)
        {
            var regUser = await _regUserRepository.GetOneByIdAsync(id);
            var user = await _userRepository.GetOneByIdAsync(id);

            if (user == null || regUser == null)
            {
                _logger.LogError("User doesn't exist");
                throw new Exception("User doesn't exist");
            }

            await _regUserRepository.RemoveAsync(id);
            await _userRepository.RemoveAsync(id);

        }

        public async Task<User> LogIn(RegUser logInRegUser)
        {
            RegUser? foundRegUser = await _regUserRepository.GetOneByLoginAndPasswordAsync(logInRegUser.Login, logInRegUser.Password);
           
            if (foundRegUser == null)
            {
                _logger.LogError("User doesn't exist");
                throw new Exception("User doesn't exist");
            }

            User? foundUser = await _userRepository.GetOneByIdAsync(foundRegUser.Id);

            if (foundUser == null)
            {
                _logger.LogError("RegUser doesn't match User");
                throw new Exception("User doesn't exist");
            }

            return foundUser;
        }

        public async Task<Account> GetUser(string id) =>
            new Account()
            {
                User = await _userRepository.GetOneByIdAsync(id),
                RegUser = await _regUserRepository.GetOneByIdAsync(id)
            };


        public string GenerateJwtToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.Phone)
            };
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(_jwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
