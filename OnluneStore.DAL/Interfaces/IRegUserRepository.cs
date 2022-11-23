using OnlineStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DAL.Interfaces
{
    public interface IRegUserRepository : IRepository<RegUser>
    {
        public Task<RegUser?> GetOneByLoginAsync(string login);
        public  Task<RegUser?> GetOneByLoginAndPasswordAsync(string login, string password);
    }
}
