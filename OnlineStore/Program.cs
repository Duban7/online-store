using Microsoft.AspNetCore.Diagnostics;
using OnlineStore.BLL.AccountService.Exceptions;
using OnlineStore.DI;
using OnlineStore.middleware;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

DependencyContainer.RegisterDependency(builder.Services, builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddlaware>();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints (endpoints =>
{
    endpoints.MapControllers();
});

app.Run();