using OnlineStore.Domain.Models;
using OnlineStore.DAL;
using OnlineStore.DAL.Implementation;
using OnlineStore.DAL.Interfaces;
using OnlineStore.BLL.ProductServices;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("OnlineStoreDatabase"));

builder.Services.AddControllers();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
