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
        private readonly IRepository<Basket> _basketRepository;
        
        public readonly ILogger<AccountService> logger;

        public AccountService(IRepository<User> userRepository, IRepository<RegUser> regUserRepository, IRepository<Basket> basketRepository, ILogger<AccountService> logger)
        {
            _userRepository = userRepository;
            _regUserRepository = regUserRepository;
            _basketRepository = basketRepository;
            this.logger = logger;
        }
        public async Task<User> CreateAccount(Account newAccount)
        {
            var user = await _regUserRepository.GetAsync(user=>user.Login == newAccount.RegUser.Login);

            if (user.FirstOrDefault<RegUser>() != null)
            {
                logger.LogError("User is alread exists: id-" + user.FirstOrDefault<RegUser>().Id);
                return null;
            }

            string id = (_regUserRepository as MongoDBRepository<RegUser>).GenerateID();
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
                Id = (_regUserRepository as MongoDBRepository<RegUser>).GenerateID(),
                IdUser = id,
                Products = new List<Product>(),
                TotalSum = 0
            };
            await _regUserRepository.CreateAsync(newRegUser);
            await _userRepository.CreateAsync(newUser);
            await _basketRepository.CreateAsync(basket);

            return newUser;
        }

        public async Task<User> UpdateAccount(Account account)
        {
            var regUser = await _regUserRepository.GetAsync(account.RegUser.Id);
            var user = await _userRepository.GetAsync(account.User.Id);

            if (user == null || regUser==null)
            {
                logger.LogError("User doesn't exist");
                return null;
            }

            await _regUserRepository.UpdateAsync(account.RegUser);
            await _userRepository.UpdateAsync(account.User);

            return account.User;
        }

        public async Task<bool> DeleteUser (string id)
        {
            var regUser = await _regUserRepository.GetAsync(id);
            var user = await _userRepository.GetAsync(id);

            if (user == null || regUser == null)
            {
                logger.LogError("User doesn't exist");
                return false;
            }

            await _regUserRepository.RemoveAsync(id);
            await _userRepository.RemoveAsync(id);

            return true;
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
