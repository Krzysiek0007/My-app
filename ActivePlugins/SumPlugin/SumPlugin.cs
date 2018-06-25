using PluginContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversePlugin
{
    public class SumPlugin : IPlugin
    {
        public string Name
        {
            get
            {
                return "SumPlugin";
            }
        }

        public string Descripton
        {
            get
            {
                return "Splits string by ‘+’ sign, parses numbers, calculates sum of them and returns it as a string";
            }
        }

        public string Execute(string input)
        {
            string[] strNumbers = input.Split("+");
            List<int> numbers = new List<int>();
            int sum = 0;
            foreach (string strNumber in strNumbers)
            {
                int number;
                if (int.TryParse(strNumber, out number))
                {
                    sum += number;
                }
                else
                {
                    return "Podano nieprawidłowy ciąg znaków";
                }
            }
            return sum.ToString();
        }
    }
}
