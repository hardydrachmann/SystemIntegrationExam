using System;
using System.Net.Mime;

namespace Dispatch
{
    public class Program
    {
        static void Main(string[] args)
        {
            //// Print with main welcome message.
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine("\n*******************************************************************");
            //Console.WriteLine("*                                                                 *");
            //Console.WriteLine("*                     Welcome to the DISPATCH                     *");
            //Console.WriteLine("*                         (version 1.0.0)                         *");
            //Console.WriteLine("*                                                                 *");
            //Console.WriteLine("*******************************************************************");
            //Console.WriteLine("*                                                                 *");
            //Console.WriteLine("* Enter a number from below to choose the task & then press Enter *");
            //Console.WriteLine("*                                                                 *");
            //Console.WriteLine("*******************************************************************");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.WriteLine("\nCar ID\t\t\tCar Location\t\tCar Status");
            //Console.WriteLine("___________________________________________________________________");
            //// MOCK PRINT FOR SYNOPSIS.
            //Console.WriteLine("1\t\t\tEsbjerg\t\t\tAvailable");
            //Console.WriteLine("2\t\t\tVarde\t\t\tOccupied");
            //Console.WriteLine("3\t\t\tRibe\t\t\tOccupied");
            //// Print with input choices.
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine("\n\n   1. Show complete information of all the cars");
            //Console.WriteLine("   2. Show complete information on a specific car by its 'Car ID'");
            //Console.WriteLine("   3. Send a command to all the cars");
            //Console.WriteLine("   4. Send a command to a specific car by its 'Car ID'");
            //Console.WriteLine("   5. View the general information page");
            //Console.WriteLine("   6. Quit the application\n");
            //Console.Write("?> ");

            //int userChoice = Int32.Parse(Console.ReadLine());
            //if (userChoice == 6)
            //{
            //    Console.Clear();
            //    Environment.Exit(0);
            //}
            //Console.Clear();
            //Console.WriteLine("uncomment and use the dispatch.Start() below with parameter or...?\n");
            Dispatch dispatch = new Dispatch();
            dispatch.Start();
        }
    }
}
