var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<OnlineStore.DAL.DatabaseSettings>(builder.Configuration.GetSection("OnlineStoreDatabase"));
builder.Services.AddSingleton<OnlineStore.DAL.MongoDBRepository<OnlineStore.Domain.Models.Basket>>();
builder.Services.AddControllers();
var app = builder.Build();
app.UseRouting();
app.UseEndpoints (endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
