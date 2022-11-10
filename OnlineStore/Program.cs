var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<OnlineStore.DatabaseSettings>(builder.Configuration.GetSection("OnlineStoreDatabase"));
builder.Services.AddSingleton<OnlineStore.Services.RegUserCollcetion>();
builder.Services.AddControllers();
var app = builder.Build();
app.UseRouting();
app.UseEndpoints (endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
