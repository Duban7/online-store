using OnlineStore.Domain.Models;
using OnlineStore.DAL;
using OnlineStore.BLL.OrderServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("OnlineStoreDatabase"));
builder.Services.AddSingleton<IRepository<Order>, MongoDBRepository<Order>>();
builder.Services.AddSingleton<IRepository<Basket>, MongoDBRepository<Basket>>();
builder.Services.AddSingleton<OrderService>();
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
