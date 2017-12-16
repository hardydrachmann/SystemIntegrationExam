using System;
using System.Collections.Generic;
using System.Text;
using EasyNetQ;
using EasyNetQ.Topology;
using Messages;

namespace Actor
{
    public class Actor
    {
        public void doStuff()
        {
            Console.Write("enter a unique number for the consumer: ");
            int consumerId = Int32.Parse(Console.ReadLine());

            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {
                // Declare an exchange:
                var exchange = bus.ExchangeDeclare("IAdvancedBus.Fanout", ExchangeType.Fanout);

                // Declare a queue:
                var queue = bus.QueueDeclare("IAdvancedBus" + consumerId);

                // Bind the queue:
                bus.Bind(exchange, queue, "routingKey");

                // Consume synchronous consumer:
                bus.Consume<TextMessage>(queue, (message, info) => Console.WriteLine("Consumed: " + message.Body.Text));
                
                Console.Write("\nconsumer is running...");
                Console.ReadKey();
            }
        }
    }
}
