using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using API_Produits.Services;
using Prometheus;
public class StockCheckConsumer : IDisposable
{
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<StockCheckConsumer> _logger;
        private readonly IServiceProvider _serviceProvider;
    private static readonly Counter _messagesConsumed = Metrics.CreateCounter("stock_check_requests_received", "Total number of Stock Check requests received");

    public StockCheckConsumer(IConnection connection, ILogger<StockCheckConsumer> logger, IServiceProvider serviceProvider)
        {
             _logger = logger;
            _serviceProvider = serviceProvider;

            try
            {
                _connection = connection;
                _channel = _connection.CreateModel();
                _logger.LogInformation("Connected to RabbitMQ for stock check.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to initialize StockCheck consumer: {ex.Message}");
            }
        }

        public void StartConsuming()
        {
            try
            {
                _channel.QueueDeclare(queue: "stock_check_request",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                _logger.LogInformation("Queue declared: stock_check_request");

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var stockCheckRequest = JsonSerializer.Deserialize<StockCheckRequest>(message);
                        _logger.LogInformation($"Stock check request received: {message}");

                        ProcessStockCheckRequest(stockCheckRequest);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error processing stock check request: {ex.Message}");
                    }
                };

                _channel.BasicConsume(queue: "stock_check_request", autoAck: true, consumer: consumer);

            _messagesConsumed.Inc();
            _logger.LogInformation("Stock check consumer started and attached to queue: stock_check_request");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to start consuming stock check requests: {ex.Message}");
            }
        }

    private async Task ProcessStockCheckRequest(StockCheckRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var stockService = scope.ServiceProvider.GetRequiredService<IStockService>();

            bool isStockAvailable = true;

            foreach (var item in request.Items)
            {
                bool available = await stockService.IsStockAvailable(item.ProductId, item.Quantity);
                if (!available)
                {
                    isStockAvailable = false;
                    _logger.LogWarning($"Insufficient stock for ProductId: {item.ProductId}. Required: {item.Quantity}.");
                }
            }

            SendStockCheckResponse(request.RequestId, isStockAvailable);

            // If stock available : update stock
            if (isStockAvailable)
            {
                foreach (var item in request.Items)
                {
                    await stockService.UpdateStockQuantity(item.ProductId, item.Quantity);
                    _logger.LogInformation($"Stock updated for ProductId: {item.ProductId}. Quantity: {item.Quantity}.");
                }
            }


            var response = new StockCheckResponse
            {
                RequestId = request.RequestId,
                IsStockAvailable = isStockAvailable
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            _logger.LogInformation($"StockCheckResponse: {jsonResponse}");
        }
    }
    private void SendStockCheckResponse(string requestId, bool isStockAvailable)
        {
            var response = new StockCheckResponse
            {
                RequestId = requestId, 
                IsStockAvailable = isStockAvailable
            };
        var jsonResponse = JsonSerializer.Serialize(response);
        var body = Encoding.UTF8.GetBytes(jsonResponse);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true; 

            _channel.BasicPublish(exchange: "",
                                   routingKey: "stock_check_response_queue",
                                   basicProperties: properties,
                                   body: body);

            _logger.LogInformation($"Sent stock check response for Request ID: {requestId}, IsStockAvailable: {isStockAvailable}");
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }

