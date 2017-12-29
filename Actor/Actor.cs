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
                var mainExchange = bus.ExchangeDeclare("MainExchange", ExchangeType.Direct);
                //var fanout = bus.ExchangeDeclare("BroadcastExchange", ExchangeType.Fanout);

				var mainQueue = bus.QueueDeclare("DirectQueue" + id);
				//var broadcastQueue = bus.QueueDeclare("FanoutQueue" + id);
				//bus.Bind(fanout, broadcastQueue, "broadcast");
				bus.Bind(mainExchange, mainQueue, "StatusRequest" + id);

                // Create a message with Fanout (broadcasting):
                IMessage<ActorDeclarationMessage> message = MessagesFactory.GetMessage<ActorDeclarationMessage>(id, "");

                // Publish the message with the same routing key (the routing key, as mentioned above, is not used with Fanout exchange):
                bus.Publish(mainExchange, "ActorDeclaration", true, message);

                bus.Consume<StatusRequestMessage>(mainQueue, (statusRequest, info) =>
                {
                    Console.WriteLine(statusRequest.Body.Payload + " from " + statusRequest.Body.Sender);
                    IMessage<StatusResponseMessage> response = MessagesFactory.GetMessage<StatusResponseMessage>(id, "Actor status");
                    bus.Publish(mainExchange, "ActorStatus", true, response);
                    Console.WriteLine(id);
                });
                Console.Read();
            }
        }
    }
}
