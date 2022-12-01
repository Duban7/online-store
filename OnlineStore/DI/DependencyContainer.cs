using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using OnlineStore.BLL.AccountService;
using OnlineStore.BLL.AccountService.Model;
using OnlineStore.DAL;
using OnlineStore.DAL.Implementation;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Domain.Models;
using OnlineStore.Options;
using OnlineStore.Validators;
using Microsoft.Extensions.Options;

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

            service.AddTransient<IMongoClient, MongoClient>((serviceProvider) =>
            {
                DatabaseSettings dbSettings = serviceProvider.GetService<IOptions<DatabaseSettings>>().Value;
                return new MongoClient(dbSettings.ConnectionString);
            });

            service.AddTransient<IMongoDatabase>((serviceProvider) =>
            {
                DatabaseSettings dbSettings = serviceProvider.GetService<IOptions<DatabaseSettings>>().Value;
                IMongoClient mongoClient = serviceProvider.GetService<IMongoClient>();
                return mongoClient.GetDatabase(dbSettings.DatabaseName);
            });

            service.AddTransient<IMongoCollection<User>>((serviceProvider) =>
            {
                IMongoDatabase mongoDatabase = serviceProvider.GetService<IMongoDatabase>();
                return mongoDatabase.GetCollection<User>("User");

            });

            service.AddTransient<IUserRepository, UserRepository>();

            service.AddTransient<IRegUserRepository, RegUserRepository>();

            service.AddTransient<IBasketRepository, BasketRepository>();

            service.AddScoped<IValidator<Account>, AccountValidator>();

            service.AddTransient<AccountService>();

            service.AddControllers();

            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();
        }
    }
}
