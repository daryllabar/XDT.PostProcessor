using Source.DLaB.Common;

namespace XDT.PostProcessor
{
    public class Settings
    {
        public string FormNamespacePostfix { get; set; }
        public string OutputRelativePath { get; set; }

        public static readonly Settings Default = new Settings
        {
            FormNamespacePostfix = Config.GetAppSettingOrDefault(nameof(FormNamespacePostfix), "Ext"),
            OutputRelativePath = Config.GetAppSettingOrDefault(nameof(OutputRelativePath), "Ext")
        };
    }
}
