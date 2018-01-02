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

        public async void Listen(Callback onActorDeclare, Callback onStatusResponse)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {
                var main = bus.ExchangeDeclare("MainExchange", ExchangeType.Direct);
                var queue = bus.QueueDeclare("MainQueue");
                bus.Bind(main, queue, "ActorDeclaration");

                bus.Consume<ActorDeclarationMessage>(queue, (message, info) =>
                {
                    // Whenever an actor declares it's existence, handle with sender id.
                    onActorDeclare(message.Body.Sender);
                });

                bus.Consume<StatusResponseMessage>(queue, (message, info) =>
                {
                    // Whenever an actor sends a status response, handle response.
                    onStatusResponse(message.Body.Payload);
                });

                await Task.Delay(1999999);
            }
        }

        public void SendStatusRequest(string id)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {
                var main = bus.ExchangeDeclare("MainExchange", ExchangeType.Direct);
                var queue = bus.QueueDeclare("StatusQueue" + id);
                bus.Bind(main, queue, "ActorStatus");

                var request = MessagesFactory.GetMessage<StatusRequestMessage>("Master", "payload");
                bus.Publish(main, "StatusRequest" + id, true, request);
            }
        }

        public void BroadcastStatusRequest()
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {
                var fanout = bus.ExchangeDeclare("BroadcastExchange", ExchangeType.Fanout);
                var request = MessagesFactory.GetMessage<StatusRequestMessage>("Master", "payload");
                bus.Publish(fanout, "Broadcast", true, request);
            }
        }

        public void SendObjectiveRequest(string id, string objective)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {
                var exchange = bus.ExchangeDeclare("ObjectiveExchange", ExchangeType.Direct);
                var request = MessagesFactory.GetMessage<ObjectiveRequestMessage>("Master", objective);
                bus.Publish(exchange, "ObjectiveRequest" + id, true, request);
            }
        }

        public void BroadcastObjectiveRequest(string objective)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {
                var fanout = bus.ExchangeDeclare("BroadcastExchange", ExchangeType.Fanout);
                var request = MessagesFactory.GetMessage<ObjectiveRequestMessage>("Master", objective);
                bus.Publish(fanout, "Broadcast", true, request);
                Console.WriteLine("Broadcast objective");
            }
        }
    }
}
