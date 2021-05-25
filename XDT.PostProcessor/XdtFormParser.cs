using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Source.DLaB.Common;

namespace XDT.PostProcessor
{
    public class XdtFormParser
    {
        private const string InterfaceGetAttributePrefix = "    getAttribute(attributeName: \"";
        public Dictionary<string, List<AttributeInfo>> AttributeByTypeName { get; }

        private enum ParserState
        {
            PreInterface,
            Interface,
            PostInterface
        }

        public XdtFormParser(string[] contents, Settings settings)
        {
            AttributeByTypeName = new Dictionary<string, List<AttributeInfo>>();
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

                        if (line.StartsWith(InterfaceGetAttributePrefix))
                        {
                            var name = line.SubstringByString(InterfaceGetAttributePrefix, "\"");
                            var type = line.SubstringByString(":").SubstringByString(":").Trim();
                            if (type.StartsWith(xrm + ".Attribute<")
                                || type.StartsWith(xrm + ".OptionSetAttribute<"))
                            {
                                type = type.SubstringByString("<", ">");
                                AttributeByTypeName.AddOrAppend(type.Capitalize() + "AttributeNames", new AttributeInfo(name, type));
                            }
                            else if (type.StartsWith(xrm + ".LookupAttribute<"))
                            {
                                var lookupName = type.SubstringByString("<", ">");
                                var lookups = lookupName.Replace("\"", "").Split(new [] {'|', ' '}, StringSplitOptions.RemoveEmptyEntries);
                                type = string.Join("_", lookups.Select(v => v.Capitalize()));
                                var returnType = $"{xrm}.EntityReference<{string.Join(" | ", lookups.Select(v => $@"""{v}"""))}>";
                            AttributeByTypeName.AddOrAppend(type.Capitalize() + "LookupAttributeNames", new AttributeInfo(name, returnType));
                            }
                            else if (type.StartsWith(xrm + ".NumberAttribute"))
                            {
                                AttributeByTypeName.AddOrAppend("NumberAttributeNames", new AttributeInfo(name, "number"));
                            }
                            else if (type.StartsWith(xrm + ".DateAttribute"))
                            {
                                AttributeByTypeName.AddOrAppend("DateAttributeNames", new AttributeInfo(name, "date"));
                            }
                        }

                        break;
                    case ParserState.PostInterface:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
