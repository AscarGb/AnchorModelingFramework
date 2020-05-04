using AnchorModeling.Attributes;
using Mono.Cecil;
using System;
using System.Linq;
using static AnchorModeling.Names;

namespace AnchorModeling.Basics
{
    public class Tie : AnchorBasic
    {
        public Tie(ModuleDefinition module, ModuleHelper helper, BaseRefs baseRefs) : base(module, helper, baseRefs)
        {
        }

        public static string GetName(string sigA, string propSigA, string sigB, bool Historical)
        {
            return $"T_{(Historical ? "H_" : "")}{sigA}_{propSigA}_to_{sigB}";
        }

        public TypeDefinition Create(PropertyDefinition propertyOfEntity, PropertyDefinition fkProp)
        {
            bool isHistorical = fkProp.CustomAttributes.Any(a => a.AttributeType.Name
                        .Equals(nameof(TemporaryAttribute), StringComparison.Ordinal));

            TypeDefinition db = propertyOfEntity.DeclaringType;

            string toPropName = Helper.FindPropDefByTypeInDb(propertyOfEntity.DeclaringType, fkProp.PropertyType.Resolve()).Name;

            string tieSig = GetName(
                propertyOfEntity.Name,
                fkProp.Name,
                toPropName,
                isHistorical);

            TypeDefinition type = new TypeDefinition(db.Namespace, tieSig, TypeAttributes.Public, BaseRefs.ObjectReference);
            Helper.CreateProperty(type, BaseRefs.IntReference, A_Id).CustomAttributes.Add(BaseRefs.CompositeKeyAttrRef);
            Helper.CreateProperty(type, BaseRefs.IntReference, ToId).CustomAttributes.Add(BaseRefs.CompositeKeyAttrRef);

            PropertyDefinition aidProp = Helper.CreateProperty(type,
                Module.Types.First(t => t.Name.Equals(Anchor.GetName(propertyOfEntity.Name),
                StringComparison.Ordinal)), Names.FromAnchor);
            CustomAttribute attr = new CustomAttribute(BaseRefs.FKAttributeConstructorReference);
            attr.ConstructorArguments.Add(new CustomAttributeArgument(BaseRefs.StringReference, A_Id));
            aidProp.CustomAttributes.Add(attr);

            PropertyDefinition toidProp = Helper.CreateProperty(type,
                Module.Types.First(t => t.Name.Equals(Anchor.GetName(toPropName),
                StringComparison.Ordinal)),
                Names.ToAnchor);
            attr = new CustomAttribute(BaseRefs.FKAttributeConstructorReference);
            attr.ConstructorArguments.Add(new CustomAttributeArgument(BaseRefs.StringReference, ToId));
            toidProp.CustomAttributes.Add(attr);

            if (isHistorical)
            {
                Helper.CreateProperty(type, BaseRefs.DateTimeTypeReference, When).CustomAttributes.Add(BaseRefs.CompositeKeyAttrRef);
            }

            PropertyDefinition TrnProp = Helper.CreateTransactionProps(type);
            TrnProp.CustomAttributes.Add(BaseRefs.CompositeKeyAttrRef);
            Helper.AddConstructor(type);
            return type;
        }
    }
}