using RabbitMQ.Client;

namespace Common
{
    public abstract class RabbitMqBaseTopicExchange : IDisposable
    {
        protected readonly string ExchangeTopic = "incoming_vital_signs";

        protected IConnection _connection;
        protected IModel _channel;

        private readonly ConnectionFactory _connectionFactory;

        protected RabbitMqBaseTopicExchange(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            if (_connection == null || _connection.IsOpen == false)
            {
                _connection = _connectionFactory.CreateConnection();
            }

            if (_channel == null || _channel.IsOpen == false)
            {
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: ExchangeTopic,
                                    type: ExchangeType.Topic,
                                    durable: true,
                                    autoDelete: false);
            }
        }

        public void Dispose()
        {
            try
            {
                _channel?.Close();
                _channel?.Dispose();

                _connection?.Close();
                _connection?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "Cannot dispose RabbitMQ channel or connection");
            }
        }
    }
}