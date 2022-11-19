using Microsoft.Extensions.Logging;
using OnlineStore.BLL.AccountService.Model;
using OnlineStore.DAL;
using OnlineStore.Domain.Models;
namespace OnlineStore.BLL.AccountService
{
    public class AccountService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<RegUser> _regUserRepository;
        
        public readonly ILogger<AccountService> logger;

        public AccountService(IRepository<User> userRepository, IRepository<RegUser> regUserRepository, ILogger<AccountService> logger)
        {
            _userRepository = userRepository;
            _regUserRepository = regUserRepository;
            this.logger = logger;
        }
        public async Task<User> CreateAccount(Account newAccount)
        {
            var user = await _regUserRepository.GetAsync(user=>user.Login == newAccount.Login);

            if (user.FirstOrDefault<RegUser>() != null)
            {
                logger.LogError("User is alread exists: id-" + user.FirstOrDefault<RegUser>().Id);
                return null;
            }

            string id = (_regUserRepository as MongoDBRepository<RegUser>).GenerateID();
            RegUser newRegUser = new RegUser()
            {
                Id = id,
                Login = newAccount.Login,
                Password = newAccount.Password
            };
            User newUser = new User()
            {
                Id = id,
                Name = newAccount.Name,
                Email = newAccount.Email,
                Phone = newAccount.Phone
            };
            await _regUserRepository.CreateAsync(newRegUser);
            await _userRepository.CreateAsync(newUser);

            return newUser;
        }

        public async Task<User?> LogIn(RegUser logInRegUser)
        {
            RegUser foundRegUser = (await _regUserRepository.GetAsync(regUser => regUser.Login == logInRegUser.Login && regUser.Password == logInRegUser.Password)).FirstOrDefault();

            if (foundRegUser == null) return null;

            User foundUser = await _userRepository.GetAsync(foundRegUser.Id);

            if (foundUser != null) return foundUser;

            logger.LogError("RegUser doesn't match User");
            return null;
        }

        public async Task<User?> GetUser(string id) =>
            await _userRepository.GetAsync(id);
    }
}
