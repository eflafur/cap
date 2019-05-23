using System;

namespace GruppoCap.Coercion
{

    public static class Coerce
    {

        // STANDARD SHARED
        public static readonly ICoercer StandardShared = new StandardCoercer();

        // TO
        //[Obsolete]
        public static Object To(Object o, Type t)
        {
            return StandardShared.To(o, t);
        }

        // TO
        //[Obsolete]
        public static T To<T>(Object o)
        {
            return StandardShared.To<T>(o);
        }

        // TO OR DEFAULT
        //[Obsolete]
        public static T ToOrDefault<T>(Object o, T defaultValue = default(T))
        {
            return StandardShared.ToOrDefault<T>(o, defaultValue);
        }

        // TO OR DEFAULT
        //[Obsolete]
        public static Object ToOrDefault(Object o, Type t, Object defaultValue)
        {
            return StandardShared.ToOrDefault(o, t, defaultValue);
        }

        // TO OR DEFAULT
        //[Obsolete]
        public static Object ToOrDefault(Object o, Type t)
        {
            return StandardShared.ToOrDefault(o, t);
        }

    }

}