using Microsoft.IdentityModel.Tokens;

namespace OnlineStore.BLL.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; } //= "OnlineStoreServer"; // издатель токена
        public string Audience { get; set; } //= "OnlineStoreClient"; // потребитель токена
        public string Key { get; set; } //= "HQaDbE2sMwIDAQABAoIBAEs63TvT94njrPDP3A/ssfCEXg1F2y0D/PjzUhM1aJGcRiOUXnGlYdViGhLnnJoNZTZm9qI1LT";   // ключ для шифрации
        public SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Key));
    }
}