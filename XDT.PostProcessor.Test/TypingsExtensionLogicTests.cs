using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Source.DLaB.Common;

namespace XDT.PostProcessor.Test
{
    [TestClass]
    public class TypingsExtensionLogicTests
    {
        public TypingsExtensionLogic Sut { get; set; }
        public static FileHelper MainAccountForm { get; }

        static TypingsExtensionLogicTests()
        {
            MainAccountForm = new FileHelper("account", "Main", "Account.d.ts", Settings.Default.FormNamespacePostfix);
        }

        [TestInitialize]
        public void Initialize()
        {
            Sut = new TypingsExtensionLogic(new Settings());
        }

        [TestMethod]
        public void WriteBaseTypes_EmptyDashboard_Should_GenerateUntypedFormAttributes()
        {
            ExecuteForAllOptions(GetEmptyDashboard(), (form, xrm) =>
            {
                var contents = new List<string>();
                Sut.WriteBaseTypes(contents, form);
                var expected = $@"    type FormAttributes = {xrm}.FormAttributesBase<
        string,
        string,
        string,
        string,
        string,
        string,
        string,
        string
    >;
    type FormControls = {xrm}.FormControlsBase<
        string,
        string,
        string,
        string,
        string,
        string,
        string,
        string,
        string,
        string,
        string,
        string,
        string,
        string
    >;";
            contents.ShouldEqualWithDiff(expected);

            });
        }

        [TestMethod]
        public void WriteAttributeTypes_EmptyDashboard_Should_GenerateNoFormAttributes()
        {
            ExecuteForAllOptions(GetEmptyDashboard(), (form, xrm) =>
            {
                var contents = new List<string>();
                Sut.WriteAttributeTypes(contents, form);
                Assert.AreEqual(0, contents.Count, "No attributes should have gotten created");

            });
        }

        [TestMethod]
        public void WriteAttributeTypes_MissingNumberAttributes_Should_NotIncludeNumberAttributesInAttributesType()
        {
            ExecuteForAllOptions(GetInviteWebForm(), (form, xrm) =>
            {
                var contents = new List<string>();
                Sut.WriteAttributeTypes(contents, form);
                Assert.IsTrue(contents[1].Trim().StartsWith("type AttributeNames = "), "Expected the item at index 1 to be the declaration of hte Attribute Names type!");
                Assert.IsFalse(contents[1].Trim().Contains("NumberAttributeNames"));

                contents = new List<string>();
                Sut.WriteControlTypes(contents, form);
                Assert.IsTrue(contents[1].Trim().StartsWith("type ControlNames = "), "Expected the item at index 1 to be the declaration of hte Control Names type!");
                Assert.IsFalse(contents[1].Trim().Contains("NumberControlNames"));
            });
        }

