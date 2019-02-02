using System;
using System.Reflection;

namespace MyCli
{
    class Program
    {
        private static string Command;
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    CommandLine();
                    Runtime run = new Runtime(Command);
                    run.Run();
                }
                catch (Exception err)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR:" + err.Message);
                    Console.WriteLine(err.StackTrace);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        private static void CommandLine()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[MyCli.kuian]:");
            Console.ForegroundColor = ConsoleColor.White;
            Command = Console.ReadLine();
        }
    }
}
