using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace GruppoCap
{
    public static class CollectionUtils
    {
        public static bool HasValues<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                return false;

            if (source.Count() == 0)
                return false;

            return true;
        }

        // IS NULL OR EMPTY
        public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            return items == null || items.Any() == false;
        }

        // IS NULL OR EMPTY
        public static Boolean IsNullOrEmpty(this StringDictionary items)
        {
            return items == null || items.Count == 0;
        }

        // ENSURE NOT NULL
        public static IEnumerable<T> EnsureNotNull<T>(this IEnumerable<T> items)
        {
            if (items != null)
                return items;

            return new T[] { };
        }

        // MAX OR DEFAULT
        public static T MaxOrDefault<T>(this IEnumerable<T> items, T defaultValue = default(T))
        {
            try
            {
                return items.Max();
            }
            catch
            {
                return defaultValue;
            }
        }

        // MIN OR DEFAULT
        public static T MinOrDefault<T>(this IEnumerable<T> items, T defaultValue = default(T))
        {
            try
            {
                return items.Min();
            }
            catch
            {
                return defaultValue;
            }
        }

        // APPEND
        public static IEnumerable<T> Append<T>(this IEnumerable<T> items, T newItem)
        {
            return items.Concat(new T[] { newItem });
        }

        // PREPEND
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> items, T newItem)
        {
            return new T[] { newItem }.Concat(items);
        }

        // TO LIST
        //public static IList ToList(this IEnumerable items)
        //{
        //    ArrayList l;

        //    l = new ArrayList();

        //    foreach (Object item in items)
        //        l.Add(item);

        //    return l;
        //}

        // BUILD LIST
        public static List<T> BuildList<T>(params T[] items)
        {
            return new List<T>(items);
        }

        // BUILD COLLECTION
        public static Collection<T> BuildCollection<T>(params T[] items)
        {
            return new Collection<T>(items.ToList<T>());
        }

        // UPPERIZE STRING ARRAY
        public static String[] UpperizeStringArray(this Object[] array)
        {
            IList<String> _new = new List<String>();
            
            foreach(Object s in array)
            {
                _new.Add(s.ToString().ToUpper());
            }

            return _new.ToArray();
        }

        // TO COMMA SEPARATED VALUES STRING
        public static String ToCommaSeparatedValuesString(this IEnumerable<String> items)
        {
            IList<String> _result = new List<String>();
            foreach (String i in items)
            {
                _result.Add(i.EnsureSurroundedBy("'".CoerceTo<Char>()));
            }

            return String.Join(", ", _result.ToArray());
        }

        // TO COMMA SEPARATED VALUES STRING
        public static String ToCommaSeparatedValuesString(this IEnumerable<Int32> items)
        {
            IList<Int32> _result = new List<Int32>();
            return String.Join(", ", items.ToArray());
        }
    }
}
