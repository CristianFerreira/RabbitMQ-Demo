using System;

namespace RabbitMQTests
{
    public class Program
    {
        static void Main(string[] args)
        {
            Modelo modelo = new Modelo { Id = 1, Name = "Adriano Maurmann" };
            modelo.Print("Inicializado modelo");
            Manager manager = new Manager();
            manager.Send(modelo);
            manager.Listen();
            Console.ReadKey();
        }
    }
}
