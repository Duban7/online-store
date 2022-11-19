using Microsoft.IdentityModel.Tokens;

namespace OnlineStore.Options
{
    public class JwtOptions
    {
        public const string ISSUER = "OnlineStoreServer"; // издатель токена
        public const string AUDIENCE = "OnlineStoreClient"; // потребитель токена
        const string KEY = "HQaDbE2sMwIDAQABAoIBAEs63TvT94njrPDP3A/sfCEXg1F2y0D/PjzUhM1aJGcRiOUXnGlYdViGhLnnJoNZTZm9qI1LT";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(KEY));
    }
}
