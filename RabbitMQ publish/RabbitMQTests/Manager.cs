using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            //connection.AutoClose = true;   deprecated!
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
            
        }





        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
