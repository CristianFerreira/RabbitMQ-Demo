using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQTests
{
    public class Modelo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public void Print(string message)
        {
            Console.WriteLine(string.Format("{0} => Id={1} Name={2}", message, Id, Name));
        }
    }
}
