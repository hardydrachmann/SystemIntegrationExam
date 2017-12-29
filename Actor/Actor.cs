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
            string id = Console.ReadLine();

            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {
                // Declare an exchange:
                var main = bus.ExchangeDeclare("MainExchange", ExchangeType.Direct);
                var fanout = bus.ExchangeDeclare("BroadcastExchange", ExchangeType.Fanout);

                // Create a message with Fanout (broadcasting):
                IMessage<ActorDeclarationMessage> message = MessagesFactory.GetMessage<ActorDeclarationMessage>(id, "");

                // Publish the message with the same routing key (the routing key, as mentioned above, is not used with Fanout exchange):
                bus.Publish<ActorDeclarationMessage>(main, "ActorDeclaration", true, message);

                var queue = bus.QueueDeclare("FanoutQueue" + id);
                bus.Bind(fanout, queue, "broadcast");

                //bus.Consume<Message>(queue, (content, info) =>
                //{
                //    Console.WriteLine(content.Body.Sender);
                //});
                Console.ReadKey();
            }
        }
    }
}
