using OnlineStore.Domain.Models;

namespace OnlineStore.BLL.AccountService.Model
{
    public class Account 
    {
        public RegUser RegUser { get; set; }
        public User User { get; set; }
    }
}
