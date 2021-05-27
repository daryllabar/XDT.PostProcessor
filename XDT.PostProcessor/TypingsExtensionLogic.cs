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
            var form = new XdtFormParser().Parse(source);
            var contents = new List<string>();
            WriteNamespace(contents, table, formType, formName, form);
            return string.Join(Environment.NewLine, contents);
        }

        private void WriteNamespace(List<string> contents, string table, string formType, string formName, ParsedXdtForm form) {
            contents.Add($"declare namespace Form{Settings.FormNamespacePostfix}.{table}.{formType} " + "{");
            WriteFormNamespace(contents, formName, form);
            WriteFormInterface(contents, table, formType, formName, form);
            contents.Add("}");
        }

        public void WriteFormNamespace(List<string> contents, string formName, ParsedXdtForm form)
        {
            contents.Add($"  namespace {formName} {{");
            var lines = new List<string>();
            var types = new List<string>();
            foreach (var namesForType in form.AttributesByTypeName)
            {
                types.Add(namesForType.Key);
                lines.Add($@"    type {namesForType.Key} = {ToPipeStringDelimited(namesForType.Value.Select(v => v.Name), true)};");
            }

            if (types.Any())
            {
                lines.Add($@"    type AttributeNames = {ToPipeStringDelimited(types.OrderBy(n => n))};");
            }

            types = new List<string>();
            foreach (var namesForType in form.ControlsByTypeName)
            {
                types.Add(namesForType.Key);
                lines.Add($@"    type {namesForType.Key} = {ToPipeStringDelimited(namesForType.Value.Select(v => v.Name), true)};");
            }

            if (types.Any())
            {
                lines.Add($@"    type ControlNames = {ToPipeStringDelimited(types.OrderBy(n => n))};");
            }

            contents.AddRange(lines.OrderBy(l => l));
            contents.Add("  }");
        }

        private void WriteFormInterface(List<string> contents, string table, string formType, string formName, ParsedXdtForm form)
        {
            contents.Add($"  interface {formName} extends Form.{table}.{formType}.{formName} {{");
            WriteAddOnChangeValues(contents, formName, form);
            WriteFireOnChange(contents, formName, form);
            WriteGetValues(contents, formName, form);
            WriteGetVisible(contents, formName, form);
            WriteRemoveOnChange(contents, formName, form);
            WriteSetValues(contents, formName, form);
            WriteSetVisible(contents, formName, form);
            contents.Add("  }"); 
        }

        public void WriteAddOnChangeValues(List<string> contents, string formName, ParsedXdtForm form)
        {
            contents.AddRange(from namesForType in form.AttributesByTypeName.OrderBy(k => k.Key)
                let first = namesForType.Value.First()
                let nullable = NonNullableValueTypes.Contains(namesForType.Key)
                    ? string.Empty
                    : " | null"
                select $@"    addOnChange(attributeName: {formName}.{namesForType.Key}, handler: (context?: {Settings.XrmNamespacePrefix}.ExecutionContext<{first.AttributeType}, undefined>) => any): void;");
            if (form.AttributesByTypeName.Count > 0)
            {
                contents.Add($@"    addOnChange(attributeNames: {formName}.AttributeNames[], handler: (context?: {Settings.XrmNamespacePrefix}.ExecutionContext<{Settings.XrmNamespacePrefix}.Attribute<any>, undefined>) => any): void;");
            }
        }

        public void WriteFireOnChange(List<string> contents, string formName, ParsedXdtForm form)
        {
            if (form.AttributesByTypeName.Count == 0)
            {
                return;
            }

            contents.Add($@"    fireOnChange(attributeName: {formName}.AttributeNames): void;");
        }

        public void WriteGetValues(List<string> contents, string formName, ParsedXdtForm form)
        {
            contents.AddRange(from namesForType in form.AttributesByTypeName.OrderBy(k => k.Key)
                let first = namesForType.Value.First()
                let nullable = NonNullableValueTypes.Contains(namesForType.Key)
                    ? string.Empty
                    : " | null"
                select $@"    getValue(attributeName: {formName}.{namesForType.Key}): {first.ValueType}{nullable};");
        }

        public void WriteGetVisible(List<string> contents, string formName, ParsedXdtForm form)
        {
            var type = GetAllAttributeAndControlNamesTypeUnion(form, formName);
            if (string.IsNullOrWhiteSpace(type))
            {
                return;
            }

            contents.Add($@"    getVisible(name: {type}): boolean;");
        }

        public void WriteRemoveOnChange(List<string> contents, string formName, ParsedXdtForm form)
        {
            if (form.AttributesByTypeName.Count == 0)
            {
                return;
            }

            contents.Add($@"    removeOnChange(attributeName: {formName}.AttributeNames | {formName}.AttributeNames[], handler: (context?: {Settings.XrmNamespacePrefix}.ExecutionContext<{Settings.XrmNamespacePrefix}.Attribute<any>, undefined>) => any): void;");
        }

        public void WriteSetValues(List<string> contents, string formName, ParsedXdtForm form)
        {
            contents.AddRange(from namesForType in form.AttributesByTypeName.OrderBy(k => k.Key)
                let first = namesForType.Value.First()
                let nullable = NonNullableValueTypes.Contains(namesForType.Key)
                    ? string.Empty
                    : " | null"
                select $@"    setValue(attributeName: {formName}.{namesForType.Key}, value: {first.ValueType}{nullable}, fireOnChange = true);");
        }

        public void WriteSetVisible(List<string> contents, string formName, ParsedXdtForm form)
        {
            var type = GetAllAttributeAndControlNamesTypeUnion(form, formName);
            if (string.IsNullOrWhiteSpace(type))
            {
                return;
            }

            contents.Add($@"    setVisible(name: {type}, visible = true): void;");
        }

        private string ToPipeStringDelimited(IEnumerable<string> values, bool applyTextWrap = false)
        {
            return applyTextWrap
                ? "\"" + string.Join("\" | \"", values) + "\""
                : string.Join(" | ", values);
        }

        public static string GetAllAttributeAndControlNamesTypeUnion(ParsedXdtForm form, string formName)
        {
            var name = $"{formName}.AttributeNames | {formName}.ControlNames";
            if (form.AttributesByTypeName.Count == 0 && form.ControlsByTypeName.Count == 0)
            {
                name = string.Empty;
            }
            else if (form.AttributesByTypeName.Count == 0)
            {
                name = formName + ".ControlNames";
            }
            else if (form.ControlsByTypeName.Count == 0)
            {
                name = formName + ".AttributeNames";
            }

            return name;
        }
    }
}
