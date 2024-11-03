# AW.NamedTypes

`AW.NamedTypes` is a .NET library for managing and dynamically resolving types by name. It provides a registry (`TypeRegistry`) that allows for registering types with custom names and resolving them back by name or type. This can be useful for plugins, dependency injection, and scenarios requiring dynamic type loading.

## Features

- Register types with a custom name.
- Resolve types by name or retrieve names from registered types.
- Automatically register types annotated with the `[NamedType]` attribute from specified assemblies.

## Installation

You can add this library to your project via NuGet

- [AW.NamedTypes](https://www.nuget.org/packages/AW.NamedTypes/)

## Usage

### Basic Registration

You can manually register a type with a custom name:

```
using AW.NamedTypes;

public record OrderCreatedEvent(Urn OrderId, ...);

...

TypeRegistry.Instance.Register(typeof(OrderCreatedEvent), "order-created-event");

...
```

### Type Resolution

You can resolve a type by its name or retrieve the registered name of a type:

```
using AW.NamedTypes;

...

if (TypeRegistry.Instance.TryResolveType("order-created-event", out Type? type))
{
    Console.WriteLine($"Resolved Type: {type.FullName}");
}

if (TypeRegistry.Instance.TryResolveName(typeof(OrderCreatedEvent), out string? typeName))
{
    Console.WriteLine($"Resolved Name: {typeName}");
}

...
```

### Auto-Registering Types with `[NamedType]` Attribute

To streamline type registration, you can annotate classes with the `[NamedType]` attribute and auto-register them from specific assemblies:

```
using AW.NamedTypes;

[NamedType("order-created-event")]
public record OrderCreatedEvent(Urn OrderId, ...);

[NamedType("order-cancelled-event")]
public record OrderCancelledEvent(Urn OrderId, ...);

...

// Register all types with NamedType attributes from the current assembly
TypeRegistry.Instance.AutoRegisterTypes(Assembly.GetExecutingAssembly());

...

```
> This is useful when types need to be available at runtime for deserialization or dynamic loading but may not be directly referenced in the code.

> The `AutoRegisterTypes` method can also be called without parameters to scan all loaded assemblies.

## License
This project is licensed under the MIT License.
