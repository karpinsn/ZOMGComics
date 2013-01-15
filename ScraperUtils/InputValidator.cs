using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperUtils
{
    public class InputValidator
    {
        public static void AssertThrowInputNotNullOrEmpty(string input, string inputName)
        {
            if (string.IsNullOrEmpty(input))
            {
                Debug.Fail(inputName + " is either null or empty and is required");
                throw new ArgumentException(inputName + " is either null or empty and is required");
            }
        }
    }
}
