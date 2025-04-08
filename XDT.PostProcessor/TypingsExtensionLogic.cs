using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Source.DLaB.Common;

namespace XDT.PostProcessor
{
    public class TypingsExtensionLogic
    {
        private static readonly HashSet<string> NonNullableValueTypes = new HashSet<string>{
            "StringAttributeNames",
            "BooleanAttributeNames"
        };

        public struct DefinedTypes
        {
            // Attributes
            public const string AllAttributes = "AttributeNames";
            public const string AnyAttributes = "AnyAttributeNames";
            public const string BooleanAttributes = "BooleanAttributeNames";
            public const string DateAttributes = "DateAttributeNames";
            public const string LookupAttributes = "LookupAttributeNames";
            public const string MultiSelectAttributes = "MultiSelectAttributeNames";
            public const string NumberAttributes = "NumberAttributeNames";
            public const string OptionSetAttributes = "OptionSetAttributeNames";
            public const string StringAttributes = "StringAttributeNames";
            public static readonly List<string> Attributes = new []
            {
                AnyAttributes,
                BooleanAttributes,
                DateAttributes,
                LookupAttributes,
                MultiSelectAttributes,
                NumberAttributes,
                OptionSetAttributes,
                StringAttributes,
            }.OrderBy(n=>n).ToList();
            // Controls
            public const string AllControls = "ControlNames";
            public const string AttributeControls = "AttributeControlNames";
            public const string BaseControls = "BaseControlNames";
            public const string BooleanControls = "BooleanControlNames";
            public const string DateControls = "DateControlNames";
            // ReSharper disable once InconsistentNaming
            public const string IFrameControls = "IFrameControlNames";
            public const string KbSearchControls = "KBSearchControlNames";
            public const string LookupControls = "LookupControlNames";
            public const string MultiSelectControls = "MultiSelectControlNames";
            public const string NumberControls = "NumberControlNames";
            public const string OptionSetControls = "OptionSetControlNames";
            public const string StringControls = "StringControlNames";
            public const string SubGridControls = "SubGridControlNames";
            public const string WebResourceControls = "WebResourceControlNames";
            public static readonly List<string> Controls = new[]
            {
                AttributeControls,
                BaseControls,
                BooleanControls,
                DateControls,
                IFrameControls,
                KbSearchControls,
                LookupControls,
                MultiSelectControls,
                NumberControls,
                OptionSetControls,
                StringControls,
                SubGridControls,
                WebResourceControls,
            }.OrderBy(n => n).ToList();
        }

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

            UpdateDtFile(Path.Combine(rootTypeDirectory, "xrm.d.ts"), true, true);
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

