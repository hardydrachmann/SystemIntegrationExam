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
                bus.Bind(main, queue, "DeclareActor");

                // Consume synchronous consumer:
                bus.Consume<TextMessage>(queue, (message, info) =>
                {
                    string id = message.Body.Text;
                    var textMessage = new TextMessage { Text = "Confirmed " + id };
                    IMessage<TextMessage> confirmMessage = new Message<TextMessage>(textMessage);
                    Console.WriteLine("Consumed message from car with ID " + message.Body.Text);
                    bus.Publish(fanout, "broadcast", true, confirmMessage);
                });
            }
        }
    }
}
