using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AnchorModeling.Names;

namespace AnchorModeling
{
    public class ModuleHelper
    {
        protected readonly ModuleDefinition Module;
        protected readonly BaseRefs BaseRefs;
        public ModuleHelper(ModuleDefinition module, BaseRefs baseRefs)
        {
            Module = module;
            BaseRefs = baseRefs;
        }

        public PropertyDefinition CreateProperty(TypeDefinition target, TypeReference propType, string propName)
        {
            MethodAttributes getSetAttr = MethodAttributes.Public |
            MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            string upperName = char.ToUpper(propName[0]) + propName.Substring(1);

            FieldDefinition anchorField = new FieldDefinition($"<{upperName}>k__BackingField", FieldAttributes.Private, propType);
            target.Fields.Add(anchorField);

            MethodDefinition setMethod = new MethodDefinition($"set_{upperName}", getSetAttr, Module.ImportReference(typeof(void)))
            {
                IsSetter = true,
                SemanticsAttributes = MethodSemanticsAttributes.Setter
            };
            setMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, propType));

            ILProcessor procSet = setMethod.Body.GetILProcessor();
            procSet.Emit(OpCodes.Ldarg_0);
            procSet.Emit(OpCodes.Ldarg_1);
            procSet.Emit(OpCodes.Stfld, anchorField);
            procSet.Emit(OpCodes.Ret);

            MethodDefinition getMethod = new MethodDefinition($"get_{upperName}", getSetAttr, propType)
            {
                IsGetter = true,
                SemanticsAttributes = MethodSemanticsAttributes.Getter
            };

            ILProcessor procGet = getMethod.Body.GetILProcessor();
            procGet.Emit(OpCodes.Ldarg_0);
            procGet.Emit(OpCodes.Ldfld, anchorField);
            procGet.Emit(OpCodes.Ret);

            PropertyDefinition anchProp = new PropertyDefinition(upperName, PropertyAttributes.None, propType)
            {
                GetMethod = getMethod,
                SetMethod = setMethod
            };

            target.Methods.Add(getMethod);
            target.Methods.Add(setMethod);
            target.Properties.Add(anchProp);


            CustomAttribute compGenAttr = new CustomAttribute(
                Module.ImportReference(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute)
                .GetConstructor(new Type[] { })));

            getMethod.CustomAttributes.Add(compGenAttr);
            setMethod.CustomAttributes.Add(compGenAttr);
            anchorField.CustomAttributes.Add(compGenAttr);

            return anchProp;
        }

        public PropertyDefinition CreateTransactionProps(TypeDefinition type)
        {
            PropertyDefinition TrnPropDef = CreateProperty(type, BaseRefs.IntReference, TransactionIdFKName);
            CreateProperty(type, BaseRefs.IntNullableReference, CloseTransactionIdFKName);

            //add FK Props
            PropertyDefinition trnProp = CreateProperty(type, BaseRefs.TransactionTypeReference, TransactionFKPropName);

            CustomAttribute attr = new CustomAttribute(BaseRefs.FKAttributeConstructorReference);
            attr.ConstructorArguments.Add(new CustomAttributeArgument(BaseRefs.StringReference, TransactionIdFKName));
            trnProp.CustomAttributes.Add(attr);

            PropertyDefinition closeTrnProp = CreateProperty(type, BaseRefs.TransactionTypeReference, CloseTransactionFKPropName);
            attr = new CustomAttribute(BaseRefs.FKAttributeConstructorReference);
            attr.ConstructorArguments.Add(new CustomAttributeArgument(BaseRefs.StringReference, CloseTransactionIdFKName));
            closeTrnProp.CustomAttributes.Add(attr);

            return TrnPropDef;
        }

        public void AddConstructor(TypeDefinition newType)
        {
            MethodAttributes attributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
            MethodDefinition method = new MethodDefinition(".ctor", attributes, Module.ImportReference(typeof(void)));
            MethodReference objectConstructor = Module.ImportReference(BaseRefs.ObjectReference.Resolve().GetConstructors().First());
            ILProcessor processor = method.Body.GetILProcessor();
            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Call, objectConstructor);
            processor.Emit(OpCodes.Ret);
            newType.Methods.Add(method);
        }

        public PropertyDefinition FindPropDefByTypeInDb(TypeDefinition db, TypeDefinition td)
        {
            return
            db.Properties.First(propertyOfEntity =>
            {
                TypeDefinition entityType = ((GenericInstanceType)propertyOfEntity.PropertyType)
                        .GenericArguments[0].Resolve();

                return entityType.Name.Equals(td.Name, StringComparison.Ordinal);
            });
        }
    }
}
