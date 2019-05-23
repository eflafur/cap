using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using GruppoCap;

namespace GruppoCap.Core.Caching
{
    public class BasicWebCache
        : ICache
    {

        // CTOR
        public BasicWebCache(Cache webCache)
        {
            Ensure.Arg(() => webCache).IsNotNull();

            WebCache = webCache;
            
            CacheItemPriority = CacheItemPriority.High;
        }

        public Cache WebCache { get; protected set; }
        public CacheItemPriority CacheItemPriority { get; set; }

        // GET BY KEY
        public T GetByKey<T>(String key)
        {
            return WebCache.Get(key).TryCastOrDefault<T>();
        }

        // GET BY KEYs
        public IDictionary<String, T> GetByKeys<T>(params String[] keys)
        {
            IDictionary<String, T> res;
            Object o;

            res = new Dictionary<String, T>();

            foreach (String key in keys)
            {
                o = WebCache.Get(key);

                if (o != null)
                {
                    res.Add(key, o.TryCastOrDefault<T>());
                }
            }

            return res;
        }

        // PUT
        public void Put<T>(String key, T value, TimeSpan duration)
        {
            WebCache.Insert(
                key,
                value,
                null,
                DateTime.Now.Add(duration),
                Cache.NoSlidingExpiration,
                CacheItemPriority,
                null
            );
        }

        // PUT
        public void Put<T>(IDictionary<String, T> items, TimeSpan duration)
        {
            foreach (var item in items)
            {
                Put<T>(item.Key, item.Value, duration);
            }
        }

        // DELETE BY KEYs
        public void DeleteByKeys(params String[] keys)
        {
            foreach (String key in keys)
            {
                WebCache.Remove(key);
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
            // EMPTY
        }

        // CACHE MINIMUM DURATION IN MINUTEs
        public int CacheMinimumDurationInMinutes
        {
            get { throw new NotImplementedException(); }
        }

        // CACHE MEDIUM DURATION IN MINUTEs
        public int CacheMediumDurationInMinutes
        {
            get { throw new NotImplementedException(); }
        }

        // CACHE HIGH DURATION IN MINUTEs
        public int CacheHighDurationInMinutes
        {
            get { throw new NotImplementedException(); }
        }

        // GET ALL REVO KEYs
        public IList<String> GetAllRevoKeys()
        {
            return null;
        }
    }
}
