using System.Text.RegularExpressions;
using Source.DLaB.Common;

namespace XDT.PostProcessor
{
    public class Settings
    {
        public string Es5RegExParser { get; set; }
        public string Es6RegExParser { get; set; }
        public string FormNamespacePostfix { get; set; }
        public string OutputRelativePath { get; set; }
        public string WebpackPostfix { get; set; }
        public string XrmNamespacePrefix => string.IsNullOrWhiteSpace(XrmNamespaceOverride) ? "Xrm" : new Regex(XrmNamespaceRegEx).Replace(" Xrm ", XrmNamespaceOverride).Trim();
        public string XrmNamespaceOverride { get; set; }
        public string XrmNamespaceRegEx { get; set; }
        public bool XrmQueryMakeEs6Compatible { get; set; }
        public bool XrmQueryMakeWebpackCompatible { get; set; }

        public static readonly Settings Default = new Settings();

        public Settings(bool loadFromConfig = true)
        {
            if (!loadFromConfig)
            {
                return;
            }
            FormNamespacePostfix = Config.GetAppSettingOrDefault(nameof(FormNamespacePostfix), "Ext");
            OutputRelativePath = Config.GetAppSettingOrDefault(nameof(OutputRelativePath), "Ext");
            XrmNamespaceOverride = Config.GetAppSettingOrDefault(nameof(XrmNamespaceOverride), "$1XdtXrm$3");
            XrmNamespaceRegEx = Config.GetAppSettingOrDefault(nameof(XrmNamespaceRegEx), "([\\s<,])(Xrm)([^Query])");
            XrmQueryMakeEs6Compatible = Config.GetAppSettingOrDefault(nameof(XrmQueryMakeEs6Compatible), true);
            XrmQueryMakeWebpackCompatible = Config.GetAppSettingOrDefault(nameof(XrmQueryMakeWebpackCompatible), true);
            WebpackPostfix = Config.GetAppSettingOrDefault(nameof(WebpackPostfix), @"
export const load = () =>{
    window.XrmQuery = XrmQuery;
    window.Filter = Filter;
    window.XQW = XQW;
};");
            Es5RegExParser = Config.GetAppSettingOrDefault(nameof(Es5RegExParser), "/function[^\\(]*\\(([a-zA-Z0-9_]+)[^\\{]*\\{([\\s\\S]*)\\}$/m");
            Es6RegExParser = Config.GetAppSettingOrDefault(nameof(Es6RegExParser), "/(?:function)*\\s*\\(?([a-zA-Z0-9_]+)\\)?(?:\\s?=>\\s?\\{?|\\s?\\{)+([\\s\\S]*[^\\}])\\}?$/m");
        }
    }
}
