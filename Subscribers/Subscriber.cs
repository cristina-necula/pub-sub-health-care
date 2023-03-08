using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace SubscriberApp
{
    public class Subscriber : SubscriberBase, IHostedService
    {
        public Subscriber(ConnectionFactory connectionFactory, IConfiguration config) : base(connectionFactory)
        {
            var routingKey = config["routingKey"];
            if (routingKey == null)
            {
                throw new ArgumentNullException(nameof(routingKey));
            }
            Subscribe(routingKey);
            Console.WriteLine($"Subscribed to {routingKey}. Waiting for new messages...");
        }

        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}
