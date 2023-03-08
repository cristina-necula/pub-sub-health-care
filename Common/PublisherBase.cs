using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Common
{
    public interface IPublisherBase
    {
        void Publish(string message, string routingKey);
    }

    public class PublisherBase : RabbitMqBaseTopicExchange, IPublisherBase
    {
        public PublisherBase(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public void Publish(string message, string routingKey)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            _channel.BasicPublish(exchange: "incoming_vital_signs",
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
