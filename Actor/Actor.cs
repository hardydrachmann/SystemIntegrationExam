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
        public void Start()
        {
            Console.Write("Enter a unique ID number for the car: ");
            int id = Int32.Parse(Console.ReadLine());
            Console.Clear();
            Console.Write("Car with ID " + id + " online...");

            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {

                // Declare an exchange:
                var main = bus.ExchangeDeclare("MainExchange", ExchangeType.Direct);
                var fanout = bus.ExchangeDeclare("BroadcastExchange", ExchangeType.Fanout);

                // Create a message with Fanout (broadcasting):
                var textMessage = new TextMessage { Text = id.ToString() };
                IMessage<TextMessage> message = new Message<TextMessage>(textMessage);

                // Publish the message with the same routing key (the routing key, as mentioned above, is not used with Fanout exchange):
                bus.Publish<TextMessage>(main, "DeclareActor", true, message);

                var queue = bus.QueueDeclare("FanoutQueue"  + id);
                bus.Bind(fanout, queue, "broadcast");
                bus.Consume<TextMessage>(queue, (content, info) =>
                {
                    Console.WriteLine("\n" + content.Body.Text);
                });
                Console.ReadKey();
            }
        }
    }
}
