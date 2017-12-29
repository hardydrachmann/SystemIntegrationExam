using EasyNetQ;
using System;
using EasyNetQ.Topology;
using Messages;

namespace Dispatch
{
    public class Dispatch
    {
        public void Start()
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {
                // Declare an exchange:
                var main = bus.ExchangeDeclare("MainExchange", ExchangeType.Direct);
                var fanout = bus.ExchangeDeclare("BroadcastExchange", ExchangeType.Fanout);

                // Declare a queue:
                var queue = bus.QueueDeclare("MainQueue");

                // Bind the queue:
                bus.Bind(main, queue, "ActorDeclaration");

                // Consume synchronous consumer:
                bus.Consume<ActorDeclarationMessage>(queue, (message, info) =>
                {
                    //string id = message.Body.Text;
                    //var textMessage = new Message { Text = "Confirmed " + id };
                    //IMessage<Message> confirmMessage = new Message<Message>(textMessage);
                    Console.WriteLine("Consumed: " + message.Body.Sender);
                    //bus.Publish(fanout, "broadcast", true, confirmMessage);
                });
            }
        }
    }
}
