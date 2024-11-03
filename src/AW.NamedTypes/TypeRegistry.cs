using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AW.NamedTypes;

public class TypeRegistry
{
    public static readonly TypeRegistry Instance = new();

    private readonly Dictionary<string, Type> _nameToType = [];
    private readonly Dictionary<Type, string> _typeToName = [];
    private readonly object _lock = new();

    public bool Register(Type type, string typeName)
    {
        ArgumentNullException.ThrowIfNull(type, nameof(type));
        ArgumentException.ThrowIfNullOrWhiteSpace(typeName, nameof(typeName));

        lock (_lock)
        {
            if (_nameToType.ContainsKey(typeName))
                return false;

            _nameToType[typeName] = type;
            _typeToName[type] = typeName;

            return true;
        }
    }

    public bool TryResolveName(Type type, out string? typeName)
    {
        ArgumentNullException.ThrowIfNull(type, nameof(type));

        lock (_lock)
        {
            return _typeToName.TryGetValue(type, out typeName);
        }
    }

    public bool TryResolveType(string typeName, out Type? type)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(typeName, nameof(typeName));

        lock (_lock)
        {
            return _nameToType.TryGetValue(typeName, out type);
        }
    }

    public void AutoRegisterTypes(params Assembly[] assemblies)
    {
        lock (_lock)
        {
            try
            {
                var assembliesToCheck = assemblies == null || assemblies.Length == 0
                    ? AppDomain.CurrentDomain.GetAssemblies()
                    : assemblies;

                foreach (var assembly in assembliesToCheck)
                {
                    if (assembly.IsDynamic)
                        continue;

                    try
                    {
                        foreach (var type in assembly.GetTypes())
                        {
                            try
                            {
                                var namedTypeAttribute = type
                                    .GetCustomAttributes(typeof(NamedTypeAttribute), false)
                                    .Cast<NamedTypeAttribute>()
                                    .FirstOrDefault();

                                if (namedTypeAttribute != null)
                                {
                                    _nameToType[namedTypeAttribute.TypeName] = type;
                                    _typeToName[type] = namedTypeAttribute.TypeName;
                                }
                            }
                            catch { }
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}
