using PluginContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversePlugin
{
    public class CountPlugin : IPlugin
    {
        public string Name
        {
            get
            {
                return "CountPlugin";
            }
        }

        public string Descripton
        {
            get
            {
                return "Counts characters in input string";
            }
        }

        public string Execute(string input)
        {
            return input.Length.ToString();
        }
    }
}
