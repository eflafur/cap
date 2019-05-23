using GruppoCap.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GruppoCap;

namespace GruppoCap
{
    public class EntityMapper<T>
    {
        // MAP ENTITIES
        public static IList<T> MapEntities(IEnumerable<dynamic> resultList, out int totalRows)
        {
            IList<T> entityList = new List<T>();
            totalRows = 0;

            if (resultList != null)
            {
                T entity = default(T);
                foreach (var result in resultList)
                {
                    entity = MapEntity(result);
                    entityList.Add(entity);

                    if (totalRows == 0)
                        totalRows = result.TotalItems;
                }
            }

            return entityList;
        }

        // MAP ENTITIES
        public static IList<T> MapEntities(IEnumerable<dynamic> resultList)
        {
            IList<T> entityList = new List<T>();

            if (resultList != null)
            {
                T entity = default(T);
                foreach (var result in resultList)
                {
                    entity = MapEntity(result);
                    entityList.Add(entity);
                }
            }

            return entityList;
        }

        // MAP ENTITY
        public static T MapEntity(dynamic resultItem)
        {
            T entity = EntityFactory.Create<T>();

            PropertyInfo[] properties = entity.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            PropertyInfo[] propertyOrigin = resultItem.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            MappedOn[] _maps = null;
            String _mappedOn = String.Empty;
            String _enumStringValue = String.Empty;

            foreach (PropertyInfo property in properties)
            {
                _mappedOn = String.Empty;
                _enumStringValue = String.Empty;

                foreach (PropertyInfo prop in propertyOrigin)
                {
                    _maps = EntityUtils.GetMappedOn(resultItem, prop);
                    if(_maps != null)
                    {
                        foreach(MappedOn _m in _maps)
                        {
                            if (_m.PropertyName == property.Name && _m.TypeAsString == entity.GetType().ToString())
                            {
                                _mappedOn = prop.Name;
                                break;
                            }
                        }
                    }
                }

                if (property.GetSetMethod() != null && _mappedOn.IsNullOrWhiteSpace() == false)
                {
                    if (property.PropertyType.IsEnum)
                    {
                        _enumStringValue = DynamicUtils.GetDynamicPropertyValue(resultItem, _mappedOn).ToString();
                        property.SetValue(entity, Enum.Parse(property.PropertyType, _enumStringValue));
                    }
                    else
                    {
                        property.SetValue(entity, DynamicUtils.GetDynamicPropertyValue(resultItem, _mappedOn));
                    }
                }
                    
            }

            return entity;
        }

        // MAP ENTITY
        public static T MapEntity(dynamic resultItem, T originalObject)
        {
            T entity = originalObject;

            PropertyInfo[] properties = entity.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            PropertyInfo[] propertyOrigin = resultItem.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            MappedOn[] _maps = null;
            String _mappedOn = String.Empty;
            String _enumStringValue = String.Empty;

            foreach (PropertyInfo property in properties)
            {
                _mappedOn = String.Empty;
                _enumStringValue = String.Empty;

                foreach (PropertyInfo prop in propertyOrigin)
                {
                    _maps = EntityUtils.GetMappedOn(resultItem, prop);
                    if (_maps != null)
                    {
                        foreach (MappedOn _m in _maps)
                        {
                            if (_m.PropertyName == property.Name && _m.TypeAsString == entity.GetType().ToString())
                            {
                                _mappedOn = prop.Name;
                                break;
                            }
                        }
                    }
                }

                if (property.GetSetMethod() != null && _mappedOn.IsNullOrWhiteSpace() == false)
                {
                    if (property.PropertyType.IsEnum)
                    {
                        _enumStringValue = DynamicUtils.GetDynamicPropertyValue(resultItem, _mappedOn).ToString();
                        property.SetValue(entity, Enum.Parse(property.PropertyType, _enumStringValue));
                    }
                    else
                    {
                        property.SetValue(entity, DynamicUtils.GetDynamicPropertyValue(resultItem, _mappedOn));
                    }
                }

            }

            return entity;
        }
    }
}
