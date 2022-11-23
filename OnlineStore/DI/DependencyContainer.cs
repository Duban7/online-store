﻿using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.BLL.AccountService;
using OnlineStore.BLL.AccountService.Model;
using OnlineStore.DAL.Implementation;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Options;
using OnlineStore.Validators;

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

            service.AddTransient<IUserRepository, UserRepository>();

            service.AddTransient<IRegUserRepository, RegUserRepository>();

            service.AddTransient<IBasketRepository, BasketRepository>();

            service.AddScoped<IValidator<Account>, AccountValidator>();

            service.AddTransient<AccountService>();
        }
    }
}
