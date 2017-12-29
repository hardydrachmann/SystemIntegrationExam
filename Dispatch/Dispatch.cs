using EasyNetQ;
using System;
using EasyNetQ.Topology;
using Messages;

namespace Dispatch
{
    public class Dispatch
    {
        public delegate void Callback(string arg);

        public void Listen(Callback onActorDeclare)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {
                var main = bus.ExchangeDeclare("MainExchange", ExchangeType.Direct);
                //var fanout = bus.ExchangeDeclare("BroadcastExchange", ExchangeType.Fanout);
                var queue = bus.QueueDeclare("MainQueue");
                bus.Bind(main, queue, "ActorDeclaration");

                // Consume synchronous consumer:
                bus.Consume<ActorDeclarationMessage>(queue, (message, info) =>
                {
                    onActorDeclare(message.Body.Sender);
                    //string id = message.Body.Text;
                    //var textMessage = new Message { Text = "Confirmed " + id };
                    //IMessage<Message> confirmMessage = new Message<Message>(textMessage);
                    //Console.WriteLine("Consumed: " + message.Body.Sender);
                    //bus.Publish(fanout, "broadcast", true, confirmMessage);
                });
                Console.Read();
            }
        }

        public void SendStatusRequest(string id, Callback callback)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {
                var main = bus.ExchangeDeclare("MainExchange", ExchangeType.Direct);
                var queue = bus.QueueDeclare("DirectQueue" + id);
                bus.Bind(main, queue, "ActorStatus");

                IMessage<StatusRequestMessage> request = MessagesFactory.GetMessage<StatusRequestMessage>("Master", "payload");
                bus.Publish(main, "StatusRequest" + id, true, request);
                bus.Consume<StatusResponseMessage>(queue, (message, info) =>
                {
                    Console.WriteLine("Received message from actor " + message.Body.Sender);
                    Console.WriteLine("Message value " + message.Body.Payload);
                    callback(message.Body.Payload);
                });
                Console.Read();
            }
        }
    }
}
