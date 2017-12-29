using EasyNetQ;
using System;
using EasyNetQ.Topology;
using Messages;
using System.Threading.Tasks;

namespace Dispatch
{
    public class Dispatch
    {
        public delegate void Callback(string arg);

        public async void Listen(Callback onActorDeclare)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {
                var main = bus.ExchangeDeclare("MainExchange", ExchangeType.Direct);
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
                await Task.Delay(1999999);
            }
        }

        public async void SendStatusRequest(string id, Callback callback)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {
                var main = bus.ExchangeDeclare("MainExchange", ExchangeType.Direct);
                var queue = bus.QueueDeclare("DirectQueue" + id);
                bus.Bind(main, queue, "ActorStatus");

                IMessage<StatusRequestMessage> request = MessagesFactory.GetMessage<StatusRequestMessage>("Master", "payload");
                bus.Consume<StatusResponseMessage>(queue, (message, info) =>
                {
                    callback(message.Body.Payload);
                });
                bus.Publish(main, "StatusRequest" + id, true, request);
                Console.WriteLine(id);
                await Task.Delay(1999999);
            }
        }

        public async void BroadcastStatusRequest(Callback callback)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {
                var fanout = bus.ExchangeDeclare("BroadcastExchange", ExchangeType.Fanout);
                IMessage<StatusRequestMessage> request = MessagesFactory.GetMessage<StatusRequestMessage>("Master", "payload");
                bus.Publish(fanout, "broadcast", true, request);
                await Task.Delay(1999999);
            }
        }
    }
}
