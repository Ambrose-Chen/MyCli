using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using MyCli.Group;
using System.Linq;

namespace MyCli
{
    class Runtime
    {
        public static List<string> Group = new List<string>();
        public static List<string> CommandList = new List<string>();
        public static string[] Command;

        private Assembly Assembly = Assembly.Load("MyCli");
        public Runtime(string Command)
        {
            Analysis(Command);
        }
        public void Run()
        {
            CommandList.Clear();

            if (Command.Length < 1)
                return;

            var Fullnames = (from v in (AppDomain.CurrentDomain.GetAssemblies()
                  .SelectMany(s => s.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IGroup))))
                  .ToList())
                             where Group.Contains(v.Name.ToLower())
                             select v.FullName).ToArray();

            string FullName = string.Empty;
            if (Fullnames.Length > 0)
                CommandListImport(Fullnames, out FullName);
            if (string.IsNullOrEmpty(FullName))
                CommandListImport("MyCli.Group.Global", out FullName);
            if (string.IsNullOrEmpty(FullName))
                throw new Exception("this command does not exist");
            IGroup obj = GetObject(FullName);
            obj.Run(Command);
        }
        public IGroup GetObject(string FullName)
        {
            Type type = Assembly.GetType(FullName);
            return (IGroup)Activator.CreateInstance(type);
        }
        public void CommandListImport(string FullName, out string ExistCommand)
        {
            Type type = Assembly.GetType(FullName);
            IGroup obj = (IGroup)Activator.CreateInstance(type);
            CommandList.AddRange(obj.Commands());
            CommandList.Sort();
            foreach (string Com in obj.Commands())
            {
                if (Com == Command[0])
                {
                    ExistCommand = FullName;
                    return;
                }
            }
            ExistCommand = string.Empty;
        }
        public void CommandListImport(string[] FullName, out string ExistCommand)
        {
            string c = string.Empty;
            foreach (string f in FullName)
            {
                if (!string.IsNullOrEmpty(c))
                {
                    ExistCommand = c;
                    return;
                }
                CommandListImport(f, out c);
            }
            ExistCommand = c;
        }

        private void Analysis(string Command) => Runtime.Command = Command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }
}
