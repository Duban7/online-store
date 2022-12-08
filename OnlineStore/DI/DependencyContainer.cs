using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using OnlineStore.BLL.AccountService.Model;
using OnlineStore.DAL;
using OnlineStore.DAL.Implementation;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Domain.Models;
using OnlineStore.Options;
using OnlineStore.Validators;
using Microsoft.Extensions.Options;
using OnlineStore.BLL.AccountService.implementation;
using OnlineStore.BLL.AccountService;
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

            services.AddScoped<IMongoCollection<User>>((serviceProvider) =>
            {
                IMongoDatabase mongoDatabase = serviceProvider.GetService<IMongoDatabase>();

                return mongoDatabase.GetCollection<User>("User");

            });

            services.AddScoped<IMongoCollection<RegUser>>((serviceProvider) =>
            {
                IMongoDatabase mongoDatabase = serviceProvider.GetService<IMongoDatabase>();

                return mongoDatabase.GetCollection<RegUser>("RegUser");

            });

            services.AddScoped<IMongoCollection<Basket>>((serviceProvider) =>
            {
                IMongoDatabase mongoDatabase = serviceProvider.GetService<IMongoDatabase>();
                return mongoDatabase.GetCollection<Basket>("User");

            });

            services.AddTransient<IUserRepository, UserRepository>();

            services.AddTransient<IRegUserRepository, RegUserRepository>();

            services.AddTransient<IBasketRepository, BasketRepository>();

            services.AddScoped<IValidator<Account>, AccountValidator>();

            services.AddTransient<IAccountService, AccountService>();

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
