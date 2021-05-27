using System;
using System.Linq;
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

        public ParsedXdtForm Parse(string[] contents, string xrmPrefix) { 
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

                        ParseGetAttributeLine(form, line, xrmPrefix);
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

        public void ParseGetAttributeLine(ParsedXdtForm form, string line, string xrm)
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
                form.AttributesByTypeName.AddOrAppend(type.Capitalize() + "AttributeNames", new AttributeInfo(name, attributeType, type));
            }
            else if (type.StartsWith(xrm + ".LookupAttribute<"))
            {
                var lookupName = type.SubstringByString("<", ">");
                var lookups = lookupName.Replace("\"", "").Split(new[] {'|', ' '}, StringSplitOptions.RemoveEmptyEntries);
                type = string.Join("_", lookups.Select(v => v.Capitalize()));
                var returnType = $"{xrm}.EntityReference<{string.Join(" | ", lookups.Select(v => $@"""{v}"""))}>";
                form.AttributesByTypeName.AddOrAppend(type.Capitalize() + "LookupAttributeNames", new AttributeInfo(name, attributeType, returnType));
            }
            else if (type.StartsWith(xrm + ".NumberAttribute"))
            {
                form.AttributesByTypeName.AddOrAppend("NumberAttributeNames", new AttributeInfo(name, attributeType, "number"));
            }
            else if (type.StartsWith(xrm + ".DateAttribute"))
            {
                form.AttributesByTypeName.AddOrAppend("DateAttributeNames", new AttributeInfo(name, attributeType, "date"));
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
