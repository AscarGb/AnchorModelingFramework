using AnchorModeling.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AnchorModeling.QueryExtensions
{
    public static class AnchorEntitiesFinder
    {
        private static readonly Dictionary<Type, AnchorPropertiesData> _anchorPropertiesDictionary = new Dictionary<Type, AnchorPropertiesData>();
        private static readonly object _anchorPropFinderLock = new object();

        public static AnchorPropertiesData FindAnchorEntities(this Type entityType, DbContext dbContext)
        {
            lock (_anchorPropFinderLock)
            {
                if (!_anchorPropertiesDictionary.TryGetValue(entityType, out var data))
                {
                    data = FindAnchorPropertiesData(dbContext.GetType(), entityType);
                    _anchorPropertiesDictionary.Add(entityType, data);
                }

                return new AnchorPropertiesData
                {
                    AnchorProperties = data.AnchorProperties.Select(a =>
                    {
                        var setData = (AnchorSetData)a.Clone();
                        setData.Query = (IQueryable)a.ContextEntityProperty.GetValue(dbContext);
                        return setData;
                    }).ToList(),
                    DbContext = dbContext
                };
            }
        }
        static AnchorPropertiesData FindAnchorPropertiesData(Type dbContextType, Type entityType) =>
            new AnchorPropertiesData
            {
                AnchorProperties = entityType.GetProperties()
                                   .Select(property => new
                                   {
                                       isTie = property.IsDefined(typeof(TieTypeAttribute)),
                                       isTemporary = property.IsDefined(typeof(TemporaryAttribute)),
                                       isAttribute = property.IsDefined(typeof(AttributeTypeAttribute)),
                                       property
                                   })
                                   .Where(a => a.isAttribute || a.isTie)
                                   .Select(a =>
                                       {
                                           var contextEntityProperty = dbContextType.GetProperty(a.isTie ?
                                               a.property.GetFirstAttributeConstructorStringArgument(typeof(TieTypeAttribute)) :
                                               a.property.GetFirstAttributeConstructorStringArgument(typeof(AttributeTypeAttribute)));

                                           return new AnchorSetData
                                           {
                                               IsTemporary = a.isTemporary,
                                               IsTie = a.isTie,
                                               ContextEntityProperty = contextEntityProperty,
                                               AttributeProperty = a.property,
                                               TableType = contextEntityProperty.PropertyType.GenericTypeArguments[0]
                                           };
                                       }).ToList()
            };
    }
}