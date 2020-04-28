using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._01.cs
{
    class StringLen : ITask
    {
        public string Run(string[] data)
        {
            return data[0].Length.ToString();
        }
    }
}
