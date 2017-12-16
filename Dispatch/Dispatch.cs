using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Text;
using EasyNetQ.Topology;
using Messages;

namespace Dispatch
{
    public class Dispatch
    {
        public void doStuff()
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost").Advanced)
            {
                // Declare an exchange:
                var exchange = bus.ExchangeDeclare("IAdvancedBusExample.Fanout", ExchangeType.Fanout);

                // Create a message with Fanout (broadcasting):
                var textMessage = new TextMessage { Text = "Test" };
                IMessage<TextMessage> message = new Message<TextMessage>(textMessage);
                // Set an expiration time on the message itself (can be used instead of on the queue):
                // message.Properties.Expiration = "1000";

                // Publish the message with the same routing key (the routing key, as mentioned above, is not used with Fanout exchange):
                bus.Publish<TextMessage>(exchange, "routingKey", false, message);
            }
        }
    }
}
