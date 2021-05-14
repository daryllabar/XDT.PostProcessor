using System.IO;


namespace XDT.PostProcessor.Test
{
    public class FileHelper
    {
        private const string VisualStudioTestFolderRoot = "\\.vs\\";
        public string ExtensionNamespace => $"Form{FormNamespacePostfix}.{Table}.{FormType}";
        public string Table { get; }
        public string FormType { get; }
        public string FileName { get; }
        public string FormName => Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(FileName));
        public string FormNamespacePostfix { get; }
        public string[] Contents { get; }
        public object XdtFullyQualifiedFormInterfaceName => $"Form.{Table}.{FormType}.{FormName}";

        public FileHelper(string table, string formType, string fileName, string formNamespacePostfix)
        {
            Table = table;
            FormType = formType;
            FileName = fileName;
            FormNamespacePostfix = formNamespacePostfix;
            var directory = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).Parent.Parent;
            if (directory.FullName.Contains(VisualStudioTestFolderRoot))
            {
                // Handle Live Unit Testing
                while (directory.FullName.Contains(VisualStudioTestFolderRoot))
                {
                    directory = directory.Parent;
                }

                directory = new DirectoryInfo(Path.Combine(directory.Parent.FullName, "XDT.PostProcessor.Test"));
            }
            var typingsDirectory = directory.GetDirectories("Typings")[0].FullName;
            Contents = File.ReadAllLines(Path.Combine(typingsDirectory, "Form", table, formType, fileName));
        }
    }
}