        [TestMethod]
        public void WriteAttributeTypes_AllTypes_Should_GenerateFormAttributes()
        {
            ExecuteForAllOptions(GetFormWithAllTypes(), (form, xrm) =>
            {
                var contents = new List<string>();
                Sut.WriteAttributeTypes(contents, form);
                contents.ShouldEqualWithDiff(@"    // Base Attributes
    type AnyAttributeNames = ""any"";
    type AttributeNames = AnyAttributeNames | BooleanAttributeNames | DateAttributeNames | LookupAttributeNames | MultiSelectAttributeNames | NumberAttributeNames | OptionSetAttributeNames | StringAttributeNames;
    type BooleanAttributeNames = ""boolean"";
    type DateAttributeNames = ""date"";
    type LookupAttributeNames = PricelevelLookupAttributeNames | Systemuser_TeamLookupAttributeNames;
    type MultiSelectAttributeNames = Address1_FreighttermscodeAttributeNames;
    type NumberAttributeNames = ""number"";
    type OptionSetAttributeNames = Account_Address1_FreighttermscodeAttributeNames;
    type StringAttributeNames = ""string"" | ""stringnullable"";
    // Form Specific Attribute Types
    type Account_Address1_FreighttermscodeAttributeNames = ""optionSet"";
    type Address1_FreighttermscodeAttributeNames = ""multiselect"";
    type PricelevelLookupAttributeNames = ""lookup"";
    type Systemuser_TeamLookupAttributeNames = ""lookupmulitple"";");
            });
        }

        [TestMethod]
        public void WriteControlTypes_EmptyDashboard_Should_GenerateUntypedFormAttributes()
        {
            ExecuteForAllOptions(GetEmptyDashboard(), (form, xrm) =>
            {
                var contents = new List<string>();
                Sut.WriteControlTypes(contents, form);
                Assert.AreEqual(0, contents.Count, "No attributes should have gotten created");

            });
        }

        [TestMethod]
        public void WriteControlTypes_AllTypes_Should_GenerateFormControls()
        {
            ExecuteForAllOptions(GetFormWithAllTypes(), (form, xrm) =>
            {
                var contents = new List<string>();
                Sut.WriteControlTypes(contents, form);
                contents.ShouldEqualWithDiff(@"    // Base Controls
    type AttributeControlNames = ""attribute"";
    type BaseControlNames = ""base"";
    type BooleanControlNames = ""boolean"";
    type ControlNames = AttributeControlNames | BaseControlNames | BooleanControlNames | DateControlNames | IFrameControlNames | KBSearchControlNames | LookupControlNames | MultiSelectControlNames | NumberControlNames | OptionSetControlNames | StringControlNames | SubGridControlNames | WebResourceControlNames;
    type DateControlNames = ""date"";
    type IFrameControlNames = ""iframe"";
    type KBSearchControlNames = ""kbsearch"";
    type LookupControlNames = PricelevelLookupControlNames | Systemuser_TeamLookupControlNames;
    type MultiSelectControlNames = Address1_FreighttermscodeControlNames;
    type NumberControlNames = ""number"";
    type OptionSetControlNames = Account_Address1_FreighttermscodeControlNames;
    type StringControlNames = ""string"";
    type SubGridControlNames = ContactSubGridControlNames;
    type WebResourceControlNames = ""webresource"";
    // Form Specific Control Types
    type Account_Address1_FreighttermscodeControlNames = ""optionset"";
    type Address1_FreighttermscodeControlNames = ""multiselect"";
    type ContactSubGridControlNames = ""subgrid"";
    type PricelevelLookupControlNames = ""lookup"";
    type Systemuser_TeamLookupControlNames = ""lookupmultiple"";");
            });
        }

        [TestMethod]
        public void EmptyDashboard_Should_GenerateEmptyInterface()
        {
            var contents = Sut.CreateFormExtContents("account", "InteractionCentricDashboard", "EmptyDashboard", GetEmptyDashboard());
            var expected = @"interface EmptyDashboard extends Form.account.InteractionCentricDashboard.EmptyDashboard {
  }
}";
            // Only grab interface portion
            contents = "interface EmptyDashboard" + contents.SubstringByString("interface EmptyDashboard");
            contents.ShouldEqualWithDiff(expected);  
        }

        private static string[] GetEmptyDashboard()
        {
            return @"
declare namespace Form.account.InteractionCentricDashboard {
  namespace EmptyDashboard { 
    namespace Tabs {
    }
    interface Attributes extends XdtXrm.AttributeCollectionBase {
      get(name: string): undefined;
      get(): XdtXrm.Attribute<any>[];
      get(index: number): XdtXrm.Attribute<any>;
      get(chooser: (item: XdtXrm.Attribute<any>, index: number) => boolean): XdtXrm.Attribute<any>[];
    }
    interface Controls extends XdtXrm.ControlCollectionBase {
      get(name: string): undefined;
      get(): XdtXrm.BaseControl[];
      get(index: number): XdtXrm.BaseControl;
      get(chooser: (item: XdtXrm.BaseControl, index: number) => boolean): XdtXrm.BaseControl[];
    }
    interface Tabs extends XdtXrm.TabCollectionBase {
      get(name: string): undefined;
      get(): XdtXrm.PageTab<XdtXrm.Collection<XdtXrm.PageSection>>[];
      get(index: number): XdtXrm.PageTab<XdtXrm.Collection<XdtXrm.PageSection>>;
      get(chooser: (item: XdtXrm.PageTab<XdtXrm.Collection<XdtXrm.PageSection>>, index: number) => boolean): XdtXrm.PageTab<XdtXrm.Collection<XdtXrm.PageSection>>[];
    }
  }
  interface EmptyDashboard extends XdtXrm.PageBase<EmptyDashboard.Attributes,EmptyDashboard.Tabs,EmptyDashboard.Controls> {
    getAttribute(attributeName: string): undefined;
    getControl(controlName: string): undefined;
  }
}
".Split(new[] { Environment.NewLine, "/n" }, StringSplitOptions.None);
        }

