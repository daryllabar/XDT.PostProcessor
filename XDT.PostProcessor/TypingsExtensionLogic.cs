using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XDT.PostProcessor
{
    public class TypingsExtensionLogic
    {
        private static readonly HashSet<string> NonNullableValueTypes = new HashSet<string>{
            "StringAttributeNames",
            "BooleanAttributeNames"
        };
        public Settings Settings { get; }
        public TypingsExtensionLogic(Settings settings)
        {
            Settings = settings;
        }

        public void ProcessAllForms(string rootTypeDirectory)
        {
            if (!Directory.Exists(rootTypeDirectory))
            {
                throw new DirectoryNotFoundException($"\"{rootTypeDirectory}\" does not exist!");
            }

            foreach (var table in Directory.GetDirectories(Path.Combine(rootTypeDirectory, "Form")))
            {
                foreach (var formType in Directory.GetDirectories(table))
                {
                    foreach (var form in Directory.GetFiles(formType, "*.d.ts"))
                    {
                        CreateFormExt(rootTypeDirectory, Path.GetFileName(table), Path.GetFileName(formType), Path.GetFileName(form));
                    }
                }
            }
        }

        public void CreateFormExt(string rootTypeDirectory, string table, string formType, string fileName)
        {
            var filePath = Path.Combine(rootTypeDirectory, "Form", table, formType, fileName);
            Console.WriteLine("Processing " + filePath);
            var formName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fileName));
            var contents = CreateFormExtContents(table, formType, formName, File.ReadAllLines(filePath));
            var extFilePath = Path.Combine(rootTypeDirectory, Settings.OutputRelativePath, "Form", table, formType, fileName);
            var extDir = Path.GetDirectoryName(extFilePath);
            if (!Directory.Exists(extDir))
            {
                Console.WriteLine("Creating Directory " + extDir);
                Directory.CreateDirectory(extDir ?? string.Empty);
            }
            File.WriteAllText(extFilePath, contents);
        }

        public string CreateFormExtContents(string table, string formType, string formName, string[] source)
        {
            var parser = new XdtFormParser(source);
            var contents = new List<string>();
            WriteNamespace(contents, table, formType, formName, parser);
            return string.Join(Environment.NewLine, contents);
        }

        private void WriteNamespace(List<string> contents, string table, string formType, string formName, XdtFormParser parser) {
            contents.Add($"declare namespace Form{Settings.FormNamespacePostfix}.{table}.{formType} " + "{");
            WriteFormNamespace(contents, formName, parser);
            WriteFormInterface(contents, table, formType, formName, parser);
            contents.Add("}");
        }

        private void WriteFormNamespace(List<string> contents, string formName, XdtFormParser parser)
        {
            contents.Add($"  namespace {formName} {{");
            foreach (var namesForType in parser.AttributeByTypeName.OrderBy(k => k.Key))
            {
                contents.Add($@"    type {namesForType.Key} = {ToPipeStringDelimited(namesForType.Value.Select(v => v.Name))};");
            }
            contents.Add("  }");
        }

        private void WriteFormInterface(List<string> contents, string table, string formType, string formName, XdtFormParser parser)
        {
            contents.Add($"  interface {formName} extends Form.{table}.{formType}.{formName} {{");
            contents.AddRange(from namesForType in parser.AttributeByTypeName.OrderBy(k => k.Key)
                let first = namesForType.Value.First()
                let nullable = NonNullableValueTypes.Contains(namesForType.Key)
                    ? string.Empty
                    : " | null"
                select $@"    getValue(attributeName: {namesForType.Key}): {first.ValueType}{nullable};");
            contents.Add("  }");
        }

        private string ToPipeStringDelimited(IEnumerable<string> values)
        {
            return "\"" + string.Join("\" | \"", values) + "\"";
        }

    }
}
