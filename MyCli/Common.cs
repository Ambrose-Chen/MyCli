using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyCli
{
    class Common
    {
        public static List<KeyValue<string, string>> FormatCommand()
        {
            List<KeyValue<string, string>> lk = new List<KeyValue<string, string>>();
            foreach (string n in Runtime.Command)
            {
                if (n.IndexOf('-') == 0)
                {
                    foreach (char c in n.Substring(1))
                        lk.Add(new KeyValue<string, string>(c.ToString(), string.Empty));
                }
                else
                {
                    if (lk.Count > 0)
                        lk[lk.Count - 1].Value = n;
                }
            }
            return lk;
        }
        public static string[] GroupList() => (from n in (AppDomain.CurrentDomain.GetAssemblies()
         .SelectMany(s => s.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IGroup))))
         .ToList())
                                               select n.Name).ToArray();
    }
}