        private static string[] GetInviteWebForm()
        {
            return @"
declare namespace Form.contact.Main {
  namespace InviteWebForm {
    namespace Tabs {
    }
    interface Attributes extends XdtXrm.AttributeCollectionBase {
      get(name: ""birthdate""): XdtXrm.DateAttribute | null;
      get(name: ""emailaddress1""): XdtXrm.Attribute<string>;
      get(name: ""familystatuscode""): XdtXrm.OptionSetAttribute<contact_familystatuscode> | null;
      get(name: ""firstname""): XdtXrm.Attribute<string>;
      get(name: ""industrycode""): XdtXrm.OptionSetAttribute<number> | null;
      get(name: ""lastname""): XdtXrm.Attribute<string>;
      get(name: ""middlename""): XdtXrm.Attribute<string> | null;
      get(name: ""mobilephone""): XdtXrm.Attribute<string> | null;
      get(name: ""name""): XdtXrm.Attribute<string> | null;
      get(name: ""ownerid""): XdtXrm.LookupAttribute<""systemuser"" | ""team"">;
      get(name: ""parentaccountid""): XdtXrm.LookupAttribute<""account""> | null;
      get(name: ""spousesname""): XdtXrm.Attribute<string> | null;
      get(name: ""statecode""): XdtXrm.OptionSetAttribute<contact_statecode>;
      get(name: ""telephone1""): XdtXrm.Attribute<string>;
      get(name: ""websiteurl""): XdtXrm.Attribute<string> | null;
      get(name: string): undefined;
      get(): XdtXrm.Attribute<any>[];
      get(index: number): XdtXrm.Attribute<any>;
      get(chooser: (item: XdtXrm.Attribute<any>, index: number) => boolean): XdtXrm.Attribute<any>[];
    }
    interface Controls extends XdtXrm.ControlCollectionBase {
      get(name: ""emailaddress1""): XdtXrm.StringControl;
      get(name: ""firstname""): XdtXrm.StringControl;
      get(name: ""footer_statecode""): XdtXrm.OptionSetControl<contact_statecode>;
      get(name: ""header_process_birthdate""): XdtXrm.DateControl | null;
      get(name: ""header_process_emailaddress1""): XdtXrm.StringControl | null;
      get(name: ""header_process_familystatuscode""): XdtXrm.OptionSetControl<contact_familystatuscode> | null;
      get(name: ""header_process_firstname""): XdtXrm.StringControl | null;
      get(name: ""header_process_industrycode""): XdtXrm.OptionSetControl<number> | null;
      get(name: ""header_process_lastname""): XdtXrm.StringControl | null;
      get(name: ""header_process_middlename""): XdtXrm.StringControl | null;
      get(name: ""header_process_mobilephone""): XdtXrm.StringControl | null;
      get(name: ""header_process_name""): XdtXrm.StringControl | null;
      get(name: ""header_process_parentaccountid""): XdtXrm.LookupControl<""account""> | null;
      get(name: ""header_process_spousesname""): XdtXrm.StringControl | null;
      get(name: ""header_process_telephone1""): XdtXrm.StringControl | null;
      get(name: ""header_process_websiteurl""): XdtXrm.StringControl | null;
      get(name: ""lastname""): XdtXrm.StringControl;
      get(name: ""ownerid""): XdtXrm.LookupControl<""systemuser"" | ""team"">;
      get(name: ""telephone1""): XdtXrm.StringControl;
      get(name: string): undefined;
      get(): XdtXrm.BaseControl[];
      get(index: number): XdtXrm.BaseControl;
      get(chooser: (item: XdtXrm.BaseControl, index: number) => boolean): XdtXrm.BaseControl[];
    }
    interface QuickViewForms extends XdtXrm.QuickViewFormCollectionBase {
      get(name: string): undefined;
      get(): XdtXrm.QuickViewFormBase[];
      get(index: number): XdtXrm.QuickViewFormBase;
      get(chooser: (item: XdtXrm.QuickViewFormBase, index: number) => boolean): XdtXrm.QuickViewFormBase[];
    }
    interface Tabs extends XdtXrm.TabCollectionBase {
      get(name: string): undefined;
      get(): XdtXrm.PageTab<XdtXrm.Collection<XdtXrm.PageSection>>[];
      get(index: number): XdtXrm.PageTab<XdtXrm.Collection<XdtXrm.PageSection>>;
      get(chooser: (item: XdtXrm.PageTab<XdtXrm.Collection<XdtXrm.PageSection>>, index: number) => boolean): XdtXrm.PageTab<XdtXrm.Collection<XdtXrm.PageSection>>[];
    }
  }
  interface InviteWebForm extends XdtXrm.PageBase<InviteWebForm.Attributes,InviteWebForm.Tabs,InviteWebForm.Controls,InviteWebForm.QuickViewForms> {
    getAttribute(attributeName: ""birthdate""): XdtXrm.DateAttribute | null;
    getAttribute(attributeName: ""emailaddress1""): XdtXrm.Attribute<string>;
    getAttribute(attributeName: ""familystatuscode""): XdtXrm.OptionSetAttribute<contact_familystatuscode> | null;
    getAttribute(attributeName: ""firstname""): XdtXrm.Attribute<string>;
    getAttribute(attributeName: ""industrycode""): XdtXrm.OptionSetAttribute<number> | null;
    getAttribute(attributeName: ""lastname""): XdtXrm.Attribute<string>;
    getAttribute(attributeName: ""middlename""): XdtXrm.Attribute<string> | null;
    getAttribute(attributeName: ""mobilephone""): XdtXrm.Attribute<string> | null;
    getAttribute(attributeName: ""name""): XdtXrm.Attribute<string> | null;
    getAttribute(attributeName: ""ownerid""): XdtXrm.LookupAttribute<""systemuser"" | ""team"">;
    getAttribute(attributeName: ""parentaccountid""): XdtXrm.LookupAttribute<""account""> | null;
    getAttribute(attributeName: ""spousesname""): XdtXrm.Attribute<string> | null;
    getAttribute(attributeName: ""statecode""): XdtXrm.OptionSetAttribute<contact_statecode>;
    getAttribute(attributeName: ""telephone1""): XdtXrm.Attribute<string>;
    getAttribute(attributeName: ""websiteurl""): XdtXrm.Attribute<string> | null;
    getAttribute(attributeName: string): undefined;
    getAttribute(delegateFunction: XdtXrm.Collection.MatchingDelegate<XdtXrm.Attribute<any>>): XdtXrm.Attribute<any>[];
    getControl(controlName: ""emailaddress1""): XdtXrm.StringControl;
    getControl(controlName: ""firstname""): XdtXrm.StringControl;
    getControl(controlName: ""footer_statecode""): XdtXrm.OptionSetControl<contact_statecode>;
    getControl(controlName: ""header_process_birthdate""): XdtXrm.DateControl | null;
    getControl(controlName: ""header_process_emailaddress1""): XdtXrm.StringControl | null;
    getControl(controlName: ""header_process_familystatuscode""): XdtXrm.OptionSetControl<contact_familystatuscode> | null;
    getControl(controlName: ""header_process_firstname""): XdtXrm.StringControl | null;
    getControl(controlName: ""header_process_industrycode""): XdtXrm.OptionSetControl<number> | null;
    getControl(controlName: ""header_process_lastname""): XdtXrm.StringControl | null;
    getControl(controlName: ""header_process_middlename""): XdtXrm.StringControl | null;
    getControl(controlName: ""header_process_mobilephone""): XdtXrm.StringControl | null;
    getControl(controlName: ""header_process_name""): XdtXrm.StringControl | null;
    getControl(controlName: ""header_process_parentaccountid""): XdtXrm.LookupControl<""account""> | null;
    getControl(controlName: ""header_process_spousesname""): XdtXrm.StringControl | null;
    getControl(controlName: ""header_process_telephone1""): XdtXrm.StringControl | null;
    getControl(controlName: ""header_process_websiteurl""): XdtXrm.StringControl | null;
    getControl(controlName: ""lastname""): XdtXrm.StringControl;
    getControl(controlName: ""ownerid""): XdtXrm.LookupControl<""systemuser"" | ""team"">;
    getControl(controlName: ""telephone1""): XdtXrm.StringControl;
    getControl(controlName: string): undefined;
    getControl(delegateFunction: XdtXrm.Collection.MatchingDelegate<XdtXrm.Control<any>>): XdtXrm.Control<any>[];
  }
}
".Split(new[] { Environment.NewLine, "/n" }, StringSplitOptions.None);
        }
        private static string[] GetFormWithAllTypes()
        {
            return @"
declare namespace Form.account.InteractionCentricDashboard {
  namespace EmptyDashboard { 
  }
  interface EmptyDashboard extends XdtXrm.PageBase<EmptyDashboard.Attributes,EmptyDashboard.Tabs,EmptyDashboard.Controls> {
    getAttribute(attributeName: ""any""): XdtXrm.Attribute<any>;
    getAttribute(attributeName: ""boolean""): XdtXrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: ""date""): XdtXrm.DateAttribute;
    getAttribute(attributeName: ""lookup""): XdtXrm.LookupAttribute<""pricelevel"">;
    getAttribute(attributeName: ""lookupmulitple""): XdtXrm.LookupAttribute<""systemuser"" | ""team"">;
    getAttribute(attributeName: ""multiselect""): XdtXrm.MultiSelectOptionSetAttribute<address1_freighttermscode>;
    getAttribute(attributeName: ""number""): XdtXrm.NumberAttribute;
    getAttribute(attributeName: ""optionSet""): XdtXrm.OptionSetAttribute<account_address1_freighttermscode>;
    getAttribute(attributeName: ""string""): XdtXrm.Attribute<string>;
    getAttribute(attributeName: ""stringnullable""): XdtXrm.Attribute<string> | null;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: ""attribute""): XdtXrm.Control<XdtXrm.Attribute<any>>;
    getControl(controlName: ""base""): XdtXrm.BaseControl;
    getControl(controlName: ""boolean""): XdtXrm.OptionSetControl<boolean>;
    getControl(controlName: ""date""): XdtXrm.DateControl;
    getControl(controlName: ""iframe""): XdtXrm.IFrameControl;
    getControl(controlName: ""kbsearch""): XdtXrm.KBSearchControl;
    getControl(controlName: ""lookup""): XdtXrm.LookupControl<""pricelevel"">;
    getControl(controlName: ""lookupmultiple""): XdtXrm.LookupControl<""systemuser"" | ""team"">;
    getControl(controlName: ""multiselect""): XdtXrm.MultiSelectOptionSetControl<address1_freighttermscode>;
    getControl(controlName: ""number""): XdtXrm.NumberControl;
    getControl(controlName: ""optionset""): XdtXrm.OptionSetControl<account_address1_freighttermscode>;
    getControl(controlName: ""string""): XdtXrm.StringControl;
    getControl(controlName: ""subgrid""): XdtXrm.SubGridControl<""contact"">;
    getControl(controlName: ""webresource""): XdtXrm.WebResourceControl;
    getControl(controlName: string): undefined;
  }
}
".Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None);
        }

        [TestMethod]
        public void WriteFormNamespace_Account_Should_GenerateAttributeTypeNames()
        {
            ExecuteForAllOptions(MainAccountForm, (file, parser, xrm) => {
                var output = new List<string>();
                Sut.WriteFormNamespace(output, file.FormName, parser);
                output.ShouldEqualWithDiff(GetExpectedAccountFormNamespace(file, xrm));
            });
        }

        private static string GetExpectedAccountFormNamespace(FileHelper file, string xrm)
        {
            return $@"  namespace {file.FormName} {{
    type FormAttributes = {xrm}.FormAttributesBase<
        AttributeNames,
        BooleanAttributeNames,
        string,
        LookupAttributeNames,
        string,
        NumberAttributeNames,
        OptionSetAttributeNames,
        StringAttributeNames
    >;
    type FormControls = {xrm}.FormControlsBase<
        ControlNames,
        AttributeControlNames,
        BaseControlNames,
        BooleanControlNames,
        string,
        string,
        string,
        LookupControlNames,
        string,
        NumberControlNames,
        OptionSetControlNames,
        StringControlNames,
        SubGridControlNames,
        string
    >;
    // Base Attributes
    type AnyAttributeNames = ""tickersymbol"";
    type AttributeNames = AnyAttributeNames | BooleanAttributeNames | LookupAttributeNames | NumberAttributeNames | OptionSetAttributeNames | StringAttributeNames;
    type BooleanAttributeNames = ""creditonhold"" | ""donotbulkemail"" | ""donotemail"" | ""donotfax"" | ""donotphone"" | ""donotpostalmail"" | ""msdyn_taxexempt"";
    type LookupAttributeNames = AccountLookupAttributeNames | ContactLookupAttributeNames | Msdyn_TaxcodeLookupAttributeNames | Msdyn_WorkhourtemplateLookupAttributeNames | PricelevelLookupAttributeNames | Systemuser_TeamLookupAttributeNames | TerritoryLookupAttributeNames | TransactioncurrencyLookupAttributeNames;
    type NumberAttributeNames = ""address1_latitude"" | ""address1_longitude"" | ""creditlimit"" | ""msdyn_travelcharge"" | ""numberofemployees"" | ""revenue"";
    type OptionSetAttributeNames = Account_Address1_FreighttermscodeAttributeNames | Account_Address1_ShippingmethodcodeAttributeNames | Account_CustomertypecodeAttributeNames | Account_IndustrycodeAttributeNames | Account_OwnershipcodeAttributeNames | Account_PaymenttermscodeAttributeNames | Account_PreferredcontactmethodcodeAttributeNames | Msdyn_TravelchargetypeAttributeNames;
    type StringAttributeNames = ""address1_city"" | ""address1_composite"" | ""address1_country"" | ""address1_line1"" | ""address1_line2"" | ""address1_line3"" | ""address1_postalcode"" | ""address1_stateorprovince"" | ""description"" | ""fax"" | ""msdyn_taxexemptnumber"" | ""msdyn_workorderinstructions"" | ""msdyusd_facebook"" | ""msdyusd_twitter"" | ""name"" | ""sic"" | ""telephone1"" | ""websiteurl"";
    // Form Specific Attribute Types
    type Account_Address1_FreighttermscodeAttributeNames = ""address1_freighttermscode"";
    type Account_Address1_ShippingmethodcodeAttributeNames = ""address1_shippingmethodcode"";
    type Account_CustomertypecodeAttributeNames = ""customertypecode"";
    type Account_IndustrycodeAttributeNames = ""industrycode"";
    type Account_OwnershipcodeAttributeNames = ""ownershipcode"";
    type Account_PaymenttermscodeAttributeNames = ""paymenttermscode"";
    type Account_PreferredcontactmethodcodeAttributeNames = ""preferredcontactmethodcode"";
    type AccountLookupAttributeNames = ""msdyn_billingaccount"" | ""parentaccountid"";
    type ContactLookupAttributeNames = ""primarycontactid"";
    type Msdyn_TaxcodeLookupAttributeNames = ""msdyn_salestaxcode"";
    type Msdyn_TravelchargetypeAttributeNames = ""msdyn_travelchargetype"";
    type Msdyn_WorkhourtemplateLookupAttributeNames = ""msdyn_workhourtemplate"";
    type PricelevelLookupAttributeNames = ""defaultpricelevelid"";
    type Systemuser_TeamLookupAttributeNames = ""ownerid"";
    type TerritoryLookupAttributeNames = ""msdyn_serviceterritory"";
    type TransactioncurrencyLookupAttributeNames = ""transactioncurrencyid"";
    // Base Controls
    type AttributeControlNames = ""name1"" | ""tickersymbol"";
    type BaseControlNames = ""ActionCards"" | ""mapcontrol"" | ""notescontrol"";
    type BooleanControlNames = ""creditonhold"" | ""donotbulkemail"" | ""donotemail"" | ""donotfax"" | ""donotphone"" | ""donotpostalmail"" | ""msdyn_taxexempt"";
    type ControlNames = AttributeControlNames | BaseControlNames | BooleanControlNames | LookupControlNames | NumberControlNames | OptionSetControlNames | StringControlNames | SubGridControlNames;
    type LookupControlNames = AccountLookupControlNames | ContactLookupControlNames | Msdyn_TaxcodeLookupControlNames | Msdyn_WorkhourtemplateLookupControlNames | PricelevelLookupControlNames | Systemuser_TeamLookupControlNames | TerritoryLookupControlNames | TransactioncurrencyLookupControlNames;
    type NumberControlNames = ""address1_latitude"" | ""address1_longitude"" | ""creditlimit"" | ""header_numberofemployees"" | ""header_revenue"" | ""msdyn_travelcharge"";
    type OptionSetControlNames = Account_Address1_FreighttermscodeControlNames | Account_Address1_ShippingmethodcodeControlNames | Account_CustomertypecodeControlNames | Account_IndustrycodeControlNames | Account_OwnershipcodeControlNames | Account_PaymenttermscodeControlNames | Account_PreferredcontactmethodcodeControlNames | Msdyn_TravelchargetypeControlNames;
    type StringControlNames = ""address1_composite"" | ""address1_composite_compositionLinkControl_address1_city"" | ""address1_composite_compositionLinkControl_address1_country"" | ""address1_composite_compositionLinkControl_address1_line1"" | ""address1_composite_compositionLinkControl_address1_line2"" | ""address1_composite_compositionLinkControl_address1_line3"" | ""address1_composite_compositionLinkControl_address1_postalcode"" | ""address1_composite_compositionLinkControl_address1_stateorprovince"" | ""description"" | ""fax"" | ""msdyn_taxexemptnumber"" | ""msdyn_workorderinstructions"" | ""msdyusd_facebook"" | ""msdyusd_twitter"" | ""name"" | ""sic"" | ""telephone1"" | ""websiteurl"";
    type SubGridControlNames = ContactSubGridControlNames | Msdyn_AccountpricelistSubGridControlNames | SharepointdocumentSubGridControlNames;
    // Form Specific Control Types
    type Account_Address1_FreighttermscodeControlNames = ""address1_freighttermscode"";
    type Account_Address1_ShippingmethodcodeControlNames = ""address1_shippingmethodcode"";
    type Account_CustomertypecodeControlNames = ""customertypecode"";
    type Account_IndustrycodeControlNames = ""industrycode"";
    type Account_OwnershipcodeControlNames = ""ownershipcode"";
    type Account_PaymenttermscodeControlNames = ""paymenttermscode"";
    type Account_PreferredcontactmethodcodeControlNames = ""preferredcontactmethodcode"";
    type AccountLookupControlNames = ""msdyn_billingaccount"" | ""parentaccountid"";
    type ContactLookupControlNames = ""primarycontactid"";
    type ContactSubGridControlNames = ""Contacts"";
    type Msdyn_AccountpricelistSubGridControlNames = ""PriceListsGrid"";
    type Msdyn_TaxcodeLookupControlNames = ""msdyn_salestaxcode"";
    type Msdyn_TravelchargetypeControlNames = ""msdyn_travelchargetype"";
    type Msdyn_WorkhourtemplateLookupControlNames = ""msdyn_workhourtemplate"";
    type PricelevelLookupControlNames = ""defaultpricelevelid"" | ""defaultpricelevelid1"";
    type SharepointdocumentSubGridControlNames = ""DocumentsSubGrid"";
    type Systemuser_TeamLookupControlNames = ""header_ownerid"";
    type TerritoryLookupControlNames = ""msdyn_serviceterritory"";
    type TransactioncurrencyLookupControlNames = ""transactioncurrencyid"";
  }}";
        }

        [TestMethod]
        public void WriteAddOnChangeValues_Account_Should_GenerateTypings()
        {
            ExecuteForAllOptions(MainAccountForm, (file, parser, xrm) => {
                var output = new List<string>();
                Sut.WriteAddOnChangeValues(output, file.FormName, parser);
                output.ShouldEqualWithDiff(GetExpectedAccountFormInterfaceAddOnChange(file, xrm));
            });
        }

        private static string GetExpectedAccountFormInterfaceAddOnChange(FileHelper file, string xrm)
        {
            return $@"    addOnChange(attributeName: {file.FormName}.Account_Address1_FreighttermscodeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<account_address1_freighttermscode>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Account_Address1_ShippingmethodcodeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<account_address1_shippingmethodcode>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Account_CustomertypecodeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<account_customertypecode>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Account_IndustrycodeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<account_industrycode>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Account_OwnershipcodeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<account_ownershipcode>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Account_PaymenttermscodeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<account_paymenttermscode>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Account_PreferredcontactmethodcodeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<account_preferredcontactmethodcode>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.AccountLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""account"">, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.AnyAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.Attribute<any>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.BooleanAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<boolean>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.ContactLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""contact"">, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Msdyn_TaxcodeLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""msdyn_taxcode"">, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Msdyn_TravelchargetypeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<msdyn_travelchargetype>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Msdyn_WorkhourtemplateLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""msdyn_workhourtemplate"">, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.NumberAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.NumberAttribute, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.PricelevelLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""pricelevel"">, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.StringAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.Attribute<string>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Systemuser_TeamLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""systemuser"" | ""team"">, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.TerritoryLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""territory"">, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.TransactioncurrencyLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""transactioncurrency"">, undefined>) => any): void;
    addOnChange(attributeNames: {file.FormName}.AttributeNames[], handler: (context?: {xrm}.ExecutionContext<{xrm}.Attribute<any>, undefined>) => any): void;";
        }

        [TestMethod]
        public void WriteGetValues_Account_Should_GenerateTypings()
        {
            ExecuteForAllOptions(MainAccountForm, (file, parser, xrm) => {
                var output = new List<string>();
                Sut.WriteGetValues(output, file.FormName, parser);
                output.ShouldEqualWithDiff(GetExpectedAccountFormInterfaceGetValue(file, xrm));
            });
        }

        private static string GetExpectedAccountFormInterfaceGetValue(FileHelper file, string xrm)
        {
            return $@"    getValue(attributeName: {file.FormName}.Account_Address1_FreighttermscodeAttributeNames): account_address1_freighttermscode | null;
    getValue(attributeName: {file.FormName}.Account_Address1_ShippingmethodcodeAttributeNames): account_address1_shippingmethodcode | null;
    getValue(attributeName: {file.FormName}.Account_CustomertypecodeAttributeNames): account_customertypecode | null;
    getValue(attributeName: {file.FormName}.Account_IndustrycodeAttributeNames): account_industrycode | null;
    getValue(attributeName: {file.FormName}.Account_OwnershipcodeAttributeNames): account_ownershipcode | null;
    getValue(attributeName: {file.FormName}.Account_PaymenttermscodeAttributeNames): account_paymenttermscode | null;
    getValue(attributeName: {file.FormName}.Account_PreferredcontactmethodcodeAttributeNames): account_preferredcontactmethodcode | null;
    getValue(attributeName: {file.FormName}.AccountLookupAttributeNames): {xrm}.EntityReference<""account""> | null;
    getValue(attributeName: {file.FormName}.AnyAttributeNames): any | null;
    getValue(attributeName: {file.FormName}.BooleanAttributeNames): boolean;
    getValue(attributeName: {file.FormName}.ContactLookupAttributeNames): {xrm}.EntityReference<""contact""> | null;
    getValue(attributeName: {file.FormName}.Msdyn_TaxcodeLookupAttributeNames): {xrm}.EntityReference<""msdyn_taxcode""> | null;
    getValue(attributeName: {file.FormName}.Msdyn_TravelchargetypeAttributeNames): msdyn_travelchargetype | null;
    getValue(attributeName: {file.FormName}.Msdyn_WorkhourtemplateLookupAttributeNames): {xrm}.EntityReference<""msdyn_workhourtemplate""> | null;
    getValue(attributeName: {file.FormName}.NumberAttributeNames): number | null;
    getValue(attributeName: {file.FormName}.PricelevelLookupAttributeNames): {xrm}.EntityReference<""pricelevel""> | null;
    getValue(attributeName: {file.FormName}.StringAttributeNames): string | null;
    getValue(attributeName: {file.FormName}.Systemuser_TeamLookupAttributeNames): {xrm}.EntityReference<""systemuser"" | ""team""> | null;
    getValue(attributeName: {file.FormName}.TerritoryLookupAttributeNames): {xrm}.EntityReference<""territory""> | null;
    getValue(attributeName: {file.FormName}.TransactioncurrencyLookupAttributeNames): {xrm}.EntityReference<""transactioncurrency""> | null;";
        }

        [TestMethod]
        public void WriteSetValues_Account_Should_GenerateTypings()
        {
            ExecuteForAllOptions(MainAccountForm, (file, parser, xrm) => {
                var output = new List<string>();
                Sut.WriteSetValues(output, file.FormName, parser);
                output.ShouldEqualWithDiff(GetExpectedAccountFormInterfaceSetValue(file, xrm));
            });
        }
        private static string GetExpectedAccountFormInterfaceSetValue(FileHelper file, string xrm)
        {
            var expected = $@"    setValue(attributeName: {file.FormName}.Account_Address1_FreighttermscodeAttributeNames, value: account_address1_freighttermscode | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.Account_Address1_ShippingmethodcodeAttributeNames, value: account_address1_shippingmethodcode | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.Account_CustomertypecodeAttributeNames, value: account_customertypecode | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.Account_IndustrycodeAttributeNames, value: account_industrycode | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.Account_OwnershipcodeAttributeNames, value: account_ownershipcode | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.Account_PaymenttermscodeAttributeNames, value: account_paymenttermscode | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.Account_PreferredcontactmethodcodeAttributeNames, value: account_preferredcontactmethodcode | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.AccountLookupAttributeNames, value: {xrm}.EntityReference<""account""> | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.AnyAttributeNames, value: any | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.BooleanAttributeNames, value: boolean, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.ContactLookupAttributeNames, value: {xrm}.EntityReference<""contact""> | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.Msdyn_TaxcodeLookupAttributeNames, value: {xrm}.EntityReference<""msdyn_taxcode""> | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.Msdyn_TravelchargetypeAttributeNames, value: msdyn_travelchargetype | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.Msdyn_WorkhourtemplateLookupAttributeNames, value: {xrm}.EntityReference<""msdyn_workhourtemplate""> | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.NumberAttributeNames, value: number | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.PricelevelLookupAttributeNames, value: {xrm}.EntityReference<""pricelevel""> | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.StringAttributeNames, value: string | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.Systemuser_TeamLookupAttributeNames, value: {xrm}.EntityReference<""systemuser"" | ""team""> | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.TerritoryLookupAttributeNames, value: {xrm}.EntityReference<""territory""> | null, fireOnChange?: boolean): void;
    setValue(attributeName: {file.FormName}.TransactioncurrencyLookupAttributeNames, value: {xrm}.EntityReference<""transactioncurrency""> | null, fireOnChange?: boolean): void;";
            return expected;
        }

        [TestMethod]
        public void XrmNamespace_Should_BeReplaced()
        {
            var file = new[] {"<start> Xrm xrmXrm XrmQuery <Xrm> ,Xrm <end>"};
            var xrm = Sut.Settings.XrmNamespacePrefix;
            Assert.IsTrue(Sut.UpdateXrmNamespace(file, false));

            file[0].ShouldEqualWithDiff($"<start> {xrm} xrmXrm XrmQuery <{xrm}> ,{xrm} <end>");
        }

        private void ExecuteForAllOptions(FileHelper file, Action<FileHelper, ParsedXdtForm, string> action)
        {
            var input = (string[])file.Contents.Clone();
            Sut.UpdateDtFile(input); 
            var parser = new XdtFormParser().Parse(input);
            action(file, parser, Sut.Settings.XrmNamespacePrefix);

            Sut.Settings.XrmNamespaceOverride = null;
            parser = new XdtFormParser().Parse(file.Contents);
            action(file, parser, Sut.Settings.XrmNamespacePrefix);
        }

        private void ExecuteForAllOptions(string[] input, Action<ParsedXdtForm, string> action)
        {
            Sut.UpdateDtFile(input);
            var parser = new XdtFormParser().Parse(input);
            action(parser, Sut.Settings.XrmNamespacePrefix);

            Sut.Settings.XrmNamespaceOverride = null;
            parser = new XdtFormParser().Parse(input);
            action(parser, Sut.Settings.XrmNamespacePrefix);
        }

        [TestMethod]
        public void FormAttributesAndControlsBases_Should_BeAdded()
        {
            var file = new[] { "" };
            Sut.AddFormAttributesAndControlsBases(file);
            var xrm = Sut.Settings.XrmNamespacePrefix;

            string.Join(Environment.NewLine, file).ShouldEqualWithDiff(@"

// Added via XDT.PostProcessor
declare namespace {XRMXRM} {
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
   * Interface for a generic {XRMXRM}.Page
   */
  interface FormContext extends {XRMXRM}.PageBase<any, any, any> {
    /**
     * Generic getAttribute
     */
    getAttribute(attrName: string): {XRMXRM}.Attribute<any> | undefined;
    /**
     * Generic getControl
     */
    getControl(ctrlName: string): {XRMXRM}.AnyControl | undefined;
  }
  interface BaseExecutionContext extends {XRMXRM}.ExecutionContext<any, any> {
    getFormContext<T extends {XRMXRM}.PageBase<any, any, any>>(): T;
  }
}
".Replace("{XRMXRM}", xrm));
        }
    }
}

