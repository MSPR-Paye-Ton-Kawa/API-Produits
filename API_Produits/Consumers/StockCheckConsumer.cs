using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using API_Produits.Services;

namespace API_Produits.Consumers
{
    public class StockCheckConsumer
{
    private readonly IStockService _stockService;

    public StockCheckConsumer(IStockService stockService)
    {
        _stockService = stockService;
    }

    public void Start()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "stock_check_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var stockCheckRequest = JsonSerializer.Deserialize<StockCheckRequest>(message);

            if (stockCheckRequest != null)
            {
                bool stockAvailable = await _stockService.IsStockAvailable(stockCheckRequest.ProductId, stockCheckRequest.Quantity);

                // Créer un message de réponse
                var responseMessage = new StockCheckResponse
                {
                    ProductId = stockCheckRequest.ProductId,
                    IsAvailable = stockAvailable
                };

                var responseJson = JsonSerializer.Serialize(responseMessage);
                var responseBody = Encoding.UTF8.GetBytes(responseJson);

                // Publier le message de réponse dans la file d'attente de l'API de commandes
                var responseQueueName = "stock_check_response_queue"; // nom de la file de réponse
                channel.QueueDeclare(queue: responseQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.BasicPublish(exchange: "", routingKey: responseQueueName, body: responseBody);
            }
        };

        channel.BasicConsume(queue: "stock_check_queue", autoAck: true, consumer: consumer);
    }

public class StockCheckRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class StockCheckResponse
    {
        public int ProductId { get; set; }
        public bool IsAvailable { get; set; }
    }
}
}