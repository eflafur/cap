using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace GruppoCap
{
    public static class EnumUtils
    {

        // PARSE
        public static T Parse<T>(String s, Boolean ignoreCase = true)
        {
            return (T)Enum.Parse(typeof(T), s, ignoreCase);
        }

        // PARSE OR
        public static T ParseOr<T>(String s, Boolean ignoreCase = true, T defaultValue = default(T))
        {
            try { return Parse<T>(s, ignoreCase); }
            catch (Exception) { return defaultValue; }
        }

        // GET DESCRIPTION OR DEFAULT
        public static String GetDescriptionOr(this Enum value, String defaultValue = null)
        {
            MemberInfo[] mi;

            mi = value.GetType().GetMember(value.ToString());

            if (mi != null && mi.Length > 0)
            {
                return mi[0].GetDescriptionOrDefault(defaultValue);
            }

            if (String.IsNullOrEmpty(defaultValue) == false)
                return defaultValue;

            return value.GetName();
        }

        // GET NAME
        public static String GetName(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }

        // GET VALUEs
        public static E[] GetValues<E>()
            where E : struct
        {
            return Enum.GetValues(typeof(E)).Cast<E>().ToArray();
        }

        // TO DICTIONARY
        public static Dictionary<T, String> ToDictionary<T>(Type enumType)
        {
            var dict = new Dictionary<T, String>();
            foreach (var name in Enum.GetNames(enumType))
                dict.Add((T)Enum.Parse(enumType, name), name);

            return dict;
        }

        // GET STRING VALUE
        public static String GetStringValue(this Enum value)
        {
            String output = null;
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs = fi.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }

    }

    // STRING VALUE
    public class StringValue : System.Attribute
    {
        private String _value;
        public StringValue(String value)
        {
            _value = value;
        }
        public String Value
        {
            get { return _value; }
        }
    }

}
