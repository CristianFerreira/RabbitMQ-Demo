using System;
using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace RabbitMQTests
{
    public class Consumer : DefaultBasicConsumer
    {
        private readonly Manager _manager;

        public Consumer(Manager manager)
        {
            this._manager = manager;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            if (properties.ContentType!=Constants.JsonMimeType)
            {
                throw new ArgumentException("Content Type recebido não pode ser tratado");
            }

            var message = Encoding.UTF8.GetString(body);
            Modelo modelo = JsonConvert.DeserializeObject<Modelo>(message);

            _manager.SendAck(deliveryTag);

            modelo.Print("Lido do Rabbit");
            
        }
    }
}
