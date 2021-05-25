namespace XDT.PostProcessor
{
    public struct AttributeInfo
    {
        //public string AttributeTypesName { get; set; }
        public string Name { get; }
        public string ValueType { get; }

        public AttributeInfo(string name, string valueType)
        {
            Name = name;
            ValueType = valueType;
        }
    }
}
