using RoyalVillaApi.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Genera el documento OpenAPI
builder.Services.AddOpenApi();
// Agrega el DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Agrega mapperly
var assembly = typeof(Program).Assembly;

// Busca todas las clases que terminen en "Mapper" y las registra automáticamente
var mapperTypes = assembly.GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Mapper"))
    .ToList();

foreach (var mapperType in mapperTypes)
    builder.Services.AddSingleton(mapperType);
// fin de mapperly


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Expone el documento OpenAPI
    app.MapOpenApi();

    // Interfaz de Scalar
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();