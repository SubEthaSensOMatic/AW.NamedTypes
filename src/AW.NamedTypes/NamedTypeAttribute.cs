using System;

namespace AW.NamedTypes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class NamedTypeAttribute : Attribute
{
    public string TypeName { get; }

    public NamedTypeAttribute(string typeName)
        => TypeName = typeName;
}
