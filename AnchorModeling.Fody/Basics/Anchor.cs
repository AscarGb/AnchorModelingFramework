using Fody;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;
using static AnchorModeling.Names;

namespace AnchorModeling.Basics
{
    public class Anchor : AnchorBasic
    {
        public Anchor(ModuleDefinition module, ModuleHelper helper, BaseRefs baseRefs) : base(module, helper, baseRefs)
        {
        }

        public static string GetName(string signature)
        {
            return $"A_{signature}";
        }

        public TypeDefinition Create(TypeDefinition db, string anchorName)
        {
            TypeDefinition type = new TypeDefinition(db.Namespace, anchorName, TypeAttributes.Public, BaseRefs.ObjectReference);            
            
            PropertyDefinition pkProp = Helper.CreateProperty(type, BaseRefs.IntReference, Id);

            pkProp.CustomAttributes.Add(BaseRefs.KeyAttrRef);
            pkProp.CustomAttributes.Add(BaseRefs.DatabaseGeneratedAttributeRef);

            Helper.CreateTransactionProps(type);
            Helper.AddConstructor(type);
            return type;
        }
    }
}
