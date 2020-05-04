using Mono.Cecil;
using System;
using System.Linq;

namespace AnchorModeling.Extensions
{
    public static class PropertyDefinitionExtensions
    {
        public static TypeReference GetFirstGenericArgument(this PropertyDefinition property)
                   => ((GenericInstanceType)property.PropertyType)
                        .GenericArguments[0];

        public static PropertyDefinition FindPropertyByCustomAttribute(this TypeDefinition type, string attributeName)
        => type.Properties
                .FirstOrDefault(p => p.CustomAttributes
                    .Any(a => a.AttributeType.Name.Equals(attributeName, StringComparison.Ordinal)));

        public static CustomAttribute GetCustomAttribute(this PropertyDefinition property, string attributeName)
        => property.CustomAttributes.FirstOrDefault(a => a.AttributeType.Name
                            .Equals(attributeName, StringComparison.Ordinal));

        public static GenericInstanceType ToGenericInstanceType(this TypeReference type, TypeReference genericType)
        {
            GenericInstanceType entityDbSetType = new GenericInstanceType(type);
            entityDbSetType.GenericArguments.Add(genericType);

            return entityDbSetType;
        }

        public static CustomAttribute ToCustomAttribute(this MethodReference constructor, TypeReference typeOfParameter, object parameterValue)
        {
            var attribute = new CustomAttribute(constructor);
            attribute.ConstructorArguments.Add(new CustomAttributeArgument(typeOfParameter, parameterValue));

            return attribute;
        }

        public static object GetFirstConstructorParameter(this CustomAttribute attribute)
            => attribute.ConstructorArguments[0].Value;

        public static PropertyDefinition FindPropertryByName(this TypeDefinition type, string propertyName)
            => type.Properties.First(fp => fp.Name.Equals(propertyName, StringComparison.Ordinal));

        public static void AddField(this TypeReference type, string fieldName, TypeReference fieldType)
        {
            type
                .Resolve()
                .Fields.Add(
                new FieldDefinition(fieldName, FieldAttributes.Public, fieldType));
        }
    }
}