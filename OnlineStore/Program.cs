using OnlineStore.Domain.Models;
using OnlineStore.BLL;
using OnlineStore.DAL;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("OnlineStoreDatabase"));
builder.Services.AddSingleton<ProductService>();
builder.Services.AddControllers();
var app = builder.Build();
app.UseRouting();
app.UseEndpoints (endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
