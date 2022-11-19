using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.BLL.AccountService;
using OnlineStore.DAL;
using OnlineStore.Domain.Models;
using OnlineStore.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
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
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("OnlineStoreDatabase"));
builder.Services.AddSingleton<IRepository<User>,MongoDBRepository<User>>();
builder.Services.AddSingleton<IRepository<RegUser>, MongoDBRepository<RegUser>>();
builder.Services.AddSingleton<AccountService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints (endpoints =>
{
    endpoints.MapControllers();
});

app.Run();