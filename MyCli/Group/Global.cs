using System;
using System.Collections.Generic;
using System.Text;

namespace MyCli.Group
{
    class Global : IGroup
    {
        public List<string> Commands()
        {
            return new List<string> { "clear", "group", "all" };
        }

        public void Run(string[] Command)
        {
            switch (Command[0])
            {
                case "clear":
                    Console.Clear();
                    break;
                case "group":
                    if (Command.Length > 1)
                    {
                        if (Command[1] == "list")
                            Console.WriteLine(string.Join(' ', Common.GroupList()));
                        else
                        {
                            if (Command[1].IndexOf("+") == 0)
                                Runtime.Group.Add(Command[1].Substring(1));
                            else if (Command[1].IndexOf("-") == 0)
                                Runtime.Group.Remove(Command[1].Substring(1));
                            else
                            {
                                Runtime.Group.Clear();
                                Runtime.Group.Add(Command[1]);
                            }
                        }
                    }
                    else
                    {
                        foreach (string g in Runtime.Group)
                            Console.WriteLine(g);
                    }
                    break;
                case "all":
                    Console.WriteLine(string.Join(' ', Runtime.CommandList));
                    break;
            }
        }
    }
}
