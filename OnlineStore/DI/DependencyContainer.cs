using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.BLL.OrderServices;
using OnlineStore.DAL.Implementation;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Options;
using MongoDB.Driver;
using OnlineStore.DAL;
using OnlineStore.Domain.Models;

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
                DatabaseSettings dbSettings = serviceProvider.GetService<DatabaseSettings>();
                return new MongoClient(dbSettings.ConnectionString);
            });

            service.AddTransient<IMongoDatabase>((serviceProvider) =>
            {
                DatabaseSettings dbSettings = serviceProvider.GetService<DatabaseSettings>();
                IMongoClient mongoClient = serviceProvider.GetService<IMongoClient>();
                return mongoClient.GetDatabase(dbSettings.DatabaseName);
            });

            service.AddTransient<IMongoCollection<User>>((serviceProvider) =>
            {
                IMongoDatabase mongoDatabase = serviceProvider.GetService<IMongoDatabase>();
                return mongoDatabase.GetCollection<User>("User");

            });

            service.AddTransient<IBasketRepository, BasketRepository>();

            service.AddTransient<IOrderRepository, OrderRepository>();

            service.AddTransient<OrderService>();
        }
    }
}