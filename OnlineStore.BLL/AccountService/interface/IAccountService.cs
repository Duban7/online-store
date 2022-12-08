using OnlineStore.BLL.AccountService.Model;
using OnlineStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BLL.AccountService
{
    public interface IAccountService
    {
        public Task<User> CreateAccount(Account newAccount);
        public Task UpdateAccount(Account account, string id);
        public Task DeleteUser(string id);
        public Task<User> LogIn(RegUser logInRegUser);
        public Task<Account> GetUser(string id);
        public string GenerateJwtToken(User user);
    }
}
