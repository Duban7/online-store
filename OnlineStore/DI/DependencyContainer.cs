using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.DAL.Implementation;
using OnlineStore.DAL.Interfaces;
using OnlineStore.BLL.Options;
using MongoDB.Driver;
using OnlineStore.DAL;
using OnlineStore.Domain.Models;
using Microsoft.Extensions.Options;
using OnlineStore.BLL.OrderServices.implementation;
using OnlineStore.BLL.OrderServices;
using System.Reflection;

namespace OnlineStore.DI
{
    public static class DependencyContainer
    {
        public static void RegisterDependency(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<DatabaseSettings>(configuration.GetSection("OnlineStoreDatabase"));

            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            services.AddAuthorization();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                IServiceProvider serviceProvider = services.BuildServiceProvider();

                JwtOptions jwtOptions = serviceProvider.GetService<IOptions<JwtOptions>>().Value;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = jwtOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });
            services.AddSingleton<IMongoClient, MongoClient>((serviceProvider) =>
            {
                DatabaseSettings dbSettings = serviceProvider.GetService<IOptions<DatabaseSettings>>().Value;
                return new MongoClient(dbSettings.ConnectionString);
            });

            services.AddScoped<IMongoDatabase>((serviceProvider) =>
            {
                DatabaseSettings dbSettings = serviceProvider.GetService<IOptions<DatabaseSettings>>().Value;
                IMongoClient mongoClient = serviceProvider.GetService<IMongoClient>();
                return mongoClient.GetDatabase(dbSettings.DatabaseName);
            });

            services.AddScoped<IMongoCollection<Order>>((serviceProvider) =>
            {
                IMongoDatabase mongoDatabase = serviceProvider.GetService<IMongoDatabase>();
                return mongoDatabase.GetCollection<Order>("Order");
            });

            services.AddScoped<IMongoCollection<Basket>>((serviceProvider) =>
            {
                IMongoDatabase mongoDatabase = serviceProvider.GetService<IMongoDatabase>();
                return mongoDatabase.GetCollection<Basket>("Basket");
            });

            services.AddTransient<IBasketRepository, BasketRepository>();

            services.AddTransient<IOrderRepository, OrderRepository>();
            
            services.AddTransient<OrderService>();
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }
    }
}