using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using Messages;

namespace Actor
{
    public class Actor
    {
        public async void Start()
        {
            Console.Write("Enter a unique ID number for the car: ");
            string id = Console.ReadLine();

            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {
                // Declare an exchange:
                var mainExchange = bus.ExchangeDeclare("MainExchange", ExchangeType.Direct);
				var mainQueue = bus.QueueDeclare("DirectQueue" + id);
				bus.Bind(mainExchange, mainQueue, "StatusRequest" + id);

                var broadcastExchange = bus.ExchangeDeclare("BroadcastExchange", ExchangeType.Fanout);
                var broadcastQueue = bus.QueueDeclare("FanoutQueue" + id);
                bus.Bind(broadcastExchange, broadcastQueue, "broadcast");

                // Create a message with Fanout (broadcasting):
                IMessage<ActorDeclarationMessage> message = MessagesFactory.GetMessage<ActorDeclarationMessage>(id, "");

                // Publish the message with the same routing key (the routing key, as mentioned above, is not used with Fanout exchange):
                bus.Publish(mainExchange, "ActorDeclaration", true, message);

                bus.Consume<StatusRequestMessage>(mainQueue, (statusRequest, info) =>
                {
                    Console.WriteLine(statusRequest.Body.Payload + " directly from " + statusRequest.Body.Sender);
                    IMessage<StatusResponseMessage> response = MessagesFactory.GetMessage<StatusResponseMessage>(id, "Actor status");
                    bus.Publish(mainExchange, "ActorStatus", true, response);
                    Console.WriteLine(id);
                });

                bus.Consume<StatusRequestMessage>(broadcastQueue, (statusRequest, info) =>
                {
                    Console.WriteLine(statusRequest.Body.Payload + " broadcast from " + statusRequest.Body.Sender);
                });
                await Task.Delay(1999999);
            }
        }
    }
}
