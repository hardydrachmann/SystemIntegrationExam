﻿using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Text;
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
                var exchange = bus.ExchangeDeclare("IAdvancedBus.Fanout", ExchangeType.Fanout);

                // Create a message with Fanout (broadcasting):
                var textMessage = new TextMessage { Text = "Test" };
                IMessage<TextMessage> message = new Message<TextMessage>(textMessage);
          
                // Publish the message with the same routing key (the routing key, as mentioned above, is not used with Fanout exchange):
                bus.Publish<TextMessage>(exchange, "routingKey", false, message);
            }
        }
    }
}
