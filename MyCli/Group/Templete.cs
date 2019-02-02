using System;
using System.Collections.Generic;
using System.Text;

namespace MyCli.Group
{
    class Templete : IGroup
    {
        public List<string> Commands()
        {
            return new List<string> { "a", "b", "c", "d" };
        }

        public void Run(string[] Command)
        {
            switch (Command[0])
            {
                case "a":
                    Console.WriteLine("templete.a");
                    break;
                case "b":
                    Console.WriteLine("templete.b");
                    break;
                case "c":
                    Console.WriteLine("templete.c");
                    break;
                case "d":
                    Console.WriteLine("templete.d");
                    break;
            }
        }
    }
}
