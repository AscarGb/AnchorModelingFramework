using AnchorModeling.Attributes;
using Mono.Cecil;
using System;
using static AnchorModeling.Names;

namespace AnchorModeling.Basics
{
    public class AnchorAttribute : AnchorBasic
    {
        public AnchorAttribute(ModuleDefinition module, ModuleHelper helper, BaseRefs baseRefs) : base(module, helper, baseRefs)
        {
        }

        public static string GetName(bool isHistorical, string anchSig, string attrSig)
        {
            return $"P_{(isHistorical ? "H_" : "")}{anchSig}_{attrSig}";
        }

        public TypeDefinition Create(TypeDefinition db, string anchorAttrSig, PropertyDefinition prop, bool isHistorical, TypeDefinition anchorEntityType)
        {
            TypeReference propertyType = prop.PropertyType;

            TypeDefinition type = new TypeDefinition(db.Namespace, anchorAttrSig, TypeAttributes.Public, BaseRefs.ObjectReference);

            PropertyDefinition pkProp = Helper.CreateProperty(type, BaseRefs.IntReference, A_Id);
            pkProp.CustomAttributes.Add(BaseRefs.CompositeKeyAttrRef);

            PropertyDefinition anchPropPk = Helper.CreateProperty(type, anchorEntityType, "Anchor");

            CustomAttribute attr = new CustomAttribute(BaseRefs.FKAttributeConstructorReference);
            attr.ConstructorArguments.Add(new CustomAttributeArgument(BaseRefs.StringReference, A_Id));
            anchPropPk.CustomAttributes.Add(attr);

            PropertyDefinition valueProp = Helper.CreateProperty(type, propertyType, Value);

            foreach (CustomAttribute propAttr in prop.CustomAttributes)
            {
                if (propAttr.AttributeType.Name.Equals(nameof(TemporaryAttribute), StringComparison.Ordinal))
                {
                    continue;
                }
                valueProp.CustomAttributes.Add(propAttr);
            }

            if (isHistorical)
            {
                Helper.CreateProperty(type, BaseRefs.DateTimeTypeReference, When).CustomAttributes.Add(BaseRefs.CompositeKeyAttrRef);
            }

            Helper.CreateTransactionProps(type).CustomAttributes.Add(BaseRefs.CompositeKeyAttrRef);
            Helper.AddConstructor(type);
            return type;
        }
    }
}
