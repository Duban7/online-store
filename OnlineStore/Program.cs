using OnlineStore.Domain.Models;
using OnlineStore.DAL;
using OnlineStore.DAL.Implementation;
using OnlineStore.DAL.Interfaces;
using OnlineStore.BLL.ProductServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("OnlineStoreDatabase"));
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<ICategoryRepository,CategoryRepository>();
builder.Services.AddSingleton<ISubcategoryRepository, SubcategoryRepository>();
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
