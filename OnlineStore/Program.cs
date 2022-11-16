using OnlineStore.Domain.Models;
using OnlineStore.DAL;
using OnlineStore.BLL.ProductServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("OnlineStoreDatabase"));
builder.Services.AddSingleton<IRepository<Product>, MongoDBRepository<Product>>();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddControllers();
var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints (endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
