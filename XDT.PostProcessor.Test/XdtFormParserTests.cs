using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XDT.PostProcessor.Test
{
    [TestClass]
    public class XdtFormParserTests
    {
        public ParsedXdtForm Form { get; set; }
        public XdtFormParser Sut { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Form = new ParsedXdtForm();
            Sut = new XdtFormParser();
        }

        [TestMethod]
        [DataRow("Xrm")]
        [DataRow("XdtXrm")]
        public void ShouldParseAttributes(string xrm)
        {
            TestAttribute(xrm + ".Attribute<any>", "any", Form.AnyAttributes);
            TestAttribute(xrm + ".DateAttribute", "date", Form.DateAttributes);
            TestAttribute(xrm + ".LookupAttribute<\"systemuser\" | \"team\">", xrm + ".EntityReference<\"systemuser\" | \"team\">", Form.LookupAttributes);
            TestAttribute(xrm + ".LookupAttribute<\"connectionrole\">", xrm + ".EntityReference<\"connectionrole\">", Form.LookupAttributes);
            TestAttribute(xrm + ".MultiSelectOptionSetAttribute<some_multiselect>", "some_multiselect", Form.MultiSelectAttributes);
            TestAttribute(xrm + ".NumberAttribute", "number", Form.NumberAttributes);
            TestAttribute(xrm + ".OptionSetAttribute<boolean>", "boolean", Form.BooleanAttributes);
            TestAttribute(xrm + ".OptionSetAttribute<some_option>", "some_option", Form.OptionSetAttributes);
            TestAttribute(xrm + ".Attribute<string>", "string", Form.StringAttributes);
        }

        [TestMethod]
        [DataRow("Xrm")]
        [DataRow("XdtXrm")]
        public void ShouldParseControls(string xrm)
        {
            //TestAttribute(xrm + ".OptionSetAttribute<boolean>", "boolean", Form.BooleanAttributes);
        }

        private void TestAttribute(string type, string value, List<AttributeInfo> list)
        {
            var att = new AttributeInfo("attName", type, value);
            Sut.ParseGetAttributeLine(Form, $"    getAttribute(attributeName: \"attName\"): {att.AttributeType};");
            AssertAndRemoveSingleValueAdded(list, att);
        }

        private void AssertAndRemoveSingleValueAdded(List<AttributeInfo> list, AttributeInfo expected)
        {
            Assert.AreEqual(1, list.Count, "Error for " + expected);
            AssertSingleValueAdded(list);
            Assert.AreEqual(expected, list[0]);
            list.RemoveAt(0);
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private void AssertSingleValueAdded<T>(List<T> list)
        {
            Assert.IsTrue(ReferenceEquals(list, Form.BooleanAttributes) || Form.BooleanAttributes?.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.DateAttributes) || Form.DateAttributes.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.NumberAttributes) || Form.NumberAttributes.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.LookupAttributes) || Form.LookupAttributes.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.OptionSetAttributes) || Form.OptionSetAttributes.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.MultiSelectAttributes) || Form.MultiSelectAttributes.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.AttributeControls) || Form.AttributeControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.BaseControls) || Form.BaseControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.DateControls) || Form.DateControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.IFrameControls) || Form.IFrameControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.KbSearchControls) || Form.KbSearchControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.LookupControls) || Form.LookupControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.NoteControls) || Form.NoteControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.MultiSelectControls) || Form.MultiSelectControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.NumberControls) || Form.NumberControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.StringControls) || Form.StringControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.SubgridControls) || Form.SubgridControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.TimerControls) || Form.TimerControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.WebResourceControls) || Form.WebResourceControls.Count == 0);
        }
    }
}
