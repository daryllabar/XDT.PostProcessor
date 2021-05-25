declare namespace Form.systemuser.Main {
  namespace User {
    namespace Tabs {
      interface ADMINISTRATION_TAB extends Xrm.SectionCollectionBase {
        get(name: "Email configuration"): Xrm.PageSection;
        get(name: "administration"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface DETAILS_TAB extends Xrm.SectionCollectionBase {
        get(name: "mailing address"): Xrm.PageSection;
        get(name: "user information_2"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface SUMMARY_TAB extends Xrm.SectionCollectionBase {
        get(name: "SOCIAL_PANE_TAB"): Xrm.PageSection;
        get(name: "online account information"): Xrm.PageSection;
        get(name: "onpremise account information"): Xrm.PageSection;
        get(name: "organization information"): Xrm.PageSection;
        get(name: "queue selection"): Xrm.PageSection;
        get(name: "teams information"): Xrm.PageSection;
        get(name: "user information"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface VirtualAgentDetailsTab extends Xrm.SectionCollectionBase {
        get(name: "tab_8_section_1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface VirtualAgentSummaryTab extends Xrm.SectionCollectionBase {
        get(name: "tab_7_section_1"): Xrm.PageSection;
        get(name: "tab_7_section_2"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface tab_6 extends Xrm.SectionCollectionBase {
        get(name: "SECTION_Skills"): Xrm.PageSection;
        get(name: "tab_6_section_2"): Xrm.PageSection;
        get(name: "tab_6_section_5"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface usrstab extends Xrm.SectionCollectionBase {
        get(name: "tab_5_section_2"): Xrm.PageSection;
        get(name: "tab_5_section_3"): Xrm.PageSection;
        get(name: "urstab_section_general"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "accessmode"): Xrm.OptionSetAttribute<systemuser_accessmode>;
      get(name: "address1_city"): Xrm.Attribute<string> | null;
      get(name: "address1_composite"): Xrm.Attribute<string> | null;
      get(name: "address1_country"): Xrm.Attribute<string> | null;
      get(name: "address1_fax"): Xrm.Attribute<string>;
      get(name: "address1_latitude"): Xrm.NumberAttribute;
      get(name: "address1_line1"): Xrm.Attribute<string> | null;
      get(name: "address1_line2"): Xrm.Attribute<string> | null;
      get(name: "address1_line3"): Xrm.Attribute<string> | null;
      get(name: "address1_longitude"): Xrm.NumberAttribute;
      get(name: "address1_postalcode"): Xrm.Attribute<string> | null;
      get(name: "address1_stateorprovince"): Xrm.Attribute<string> | null;
      get(name: "address1_telephone1"): Xrm.Attribute<string>;
      get(name: "address1_telephone2"): Xrm.Attribute<string>;
      get(name: "address1_telephone3"): Xrm.Attribute<string>;
      get(name: "address2_city"): Xrm.Attribute<string> | null;
      get(name: "address2_composite"): Xrm.Attribute<string> | null;
      get(name: "address2_country"): Xrm.Attribute<string> | null;
      get(name: "address2_line1"): Xrm.Attribute<string> | null;
      get(name: "address2_line2"): Xrm.Attribute<string> | null;
      get(name: "address2_line3"): Xrm.Attribute<string> | null;
      get(name: "address2_postalcode"): Xrm.Attribute<string> | null;
      get(name: "address2_stateorprovince"): Xrm.Attribute<string> | null;
      get(name: "applicationid"): Xrm.Attribute<string>;
      get(name: "azureactivedirectoryobjectid"): Xrm.Attribute<string>;
      get(name: "businessunitid"): Xrm.LookupAttribute<"businessunit">;
      get(name: "caltype"): Xrm.OptionSetAttribute<systemuser_caltype>;
      get(name: "defaultmailbox"): Xrm.LookupAttribute<"mailbox">;
      get(name: "domainname"): Xrm.Attribute<string>;
      get(name: "fullname"): Xrm.Attribute<string> | null;
      get(name: "homephone"): Xrm.Attribute<string>;
      get(name: "internalemailaddress"): Xrm.Attribute<string>;
      get(name: "invitestatuscode"): Xrm.OptionSetAttribute<systemuser_invitestatuscode>;
      get(name: "isdisabled"): Xrm.OptionSetAttribute<boolean>;
      get(name: "mobilealertemail"): Xrm.Attribute<string>;
      get(name: "mobilephone"): Xrm.Attribute<string>;
      get(name: "msdyn_agentType"): Xrm.OptionSetAttribute<msdyn_systemuser_msdyn_agenttype>;
      get(name: "msdyn_botapplicationid"): Xrm.Attribute<string>;
      get(name: "msdyn_botdescription"): Xrm.Attribute<string>;
      get(name: "msdyn_botprovider"): Xrm.OptionSetAttribute<msdyn_systemuser_msdyn_botprovider>;
      get(name: "msdyn_capacity"): Xrm.NumberAttribute;
      get(name: "msdyn_defaultpresenceiduser"): Xrm.LookupAttribute<"msdyn_presence">;
      get(name: "msdyusd_usdconfigurationid"): Xrm.LookupAttribute<"msdyusd_configuration">;
      get(name: "nickname"): Xrm.Attribute<string>;
      get(name: "parentsystemuserid"): Xrm.LookupAttribute<"systemuser">;
      get(name: "personalemailaddress"): Xrm.Attribute<string>;
      get(name: "preferredaddresscode"): Xrm.OptionSetAttribute<systemuser_preferredaddresscode>;
      get(name: "preferredphonecode"): Xrm.OptionSetAttribute<systemuser_preferredphonecode>;
      get(name: "queueid"): Xrm.LookupAttribute<"queue">;
      get(name: "siteid"): Xrm.LookupAttribute<"site">;
      get(name: "territoryid"): Xrm.LookupAttribute<"territory">;
      get(name: "title"): Xrm.Attribute<string>;
      get(name: "windowsliveid"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "BookableResources"): Xrm.SubGridControl<"bookableresource">;
      get(name: "LiveEngagementQueues"): Xrm.SubGridControl<"queue">;
      get(name: "OmnichannelQueues"): Xrm.SubGridControl<"queue">;
      get(name: "TeamsSubGrid"): Xrm.SubGridControl<"team">;
      get(name: "accessmode"): Xrm.OptionSetControl<systemuser_accessmode>;
      get(name: "address1_composite"): Xrm.StringControl | null;
      get(name: "address1_composite_compositionLinkControl_address1_city"): Xrm.StringControl | null;
      get(name: "address1_composite_compositionLinkControl_address1_country"): Xrm.StringControl | null;
      get(name: "address1_composite_compositionLinkControl_address1_line1"): Xrm.StringControl | null;
      get(name: "address1_composite_compositionLinkControl_address1_line2"): Xrm.StringControl | null;
      get(name: "address1_composite_compositionLinkControl_address1_line3"): Xrm.StringControl | null;
      get(name: "address1_composite_compositionLinkControl_address1_postalcode"): Xrm.StringControl | null;
      get(name: "address1_composite_compositionLinkControl_address1_stateorprovince"): Xrm.StringControl | null;
      get(name: "address1_fax"): Xrm.StringControl;
      get(name: "address1_latitude"): Xrm.NumberControl;
      get(name: "address1_longitude"): Xrm.NumberControl;
      get(name: "address1_telephone1"): Xrm.StringControl;
      get(name: "address1_telephone2"): Xrm.StringControl;
      get(name: "address1_telephone3"): Xrm.StringControl;
      get(name: "address2_composite"): Xrm.StringControl | null;
      get(name: "address2_composite_compositionLinkControl_address2_city"): Xrm.StringControl | null;
      get(name: "address2_composite_compositionLinkControl_address2_country"): Xrm.StringControl | null;
      get(name: "address2_composite_compositionLinkControl_address2_line1"): Xrm.StringControl | null;
      get(name: "address2_composite_compositionLinkControl_address2_line2"): Xrm.StringControl | null;
      get(name: "address2_composite_compositionLinkControl_address2_line3"): Xrm.StringControl | null;
      get(name: "address2_composite_compositionLinkControl_address2_postalcode"): Xrm.StringControl | null;
      get(name: "address2_composite_compositionLinkControl_address2_stateorprovince"): Xrm.StringControl | null;
      get(name: "applicationid"): Xrm.StringControl;
      get(name: "azureactivedirectoryobjectid"): Xrm.StringControl;
      get(name: "businessunitid"): Xrm.LookupControl<"businessunit">;
      get(name: "caltype"): Xrm.OptionSetControl<systemuser_caltype>;
      get(name: "default_presence_user"): Xrm.LookupControl<"msdyn_presence">;
      get(name: "defaultmailbox"): Xrm.LookupControl<"mailbox">;
      get(name: "domainname"): Xrm.StringControl;
      get(name: "footer_isdisabled"): Xrm.OptionSetControl<boolean>;
      get(name: "fullname"): Xrm.StringControl | null;
      get(name: "fullname1"): Xrm.StringControl | null;
      get(name: "homephone"): Xrm.StringControl;
      get(name: "internalemailaddress"): Xrm.StringControl;
      get(name: "invitestatuscode"): Xrm.OptionSetControl<systemuser_invitestatuscode>;
      get(name: "mobilealertemail"): Xrm.StringControl;
      get(name: "mobilephone"): Xrm.StringControl;
      get(name: "msdyn_agentType"): Xrm.OptionSetControl<msdyn_systemuser_msdyn_agenttype>;
      get(name: "msdyn_botapplicationid"): Xrm.StringControl;
      get(name: "msdyn_botdescription"): Xrm.StringControl;
      get(name: "msdyn_botprovider"): Xrm.OptionSetControl<msdyn_systemuser_msdyn_botprovider>;
      get(name: "msdyn_capacity"): Xrm.NumberControl;
      get(name: "msdyn_capacity1"): Xrm.NumberControl;
      get(name: "msdyusd_usdconfigurationid"): Xrm.LookupControl<"msdyusd_configuration">;
      get(name: "nickname"): Xrm.StringControl;
      get(name: "notescontrol"): Xrm.BaseControl;
      get(name: "parentsystemuserid"): Xrm.LookupControl<"systemuser">;
      get(name: "personalemailaddress"): Xrm.StringControl;
      get(name: "preferredaddresscode"): Xrm.OptionSetControl<systemuser_preferredaddresscode>;
      get(name: "preferredphonecode"): Xrm.OptionSetControl<systemuser_preferredphonecode>;
      get(name: "queueid"): Xrm.LookupControl<"queue">;
      get(name: "siteid"): Xrm.LookupControl<"site">;
      get(name: "territoryid"): Xrm.LookupControl<"territory">;
      get(name: "title"): Xrm.StringControl;
      get(name: "windowsliveid"): Xrm.StringControl;
      get(name: string): undefined;
      get(): Xrm.BaseControl[];
      get(index: number): Xrm.BaseControl;
      get(chooser: (item: Xrm.BaseControl, index: number) => boolean): Xrm.BaseControl[];
    }
    interface Tabs extends Xrm.TabCollectionBase {
      get(name: "ADMINISTRATION_TAB"): Xrm.PageTab<Tabs.ADMINISTRATION_TAB>;
      get(name: "DETAILS_TAB"): Xrm.PageTab<Tabs.DETAILS_TAB>;
      get(name: "SUMMARY_TAB"): Xrm.PageTab<Tabs.SUMMARY_TAB>;
      get(name: "VirtualAgentDetailsTab"): Xrm.PageTab<Tabs.VirtualAgentDetailsTab>;
      get(name: "VirtualAgentSummaryTab"): Xrm.PageTab<Tabs.VirtualAgentSummaryTab>;
      get(name: "tab_6"): Xrm.PageTab<Tabs.tab_6>;
      get(name: "usrstab"): Xrm.PageTab<Tabs.usrstab>;
      get(name: string): undefined;
      get(): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
      get(index: number): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>;
      get(chooser: (item: Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>, index: number) => boolean): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
    }
  }
  interface User extends Xrm.PageBase<User.Attributes,User.Tabs,User.Controls> {
    getAttribute(attributeName: "accessmode"): Xrm.OptionSetAttribute<systemuser_accessmode>;
    getAttribute(attributeName: "address1_city"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "address1_composite"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "address1_country"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "address1_fax"): Xrm.Attribute<string>;
    getAttribute(attributeName: "address1_latitude"): Xrm.NumberAttribute;
    getAttribute(attributeName: "address1_line1"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "address1_line2"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "address1_line3"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "address1_longitude"): Xrm.NumberAttribute;
    getAttribute(attributeName: "address1_postalcode"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "address1_stateorprovince"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "address1_telephone1"): Xrm.Attribute<string>;
    getAttribute(attributeName: "address1_telephone2"): Xrm.Attribute<string>;
    getAttribute(attributeName: "address1_telephone3"): Xrm.Attribute<string>;
    getAttribute(attributeName: "address2_city"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "address2_composite"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "address2_country"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "address2_line1"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "address2_line2"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "address2_line3"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "address2_postalcode"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "address2_stateorprovince"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "applicationid"): Xrm.Attribute<string>;
    getAttribute(attributeName: "azureactivedirectoryobjectid"): Xrm.Attribute<string>;
    getAttribute(attributeName: "businessunitid"): Xrm.LookupAttribute<"businessunit">;
    getAttribute(attributeName: "caltype"): Xrm.OptionSetAttribute<systemuser_caltype>;
    getAttribute(attributeName: "defaultmailbox"): Xrm.LookupAttribute<"mailbox">;
    getAttribute(attributeName: "domainname"): Xrm.Attribute<string>;
    getAttribute(attributeName: "fullname"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "homephone"): Xrm.Attribute<string>;
    getAttribute(attributeName: "internalemailaddress"): Xrm.Attribute<string>;
    getAttribute(attributeName: "invitestatuscode"): Xrm.OptionSetAttribute<systemuser_invitestatuscode>;
    getAttribute(attributeName: "isdisabled"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "mobilealertemail"): Xrm.Attribute<string>;
    getAttribute(attributeName: "mobilephone"): Xrm.Attribute<string>;
    getAttribute(attributeName: "msdyn_agentType"): Xrm.OptionSetAttribute<msdyn_systemuser_msdyn_agenttype>;
    getAttribute(attributeName: "msdyn_botapplicationid"): Xrm.Attribute<string>;
    getAttribute(attributeName: "msdyn_botdescription"): Xrm.Attribute<string>;
    getAttribute(attributeName: "msdyn_botprovider"): Xrm.OptionSetAttribute<msdyn_systemuser_msdyn_botprovider>;
    getAttribute(attributeName: "msdyn_capacity"): Xrm.NumberAttribute;
    getAttribute(attributeName: "msdyn_defaultpresenceiduser"): Xrm.LookupAttribute<"msdyn_presence">;
    getAttribute(attributeName: "msdyusd_usdconfigurationid"): Xrm.LookupAttribute<"msdyusd_configuration">;
    getAttribute(attributeName: "nickname"): Xrm.Attribute<string>;
    getAttribute(attributeName: "parentsystemuserid"): Xrm.LookupAttribute<"systemuser">;
    getAttribute(attributeName: "personalemailaddress"): Xrm.Attribute<string>;
    getAttribute(attributeName: "preferredaddresscode"): Xrm.OptionSetAttribute<systemuser_preferredaddresscode>;
    getAttribute(attributeName: "preferredphonecode"): Xrm.OptionSetAttribute<systemuser_preferredphonecode>;
    getAttribute(attributeName: "queueid"): Xrm.LookupAttribute<"queue">;
    getAttribute(attributeName: "siteid"): Xrm.LookupAttribute<"site">;
    getAttribute(attributeName: "territoryid"): Xrm.LookupAttribute<"territory">;
    getAttribute(attributeName: "title"): Xrm.Attribute<string>;
    getAttribute(attributeName: "windowsliveid"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "BookableResources"): Xrm.SubGridControl<"bookableresource">;
    getControl(controlName: "LiveEngagementQueues"): Xrm.SubGridControl<"queue">;
    getControl(controlName: "OmnichannelQueues"): Xrm.SubGridControl<"queue">;
    getControl(controlName: "TeamsSubGrid"): Xrm.SubGridControl<"team">;
    getControl(controlName: "accessmode"): Xrm.OptionSetControl<systemuser_accessmode>;
    getControl(controlName: "address1_composite"): Xrm.StringControl | null;
    getControl(controlName: "address1_composite_compositionLinkControl_address1_city"): Xrm.StringControl | null;
    getControl(controlName: "address1_composite_compositionLinkControl_address1_country"): Xrm.StringControl | null;
    getControl(controlName: "address1_composite_compositionLinkControl_address1_line1"): Xrm.StringControl | null;
    getControl(controlName: "address1_composite_compositionLinkControl_address1_line2"): Xrm.StringControl | null;
    getControl(controlName: "address1_composite_compositionLinkControl_address1_line3"): Xrm.StringControl | null;
    getControl(controlName: "address1_composite_compositionLinkControl_address1_postalcode"): Xrm.StringControl | null;
    getControl(controlName: "address1_composite_compositionLinkControl_address1_stateorprovince"): Xrm.StringControl | null;
    getControl(controlName: "address1_fax"): Xrm.StringControl;
    getControl(controlName: "address1_latitude"): Xrm.NumberControl;
    getControl(controlName: "address1_longitude"): Xrm.NumberControl;
    getControl(controlName: "address1_telephone1"): Xrm.StringControl;
    getControl(controlName: "address1_telephone2"): Xrm.StringControl;
    getControl(controlName: "address1_telephone3"): Xrm.StringControl;
    getControl(controlName: "address2_composite"): Xrm.StringControl | null;
    getControl(controlName: "address2_composite_compositionLinkControl_address2_city"): Xrm.StringControl | null;
    getControl(controlName: "address2_composite_compositionLinkControl_address2_country"): Xrm.StringControl | null;
    getControl(controlName: "address2_composite_compositionLinkControl_address2_line1"): Xrm.StringControl | null;
    getControl(controlName: "address2_composite_compositionLinkControl_address2_line2"): Xrm.StringControl | null;
    getControl(controlName: "address2_composite_compositionLinkControl_address2_line3"): Xrm.StringControl | null;
    getControl(controlName: "address2_composite_compositionLinkControl_address2_postalcode"): Xrm.StringControl | null;
    getControl(controlName: "address2_composite_compositionLinkControl_address2_stateorprovince"): Xrm.StringControl | null;
    getControl(controlName: "applicationid"): Xrm.StringControl;
    getControl(controlName: "azureactivedirectoryobjectid"): Xrm.StringControl;
    getControl(controlName: "businessunitid"): Xrm.LookupControl<"businessunit">;
    getControl(controlName: "caltype"): Xrm.OptionSetControl<systemuser_caltype>;
    getControl(controlName: "default_presence_user"): Xrm.LookupControl<"msdyn_presence">;
    getControl(controlName: "defaultmailbox"): Xrm.LookupControl<"mailbox">;
    getControl(controlName: "domainname"): Xrm.StringControl;
    getControl(controlName: "footer_isdisabled"): Xrm.OptionSetControl<boolean>;
    getControl(controlName: "fullname"): Xrm.StringControl | null;
    getControl(controlName: "fullname1"): Xrm.StringControl | null;
    getControl(controlName: "homephone"): Xrm.StringControl;
    getControl(controlName: "internalemailaddress"): Xrm.StringControl;
    getControl(controlName: "invitestatuscode"): Xrm.OptionSetControl<systemuser_invitestatuscode>;
    getControl(controlName: "mobilealertemail"): Xrm.StringControl;
    getControl(controlName: "mobilephone"): Xrm.StringControl;
    getControl(controlName: "msdyn_agentType"): Xrm.OptionSetControl<msdyn_systemuser_msdyn_agenttype>;
    getControl(controlName: "msdyn_botapplicationid"): Xrm.StringControl;
    getControl(controlName: "msdyn_botdescription"): Xrm.StringControl;
    getControl(controlName: "msdyn_botprovider"): Xrm.OptionSetControl<msdyn_systemuser_msdyn_botprovider>;
    getControl(controlName: "msdyn_capacity"): Xrm.NumberControl;
    getControl(controlName: "msdyn_capacity1"): Xrm.NumberControl;
    getControl(controlName: "msdyusd_usdconfigurationid"): Xrm.LookupControl<"msdyusd_configuration">;
    getControl(controlName: "nickname"): Xrm.StringControl;
    getControl(controlName: "notescontrol"): Xrm.BaseControl;
    getControl(controlName: "parentsystemuserid"): Xrm.LookupControl<"systemuser">;
    getControl(controlName: "personalemailaddress"): Xrm.StringControl;
    getControl(controlName: "preferredaddresscode"): Xrm.OptionSetControl<systemuser_preferredaddresscode>;
    getControl(controlName: "preferredphonecode"): Xrm.OptionSetControl<systemuser_preferredphonecode>;
    getControl(controlName: "queueid"): Xrm.LookupControl<"queue">;
    getControl(controlName: "siteid"): Xrm.LookupControl<"site">;
    getControl(controlName: "territoryid"): Xrm.LookupControl<"territory">;
    getControl(controlName: "title"): Xrm.StringControl;
    getControl(controlName: "windowsliveid"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}