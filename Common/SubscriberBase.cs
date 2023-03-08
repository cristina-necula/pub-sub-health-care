using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Common
{
    public interface ISubscriberBase
    {
        public void Subscribe(string routingKey);
    }

    public class SubscriberBase : RabbitMqBaseTopicExchange
    {
        public SubscriberBase(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public void Subscribe(string routingKey)
        {
            _channel.QueueDeclare(queue: routingKey,
                                durable: false,
                                exclusive: false,
                                autoDelete: false);
            _channel.QueueBind(queue: routingKey,
                                exchange: ExchangeTopic,
                                routingKey: routingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = eventArgs.RoutingKey;
                Console.WriteLine($" [x] Received '{routingKey}':'{message}'");
            };

            _channel.BasicConsume(queue: routingKey,
                                 autoAck: true,
                                 consumer: consumer);

        }
    }
}
