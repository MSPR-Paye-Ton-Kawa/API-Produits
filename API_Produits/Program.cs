using Microsoft.EntityFrameworkCore;
using API_Produits.Models;
using API_Produits.Services;
using Prometheus;
using Serilog;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Configuration de Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuration de RabbitMQ
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory() { HostName = "localhost" }; 
    IConnection connection = null;
    var logger = sp.GetRequiredService<ILogger<IConnection>>();

    try
    {
        connection = factory.CreateConnection();

        if (connection.IsOpen)
        {
            logger.LogInformation("RabbitMQ connection created successfully and is open.");
        }
        else
        {
            logger.LogWarning("RabbitMQ connection was created but is not open.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to create RabbitMQ connection.");
        throw; 
    }

    return connection;
});

builder.Services.AddSingleton<StockCheckConsumer>();
builder.Services.AddHostedService<RabbitMQHostedService>();
builder.Services.AddScoped<IStockService, StockService>();

// Configuration de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Utiliser le middleware Prometheus
app.UseMetricServer();  // Ajoute un endpoint pour les m�triques Prometheus
app.UseHttpMetrics();   // Collecte les m�triques HTTP (requ�tes, latence, etc.)

// Always use Swagger in both development and production
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Produits V1");
});

app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

// D�marrer le consommateur de stock
using (var scope = app.Services.CreateScope())
{
    var stockCheckConsumer = scope.ServiceProvider.GetRequiredService<StockCheckConsumer>();
    stockCheckConsumer.Start();
}

app.Run();
