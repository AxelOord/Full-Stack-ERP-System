using Autofac;
using Autofac.Extensions.DependencyInjection;
using Scalar.AspNetCore;
using Persistence;
using Api;
using Api.Infrastructure;
using Domain.Shared.ApiResponse;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services that don't need Autofac
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<LinkBuilder>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .AllowAnyOrigin();
        });
});

// Register persistence services and other non-Autofac services
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Configure Autofac as the DI provider
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Configure the Autofac container
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.ConfigureAutofacContainer();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseCors("AllowAll");
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



app.UseStatusCodePages();

await app.RunAsync();
