using System;
using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace RabbitMQTests
{
    public class Manager : IDisposable
    {
        private readonly IModel channel;

        public Manager()
        {
            var connectionFactory = new ConnectionFactory { Uri = new Uri(Constants.RabbitMqUri) };
            var connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
        }

        public void Send(Modelo modelo)
        {
            channel.ExchangeDeclare(exchange: Constants.TestExchange, type: ExchangeType.Direct);
            channel.QueueDeclare(queue: Constants.TestQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: Constants.TestQueue, exchange: Constants.TestExchange, routingKey: "");

            var serializedCommand = JsonConvert.SerializeObject(modelo);

            var messageProperties = channel.CreateBasicProperties();
            messageProperties.ContentType = Constants.JsonMimeType;

            channel.BasicPublish(
                exchange: Constants.TestExchange,
                routingKey: "",
                basicProperties: messageProperties,
                body: Encoding.UTF8.GetBytes(serializedCommand));

            modelo.Print("Enviado para o Rabbit");

        }


        public void Listen()
        {
            channel.QueueDeclare(queue: Constants.TestQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new Consumer(this);

            channel.BasicConsume(
                queue: Constants.TestQueue,
                autoAck: false,
                consumer: consumer);



        }

        public void SendAck(ulong deliveryTag)
        {
            channel.BasicAck(deliveryTag: deliveryTag, multiple: false);
        }

        public void Dispose()
        {
            channel.Close();
            channel.Dispose();
        }
    }
}
