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

        [TestMethod]
        [DataRow("Xrm")]
        [DataRow("XdtXrm")]
        public void ShouldParseControls(string xrm)
        {
            TestControl($"{xrm}.Control<{xrm}.Attribute<any>>", Form.AttributeControls);
            TestControl(xrm + ".BaseControl", Form.BaseControls);
            TestControl(xrm + ".BooleanControl", Form.BooleanControls);
            TestControl(xrm + ".DateControl", Form.DateControls);
            TestControl(xrm + ".IFrameControl", Form.IFrameControls);
            TestControl(xrm + ".KBSearchControl", Form.KbSearchControls);
            TestControl(xrm + ".StringControl", Form.StringControls);
            TestControl(xrm + ".NumberControl", Form.NumberControls);
            TestControl(xrm + ".WebResourceControl", Form.WebResourceControls);
            TestControl(xrm + ".OptionSetControl<some_option_set>", Form.OptionSetControls);
            TestControl(xrm + ".MultiSelectOptionSetControl<some_option_set>", Form.MultiSelectControls);
            TestControl(xrm + ".LookupControl<\"systemuser\" | \"team\">", Form.LookupControls);
            TestControl(xrm + ".LookupControl<\"systemuser\">", Form.LookupControls);
            TestControl(xrm + ".SubGridControl<\"systemuser\">", Form.SubGridControls);
        }

        private void TestControl(string type, List<ControlInfo> list)
        {
            var ctrl = new ControlInfo("ctrlName", type);
            Sut.ParseGetControlLine(Form, $"    getControl(controlName: \"ctrlName\"): {ctrl.ControlType};");
            AssertAndRemoveSingleValueAdded(list, ctrl);
        }

        private void AssertAndRemoveSingleValueAdded(List<ControlInfo> list, ControlInfo expected)
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
            Assert.IsTrue(ReferenceEquals(list, Form.MultiSelectControls) || Form.MultiSelectControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.NumberControls) || Form.NumberControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.StringControls) || Form.StringControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.SubGridControls) || Form.SubGridControls.Count == 0);
            Assert.IsTrue(ReferenceEquals(list, Form.WebResourceControls) || Form.WebResourceControls.Count == 0);
        }
    }
}
