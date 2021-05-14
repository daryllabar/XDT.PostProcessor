using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDT.PostProcessor
{
    public static class Extensions
    {
        public static string Capitalize(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            if (value.Length == 1)
            {
                return value.ToUpper();
            }
            else
            {
                return char.ToUpper(value[0]) + value.Substring(1);
            }
        }
    }
}
