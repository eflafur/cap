using System;
using System.Globalization;
using System.Linq;

namespace GruppoCap.Coercion
{

    public class StandardCoercer
        : ICoercer
    {

        // ACTUAL CONVERSION
        private Object ActualConversion(Object o, Type t)
        {
            if (t == typeof(String))
            {
                if (o == null)
                    return null;

                return o.ToString();
            }

            if (t == typeof(Guid))
            {
                return new Guid(o.ToString());
            }

            if (t.IsEnum && o != null)
            {
                if (o.GetType() == typeof(String))
                {
                    return Enum.Parse(t, o.ToString());
                }

                if (o.GetType().IsAssignableFrom(typeof(Int32)))
                {
                    if (Enum.GetValues(t).Cast<Int32>().Contains(Convert.ToInt32(o)) == false)
                    {
                        throw new ArgumentOutOfRangeException("o");
                    }

                    return Enum.ToObject(t, o);
                }
            }

            return Convert.ChangeType(o, t, CultureInfo.InvariantCulture.NumberFormat);
        }

        // TO
        public Object To(Object o, Type t)
        {
            if (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                if (o == null)
                {
                    return null;
                }
                else
                {
                    return ActualConversion(o, Nullable.GetUnderlyingType(t));
                }
            }
            else
            {
                return ActualConversion(o, t);
            }
        }

        // TO
        public T To<T>(Object o)
        {
            return (T)To(o, typeof(T));
        }

        // TO OR DEFAULT
        public T ToOrDefault<T>(Object o, T defaultValue = default(T))
        {
            try { return To<T>(o); }
            catch { return defaultValue; }
        }

        // TO OR DEFAULT
        public Object ToOrDefault(Object o, Type t, Object defaultValue)
        {
            try { return To(o, t); }
            catch { return defaultValue; }
        }

        // TO OR DEFAULT
        public Object ToOrDefault(Object o, Type t)
        {
            try { return To(o, t); }
            catch { return TypeUtils.GetDefaultValue(t); }
        }

    }

}