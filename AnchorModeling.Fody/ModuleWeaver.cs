using AnchorModeling;
using AnchorModeling.Attributes;
using AnchorModeling.Basics;
using AnchorModeling.Common;
using AnchorModeling.Extensions;
using Fody;
using Microsoft.EntityFrameworkCore;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using static AnchorModeling.Names;
using AnchorAttribute = AnchorModeling.Basics.AnchorAttribute;

public class ModuleWeaver : BaseModuleWeaver
{
    BaseRefs _baseRefs;
    ModuleHelper _helper;
    Anchor _anchor;
    AnchorAttribute _attribute;
    Tie _tie;
    readonly PropComparer _propComparer = new PropComparer();

    public override void Execute()
    {
        //  System.Diagnostics.Debugger.Launch();

        InitHelpers();

        LogMessage("Constructing entities...", MessageImportance.High);

        ModuleDefinition.Types
            .Where(t => _baseRefs.DbTypeRef.IsAssignableFromClass(t))
            .ToList()
            .ForEach(db => ProcessDbContext(db));
    }

    void InitHelpers()
    {
        _baseRefs = new BaseRefs(ModuleDefinition);
        _helper = new ModuleHelper(ModuleDefinition, _baseRefs);
        _anchor = new Anchor(ModuleDefinition, _helper, _baseRefs);
        _attribute = new AnchorAttribute(ModuleDefinition, _helper, _baseRefs);
        _tie = new Tie(ModuleDefinition, _helper, _baseRefs);
    }

    void ProcessDbContext(TypeDefinition db)
    {
        var entityForeignKeys = new List<ForeignKeySet>();

        db.Properties.ToList().ForEach(dbProperty =>
        {
            var entityType = dbProperty.GetFirstGenericArgument();

            LogMessage($"Take entity: {entityType.Name}", MessageImportance.High);

            AddApplTime(entityType);

            BuildAnchorModels(db, dbProperty, entityForeignKeys);
        });

        entityForeignKeys.ForEach(foreignKeyProperty => BuildTieEntity(db, foreignKeyProperty));
    }

    void BuildTieEntity(TypeDefinition db, ForeignKeySet foreignKeyProperty)
    {
        var tieDef = _tie.Create(foreignKeyProperty.PropertyOfEntity, foreignKeyProperty.ForeignKeyProperty);

        var anchTieAttr = _baseRefs.TieTypeAttributeConstructorRef.ToCustomAttribute(_baseRefs.StringReference, tieDef.Name);
        var tableTypeAttr = _baseRefs.TableTypeAttributeConstructorRef.ToCustomAttribute(_baseRefs.StringReference, foreignKeyProperty.ForeignKeyProperty.PropertyType.FullName);

        foreignKeyProperty.ForeignKeyIdProperty.CustomAttributes.Add(anchTieAttr);
        foreignKeyProperty.ForeignKeyIdProperty.CustomAttributes.Add(tableTypeAttr);

        var entityTieDbSetType = _baseRefs.GenericDbSetRef.ToGenericInstanceType(tieDef);

        ModuleDefinition.Types.Add(tieDef);
        _helper.CreateProperty(db, entityTieDbSetType, tieDef.Name);

        LogMessage($"Make tie: {tieDef.Name}", MessageImportance.High);
    }

    TypeDefinition BuildAnchorEntity(TypeDefinition db, string anchorName, TypeDefinition entityType)
    {
        var anchorEntityType = _anchor.Create(db, anchorName);

        var anchAttr = _baseRefs.AnchorTypeAttributeConstructorRef.ToCustomAttribute(_baseRefs.StringReference, anchorEntityType.Name);

        entityType.CustomAttributes.Add(anchAttr);

        var entityDbSetType = _baseRefs.GenericDbSetRef.ToGenericInstanceType(anchorEntityType);

        ModuleDefinition.Types.Add(anchorEntityType);

        _helper.CreateProperty(db, entityDbSetType, anchorName);

        LogMessage($"Make ancor: {anchorName}", MessageImportance.High);

        return anchorEntityType;
    }

    void BuildAttributeEntity(TypeDefinition db, PropertyDefinition dbProperty, TypeDefinition anchorEntityType, PropertyDefinition prop)
    {
        var isHistorical = IsHistorical(prop);

        var anchorAttrSig = AnchorAttribute.GetName(isHistorical, dbProperty.Name, prop.Name);

        var anchorAttributeEntityType = _attribute.Create(db, anchorAttrSig, prop, isHistorical, anchorEntityType);

        ModuleDefinition.Types.Add(anchorAttributeEntityType);

        var anchAttrAttr = new CustomAttribute(_baseRefs.AttributeTypeAttributeRef);

        anchAttrAttr.ConstructorArguments.Add(new CustomAttributeArgument(_baseRefs.StringReference, anchorAttributeEntityType.Name));
        prop.CustomAttributes.Add(anchAttrAttr);

        var entityAttrDbSetType = new GenericInstanceType(ModuleDefinition.ImportReference(typeof(DbSet<>)));
        entityAttrDbSetType.GenericArguments.Add(anchorAttributeEntityType);

        _helper.CreateProperty(db, entityAttrDbSetType, anchorAttrSig);

        LogMessage($"Make attribute: {anchorAttrSig}", MessageImportance.High);
    }

    void AddApplTime(TypeReference entityType)
    {
        entityType.AddField(When, _baseRefs.DateTimeTypeReference);
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "netstandard";
        yield return "mscorlib";
    }

   // public override bool ShouldCleanReference => false;

    void FindForeignKeys(TypeDefinition entityType, PropertyDefinition propertyOfEntity, List<ForeignKeySet> foreignKeyProps, List<PropertyDefinition> ignoredProps)
    {
        foreach (PropertyDefinition prop in entityType.Properties.ToList())
        {
            if (prop.HasCustomAttributes)
            {
                CustomAttribute fkAttr = prop.GetCustomAttribute(nameof(ForeignKeyAttribute));

                if (fkAttr != null)
                {
                    var fkIdProp = entityType.FindPropertryByName(fkAttr.GetFirstConstructorParameter().ToString());

                    ignoredProps.Add(fkIdProp);
                    ignoredProps.Add(prop);

                    foreignKeyProps.Add(new ForeignKeySet
                    {
                        PropertyOfEntity = propertyOfEntity,
                        ForeignKeyProperty = prop,
                        ForeignKeyIdProperty = fkIdProp
                    });
                }
            }
        }
    }

    bool IsHistorical(PropertyDefinition prop) => prop.CustomAttributes.Any(a => a.AttributeType.Name.Equals(nameof(TemporaryAttribute), StringComparison.Ordinal));

    void BuildAnchorModels(TypeDefinition db, PropertyDefinition dbProperty, List<ForeignKeySet> foreignKeyProps)
    {
        var anchorName = Anchor.GetName(dbProperty.Name);

        var entityType = dbProperty.GetFirstGenericArgument().Resolve();

        var keyAttributeProp = entityType.FindPropertyByCustomAttribute(nameof(KeyAttribute)) ?? throw new KeyMissingException(anchorName);

        var ignoredProps = new List<PropertyDefinition>
            {
                keyAttributeProp
            };

        var anchorEntityType = BuildAnchorEntity(db, anchorName, entityType);

        FindForeignKeys(entityType, dbProperty, foreignKeyProps, ignoredProps);

        entityType.Properties.Except(ignoredProps, _propComparer).ToList()
            .ForEach(entityProperty => BuildAttributeEntity(db, dbProperty, anchorEntityType, entityProperty));

    }
}