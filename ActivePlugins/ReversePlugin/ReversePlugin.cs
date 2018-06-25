using PluginContracts;
using System;

namespace ReversePlugin
{
    public class ReversePlugin : IPlugin
    {
        public string Name
        {
            get
            {
                return "ReversePlugin";
            }
        }

        public string Descripton
        {
            get
            {
                return "Reverses input and returns it";
            }
        }

        public string Execute(string input)
        {
            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
