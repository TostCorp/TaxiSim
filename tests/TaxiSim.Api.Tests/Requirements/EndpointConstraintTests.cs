using Microsoft.AspNetCore.Mvc.Routing;

using System.Collections;
using System.Reflection;

using TaxiSim.Api.SharedContext;

using TostCorp.CollectionExtensions;

namespace TaxiSim.Api.Tests.Requirements;
public class EndpointConstraintTests
{
    [Fact]
    public void EndpointsShouldNotReturnArrayAsResult()
    {
        var endpoints = typeof(Program).Assembly.GetTypes()
            .FindAll(p => p.BaseType == typeof(ApiControllerBase))
            .SelectMany(GetMethodsWithAttribute<HttpMethodAttribute>);

        endpoints.Should().AllSatisfy(static x => x.ReturnType.Should().NotBeAssignableTo<IEnumerable>(), "Every endpoint should return an object, not an Enumerable type");
    }

    private static MethodInfo[] GetMethodsWithAttribute<T>(Type type) where T : Attribute
    {
        return type.GetMethods().FindAll(p => p.CustomAttributes.Any(x => x.AttributeType.IsAssignableTo(typeof(T))));
    }
}
