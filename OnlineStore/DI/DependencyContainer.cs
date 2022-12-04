using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.BLL.OrderServices;
using OnlineStore.DAL.Implementation;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Options;
using MongoDB.Driver;
using OnlineStore.DAL;
using OnlineStore.Domain.Models;
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
            service.AddSingleton<IMongoClient, MongoClient>((serviceProvider) =>
            {
                DatabaseSettings dbSettings = serviceProvider.GetService<IOptions<DatabaseSettings>>().Value;
                return new MongoClient(dbSettings.ConnectionString);
            });

            service.AddScoped<IMongoDatabase>((serviceProvider) =>
            {
                DatabaseSettings dbSettings = serviceProvider.GetService<IOptions<DatabaseSettings>>().Value;
                IMongoClient mongoClient = serviceProvider.GetService<IMongoClient>();
                return mongoClient.GetDatabase(dbSettings.DatabaseName);
            });

            service.AddScoped<IMongoCollection<Order>>((serviceProvider) =>
            {
                IMongoDatabase mongoDatabase = serviceProvider.GetService<IMongoDatabase>();
                return mongoDatabase.GetCollection<Order>("Order");
            });

            service.AddScoped<IMongoCollection<Basket>>((serviceProvider) =>
            {
                IMongoDatabase mongoDatabase = serviceProvider.GetService<IMongoDatabase>();
                return mongoDatabase.GetCollection<Basket>("Basket");
            });

            service.AddTransient<IBasketRepository, BasketRepository>();

            service.AddTransient<IOrderRepository, OrderRepository>();
            
            service.AddTransient<OrderService>();
            service.AddControllers();

            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();
        }
    }
}