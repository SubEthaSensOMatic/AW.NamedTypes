using System.Reflection;

namespace AW.NamedTypes.Tests;

[TestClass]
public class AutoLoadTests
{
    [TestMethod]
    public void TestAutoLoadTypes()
    {
        TypeRegistry.Instance.AutoRegisterTypes(Assembly.LoadFrom("LibWithNamedTypes.dll"));
        Assert.IsTrue(TypeRegistry.Instance.TryResolveType("my-type", out var type));

        var instanceOfMyType = Activator.CreateInstance(type);
        Assert.IsNotNull(instanceOfMyType);
    }
}
