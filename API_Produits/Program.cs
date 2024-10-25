using Microsoft.EntityFrameworkCore;
using API_Produits.Models;
using API_Produits.Consumers;
using API_Produits.Services;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ajoutez le consommateur comme un service singleton
builder.Services.AddScoped<StockCheckConsumer>();


builder.Services.AddScoped<IStockService, StockService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Utiliser le middleware Prometheus
app.UseMetricServer();  // Ajoute un endpoint pour les métriques Prometheus
app.UseHttpMetrics();   // Collecte les métriques HTTP (requêtes, latence, etc.)

// Configure the HTTP request pipeline.
// Always use Swagger in both development and production
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Produits V1");
});

// Temporarily disable HTTPS redirection
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


// Démarrer le consommateur de stock
using (var scope = app.Services.CreateScope())
{
    var stockCheckConsumer = scope.ServiceProvider.GetRequiredService<StockCheckConsumer>();
    stockCheckConsumer.Start();
}

app.Run();
