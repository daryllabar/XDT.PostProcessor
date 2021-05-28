namespace XDT.PostProcessor
{
    public readonly struct ControlInfo
    {
        public string ControlType { get; }
        public string Name { get; }

        public ControlInfo(string name, string controlType)
        {
            Name = name;
            ControlType = controlType;
        }

        public override string ToString()
        {
            return $"{{ ControlType: {ControlType}, Name: {Name} }}";
        }
    }
}
