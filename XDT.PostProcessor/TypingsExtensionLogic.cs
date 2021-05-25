using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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

            UpdateDtFile(Path.Combine(rootTypeDirectory, "xrm.d.ts"));
            foreach (var table in Directory.GetDirectories(Path.Combine(rootTypeDirectory, "Form")))
            {
                foreach (var formType in Directory.GetDirectories(table))
                {
                    foreach (var form in Directory.GetFiles(formType, "*.d.ts"))
                    {
                        var fileContents = UpdateDtFile(form, false);
                        CreateFormExt(rootTypeDirectory, Path.GetFileName(table), Path.GetFileName(formType), Path.GetFileName(form), fileContents);
                    }
                }
            }
        }

        private string[] UpdateDtFile(string path, bool logNoXrmNamespaceOverride = true)
        {
            var lines = File.ReadAllLines(path);
            if (UpdateDtFile(lines, logNoXrmNamespaceOverride))
            {
                File.WriteAllLines(path, lines);
            }

            return lines;
        }

        public bool UpdateDtFile(string[] file, bool logNoXrmNamespaceOverride = false)
        {
            return UpdateXrmNamespace(file, logNoXrmNamespaceOverride);
        }

        public bool UpdateXrmNamespace(string[] file, bool logNoXrmNamespaceOverride = true)
        {
            if (string.IsNullOrWhiteSpace(Settings.XrmNamespaceOverride))
            {
                if (logNoXrmNamespaceOverride)
                {
                    Console.WriteLine("No XrmNamespaceOverride provided. Skipping update of DT Files.");
                }
                return false;
            }

            var regex = new Regex(Settings.XrmNamespaceRegEx);
            var hasUpdated = false;
            for(var i = 0; i < file.Length; i++)
            {
                var line = file[i];
                var newLine = regex.Replace(line, Settings.XrmNamespaceOverride);
                file[i] = newLine;
                hasUpdated = hasUpdated || line != newLine;
            }
            return hasUpdated;
        }

        public void CreateFormExt(string rootTypeDirectory, string table, string formType, string fileName, string[] fileContents)
        {
            var filePath = Path.Combine(rootTypeDirectory, "Form", table, formType, fileName);
            Console.WriteLine("Processing " + filePath);
            var formName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fileName));
            var contents = CreateFormExtContents(table, formType, formName, fileContents);
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
            var parser = new XdtFormParser(source, Settings);
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
            var types = new List<string>();
            foreach (var namesForType in parser.AttributeByTypeName.OrderBy(k => k.Key))
            {
                types.Add(namesForType.Key);
                contents.Add($@"    type {namesForType.Key} = {ToPipeStringDelimited(namesForType.Value.Select(v => v.Name), true)};");
            }

            if (types.Any())
            {
                contents.Add($@"    type AttributeNames = {ToPipeStringDelimited(types)};");
            }
            contents.Add("  }");
        }

        private void WriteFormInterface(List<string> contents, string table, string formType, string formName, XdtFormParser parser)
        {
            contents.Add($"  interface {formName} extends Form.{table}.{formType}.{formName} {{");
            WriteAddOnChangeValues(contents, formName, parser);
            WriteGetValues(contents, formName, parser);
            WriteSetValues(contents, formName, parser);
            contents.Add("  }"); 
        }

        private void WriteAddOnChangeValues(List<string> contents, string formName, XdtFormParser parser)
        {
            contents.AddRange(from namesForType in parser.AttributeByTypeName.OrderBy(k => k.Key)
                let first = namesForType.Value.First()
                let nullable = NonNullableValueTypes.Contains(namesForType.Key)
                    ? string.Empty
                    : " | null"
                select $@"    addOnChange(attributeName: {formName}.{namesForType.Key}, handler: (context?: {Settings.XrmNamespacePrefix}.ExecutionContext<{first.AttributeType}, undefined>) => any): void;");
            if (parser.AttributeByTypeName.Count > 0)
            {
                contents.Add($@"    addOnChange(attributeNames: {formName}.AttributeNames[], handler: (context?: {Settings.XrmNamespacePrefix}.ExecutionContext<{Settings.XrmNamespacePrefix}.Attribute<any>, undefined>) => any): void;");
            }
        }

        private void WriteGetValues(List<string> contents, string formName, XdtFormParser parser)
        {
            contents.AddRange(from namesForType in parser.AttributeByTypeName.OrderBy(k => k.Key)
                let first = namesForType.Value.First()
                let nullable = NonNullableValueTypes.Contains(namesForType.Key)
                    ? string.Empty
                    : " | null"
                select $@"    getValue(attributeName: {formName}.{namesForType.Key}): {first.ValueType}{nullable};");
        }

        private void WriteSetValues(List<string> contents, string formName, XdtFormParser parser)
        {
            contents.AddRange(from namesForType in parser.AttributeByTypeName.OrderBy(k => k.Key)
                let first = namesForType.Value.First()
                let nullable = NonNullableValueTypes.Contains(namesForType.Key)
                    ? string.Empty
                    : " | null"
                select $@"    setValue(attributeName: {formName}.{namesForType.Key}, value: {first.ValueType}{nullable}, fireOnChange = true);");
        }

        private string ToPipeStringDelimited(IEnumerable<string> values, bool applyTextWrap = false)
        {
            return applyTextWrap
                ? "\"" + string.Join("\" | \"", values) + "\""
                : string.Join(" | ", values);
        }

    }
}
