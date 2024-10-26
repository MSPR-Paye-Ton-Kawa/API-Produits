using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

public class RabbitMQHostedService : IHostedService
{
    private readonly StockCheckConsumer _stockCheckConsumer;
    public RabbitMQHostedService(StockCheckConsumer stockCheckConsumer)
    {
        _stockCheckConsumer = stockCheckConsumer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _stockCheckConsumer.StartConsuming();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _stockCheckConsumer.Dispose();
        return Task.CompletedTask;
    }
}
