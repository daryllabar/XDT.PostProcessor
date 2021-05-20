using System;

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

            var logic = new TypingsExtensionLogic(Settings.Default);
            logic.ProcessAllForms(args[0]);

            if (args.Length >= 2)
            {
                var queryLogic = new XrmQueryUpdateLogic(Settings.Default);
                queryLogic.Process(args[1]);
            }
        }
    }
}
