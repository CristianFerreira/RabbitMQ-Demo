using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQTests
{
    public class Program
    {
        static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory { Uri = new Uri(Constants.RabbitMqUri) };
            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                channel.BasicAck(ea.DeliveryTag, true);
            }; 
            channel.BasicConsume(queue: "IDP_NotificationQueue", autoAck: false, consumer: consumer);

        }
    }
}
