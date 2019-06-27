using System;
using System.Collections.Generic;

namespace GruppoCap.Core.Caching
{
    public interface ICache : IDisposable
    {
        Int32 CacheMinimumDurationInMinutes { get; }
        Int32 CacheMediumDurationInMinutes { get; }
        Int32 CacheHighDurationInMinutes { get; }

        T GetByKey<T>(String key);
        IDictionary<String, T> GetByKeys<T>(params String[] keys);

        void Put<T>(String key, T value, TimeSpan duration);
        void Put<T>(IDictionary<String, T> items, TimeSpan duration);

        void DeleteByKeys(params String[] keys);

        IList<String> GetAllRevoKeys();
    }
}
