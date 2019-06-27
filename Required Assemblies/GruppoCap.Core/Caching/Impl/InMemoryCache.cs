using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;


namespace GruppoCap.Core.Caching
{
    public class InMemoryCache
        : ICache
    {

        private Int32 _cacheMinumumDurationInMinutes = 1;
        private Int32 _cacheMediumDurationInMinutes = 1;
        private Int32 _cacheHighDurationInMinutes = 1;

        // CTOR
        public InMemoryCache(Int32 cacheMinimumDurationInMinutes, Int32 cacheMediumDurationInMinutes, Int32 cacheHighDurationInMinutes, MemoryCache cache = null)
        {
            if (cache != null)
            {
                Cache = cache;
                RequiresDispose = false;
            }
            else
            {
                Cache = new MemoryCache("revo");
                RequiresDispose = true;
            }

            _cacheMinumumDurationInMinutes = cacheMinimumDurationInMinutes;
            _cacheMediumDurationInMinutes = cacheMediumDurationInMinutes;
            _cacheHighDurationInMinutes = cacheHighDurationInMinutes;
        }

        public Int32 CacheMinimumDurationInMinutes
        {
            get { return _cacheMinumumDurationInMinutes; }
        }

        public Int32 CacheMediumDurationInMinutes
        {
            get { return _cacheMediumDurationInMinutes; }
        }

        public Int32 CacheHighDurationInMinutes
        {
            get { return _cacheHighDurationInMinutes; }
        }

        // CACHE
        public MemoryCache Cache { get; protected set; }
        public Boolean RequiresDispose { get; protected set; }

        // GET BY KEY
        public T GetByKey<T>(String key)
        {
            return Cache.Get(key).TryCastOrDefault<T>();
        }

        // GET BY KEYs
        public IDictionary<String, T> GetByKeys<T>(params String[] keys)
        {
            IDictionary<String, Object> _res;

            _res = Cache.GetValues(keys);

            if (_res == null)
            {
                return new Dictionary<String, T>();
            }

            return _res
                .Where(kvp => kvp.Value != null)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.TryCastOrDefault<T>()
                )
            ;
        }

        // PUT
        public void Put<T>(String key, T value, TimeSpan duration)
        {
            if (value == null)
                return;

            Cache.Set(key, value, DateTimeOffset.Now.Add(duration));
        }

        // PUT
        public void Put<T>(IDictionary<String, T> items, TimeSpan duration)
        {
            if (items.IsNullOrEmpty())
                return;

            foreach (var item in items)
            {
                Put<T>(item.Key, item.Value, duration);
            }
        }

        // DELETE BY KEYs
        public void DeleteByKeys(params String[] keys)
        {
            foreach (var key in keys)
            {
                Cache.Remove(key);
            }
        }

        // DISPOSE
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // DISPOSE
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (RequiresDispose)
                {
                    Cache.Dispose();
                    Cache = null;
                }
            }
        }

        // GET ALL REVO KEYS
        public IList<String> GetAllRevoKeys()
        {
            IList<String> _retVal = new List<String>();
            CacheItem _cacheItem;

            foreach (var _item in Cache.Where(ci => ci.Key.StartsWith("revo:")))
            {
                _cacheItem = Cache.GetCacheItem(_item.Key);
                _retVal.Add(_cacheItem.Key);
            }

            return _retVal;
        }
    }
}
