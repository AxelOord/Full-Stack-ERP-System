using System.Reflection;
using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.Extensions;
using Infrastructure.Middleware;
using Microsoft.OpenApi.Models;
using Persistence;
using Scalar.AspNetCore;
using Shared;
using Shared.Results.Response;

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
builder.Services.AddScoped<LinkBuilder>(); // FIXME: should not be scoped but something like a extension method

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
  // TODO: create IOpenApiSchemaTransformer to inject here
  options.AddSchemaTransformer((schema, context, cancellationToken) =>
  {
      var type = context.JsonTypeInfo.Type;
      foreach (var property in type.GetProperties())
      {
        if (property.GetCustomAttribute<NullableProp>() != null)
        {
          var schemaProperties = new Dictionary<string, OpenApiSchema>(schema.Properties, StringComparer.OrdinalIgnoreCase);

          schemaProperties.TryGetValue(property.Name, out var prop);

          if (prop != null && prop.Nullable && (string)prop.Annotations.First().Value == property.PropertyType.Name)
          {
            Console.WriteLine($"Fixing nullable reference for: {property.Name}"); // We really need a debug logger / logger setup
            prop.Nullable = true;
            prop.Reference = new OpenApiReference { Id = property.PropertyType.Name, Type = ReferenceType.Schema };
          }
        }
      }

      return Task.CompletedTask;
  });
});

builder.Services.AddAutoMapper(typeof(MappingProfile)); // TODO: should be auto discovered from multiple assemblies
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
