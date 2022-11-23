using OnlineStore.DAL;
using OnlineStore.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("OnlineStoreDatabase"));
DependencyContainer.RegisterDependency(builder.Services);
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