using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XDT.PostProcessor.Test
{
    [TestClass]
    public class LogicTests
    {
        public Logic Sut { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Sut = new Logic(Settings.Default);
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
            var contents = Sut.CreateFormExtContents(file.Table, file.FormType, file.FormName, file.Contents);
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
  }}
  interface {file.FormName} extends {file.XdtFullyQualifiedFormInterfaceName} {{
    getValue(attributeName: Account_address1_freighttermscodeAttributeNames): account_address1_freighttermscode | null;
    getValue(attributeName: Account_address1_shippingmethodcodeAttributeNames): account_address1_shippingmethodcode | null;
    getValue(attributeName: Account_customertypecodeAttributeNames): account_customertypecode | null;
    getValue(attributeName: Account_industrycodeAttributeNames): account_industrycode | null;
    getValue(attributeName: Account_ownershipcodeAttributeNames): account_ownershipcode | null;
    getValue(attributeName: Account_paymenttermscodeAttributeNames): account_paymenttermscode | null;
    getValue(attributeName: Account_preferredcontactmethodcodeAttributeNames): account_preferredcontactmethodcode | null;
    getValue(attributeName: AccountLookupAttributeNames): Xrm.EntityReference<""account""> | null;
    getValue(attributeName: AnyAttributeNames): any | null;
    getValue(attributeName: BooleanAttributeNames): boolean;
    getValue(attributeName: ContactLookupAttributeNames): Xrm.EntityReference<""contact""> | null;
    getValue(attributeName: Msdyn_taxcodeLookupAttributeNames): Xrm.EntityReference<""msdyn_taxcode""> | null;
    getValue(attributeName: Msdyn_travelchargetypeAttributeNames): msdyn_travelchargetype | null;
    getValue(attributeName: Msdyn_workhourtemplateLookupAttributeNames): Xrm.EntityReference<""msdyn_workhourtemplate""> | null;
    getValue(attributeName: NumberAttributeNames): number | null;
    getValue(attributeName: PricelevelLookupAttributeNames): Xrm.EntityReference<""pricelevel""> | null;
    getValue(attributeName: StringAttributeNames): string;
    getValue(attributeName: Systemuser_TeamLookupAttributeNames): Xrm.EntityReference<""systemuser"" | ""team""> | null;
    getValue(attributeName: TerritoryLookupAttributeNames): Xrm.EntityReference<""territory""> | null;
    getValue(attributeName: TransactioncurrencyLookupAttributeNames): Xrm.EntityReference<""transactioncurrency""> | null;
  }}
}}";
            contents.ShouldEqualWithDiff(expected);
        }
    }
}
