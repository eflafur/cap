using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SepaManager.Base
{
    public static class Extensions
    {
        public static List<T> ToMappedList<T>(this IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (dr.HasColumn(prop.Name))
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                            prop.SetValue(obj, dr[prop.Name], null);
                }
                list.Add(obj);
            }
            return list;
        }
        public static byte[] ToByteArray(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

            public static string Left(this string value, int maxLength)
            {
                if (string.IsNullOrEmpty(value)) return value;
                maxLength = Math.Abs(maxLength);

                return (value.Length <= maxLength
                       ? value
                       : value.Substring(0, maxLength)
                       );
            }
        
    }
}