using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XDT.PostProcessor.Test
{
    [TestClass]
    public class TypingsExtensionLogicTests
    {
        public TypingsExtensionLogic Sut { get; set; }
        public string XdtXrm { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Sut = new TypingsExtensionLogic(new Settings());
            XdtXrm = new Regex(Sut.Settings.XrmNamespaceRegEx).Replace(" Xrm ", Sut.Settings.XrmNamespaceOverride).Trim();
        }

        [TestMethod]
        public void AccountsDashboard_Should_GenerateEmptyInterface()
        {
            var file = new FileHelper("account", "InteractionCentricDashboard", "AccountsDashboard.d.ts", Sut.Settings.FormNamespacePostfix);
            var contents = Sut.CreateFormExtContents(file.Table, file.FormType, file.FormName, file.Contents);
            var expected = $@"declare namespace {file.ExtensionNamespace} {{
  namespace {file.FormName} {{
  }}
  interface {file.FormName} extends {file.XdtFullyQualifiedFormInterfaceName} {{
  }}
}}";
            contents.ShouldEqualWithDiff(expected);  
        }

        [TestMethod]
        public void Account_Should_GenerateAttributeTypeNames()
        {
            var file = new FileHelper("account", "Main", "Account.d.ts", Sut.Settings.FormNamespacePostfix);
            var prefix = Sut.Settings.XrmNamespaceOverride;
            Sut.Settings.XrmNamespaceOverride = null;
            var contents = Sut.CreateFormExtContents(file.Table, file.FormType, file.FormName, file.Contents);
            contents.ShouldEqualWithDiff(GetExpectedAccount(file, "Xrm"));

            Sut.Settings.XrmNamespaceOverride = prefix;
            Sut.UpdateDtFile(file.Contents);
            contents = Sut.CreateFormExtContents(file.Table, file.FormType, file.FormName, file.Contents);
            contents.ShouldEqualWithDiff(GetExpectedAccount(file, XdtXrm));
        }

        private static string GetExpectedAccount(FileHelper file, string xrm)
        {
            var expected = $@"declare namespace {file.ExtensionNamespace} {{
  namespace {file.FormName} {{
    type Account_address1_freighttermscodeAttributeNames = ""address1_freighttermscode"";
    type Account_address1_shippingmethodcodeAttributeNames = ""address1_shippingmethodcode"";
    type Account_customertypecodeAttributeNames = ""customertypecode"";
    type Account_industrycodeAttributeNames = ""industrycode"";
    type Account_ownershipcodeAttributeNames = ""ownershipcode"";
    type Account_paymenttermscodeAttributeNames = ""paymenttermscode"";
    type Account_preferredcontactmethodcodeAttributeNames = ""preferredcontactmethodcode"";
    type AccountLookupAttributeNames = ""msdyn_billingaccount"" | ""parentaccountid"";
    type AnyAttributeNames = ""tickersymbol"";
    type BooleanAttributeNames = ""creditonhold"" | ""donotbulkemail"" | ""donotemail"" | ""donotfax"" | ""donotphone"" | ""donotpostalmail"" | ""msdyn_taxexempt"";
    type ContactLookupAttributeNames = ""primarycontactid"";
    type Msdyn_taxcodeLookupAttributeNames = ""msdyn_salestaxcode"";
    type Msdyn_travelchargetypeAttributeNames = ""msdyn_travelchargetype"";
    type Msdyn_workhourtemplateLookupAttributeNames = ""msdyn_workhourtemplate"";
    type NumberAttributeNames = ""address1_latitude"" | ""address1_longitude"" | ""creditlimit"" | ""msdyn_travelcharge"" | ""numberofemployees"" | ""revenue"";
    type PricelevelLookupAttributeNames = ""defaultpricelevelid"";
    type StringAttributeNames = ""address1_city"" | ""address1_composite"" | ""address1_country"" | ""address1_line1"" | ""address1_line2"" | ""address1_line3"" | ""address1_postalcode"" | ""address1_stateorprovince"" | ""description"" | ""fax"" | ""msdyn_taxexemptnumber"" | ""msdyn_workorderinstructions"" | ""msdyusd_facebook"" | ""msdyusd_twitter"" | ""name"" | ""sic"" | ""telephone1"" | ""websiteurl"";
    type Systemuser_TeamLookupAttributeNames = ""ownerid"";
    type TerritoryLookupAttributeNames = ""msdyn_serviceterritory"";
    type TransactioncurrencyLookupAttributeNames = ""transactioncurrencyid"";
    type AttributeNames = Account_address1_freighttermscodeAttributeNames | Account_address1_shippingmethodcodeAttributeNames | Account_customertypecodeAttributeNames | Account_industrycodeAttributeNames | Account_ownershipcodeAttributeNames | Account_paymenttermscodeAttributeNames | Account_preferredcontactmethodcodeAttributeNames | AccountLookupAttributeNames | AnyAttributeNames | BooleanAttributeNames | ContactLookupAttributeNames | Msdyn_taxcodeLookupAttributeNames | Msdyn_travelchargetypeAttributeNames | Msdyn_workhourtemplateLookupAttributeNames | NumberAttributeNames | PricelevelLookupAttributeNames | StringAttributeNames | Systemuser_TeamLookupAttributeNames | TerritoryLookupAttributeNames | TransactioncurrencyLookupAttributeNames;
  }}
  interface {file.FormName} extends {file.XdtFullyQualifiedFormInterfaceName} {{
    addOnChange(attributeName: {file.FormName}.Account_address1_freighttermscodeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<account_address1_freighttermscode>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Account_address1_shippingmethodcodeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<account_address1_shippingmethodcode>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Account_customertypecodeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<account_customertypecode>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Account_industrycodeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<account_industrycode>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Account_ownershipcodeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<account_ownershipcode>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Account_paymenttermscodeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<account_paymenttermscode>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Account_preferredcontactmethodcodeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<account_preferredcontactmethodcode>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.AccountLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""account"">, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.AnyAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.Attribute<any>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.BooleanAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<boolean>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.ContactLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""contact"">, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Msdyn_taxcodeLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""msdyn_taxcode"">, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Msdyn_travelchargetypeAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.OptionSetAttribute<msdyn_travelchargetype>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Msdyn_workhourtemplateLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""msdyn_workhourtemplate"">, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.NumberAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.NumberAttribute, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.PricelevelLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""pricelevel"">, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.StringAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.Attribute<string>, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.Systemuser_TeamLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""systemuser"" | ""team"">, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.TerritoryLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""territory"">, undefined>) => any): void;
    addOnChange(attributeName: {file.FormName}.TransactioncurrencyLookupAttributeNames, handler: (context?: {xrm}.ExecutionContext<{xrm}.LookupAttribute<""transactioncurrency"">, undefined>) => any): void;
    addOnChange(attributeNames: {file.FormName}.AttributeNames[], handler: (context?: {xrm}.ExecutionContext<{xrm}.Attribute<any>, undefined>) => any): void;
    getValue(attributeName: {file.FormName}.Account_address1_freighttermscodeAttributeNames): account_address1_freighttermscode | null;
    getValue(attributeName: {file.FormName}.Account_address1_shippingmethodcodeAttributeNames): account_address1_shippingmethodcode | null;
    getValue(attributeName: {file.FormName}.Account_customertypecodeAttributeNames): account_customertypecode | null;
    getValue(attributeName: {file.FormName}.Account_industrycodeAttributeNames): account_industrycode | null;
    getValue(attributeName: {file.FormName}.Account_ownershipcodeAttributeNames): account_ownershipcode | null;
    getValue(attributeName: {file.FormName}.Account_paymenttermscodeAttributeNames): account_paymenttermscode | null;
    getValue(attributeName: {file.FormName}.Account_preferredcontactmethodcodeAttributeNames): account_preferredcontactmethodcode | null;
    getValue(attributeName: {file.FormName}.AccountLookupAttributeNames): {xrm}.EntityReference<""account""> | null;
    getValue(attributeName: {file.FormName}.AnyAttributeNames): any | null;
    getValue(attributeName: {file.FormName}.BooleanAttributeNames): boolean;
    getValue(attributeName: {file.FormName}.ContactLookupAttributeNames): {xrm}.EntityReference<""contact""> | null;
    getValue(attributeName: {file.FormName}.Msdyn_taxcodeLookupAttributeNames): {xrm}.EntityReference<""msdyn_taxcode""> | null;
    getValue(attributeName: {file.FormName}.Msdyn_travelchargetypeAttributeNames): msdyn_travelchargetype | null;
    getValue(attributeName: {file.FormName}.Msdyn_workhourtemplateLookupAttributeNames): {xrm}.EntityReference<""msdyn_workhourtemplate""> | null;
    getValue(attributeName: {file.FormName}.NumberAttributeNames): number | null;
    getValue(attributeName: {file.FormName}.PricelevelLookupAttributeNames): {xrm}.EntityReference<""pricelevel""> | null;
    getValue(attributeName: {file.FormName}.StringAttributeNames): string;
    getValue(attributeName: {file.FormName}.Systemuser_TeamLookupAttributeNames): {xrm}.EntityReference<""systemuser"" | ""team""> | null;
    getValue(attributeName: {file.FormName}.TerritoryLookupAttributeNames): {xrm}.EntityReference<""territory""> | null;
    getValue(attributeName: {file.FormName}.TransactioncurrencyLookupAttributeNames): {xrm}.EntityReference<""transactioncurrency""> | null;
    setValue(attributeName: {file.FormName}.Account_address1_freighttermscodeAttributeNames, value: account_address1_freighttermscode | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Account_address1_shippingmethodcodeAttributeNames, value: account_address1_shippingmethodcode | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Account_customertypecodeAttributeNames, value: account_customertypecode | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Account_industrycodeAttributeNames, value: account_industrycode | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Account_ownershipcodeAttributeNames, value: account_ownershipcode | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Account_paymenttermscodeAttributeNames, value: account_paymenttermscode | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Account_preferredcontactmethodcodeAttributeNames, value: account_preferredcontactmethodcode | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.AccountLookupAttributeNames, value: {xrm}.EntityReference<""account""> | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.AnyAttributeNames, value: any | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.BooleanAttributeNames, value: boolean, fireOnChange = true);
    setValue(attributeName: {file.FormName}.ContactLookupAttributeNames, value: {xrm}.EntityReference<""contact""> | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Msdyn_taxcodeLookupAttributeNames, value: {xrm}.EntityReference<""msdyn_taxcode""> | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Msdyn_travelchargetypeAttributeNames, value: msdyn_travelchargetype | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Msdyn_workhourtemplateLookupAttributeNames, value: {xrm}.EntityReference<""msdyn_workhourtemplate""> | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.NumberAttributeNames, value: number | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.PricelevelLookupAttributeNames, value: {xrm}.EntityReference<""pricelevel""> | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.StringAttributeNames, value: string, fireOnChange = true);
    setValue(attributeName: {file.FormName}.Systemuser_TeamLookupAttributeNames, value: {xrm}.EntityReference<""systemuser"" | ""team""> | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.TerritoryLookupAttributeNames, value: {xrm}.EntityReference<""territory""> | null, fireOnChange = true);
    setValue(attributeName: {file.FormName}.TransactioncurrencyLookupAttributeNames, value: {xrm}.EntityReference<""transactioncurrency""> | null, fireOnChange = true);
  }}
}}";
            return expected;
        }

        [TestMethod]
        public void XrmNamespace_Should_BeReplaced()
        {
            var file = new[] {"<start> Xrm xrmXrm XrmQuery <Xrm> ,Xrm <end>"};
            Assert.IsTrue(Sut.UpdateXrmNamespace(file, false));
            file[0].ShouldEqualWithDiff($"<start> {XdtXrm} xrmXrm XrmQuery <{XdtXrm}> ,{XdtXrm} <end>");
        }
    }
}

