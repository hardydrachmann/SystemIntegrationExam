using System;
using System.Collections.Generic;
using System.Text;

namespace Dispatch
{
    public class MainMenu
    {
        private Dispatch dispatch;

        public MainMenu()
        {
            dispatch = new Dispatch();
            dispatch.Start();
        }

        public void Start()
        {
            while (true)
            {
                Console.Clear();

                //// Print with main welcome message.
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n*******************************************************************");
                Console.WriteLine("*                                                                 *");
                Console.WriteLine("*                     Welcome to the DISPATCH                     *");
                Console.WriteLine("*                         (version 1.0.0)                         *");
                Console.WriteLine("*                                                                 *");
                Console.WriteLine("*******************************************************************");
                Console.WriteLine("*                                                                 *");
                Console.WriteLine("* Enter a number from below to choose the task & then press Enter *");
                Console.WriteLine("*                                                                 *");
                Console.WriteLine("*******************************************************************");
                // Generic output section.
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nCar ID\t\t\tCar Location\t\tCar Status");
                Console.WriteLine("___________________________________________________________________");
                Console.WriteLine("1\t\t\tcall method here");
                // Print with input choices.
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\n   1. Request status of all cars");
                Console.WriteLine("   2. Request status of a specific car");
                Console.WriteLine("   3. Send an objective command to all cars");
                Console.WriteLine("   4. Send an objective command to a specific car");
                Console.WriteLine("   0. Quit the application\n");
                Console.Write("?> ");

                string userChoice = Console.ReadLine();
                switch (userChoice)
                {
                    case "1":
                        getAllCarsStatus();
                        break;
                    case "2":
                        getCarStatus();
                        break;
                    case "3":
                        broadcastCommand();
                        break;
                    case "4":
                        directCommand();
                        break;
                    default:
                        exit();
                        break;
                }
            }
        }

        private void getAllCarsStatus()
        {
            Console.WriteLine("1");
        }

        private void getCarStatus()
        {
            Console.WriteLine("2");
        }

        private void broadcastCommand()
        {
            Console.WriteLine("3");
        }

        private void directCommand()
        {
            Console.WriteLine("4");
        }

        private void exit()
        {
            Console.Clear();
            Environment.Exit(0);
        }
    }
}
