using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using Source.DLaB.Common;

namespace XDT.PostProcessor
{
    public class XdtFormParser
    {
        private const string InterfaceGetAttributePrefix = "    getAttribute(attributeName: \"";
        private const string InterfaceGetControlPrefix = "    getControl(controlName: \"";

        private enum ParserState
        {
            PreInterface,
            Interface,
            PostInterface
        }

        public ParsedXdtForm Parse(string[] contents) { 
            var form = new ParsedXdtForm();
            if (contents.Length == 0)
            {
                return form;
            }

            var state = ParserState.PreInterface;
            foreach (var line in contents)
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

                        ParseGetAttributeLine(form, line);
                        ParseGetControlLine(form, line);

                        break;
                    case ParserState.PostInterface:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return form;
        }

        public void ParseGetAttributeLine(ParsedXdtForm form, string line)
        {
            if (!line.StartsWith(InterfaceGetAttributePrefix))
            {
                return;
            }

            var name = line.SubstringByString(InterfaceGetAttributePrefix, "\"");
            var type = line.SubstringByString(":").SubstringByString(":").Trim().Split(';')[0];
            var attributeType = type.EndsWith(" | null")
                ? type.SubstringByString(0, " | null")
                : type;

            var xrm = type.SubstringByString(0, ".");
            type = type.SubstringByString(".");
            var baseType = type.IndexOf('<')> -1
                ? type.SubstringByString(0, "<")
                : type;
            switch (baseType)
            {
                case "Attribute":
                case "OptionSetAttribute":
                case "MultiSelectOptionSetAttribute":
                    type = type.SubstringByString("<", ">");
                    var att = new AttributeInfo(name, attributeType, type);
                    form.AttributesByTypeName.AddOrAppend(type.Capitalize() + "AttributeNames", att);
                    if (form.AttributesByXdtType.TryGetValue(type, out var list) || form.AttributesByXdtType.TryGetValue(baseType, out list))
                    {
                        list.Add(att);
                    }
                    else
                    {
                        throw new Exception("Unrecognized Attribute Type: " + line);
                    }
                    break;
                case "DateAttribute":
                    att = new AttributeInfo(name, attributeType, "date");
                    form.AttributesByTypeName.AddOrAppend("DateAttributeNames", att);
                    form.DateAttributes.Add(att);
                    break;

                case "LookupAttribute":
                    var lookupName = type.SubstringByString("<", ">");
                    var lookups = lookupName.Replace("\"", "").Split(new[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    type = string.Join("_", lookups.Select(v => v.Capitalize()));
                    var returnType = $"{xrm}.EntityReference<{string.Join(" | ", lookups.Select(v => $@"""{v}"""))}>";
                    att = new AttributeInfo(name, attributeType, returnType);
                    form.AttributesByTypeName.AddOrAppend(type.Capitalize() + "LookupAttributeNames", att);
                    form.LookupAttributes.Add(att);
                    break;

                case "NumberAttribute":
                    att = new AttributeInfo(name, attributeType, "number");
                    form.AttributesByTypeName.AddOrAppend("NumberAttributeNames", att);
                    form.NumberAttributes.Add(att);
                    break;

            }
        }
        public void ParseGetControlLine(ParsedXdtForm form, string line)
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
                    form.ControlsByTypeName.AddOrAppend(baseType + "ControlNames", new ControlInfo(name, controlType));
                    break;
                case "OptionSet":
                case "MultiSelectOptionSet":
                    type = type.SubstringByString("<", ">");
                    form.ControlsByTypeName.AddOrAppend(type.Capitalize() + "ControlNames", new ControlInfo(name, controlType));
                    break;
                case "Lookup":
                case "SubGrid":
                    var lookupName = type.SubstringByString("<", ">");
                    var lookups = lookupName.Replace("\"", "").Split(new[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    type = string.Join("_", lookups.Select(v => v.Capitalize())); 
                    form.ControlsByTypeName.AddOrAppend(type.Capitalize() + baseType + "ControlNames", new ControlInfo(name, controlType));
                    break;
            }
        }
    }
}
