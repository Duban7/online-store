using Microsoft.AspNetCore.Diagnostics;
using OnlineStore.DAL;
using OnlineStore.DI;
using OnlineStore.Options;

var builder = WebApplication.CreateBuilder(args);

DependencyContainer.RegisterDependency(builder.Services, builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async( context) =>
    {
        context.Response.StatusCode = StatusCodes.Status502BadGateway;
        context.Response.ContentType = "application/json";
        IExceptionHandlerFeature? exceptionhandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        context.RequestServices.GetService<ILogger<Program>>().LogError(exceptionhandlerPathFeature.Error.Message);
        await context.Response.WriteAsJsonAsync(exceptionhandlerPathFeature.Error.Message);
    });
});

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