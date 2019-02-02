using System;
using System.Collections.Generic;
using System.Text;

namespace MyCli
{
    interface IGroup
    {
        List<string> Commands();
        void Run(string[] Command);
    }
}
