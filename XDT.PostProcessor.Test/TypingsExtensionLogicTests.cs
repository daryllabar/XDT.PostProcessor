using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void EmptyDashboard_Should_GenerateEmptyInterface()
        {
            var input = @"
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
".Split(new []{Environment.NewLine}, StringSplitOptions.None);
            var contents = Sut.CreateFormExtContents("account", "InteractionCentricDashboard", "EmptyDashboard", input);
            var expected = $@"declare namespace FormExt.account.InteractionCentricDashboard {{
  namespace EmptyDashboard {{
  }}
  interface EmptyDashboard extends Form.account.InteractionCentricDashboard.EmptyDashboard {{
  }}
}}";
            contents.ShouldEqualWithDiff(expected);  
        }

        [TestMethod]
        public void WriteFormNamespace_Account_Should_GenerateAttributeTypeNames()
        {
            ExecuteForAllOptions(MainAccountForm, (file, parser, xrm)=>{
                var output = new List<string>();
                Sut.WriteFormNamespace(output, file.FormName, parser);
                output.ShouldEqualWithDiff(GetExpectedAccountFormNamespace(file));
            });
        }

        private static string GetExpectedAccountFormNamespace(FileHelper file)
        {
            return $@"  namespace {file.FormName} {{
    type Account_Address1_FreighttermscodeAttributeNames = ""address1_freighttermscode"";
    type Account_Address1_FreighttermscodeControlNames = ""address1_freighttermscode"";
    type Account_Address1_ShippingmethodcodeAttributeNames = ""address1_shippingmethodcode"";
    type Account_Address1_ShippingmethodcodeControlNames = ""address1_shippingmethodcode"";
    type Account_CustomertypecodeAttributeNames = ""customertypecode"";
    type Account_CustomertypecodeControlNames = ""customertypecode"";
    type Account_IndustrycodeAttributeNames = ""industrycode"";
    type Account_IndustrycodeControlNames = ""industrycode"";
    type Account_OwnershipcodeAttributeNames = ""ownershipcode"";
    type Account_OwnershipcodeControlNames = ""ownershipcode"";
    type Account_PaymenttermscodeAttributeNames = ""paymenttermscode"";
    type Account_PaymenttermscodeControlNames = ""paymenttermscode"";
    type Account_PreferredcontactmethodcodeAttributeNames = ""preferredcontactmethodcode"";
    type Account_PreferredcontactmethodcodeControlNames = ""preferredcontactmethodcode"";
    type AccountLookupAttributeNames = ""msdyn_billingaccount"" | ""parentaccountid"";
    type AccountLookupControlNames = ""msdyn_billingaccount"" | ""parentaccountid"";
    type AnyAttributeNames = ""tickersymbol"";
    type AttributeNames = Account_Address1_FreighttermscodeAttributeNames | Account_Address1_ShippingmethodcodeAttributeNames | Account_CustomertypecodeAttributeNames | Account_IndustrycodeAttributeNames | Account_OwnershipcodeAttributeNames | Account_PaymenttermscodeAttributeNames | Account_PreferredcontactmethodcodeAttributeNames | AccountLookupAttributeNames | AnyAttributeNames | BooleanAttributeNames | ContactLookupAttributeNames | Msdyn_TaxcodeLookupAttributeNames | Msdyn_TravelchargetypeAttributeNames | Msdyn_WorkhourtemplateLookupAttributeNames | NumberAttributeNames | PricelevelLookupAttributeNames | StringAttributeNames | Systemuser_TeamLookupAttributeNames | TerritoryLookupAttributeNames | TransactioncurrencyLookupAttributeNames;
    type BaseControlNames = ""ActionCards"" | ""mapcontrol"" | ""notescontrol"";
    type BooleanAttributeNames = ""creditonhold"" | ""donotbulkemail"" | ""donotemail"" | ""donotfax"" | ""donotphone"" | ""donotpostalmail"" | ""msdyn_taxexempt"";
    type BooleanControlNames = ""creditonhold"" | ""donotbulkemail"" | ""donotemail"" | ""donotfax"" | ""donotphone"" | ""donotpostalmail"" | ""msdyn_taxexempt"";
    type ContactLookupAttributeNames = ""primarycontactid"";
    type ContactLookupControlNames = ""primarycontactid"";
    type ContactSubGridControlNames = ""Contacts"";
    type ControlNames = Account_Address1_FreighttermscodeControlNames | Account_Address1_ShippingmethodcodeControlNames | Account_CustomertypecodeControlNames | Account_IndustrycodeControlNames | Account_OwnershipcodeControlNames | Account_PaymenttermscodeControlNames | Account_PreferredcontactmethodcodeControlNames | AccountLookupControlNames | BaseControlNames | BooleanControlNames | ContactLookupControlNames | ContactSubGridControlNames | Msdyn_AccountpricelistSubGridControlNames | Msdyn_TaxcodeLookupControlNames | Msdyn_TravelchargetypeControlNames | Msdyn_WorkhourtemplateLookupControlNames | NumberControlNames | PricelevelLookupControlNames | SharepointdocumentSubGridControlNames | StringControlNames | Systemuser_TeamLookupControlNames | TerritoryLookupControlNames | TransactioncurrencyLookupControlNames;
    type Msdyn_AccountpricelistSubGridControlNames = ""PriceListsGrid"";
    type Msdyn_TaxcodeLookupAttributeNames = ""msdyn_salestaxcode"";
    type Msdyn_TaxcodeLookupControlNames = ""msdyn_salestaxcode"";
    type Msdyn_TravelchargetypeAttributeNames = ""msdyn_travelchargetype"";
    type Msdyn_TravelchargetypeControlNames = ""msdyn_travelchargetype"";
    type Msdyn_WorkhourtemplateLookupAttributeNames = ""msdyn_workhourtemplate"";
    type Msdyn_WorkhourtemplateLookupControlNames = ""msdyn_workhourtemplate"";
    type NumberAttributeNames = ""address1_latitude"" | ""address1_longitude"" | ""creditlimit"" | ""msdyn_travelcharge"" | ""numberofemployees"" | ""revenue"";
    type NumberControlNames = ""address1_latitude"" | ""address1_longitude"" | ""creditlimit"" | ""header_numberofemployees"" | ""header_revenue"" | ""msdyn_travelcharge"";
    type PricelevelLookupAttributeNames = ""defaultpricelevelid"";
    type PricelevelLookupControlNames = ""defaultpricelevelid"" | ""defaultpricelevelid1"";
    type SharepointdocumentSubGridControlNames = ""DocumentsSubGrid"";
    type StringAttributeNames = ""address1_city"" | ""address1_composite"" | ""address1_country"" | ""address1_line1"" | ""address1_line2"" | ""address1_line3"" | ""address1_postalcode"" | ""address1_stateorprovince"" | ""description"" | ""fax"" | ""msdyn_taxexemptnumber"" | ""msdyn_workorderinstructions"" | ""msdyusd_facebook"" | ""msdyusd_twitter"" | ""name"" | ""sic"" | ""telephone1"" | ""websiteurl"";
    type StringControlNames = ""address1_composite"" | ""address1_composite_compositionLinkControl_address1_city"" | ""address1_composite_compositionLinkControl_address1_country"" | ""address1_composite_compositionLinkControl_address1_line1"" | ""address1_composite_compositionLinkControl_address1_line2"" | ""address1_composite_compositionLinkControl_address1_line3"" | ""address1_composite_compositionLinkControl_address1_postalcode"" | ""address1_composite_compositionLinkControl_address1_stateorprovince"" | ""description"" | ""fax"" | ""msdyn_taxexemptnumber"" | ""msdyn_workorderinstructions"" | ""msdyusd_facebook"" | ""msdyusd_twitter"" | ""name"" | ""sic"" | ""telephone1"" | ""websiteurl"";
    type Systemuser_TeamLookupAttributeNames = ""ownerid"";
    type Systemuser_TeamLookupControlNames = ""header_ownerid"";
    type TerritoryLookupAttributeNames = ""msdyn_serviceterritory"";
    type TerritoryLookupControlNames = ""msdyn_serviceterritory"";
    type TransactioncurrencyLookupAttributeNames = ""transactioncurrencyid"";
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
    getValue(attributeName: {file.FormName}.StringAttributeNames): string;
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
            var expected = $@"    setValue(attributeName: {file.FormName}.Account_Address1_FreighttermscodeAttributeNames, value: account_address1_freighttermscode | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Account_Address1_ShippingmethodcodeAttributeNames, value: account_address1_shippingmethodcode | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Account_CustomertypecodeAttributeNames, value: account_customertypecode | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Account_IndustrycodeAttributeNames, value: account_industrycode | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Account_OwnershipcodeAttributeNames, value: account_ownershipcode | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Account_PaymenttermscodeAttributeNames, value: account_paymenttermscode | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Account_PreferredcontactmethodcodeAttributeNames, value: account_preferredcontactmethodcode | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.AccountLookupAttributeNames, value: {xrm}.EntityReference<""account""> | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.AnyAttributeNames, value: any | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.BooleanAttributeNames, value: boolean, fireOnChange = true);
    setValue(attributeName: {file.FormName}.ContactLookupAttributeNames, value: {xrm}.EntityReference<""contact""> | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Msdyn_TaxcodeLookupAttributeNames, value: {xrm}.EntityReference<""msdyn_taxcode""> | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Msdyn_TravelchargetypeAttributeNames, value: msdyn_travelchargetype | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Msdyn_WorkhourtemplateLookupAttributeNames, value: {xrm}.EntityReference<""msdyn_workhourtemplate""> | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.NumberAttributeNames, value: number | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.PricelevelLookupAttributeNames, value: {xrm}.EntityReference<""pricelevel""> | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.StringAttributeNames, value: string, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Systemuser_TeamLookupAttributeNames, value: {xrm}.EntityReference<""systemuser"" | ""team""> | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.TerritoryLookupAttributeNames, value: {xrm}.EntityReference<""territory""> | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.TransactioncurrencyLookupAttributeNames, value: {xrm}.EntityReference<""transactioncurrency""> | null, fireOnChange = true);";
            return expected;
        }

        [TestMethod]
        public void SingleLineFunctions_Account_Should_GenerateTypings()
        {
            ExecuteForAllOptions(MainAccountForm, (file, form, xrm) => {
                var output = new List<string>();
                var allTypeUnion = TypingsExtensionLogic.GetAllAttributeAndControlNamesTypeUnion(form, file.FormName);
                Sut.WriteGetVisible(output, file.FormName, form);
                output.ShouldEqualWithDiff($"    getVisible(name: {allTypeUnion}): boolean;");
                output = new List<string>();                
                Sut.WriteFireOnChange(output, file.FormName, form);
                output.ShouldEqualWithDiff($"    fireOnChange(attributeName: {file.FormName}.AttributeNames): void;");
                output = new List<string>();
                Sut.WriteRemoveOnChange(output, file.FormName, form);
                output.ShouldEqualWithDiff($"    removeOnChange(attributeName: {file.FormName}.AttributeNames | {file.FormName}.AttributeNames[], handler: (context?: {xrm}.ExecutionContext<{xrm}.Attribute<any>, undefined>) => any): void;");
                output = new List<string>();
                Sut.WriteSetVisible(output, file.FormName, form);
                output.ShouldEqualWithDiff($"    setVisible(name: {allTypeUnion}, visible = true): void;");
            });
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
    }
}

