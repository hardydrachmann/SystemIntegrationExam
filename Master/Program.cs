using System;

namespace Master
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Master is ready!\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter a text message\n> ");
            string message = Console.ReadLine();
            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("The message is ready to be send...\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Choose a send/receive path from the below options by entering the corresponding number\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("1. master -> middleman -> slave1 -> middleman -> master\n" +
                              "2. master -> middleman -> slave2 -> middleman -> master\n" +
                              "3. master -> middleman -> slave1 & slave2 -> middleman -> master\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter a number\n> ");
            int pathChoice = Int32.Parse(Console.ReadLine());
            
            Master master = new Master(message);
            master.sendMessage(pathChoice);
        }
    }
}
