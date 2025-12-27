using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Paye.Infrastructure.DependencyInjection;
using Paye.Application;
using Serilog;
using Paye.Api.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

// Optional integrations are no-ops in the template workspace
builder.Services.AddApplication();
InfrastructureServiceRegistration.AddInfrastructure(builder.Services, builder.Configuration);

// Authorization is configured using Roles directly in [Authorize] attributes

var app = builder.Build();

// Register global exception handler
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Paye API V1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Paye API Documentation";
    });

    // Redirect root URL (/) to Swagger UI
    app.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger", permanent: false);
        return Task.CompletedTask;
    });
}

app.UseHttpsRedirection();

// CRITICAL: Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();