        private string[] UpdateDtFile(string path, bool logNoXrmNamespaceOverride = true, bool isXrmDTs = false)
        {
            var lines = File.ReadAllLines(path);
            if (UpdateDtFile(lines, logNoXrmNamespaceOverride)
                | (isXrmDTs && AddFormAttributesAndControlsBases(lines)))
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

        public bool AddFormAttributesAndControlsBases(string[] file)
        {
            if (!Settings.AddToXrmDefinitionFile)
            {
                return false;
            }
            file[file.Length-1] = file[file.Length - 1] + Environment.NewLine + Environment.NewLine + @"  // Added via XDT.PostProcessor
  declare namespace " + Settings.XrmNamespacePrefix + @" {
    type EmptyFormAttributes = FormAttributesBase<string, string, string, string, string, string, string, string>;
    type EmptyFormControls = FormControlsBase<string, string, string, string, string, string, string, string, string, string, string, string, string, string>;
    type FormAttributesBase<TAll extends string, TBoolean extends string, TDate extends string, TLookup extends string, TMultiSelect extends string, TNumber extends string, TOptionSet extends string, TString extends string> = {
      All: TAll;
      Boolean: TBoolean;
      Date: TDate;
      Lookup: TLookup;
      MultiSelect: TMultiSelect;
      Number: TNumber;
      OptionSet: TOptionSet;
      String: TString;
    };
    type FormControlsBase<TAll extends string, TAttributeControl extends string, TBaseControl extends string, TBooleanControl extends string, TDateControl extends string, TIFrame extends string, TKbSearch extends string, TLookup extends string, TMultiSelect extends string, TNumberControl extends string, TOptionSetControl extends string, TStringControl extends string, TSubgrid extends string, TWebResource extends string> = {
      All: TAll;
      AttributeControl: TAttributeControl;
      BaseControl: TBaseControl;
      BooleanControl: TBooleanControl;
      DateControl: TDateControl;
      IFrame: TIFrame;
      KbSearch: TKbSearch;
      Lookup: TLookup;
      MultiSelect: TMultiSelect;
      NumberControl: TNumberControl;
      OptionSetControl: TOptionSetControl;
      StringControl: TStringControl;
      Subgrid: TSubgrid;
        WebResource: TWebResource;
    };
    /**
     * Interface for a generic XdtXrm.Page
     */
    interface FormContext extends XdtXrm.PageBase<any, any, any> {
      /**
       * Generic getAttribute
       */
      getAttribute(attrName: string): XdtXrm.Attribute<any> | undefined;
      /**
       * Generic getControl
       */
      getControl(ctrlName: string): XdtXrm.AnyControl | undefined;
    }
    interface BaseExecutionContext extends XdtXrm.ExecutionContext<any, any> {
      getFormContext<T extends XdtXrm.PageBase<any, any, any>>(): T;
    }
  }
";
            return true;
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
            WriteBaseTypes(contents, form);
            WriteAttributeTypes(contents, form);
            WriteControlTypes(contents, form);
            contents.Add("  }");
        }

        public void WriteAttributeTypes(List<string> contents, ParsedXdtForm form)
        {
            string GenerateStandardDefinition(string name, List<AttributeInfo> attributes)
            {
                return attributes.Count == 0
                    ? string.Empty
                    : $"    type {name} = {attributes.Select(v => v.Name).ToSortedPipeStringDelimited(true, "string")};";
            }

            string GenerateLookupDefinition()
            {
                var attributes = form.AttributesByTypeName.Where(k => k.Key.EndsWith(DefinedTypes.LookupAttributes)).ToArray();
                return attributes.Length == 0
                    ? string.Empty
                    : $"    type {DefinedTypes.LookupAttributes} = {attributes.Select(v => v.Key).ToSortedPipeStringDelimited(false, "string")};";
            }

            string GenerateOptionSetDefinition(string name, string attributeName)
            {
                var types = form.AttributesByTypeName.SelectMany(t => t.Value)
                                .Where(v => v.AttributeType.SubstringByString(".", "<") == attributeName && v.ValueType != "boolean")
                                .Select(v => v.ValueType.Capitalize() + "AttributeNames")
                                .ToArray();
                return types.Length == 0
                    ? string.Empty
                    : $"    type {name} = {types.ToSortedPipeStringDelimited(false, "string")};";
            }

            contents.AddSortedSection(new[]
            {
                form.AttributesByTypeName.Count == 0 ? string.Empty : $"    type {DefinedTypes.AllAttributes} = {GetPopulatedBaseAttributeTypes(form).ToSortedPipeStringDelimited()};",
                GenerateStandardDefinition(DefinedTypes.AnyAttributes, form.AnyAttributes),
                GenerateStandardDefinition(DefinedTypes.BooleanAttributes, form.BooleanAttributes),
                GenerateStandardDefinition(DefinedTypes.DateAttributes, form.DateAttributes),
                GenerateStandardDefinition(DefinedTypes.NumberAttributes, form.NumberAttributes),
                GenerateStandardDefinition(DefinedTypes.StringAttributes, form.StringAttributes),
                GenerateLookupDefinition(),
                GenerateOptionSetDefinition(DefinedTypes.MultiSelectAttributes, "MultiSelectOptionSetAttribute"),
                GenerateOptionSetDefinition(DefinedTypes.OptionSetAttributes, "OptionSetAttribute"),
            }.Where(l => !string.IsNullOrWhiteSpace(l)), "    // Base Attributes");
            contents.AddSortedSection(form.AttributesByTypeName
                                          .Where(kvp => !DefinedTypes.Attributes.Contains(kvp.Key))
                                          .Select(namesForType => $@"    type {namesForType.Key} = {namesForType.Value.Select(v => v.Name).ToSortedPipeStringDelimited(true)};"), "    // Form Specific Attribute Types");
        }

        private static List<string> GetPopulatedBaseAttributeTypes(ParsedXdtForm form)
        {
            var baseAttributes = form.AttributesByTypeName.Keys.Where(k => DefinedTypes.Attributes.Contains(k)).ToList();
            if (form.LookupAttributes.Any())
            {
                baseAttributes.Add(DefinedTypes.LookupAttributes);
            }

            if (form.MultiSelectAttributes.Any())
            {
                baseAttributes.Add(DefinedTypes.MultiSelectAttributes);
            }

            if (form.OptionSetAttributes.Any())
            {
                baseAttributes.Add(DefinedTypes.OptionSetAttributes);
            }

            return baseAttributes;
        }

        public void WriteControlTypes(List<string> contents, ParsedXdtForm form)
        {
            string GenerateStandardDefinition(string name, List<ControlInfo> controls)
            {
                return controls.Count == 0
                    ? string.Empty
                    : $"    type {name} = {controls.Select(v => v.Name).ToSortedPipeStringDelimited(true, "string")};";
            }

            string GenerateLookupDefinition()
            {
                var controls = form.ControlsByTypeName.Where(k => k.Key.EndsWith(DefinedTypes.LookupControls)).ToArray();
                return controls.Length == 0
                    ? string.Empty
                    : $"    type {DefinedTypes.LookupControls} = {controls.Select(v => v.Key).ToSortedPipeStringDelimited(false, "string")};";
            }

            string GenerateOptionSetDefinition(string name, string attributeName)
            {
                var types = form.ControlsByTypeName
                    .Where(kvp => !DefinedTypes.Controls.Contains(kvp.Key))
                    .Select(kvp => new {kvp.Key, ValueType = kvp.Value.FirstOrDefault().ControlType?.SubstringByString(".", "<")})
                    .Where(kvp => kvp.ValueType == attributeName)
                    .Select(kvp => kvp.Key)
                    .ToArray();

                return types.Length == 0
                    ? string.Empty
                    : $"    type {name} = {types.ToSortedPipeStringDelimited(false, "string")};";
            }

            contents.AddSortedSection(new[]
            {
                form.ControlsByTypeName.Count == 0 ? string.Empty : $"    type {DefinedTypes.AllControls} = {GetPopulatedBaseControlTypes(form).ToSortedPipeStringDelimited()};",
                GenerateStandardDefinition(DefinedTypes.AttributeControls, form.AttributeControls),
                GenerateStandardDefinition(DefinedTypes.BaseControls, form.BaseControls),
                GenerateStandardDefinition(DefinedTypes.BooleanControls, form.BooleanControls),
                GenerateStandardDefinition(DefinedTypes.DateControls, form.DateControls),
                GenerateStandardDefinition(DefinedTypes.IFrameControls, form.IFrameControls),
                GenerateStandardDefinition(DefinedTypes.KbSearchControls, form.KbSearchControls),
                GenerateStandardDefinition(DefinedTypes.NumberControls, form.NumberControls),
                GenerateStandardDefinition(DefinedTypes.StringControls, form.StringControls),
                GenerateStandardDefinition(DefinedTypes.WebResourceControls, form.WebResourceControls),
                GenerateLookupDefinition(),
                GenerateOptionSetDefinition(DefinedTypes.MultiSelectControls, "MultiSelectOptionSetControl"),
                GenerateOptionSetDefinition(DefinedTypes.OptionSetControls, "OptionSetControl"),
                GenerateOptionSetDefinition(DefinedTypes.SubGridControls, "SubGridControl"),
            }.Where(l => !string.IsNullOrWhiteSpace(l)), "    // Base Controls");
            contents.AddSortedSection(form.ControlsByTypeName
                                          .Where(kvp => !DefinedTypes.Controls.Contains(kvp.Key))
                                          .Select(namesForType => $@"    type {namesForType.Key} = {namesForType.Value.Select(v => v.Name).ToSortedPipeStringDelimited(true)};"), "    // Form Specific Control Types");
        }

        private static List<string> GetPopulatedBaseControlTypes(ParsedXdtForm form)
        {
            var baseControls = form.ControlsByTypeName.Keys.Where(k => DefinedTypes.Controls.Contains(k)).ToList();
            var formSpecificControlTypes = new Dictionary<string, List<ControlInfo>>
            {
                {DefinedTypes.LookupControls, form.LookupControls},
                {DefinedTypes.MultiSelectControls, form.MultiSelectControls},
                {DefinedTypes.OptionSetControls, form.OptionSetControls},
                {DefinedTypes.SubGridControls, form.SubGridControls},
            };
            baseControls.AddRange(formSpecificControlTypes
                                  .Where(kvp => kvp.Value.Any())
                                  .Select(kvp => kvp.Key));

            return baseControls;
        }

        public void WriteBaseTypes(List<string> contents, ParsedXdtForm form)
        {
            string DefaultToStringIfEmpty<T>(string name, List<T> attributes)
            {
                return attributes.Count == 0 ? "string" : name;
            }
            contents.Add($"    type FormAttributes = {Settings.XrmNamespacePrefix}.FormAttributesBase<");
            contents.Add($"        {(form.AttributesByTypeName.Count == 0 ? "string" : DefinedTypes.AllAttributes)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.BooleanAttributes, form.BooleanAttributes)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.DateAttributes, form.DateAttributes)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.LookupAttributes, form.LookupAttributes)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.MultiSelectAttributes, form.MultiSelectAttributes)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.NumberAttributes, form.NumberAttributes)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.OptionSetAttributes, form.OptionSetAttributes)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.StringAttributes, form.StringAttributes)}");
            contents.Add("    >;");
            contents.Add($"    type FormControls = {Settings.XrmNamespacePrefix}.FormControlsBase<");
            contents.Add($"        {(form.ControlsByTypeName.Count == 0 ? "string" : DefinedTypes.AllControls)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.AttributeControls, form.AttributeControls)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.BaseControls, form.BaseControls)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.BooleanControls, form.BooleanControls)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.DateControls, form.DateControls)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.IFrameControls, form.IFrameControls)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.KbSearchControls, form.KbSearchControls)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.LookupControls, form.LookupControls)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.MultiSelectControls, form.MultiSelectControls)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.NumberControls, form.NumberControls)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.OptionSetControls, form.OptionSetControls)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.StringControls, form.StringControls)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.SubGridControls, form.SubGridControls)},");
            contents.Add($"        {DefaultToStringIfEmpty(DefinedTypes.WebResourceControls, form.WebResourceControls)}");
            contents.Add("    >;");
        }

        private void WriteFormInterface(List<string> contents, string table, string formType, string formName, ParsedXdtForm form)
        {
            contents.Add($"  interface {formName} extends Form.{table}.{formType}.{formName} {{");
            WriteAddOnChangeValues(contents, formName, form);
            WriteGetValues(contents, formName, form);
            WriteSetValues(contents, formName, form);
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

        public void WriteGetValues(List<string> contents, string formName, ParsedXdtForm form)
        {
            contents.AddRange(from namesForType in form.AttributesByTypeName.OrderBy(k => k.Key)
                let first = namesForType.Value.First()
                let nullable = NonNullableValueTypes.Contains(namesForType.Key)
                    ? string.Empty
                    : " | null"
                select $@"    getValue(attributeName: {formName}.{namesForType.Key}): {first.ValueType}{nullable};");
        }

        public void WriteSetValues(List<string> contents, string formName, ParsedXdtForm form)
        {
            contents.AddRange(from namesForType in form.AttributesByTypeName.OrderBy(k => k.Key)
                let first = namesForType.Value.First()
                let nullable = NonNullableValueTypes.Contains(namesForType.Key)
                    ? string.Empty
                    : " | null"
                select $@"    setValue(attributeName: {formName}.{namesForType.Key}, value: {first.ValueType}{nullable}, fireOnChange?: boolean): void;");
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
