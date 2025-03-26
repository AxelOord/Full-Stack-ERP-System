using App;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.Extensions;
using Infrastructure.Middleware;
using Persistence;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
        .InstallServicesFromAssemblies(
            builder.Configuration,
            App.AssemblyReference.Assembly,
            Persistence.AssemblyReference.Assembly)
        .InstallModulesFromAssemblies(
            builder.Configuration,
            Warehouse.Infrastructure.AssemblyReference.Assembly);

// TDOD: should be conf?
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .AllowAnyOrigin();
        });
}); // TODO: this should be conf

builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer<NullableSchemaTransformer>();
    options.AddSchemaTransformer<RecursiveReferenceSchemaTransformer>();
});

builder.Services.AddRouting(options => options.LowercaseUrls = true); // TODO: should be conf

// Configure Autofac as the DI provider
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.ConfigureAutofacContainer();
});

var app = builder.Build();

app.MapHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.AddServer(new ScalarServer("https://localhost:7013", "https"));
        options.AddServer(new ScalarServer("http://localhost:5135", "http"));
    });
    app.UseCors("AllowAll"); // TODO: this should be conf
}

app.UseMiddleware<ExceptionHandlingMiddleware>(); // TODO: need a way to auto register middleware

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseStatusCodePages();

await app.RunAsync();
