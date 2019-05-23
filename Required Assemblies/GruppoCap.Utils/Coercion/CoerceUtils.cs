using GruppoCap.Coercion;
using System;

namespace GruppoCap
{
    public static class CoerceUtils
    {

        // TRY CAST OR DEFAULT
        public static T TryCastOrDefault<T>(this Object o, T defaultValue = default(T))
        {
            if (o == null)
                return defaultValue;

            try { return (T)o; }
            catch { return defaultValue; }
        }

        // COERCE TO
        public static T CoerceTo<T>(this Object o)
        {
            return Coerce.To<T>(o);
        }

        // COERCE TO OR DEFAULT
        public static T CoerceToOrDefault<T>(this Object o, T defaultValue = default(T))
        {
            return Coerce.ToOrDefault<T>(o, defaultValue);
        }

    }

}