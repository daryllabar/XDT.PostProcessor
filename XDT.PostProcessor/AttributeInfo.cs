using System;

namespace XDT.PostProcessor
{
    public readonly struct AttributeInfo : IEquatable<AttributeInfo>
    {
        public string AttributeType { get; }
        public string Name { get; }
        public string ValueType { get; }

        public AttributeInfo(string name, string attributeType, string valueType)
        {
            Name = name;
            AttributeType = attributeType;
            ValueType = valueType;
        }

        public override int GetHashCode() => (AttributeType + Name + ValueType).GetHashCode();
        public override bool Equals(object other) => other is AttributeInfo l && Equals(l);
        public bool Equals(AttributeInfo other) => AttributeType == other.AttributeType
                                                   && Name == other.Name
                                                   && ValueType == other.ValueType;

        public override string ToString()
        {
            return $"{{ AttributeType: {AttributeType}, Name: {Name}, ValueType: {ValueType} }}";
        }
    }
}
