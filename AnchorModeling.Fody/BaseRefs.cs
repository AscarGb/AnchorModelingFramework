using AnchorModeling.Attributes;
using AnchorModeling.Entities;
using EntityFrameworkCore.Attributes;
using Microsoft.EntityFrameworkCore;
using Mono.Cecil;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnchorModeling
{
    public class BaseRefs
    {
        public readonly TypeReference ObjectReference;
        public readonly TypeReference IntReference;
        public readonly TypeReference IntNullableReference;
        public readonly TypeReference StringReference;
        public readonly TypeReference DateTimeTypeReference;
        public readonly TypeReference TransactionTypeReference;
        public readonly MethodReference FKAttributeConstructorReference;
        public readonly CustomAttribute KeyAttrRef;
        public readonly CustomAttribute CompositeKeyAttrRef;
        public readonly CustomAttribute DatabaseGeneratedAttributeRef;
        public readonly ModuleDefinition Module;
        public readonly MethodReference AnchorTypeAttributeConstructorRef;
        public readonly MethodReference AttributeTypeAttributeRef;
        public readonly MethodReference TieTypeAttributeConstructorRef;
        public readonly MethodReference TableTypeAttributeConstructorRef;
        public readonly TypeReference TypeRef;
        public readonly TypeDefinition DbTypeRef;
        public readonly TypeReference GenericDbSetRef;

        public BaseRefs(ModuleDefinition module)
        {
            Module = module;
            ObjectReference = Module.ImportReference(typeof(object));
            IntReference = Module.ImportReference(typeof(int));
            IntNullableReference = Module.ImportReference(typeof(int?));
            StringReference = Module.ImportReference(typeof(string));

            DateTimeTypeReference = Module.ImportReference(typeof(DateTime));

            TransactionTypeReference = Module.ImportReference(typeof(Transaction));

            FKAttributeConstructorReference
                = Module.ImportReference(typeof(ForeignKeyAttribute)
                .GetConstructor(new[] { typeof(string) }));

            KeyAttrRef = new CustomAttribute(
             Module.ImportReference(typeof(KeyAttribute)
             .GetConstructor(new Type[] { })));

            CompositeKeyAttrRef = new CustomAttribute(
                Module.ImportReference(typeof(CompositeKeyAttribute)
                .GetConstructor(new Type[] { })));

            DatabaseGeneratedAttributeRef = new CustomAttribute(
                Module.ImportReference(typeof(DatabaseGeneratedAttribute)
                .GetConstructor(new Type[] { typeof(DatabaseGeneratedOption) })));

            DatabaseGeneratedAttributeRef.ConstructorArguments.Add(new CustomAttributeArgument(
                Module.ImportReference(typeof(DatabaseGeneratedOption)),
                DatabaseGeneratedOption.None
                ));

            AnchorTypeAttributeConstructorRef = Module.ImportReference(
                    typeof(AnchorTypeAttribute).GetConstructor(new Type[] { typeof(string) }));

            AttributeTypeAttributeRef = Module.ImportReference(
                 typeof(AttributeTypeAttribute).GetConstructor(new Type[] { typeof(string) }));

            TieTypeAttributeConstructorRef = Module.ImportReference(
              typeof(TieTypeAttribute).GetConstructor(new Type[] { typeof(string) }));

            TableTypeAttributeConstructorRef = Module.ImportReference(
          typeof(TableTypeAttribute).GetConstructor(new Type[] { typeof(string) }));

            TypeRef = Module.ImportReference(typeof(Type));

            DbTypeRef = Module.ImportReference(typeof(DbContext)).Resolve();
            //new TypeDefinition("Microsoft.EntityFrameworkCore", "DbContext",
            //    TypeAttributes.Public,
            //    ObjectReference);

            GenericDbSetRef = Module.ImportReference(typeof(DbSet<>));
        }
    }
}