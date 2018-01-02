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
                // Declare queues and exchanges:
                // Main (direct).
                var mainExchange = bus.ExchangeDeclare("MainExchange", ExchangeType.Direct);
                var mainQueue = bus.QueueDeclare("StatusQueue" + id);
                bus.Bind(mainExchange, mainQueue, "StatusRequest" + id);

                // Objective (direct).
                var objectiveExchange = bus.ExchangeDeclare("ObjectiveExchange", ExchangeType.Direct);
                var objectiveQueue = bus.QueueDeclare("ObjectiveQueue" + id);
                bus.Bind(objectiveExchange, objectiveQueue, "ObjectiveRequest" + id);

                // Broadcast (fanout).
                var broadcastExchange = bus.ExchangeDeclare("BroadcastExchange", ExchangeType.Fanout);
                var broadcastQueue = bus.QueueDeclare("BroadcastQueue" + id);
                bus.Bind(broadcastExchange, broadcastQueue, "Broadcast");

                var message = MessagesFactory.GetMessage<ActorDeclarationMessage>(id, "");
                bus.Publish(mainExchange, "ActorDeclaration", true, message);

                // Consume messages using message handlers.
                // Status request:
                Action<IMessage<StatusRequestMessage>, MessageReceivedInfo> handleStatusRequest = (request, info) =>
                {
                    var req = request.Body;
                    Console.WriteLine("Received status request from " + req.Sender);
                    string status = arbitraryStatus();
                    Console.WriteLine("Declaring status: " + status);
                    IMessage<StatusResponseMessage> response = MessagesFactory.GetMessage<StatusResponseMessage>(id, status);
                    bus.Publish(mainExchange, "ActorStatus", true, response);
                };
                bus.Consume<StatusRequestMessage>(mainQueue, handleStatusRequest);
                bus.Consume<StatusRequestMessage>(broadcastQueue, handleStatusRequest);

                // Objective request:
                Action<IMessage<ObjectiveRequestMessage>, MessageReceivedInfo> handleObjectiveRequest = (request, info) =>
                {
                    var req = request.Body;
                    Console.WriteLine("Received objective from " + req.Sender + ": " + req.Payload);
                };
                bus.Consume<ObjectiveRequestMessage>(objectiveQueue, handleObjectiveRequest);
                bus.Consume<ObjectiveRequestMessage>(broadcastQueue, handleObjectiveRequest);

                await Task.Delay(1999999);
            }
        }

        private string arbitraryStatus()
        {
            return new Random().Next(0, 2) > 0 ? "active" : "inactive";
        }
    }
}
