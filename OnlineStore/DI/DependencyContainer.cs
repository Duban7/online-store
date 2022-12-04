using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.BLL.ProductServices;
using OnlineStore.DAL.Implementation;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Options;

namespace OnlineStore.DI
{
    public static class DependencyContainer
    {
        public static void RegisterDependency(this IServiceCollection service)
        {
            service.AddAuthorization();

            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = JwtOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = JwtOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = JwtOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });

            service.AddTransient<IProductRepository, ProductRepository>();

            service.AddTransient<ICategoryRepository, CategoryRepository>();

            service.AddTransient<ISubcategoryRepository, SubcategoryRepository>();

            service.AddTransient<ProductService>();
        }
    }
}