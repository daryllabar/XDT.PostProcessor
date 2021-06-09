using System;
using System.IO;
using System.Linq;

namespace XDT.PostProcessor
{
    public class XrmQueryUpdateLogic
    {
        private static readonly string[] Files = {
            "dg.xrmquery.web.js",
            "dg.xrmquery.web.min.js",
            "dg.xrmquery.web.promise.min.js"
        };

        public Settings Settings { get; }
        public XrmQueryUpdateLogic(Settings settings)
        {
            Settings = settings;
        }

        public void Process(string xrmQueryDirectory)
        {
            Console.WriteLine("Processing Xrm Query Update.");
            if (!Directory.Exists(xrmQueryDirectory))
            {
                throw new DirectoryNotFoundException($"\"{xrmQueryDirectory}\" does not exist!");
            }

            AssertFilesExist(xrmQueryDirectory);
            foreach (var file in Files.Select(f => Path.Combine(xrmQueryDirectory, f)))
            {
                Console.WriteLine("Processing " + file);
                var input = File.ReadAllText(file);
                var output = ProcessFile(input);
                Console.WriteLine("Updating " + file);
                File.WriteAllText(file, output);
                if (Settings.XrmQueryMakeWebpackCompatible)
                {
                    var definitionFile = Path.ChangeExtension(file, "d.ts");
                    Console.WriteLine("Creating type definition file: " + definitionFile);
                    File.WriteAllText(definitionFile, "export function load(): void;");
                }
            }
        }

        private void AssertFilesExist(string xrmQueryDirectory)
        {
            foreach (var file in Files)
            {
                AssertFileExist(xrmQueryDirectory, file);
            }
        }

        private void AssertFileExist(string xrmQueryDirectory, string file)
        {
            file = Path.Combine(xrmQueryDirectory, file);
            if (!File.Exists(file))
            {
                throw new FileNotFoundException($"\"{file}\" does not exist!");
            }
        }

        public string ProcessFile(string input)
        {
            return MakeWebpackCompatible(input);
        }

        private string MakeWebpackCompatible(string input)
        {
            return !Settings.XrmQueryMakeWebpackCompatible
                ? input
                : input + Settings.WebpackPostfix;
        }
    }
}
