using System;
using System.Collections.Generic;
using System.Linq;
using Source.DLaB.Common;

namespace XDT.PostProcessor
{
    public class XdtFormParser
    {
        private const string InterfaceGetAttributePrefix = "    getAttribute(attributeName: \"";
        private const string InterfaceGetControlPrefix = "    getControl(controlName: \"";
        public Dictionary<string, List<AttributeInfo>> AttributesByTypeName { get; }
        public Dictionary<string, List<ControlInfo>> ControlsByTypeName { get; }
        public List<AttributeInfo> BooleanAttributes { get; set; }
        public List<AttributeInfo> DateAttributes { get; set; }
        public List<AttributeInfo> NumberAttributes { get; set; }
        public List<AttributeInfo> LookupAttributes { get; set; }
        public List<AttributeInfo> OptionSetAttributes { get; set; }
        public List<AttributeInfo> MultiSelectAttributes { get; set; }
        public List<ControlInfo> AttributeControls { get; set; }
        public List<ControlInfo> BaseControls { get; set; }
        public List<ControlInfo> DateControls { get; set; }
        public List<ControlInfo> IFrameControls { get; set; }
        public List<ControlInfo> KbSearchControls { get; set; }
        public List<ControlInfo> LookupControls { get; set; }
        public List<ControlInfo> NoteControls { get; set; }
        public List<ControlInfo> MultiSelectControls { get; set; }
        public List<ControlInfo> NumberControls { get; set; }
        public List<ControlInfo> StringControls { get; set; }
        public List<ControlInfo> SubgridControls { get; set; }
        public List<ControlInfo> TimerControls { get; set; }
        public List<ControlInfo> WebResourceControls { get; set; }

        private enum ParserState
        {
            PreInterface,
            Interface,
            PostInterface
        }

        public XdtFormParser(string[] contents, Settings settings)
        {
            AttributesByTypeName = new Dictionary<string, List<AttributeInfo>>();
            ControlsByTypeName = new Dictionary<string, List<ControlInfo>>();
            BooleanAttributes = new List<AttributeInfo>();
            DateAttributes = new List<AttributeInfo>();
            NumberAttributes = new List<AttributeInfo>();
            LookupAttributes = new List<AttributeInfo>();
            OptionSetAttributes = new List<AttributeInfo>();
            MultiSelectAttributes = new List<AttributeInfo>();
            AttributeControls = new List<ControlInfo>();
            BaseControls = new List<ControlInfo>();
            DateControls = new List<ControlInfo>();
            IFrameControls = new List<ControlInfo>();
            KbSearchControls = new List<ControlInfo>();
            LookupControls = new List<ControlInfo>();
            NoteControls = new List<ControlInfo>();
            MultiSelectControls = new List<ControlInfo>();
            NumberControls = new List<ControlInfo>();
            StringControls = new List<ControlInfo>();
            SubgridControls = new List<ControlInfo>();
            TimerControls = new List<ControlInfo>();
            WebResourceControls = new List<ControlInfo>();
            if (contents.Length == 0)
            {
                return;
            }

            var state = ParserState.PreInterface;
            var xrm = settings.XrmNamespacePrefix;
            foreach(var line in contents)
            {
                switch (state)
                {
                    case ParserState.PreInterface:
                        if (line.StartsWith("  interface"))
                        {
                            state = ParserState.Interface;
                        }
                        break;
                    case ParserState.Interface:
                        if (line.StartsWith("  }"))
                        {
                            state = ParserState.PostInterface;
                        }

                        ParseGetAttributeLine(line, xrm);
                        ParseGetControlLine(line, xrm);

                        break;
                    case ParserState.PostInterface:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public string GetAllAttributeAndControlNamesTypeUnion(string formName)
        {
            var name = $"{formName}.AttributeNames | {formName}.ControlNames";
            if (AttributesByTypeName.Count == 0 && ControlsByTypeName.Count == 0)
            {
                name = string.Empty;
            }
            else if (AttributesByTypeName.Count == 0)
            {
                name = formName + ".ControlNames";
            }
            else if (ControlsByTypeName.Count == 0)
            {
                name = formName + ".AttributeNames";
            }

            return name;
        }

        private void ParseGetAttributeLine(string line, string xrm)
        {
            if (!line.StartsWith(InterfaceGetAttributePrefix))
            {
                return;
            }

            var name = line.SubstringByString(InterfaceGetAttributePrefix, "\"");
            var type = line.SubstringByString(":").SubstringByString(":").Trim();
            var attributeType = type.Split(';')[0];
            if (attributeType.EndsWith(" | null"))
            {
                attributeType = attributeType.SubstringByString(0, " | null");
            }

            if (type.StartsWith(xrm + ".Attribute<")
                || type.StartsWith(xrm + ".OptionSetAttribute<"))
            {
                type = type.SubstringByString("<", ">");
                AttributesByTypeName.AddOrAppend(type.Capitalize() + "AttributeNames", new AttributeInfo(name, attributeType, type));
            }
            else if (type.StartsWith(xrm + ".LookupAttribute<"))
            {
                var lookupName = type.SubstringByString("<", ">");
                var lookups = lookupName.Replace("\"", "").Split(new[] {'|', ' '}, StringSplitOptions.RemoveEmptyEntries);
                type = string.Join("_", lookups.Select(v => v.Capitalize()));
                var returnType = $"{xrm}.EntityReference<{string.Join(" | ", lookups.Select(v => $@"""{v}"""))}>";
                AttributesByTypeName.AddOrAppend(type.Capitalize() + "LookupAttributeNames", new AttributeInfo(name, attributeType, returnType));
            }
            else if (type.StartsWith(xrm + ".NumberAttribute"))
            {
                AttributesByTypeName.AddOrAppend("NumberAttributeNames", new AttributeInfo(name, attributeType, "number"));
            }
            else if (type.StartsWith(xrm + ".DateAttribute"))
            {
                AttributesByTypeName.AddOrAppend("DateAttributeNames", new AttributeInfo(name, attributeType, "date"));
            }
        }
        private void ParseGetControlLine(string line, string xrm)
        {
            if (!line.StartsWith(InterfaceGetControlPrefix))
            {
                return;
            }

            var name = line.SubstringByString(InterfaceGetControlPrefix, "\"");
            var controlType = line.SubstringByString(":").SubstringByString(":").Trim().Split(';')[0]; // XdtXrm.OptionSetControl<contact_familystatuscode>
            if (controlType.EndsWith(" | null"))
            {
                controlType = controlType.SubstringByString(0, " | null");
            }

            var type = controlType.Split('.')[1]; // OptionSetControl<contact_familystatuscode>
            var baseType = type.SubstringByString(0, "Control"); // OptionSet

            switch (baseType)
            {
                case "Base":
                case "String":
                case "Number":
                case "Date":
                case "KBSearchControl":
                    ControlsByTypeName.AddOrAppend(baseType + "ControlNames", new ControlInfo(name, controlType));
                    break;
                case "OptionSet":
                case "MultiSelectOptionSet":
                    type = type.SubstringByString("<", ">");
                    ControlsByTypeName.AddOrAppend(type.Capitalize() + "ControlNames", new ControlInfo(name, controlType));
                    break;
                case "Lookup":
                case "SubGrid":
                    var lookupName = type.SubstringByString("<", ">");
                    var lookups = lookupName.Replace("\"", "").Split(new[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    type = string.Join("_", lookups.Select(v => v.Capitalize())); 
                    ControlsByTypeName.AddOrAppend(type.Capitalize() + baseType + "ControlNames", new ControlInfo(name, controlType));
                    break;
            }
        }
    }
}
