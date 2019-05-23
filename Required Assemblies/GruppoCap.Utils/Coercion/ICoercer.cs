using System;

namespace GruppoCap.Coercion
{

    public interface ICoercer
    {
        Object To(Object o, Type t);
        T To<T>(Object o);
        T ToOrDefault<T>(Object o, T defaultValue = default(T));
        Object ToOrDefault(Object o, Type t, Object defaultValue);
        Object ToOrDefault(Object o, Type t);
    }

}