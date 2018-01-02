using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dispatch
{
    public class MainMenu
    {
        private Dispatch dispatch = new Dispatch();
        private List<string> actors = new List<string>();

        public MainMenu()
        {
            new Task(() => dispatch.Listen(HandleActorID, (payload) =>
            {
                Console.WriteLine(payload);
            })).Start();
        }

        public void HandleActorID(string id)
        {
            actors.Add(id);
        }

        public void Start()
        {
            while (true)
            {
                Console.Clear();

                // Print with main welcome message.
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
                getActors();
                // Print with input choices.
                Console.WriteLine("\n\n   1. Request status of all cars");
                Console.WriteLine("   2. Request status of a specific car");
                Console.WriteLine("   3. Send an objective command to all cars");
                Console.WriteLine("   4. Send an objective command to a specific car\n");
                Console.Write("   0. Quit the application\n\n> ");

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
            dispatch.BroadcastStatusRequest();
        }

        private void getCarStatus()
        {
            Console.Clear();
            getActors();
            Console.Write("\nEnter ID for the actor to get status from\n> ");
            string id = Console.ReadLine();
            dispatch.SendStatusRequest(id);
            Console.Write("\n\nPress enter to return to the main menu...");
            Console.ReadLine();
        }

        private void broadcastCommand()
        {
            Console.Clear();
            Console.Write("\nEnter a command to send to all actors\n> ");
            string broadcastInput = Console.ReadLine();
            dispatch.BroadcastObjectiveRequest(broadcastInput);
        }

        private void directCommand()
        {
            Console.Clear();
            getActors();
            Console.Write("\nEnter ID for the actor to send a message to\n> ");
            string id = Console.ReadLine();
            Console.Write("\nEnter a command to send to actor " + id + "\n> ");
            string directInput = Console.ReadLine();
            dispatch.SendObjectiveRequest(id, directInput);
        }

        private void exit()
        {
            Environment.Exit(0);
        }

        private void getActors()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n___________________________________________________________________\n");
            foreach (var actor in actors)
            {
                Console.WriteLine("Actor ID: " + actor);
            }
            Console.WriteLine("\n___________________________________________________________________");
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }
}
