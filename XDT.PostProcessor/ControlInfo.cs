namespace XDT.PostProcessor
{
    public struct ControlInfo
    {
        public string ControlType { get; set; }
        public string Name { get; }

        public ControlInfo(string name, string controlType)
        {
            Name = name;
            ControlType = controlType;
        }
    }
}
