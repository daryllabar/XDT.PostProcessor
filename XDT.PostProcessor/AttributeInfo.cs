namespace XDT.PostProcessor
{
    public struct AttributeInfo
    {
        public string AttributeType { get; set; }
        public string Name { get; }
        public string ValueType { get; }

        public AttributeInfo(string name, string attributeType, string valueType)
        {
            Name = name;
            AttributeType = attributeType;
            ValueType = valueType;
        }
    }
}
