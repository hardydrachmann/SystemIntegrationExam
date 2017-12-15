using System;
using System.Collections.Generic;
using System.Text;
using EasyNetQ;
using Messages;

namespace Middleman
{
    public class Middleman
    {
        public void Listen()
        {
            using (IBus bus = RabbitHutch.CreateBus("host=localhost;persistentMessages=false"))
            {
                bus.Receive<RequestMessage>("master_to_slave1_message_request", (message) =>
                {
                    Console.WriteLine("Message from master: " + message.Message);

                    ResponseMessage responseMessage = new ResponseMessage { Message = message.Message, Sender = "Middleman"};
                    bus.Send(message.Sender, responseMessage);
                });
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Middleman is ready!\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadLine();
            }
        }
    }
}
