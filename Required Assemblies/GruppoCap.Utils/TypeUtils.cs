using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GruppoCap
{
    public static class TypeUtils
    {
        // IS NULLABLE
        public static Boolean IsNullable(Type type)
        {
            //Ensure.Arg(() => type).IsNotNull();

            return Nullable.GetUnderlyingType(type) != null;
        }

        // GET DEFAULT VALUE
        public static Object GetDefaultValue(Type type)
        {
            //Ensure.Arg(() => type).IsNotNull();

            if (type.IsValueType == false)
            {
                return null;
            }

            return Activator.CreateInstance(type);
        }

        // GET DESCRIPTION OR DEFAULT ( MemberInfo )
        public static String GetDescriptionOrDefault(this MemberInfo mi, String defaultValue = null)
        {
            if (mi != null)
            {
                Object attr = mi.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();

                if (attr != null)
                    return ((DescriptionAttribute)attr).Description;
            }

            return defaultValue;
        }

        // GET DESCRIPTION OR DEFAULT ( ParameterInfo )
        public static String GetDescriptionOrDefault(this ParameterInfo pi, String defaultValue = null)
        {
            if (pi != null)
            {
                Object attr = pi.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();

                if (attr != null)
                    return ((DescriptionAttribute)attr).Description;
            }

            return defaultValue;
        }

        // IS NULL OR EMPTY
        public static Boolean IsNullOrEmpty(this Guid? value)
        {
            return value == null || value.Value == Guid.Empty;
        }

        // IS DBNULL
        public static Boolean IsDBNull(Object o)
        {
            return o == DBNull.Value;
        }

        // IS NULL OR DBNULL
        public static Boolean IsNullOrDBNull(Object o)
        {
            return o == null || o == DBNull.Value;
        }

        // TO NULL IF DBNULL
        public static T ToNullIfDBNull<T>(T o)
            where T : class
        {
            return
                IsDBNull(o)
                ? null
                : o
            ;
        }

        // TO DBNULL IF NULL
        public static Object ToDBNullIfNull(Object o)
        {
            return
                o == null
                ? DBNull.Value
                : o
            ;
        }

        // TO DBNULL IF NULL OR EMPTY
        public static Object ToDBNullIfNullOrEmpty(String s)
        {
            return
                String.IsNullOrEmpty(s)
                ? (Object)DBNull.Value
                : s
            ;
        }

        // TO DBNULL IF NULL OR WHITESPACE
        public static Object ToDBNullIfNullOrWhiteSpace(String s)
        {
            return
                String.IsNullOrWhiteSpace(s)
                ? (Object)DBNull.Value
                : s
            ;
        }

        // TO DBNULL IF EMPTY
        public static Object ToDBNullIfEmpty(Guid g)
        {
            if (g == Guid.Empty)
                return DBNull.Value;

            return g;
        }

        // GET PROPERTY INFOs AS DICTIONARY
        public static IDictionary<String, PropertyInfo> GetPropertyInfosAsDictionary(this Type type)
        {
            return type.GetProperties().ToDictionary(pi => pi.Name);
        }

        // GET GENRIC ARGUMENT OF BASE GENERIC TYPE
        public static Type GetGenericArgumentOfBaseGenericType(Type typeToScan, Type baseOpenGenericType)
        {
            if (typeToScan == null)
                return null;

            Type[] baseTypes;
            Type[] genericArguments;
            String baseOpenGenericTypeName;

            baseOpenGenericTypeName = baseOpenGenericType.Name;

            baseTypes = typeToScan.GetInterfaces();

            if (baseTypes.Length == 0)
                return null;

            foreach (Type baseType in baseTypes)
            {
                if (baseType.IsGenericType == false)
                    continue;

                if (baseType.Name != baseOpenGenericTypeName)
                    continue;

                genericArguments = baseType.GetGenericArguments();

                if (genericArguments.Length != 1)
                    continue;

                return genericArguments[0];
            }

            return null;
        }

        // GET SINGLE GENERIC ARGUMENT OF OPEN GENERIC INTERFACE
        public static Type GetSingleGenericArgumentOfOpenGenericInterface(Type typeToScan, Type openGenericInterfaceType, Boolean searchAllImplementedInterfaces = true)
        {
            if (typeToScan.IsGenericType && typeToScan.GetGenericTypeDefinition() == openGenericInterfaceType)
            {
                return typeToScan.GetGenericArguments()[0];
            }

            if (searchAllImplementedInterfaces == false)
                return null;

            Type _res = null;

            foreach (Type interfaceType in typeToScan.GetInterfaces())
            {
                _res = GetSingleGenericArgumentOfOpenGenericInterface(interfaceType, openGenericInterfaceType, false);

                if (_res != null)
                    return _res;
            }

            return null;
        }

        // GET GENERIC TYPE
        public static Type GetGenericType(Type genericType, Type specificType)
        {
            return genericType.MakeGenericType(new Type[] { specificType });
        }
        // TRY GET FIRST USAGE OF ATTRIBUTE IN PROPERTIEs
        //public static Boolean TryGetFirstUsageOfAttributeInProperties<A>(this Type type, out PropertyInfo pi, out A att)
        //    where A : Attribute
        //{
        //    //Ensure.Arg(() => type).IsNotNull();

        //    Type _a;

        //    _a = typeof(A);

        //    foreach (PropertyInfo p in type.GetProperties())
        //    {
        //        foreach (Object a in p.GetCustomAttributes(_a, false))
        //        {
        //            pi = p;
        //            att = a as A;

        //            return true;
        //        }
        //    }

        //    pi = null;
        //    att = null;

        //    return false;
        //}

        // CREATE INSTANCE CREATOR
        //public static Func<Object> CreateInstanceCreator(Type type)
        //{
        //    return Expression.Lambda<Func<Object>>(Expression.New(type)).Compile();
        //}
    }
}
