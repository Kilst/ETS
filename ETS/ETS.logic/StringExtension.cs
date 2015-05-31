using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.logic
{
    public static class StringExtension
    {
        // A new string extension method to reset the string
        //eg. txtbox.Text.Reset();

        // Turns out you can't change the original string's value through an extension method..
        public static string Reset(this String str)
        {
            str = "";
            return str;
        }
    }
}
