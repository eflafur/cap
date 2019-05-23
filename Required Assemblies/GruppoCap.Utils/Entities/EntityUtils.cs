using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap
{
    public static class EntityUtils
    {
        // GET MAPPED ON
        public static String GetMappedOnProperty(dynamic obj, PropertyInfo property)
        {
            String output = null;
            Type type = obj.GetType();

            PropertyInfo pi = type.GetProperty(property.Name);
            MappedOn[] attrs = pi.GetCustomAttributes(typeof(MappedOn), false) as MappedOn[];
            if (attrs.Length > 0)
            {
                output = attrs[0].PropertyName;
            }

            return output;
        }

        //// GET MAPPED ON
        //public static MappedOn GetMappedOn(dynamic obj, PropertyInfo property)
        //{
        //    MappedOn _map = null;
        //    Type type = obj.GetType();

        //    PropertyInfo pi = type.GetProperty(property.Name);
        //    MappedOn[] attrs = pi.GetCustomAttributes(typeof(MappedOn), false) as MappedOn[];
        //    if (attrs.Length > 0)
        //    {


        //        _map = attrs[0];
        //    }

        //    return _map;
        //}

        // GET MAPPED ON
        public static MappedOn[] GetMappedOn(dynamic obj, PropertyInfo property)
        {
            MappedOn[] _maps = null;
            Type type = obj.GetType();

            PropertyInfo pi = type.GetProperty(property.Name);

            MappedOn[] attrs = pi.GetCustomAttributes(typeof(MappedOn), false) as MappedOn[];

            if (attrs.Length > 0)
                _maps = attrs;

            return _maps;
        }

        // Uppercase all string property on Top level
        public static T ToUpper<T>(this T entity)
        {
            entity.GetType().GetProperties().Where(
                p => p.PropertyType == typeof(string) &&
                p.CanRead &&
                p.CanWrite).ToList<PropertyInfo>().ForEach(
                    x =>
                    {
                        var objVal = x.GetValue(entity);
                        x.SetValue(entity, objVal != null ? objVal.ToString().ToUpper() : null);
                    });

            return entity;
        }
        // Lowercase all string property on Top level
        public static T ToLower<T>(this T entity)
        {
            entity.GetType().GetProperties().Where(
                p => p.PropertyType == typeof(string) &&
                p.CanRead &&
                p.CanWrite).ToList<PropertyInfo>().ForEach(
                    x =>
                    {
                        var objVal = x.GetValue(entity);
                        x.SetValue(entity, objVal != null ? objVal.ToString().ToLower() : null);
                    });

            return entity;
        }

        /// <summary>
        /// set difference between source object and newEntity param.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="newEntity"></param>
        /// <param name="IgnoreNullorDefaultValue">if property is nullable, ignore newEntity's property with null value
        /// if property is not nullable, ignore newEntity's property with type default value
        /// </param>
        /// <returns></returns>
        public static T SetDifference<T>(this T entity, T newEntity, bool IgnoreNullOrDefaultValue)
        {
            return SetDifference(entity, newEntity, IgnoreNullOrDefaultValue, string.Empty);
        }

        /// <summary>
        /// set difference between source object and newEntity param.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="newEntity"></param>
        /// <param name="IgnoreNullorDefaultValue">if property is nullable, ignore newEntity's property with null value
        /// if property is not nullable, ignore newEntity's property with type default value
        /// </param>
        /// <returns></returns>
        public static T SetDifference<T>(this T entity, T newEntity, bool IgnoreNullOrDefaultValue, string scope)
        {
            if (entity == null)
                entity = EntityFactory.Create<T>();
            if (newEntity == null)
                return entity;

            entity.GetType().GetProperties().Where(
                p =>
                    p.CanRead &&
                    p.CanWrite).ToList<PropertyInfo>().ForEach(
                        x =>
                        {
                            if (x.GetCustomAttribute<IgnoreSetDifference>() == null)
                            {
                                if (newEntity.GetType().GetProperty(x.Name) != null)
                                {
                                    var objVal = x.GetValue(entity);
                                    var objNewVal = x.GetValue(newEntity);
                                    if (IgnoreNullOrDefaultValue || (x.GetCustomAttribute<IgnoreSetDifferencNullOrDefaultValue>() != null 
                                                        && string.Equals(scope, x.GetCustomAttribute<IgnoreSetDifferencNullOrDefaultValue>().Scope)))
                                    {
                                        if (IsNullable(x))
                                        {
                                            if (objNewVal != null)
                                            {
                                                if (!Equals(objVal, objNewVal))
                                                    x.SetValue(entity, objNewVal);
                                            }
                                        }
                                        else if (!Equals(objNewVal, GetDefault(x.PropertyType)))
                                        {
                                            x.SetValue(entity, objNewVal);
                                        }
                                    }
                                    else
                                        x.SetValue(entity, objNewVal);
                                }
                            }
                        });

            return entity;
        }

        /// <summary>
        /// Verify difference between two items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="newEntity"></param>
        /// <param name="IgnoreNullOrDefaultValue"></param>
        /// <returns></returns>
        public static bool HasDifference<T>(this T entity, T newEntity, bool IgnoreNullOrDefaultValue)
        {
            var props = entity.GetType().GetProperties().Where(p => p.CanRead && p.CanWrite).ToList<PropertyInfo>();

            foreach (PropertyInfo x in props)
            {
                if (x.GetCustomAttribute<ExcludeCompare>() == null)
                {
                    var objVal = x.GetValue(entity);
                    var objNewVal = x.GetValue(newEntity);
                    if (IgnoreNullOrDefaultValue)
                    {
                        if (IsNullable(x))
                        {
                            if (objNewVal != null)
                            {
                                if (!Equals(objVal, objNewVal))
                                    return true;
                            }
                        }
                        else if (!Equals(objNewVal, GetDefault(x.PropertyType)))
                        {
                            return true;
                        }
                    }
                    else
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// set difference between source object and newEntity param.
        /// Include null or default values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public static T SetDifference<T>(this T entity, T newEntity)
        {
            return entity.SetDifference(newEntity, false);
        }

        private static bool IsNullable<T>(T obj)
        {
            if (obj == null) return true;
            Type type = typeof(T);
            if (!type.IsValueType) return true;
            if (Nullable.GetUnderlyingType(type) != null) return true;
            return false;
        }
        private static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class MappedOn : System.Attribute
    {
        private Type _type;
        private String _typeAsString;
        private String _propertyName;

        public MappedOn(String type, String propertyName)
        {
            _type = Type.GetType(type);
            _typeAsString = type;
            _propertyName = propertyName;
        }

        public Type Type
        {
            get { return _type; }
        }

        public String TypeAsString
        {
            get { return _typeAsString; }
        }

        public String PropertyName
        {
            get { return _propertyName; }
        }
    }

    /// <summary>
    /// Exclude property from HasDifference function
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ExcludeCompare : Attribute
    {

    }

    /// <summary>
    /// Exclude property from SetDifference function
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IgnoreSetDifference : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IgnoreSetDifferencNullOrDefaultValue : Attribute
    {
        private string _scope = string.Empty;

        public IgnoreSetDifferencNullOrDefaultValue()
        {
        }

        public IgnoreSetDifferencNullOrDefaultValue(String scope)
        {
            _scope = scope;
        }

        public String Scope
        {
            get { return _scope; }
        }
    }

}
