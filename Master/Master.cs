using System;
using System.Collections.Generic;
using System.Text;
using EasyNetQ;
using Messages;

namespace Master
{
    public class Master
    {
        private string message;

        public Master(string message) { this.message = message; }

        public void sendMessage(int pathChoice)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            using (IBus bus = RabbitHutch.CreateBus("host=localhost;persistentMessages=false"))
            {
                RequestMessage requestMessage = new RequestMessage { Message = message };

                // Send message
                switch (pathChoice)
                {
                    case 1:
                        Console.WriteLine("Path selected: master -> middleman -> slave1 -> middleman -> master");
                        bus.Send("master_to_slave1_message_request", requestMessage);
                        break;
                    case 2:
                        Console.WriteLine("Path selected: master -> middleman -> slave2 -> middleman -> master");
                        bus.Send("master_to_slave2_message_request", requestMessage);
                        break;
                    case 3:
                        Console.WriteLine("Path selected: master -> middleman -> slave1 & slave2 -> middleman -> master");
                        bus.Send("master_to_all_message_request", requestMessage);
                        break;
                }
                
                // Receive message
                bus.Receive<ResponseMessage>("Middleman", (message) =>
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nThe message:\n" + message + "\n...was received again from "+ message.Sender);
                    Console.ForegroundColor = ConsoleColor.White;
                });

                Console.ReadLine();
            }
        }
    }
}
