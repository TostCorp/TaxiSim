using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Reflection;
using Vogen;

namespace TaxiSim.Api.Swagger.SchemaFilters;

public class VogenSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.GetCustomAttribute<ValueObjectAttribute>() is not ValueObjectAttribute attribute)
        {
            return;
        }

        // Since we don't hold the actual type, we can only use the generic attribute
        if (attribute.GetType() is not { IsGenericType: true, GenericTypeArguments.Length: 1 } type)
        {
            return;
        }

        var schemaValueObject = context.SchemaGenerator.GenerateSchema(
            type.GenericTypeArguments[0],
            context.SchemaRepository,
            context.MemberInfo, context.ParameterInfo);

        CopyPublicProperties(schemaValueObject, schema);
    }

    private static void CopyPublicProperties<T>(T oldObject, T newObject)
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

        if (ReferenceEquals(oldObject, newObject))
        {
            return;
        }

        var type = typeof(T);
        if (type.GetProperties(flags) is not { Length: > 0 } propertyList)
        {
            return;
        }

        foreach (var newObjProp in propertyList)
        {
            if (!newObjProp.CanRead || !newObjProp.CanWrite)
            {
                continue;
            }

            var value = newObjProp.GetValue(oldObject);
            newObjProp.SetValue(newObject, value);
        }
    }
}
