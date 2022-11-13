using OnlineStore.Domain.CustomAttribute;

namespace OnlineStore.Domain.Models
{
    [BsonCollectionAttribute("RegUser")]
    public class RegUser : Model
    {
        public string Password { get; set; }
        public string Login { get; set; }
    }
}
