using CitizenTech.Api.Data;
using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// A senha NUNCA fica no repositório: a connection string vem da variável de ambiente ORACLE_CONNECTION
// (ou de dotnet user-secrets). Veja o README.
var connectionString =
    Environment.GetEnvironmentVariable("ORACLE_CONNECTION")
    ?? builder.Configuration.GetConnectionString("Oracle");

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Defina a variável de ambiente ORACLE_CONNECTION (veja o README).");

// Compatibilidade com Oracle 11g/12.1: o provider gera sequence + trigger em vez de coluna IDENTITY.
// (Também funciona em versões mais novas do banco.)
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseOracle(connectionString));

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Aplica as Migrations pendentes ao iniciar (cria as tabelas NET_* no Oracle)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.MapControllers();
app.Run();