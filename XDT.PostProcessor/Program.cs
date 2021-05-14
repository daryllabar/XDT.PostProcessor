using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDT.PostProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                throw new ArgumentException("Missing path to XDT XRM Typings directory!");
            }
            var logic = new Logic(Settings.Default);
            logic.ProcessAllForms(args[0]);
        }
    }
}
