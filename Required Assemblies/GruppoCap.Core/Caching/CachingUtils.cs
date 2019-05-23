using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core.Caching
{
    public static class CachingUtils
    {
        // GET CACHE KEY
        public static String GetCacheKeyForEntity(Object entityId)
        {
            Ensure.Arg(() => entityId).IsNotNull();

            // E.G.: qb:ent:person:1234
            return @"revo:entity:" + entityId.ToString();
        }

        public static String GetCacheKey(String cacheKey)
        {
            Ensure.Arg(() => cacheKey).IsNotNullOrWhiteSpace();

            // E.G.: qb:ent:person:1234
            return @"revo:cache:" + cacheKey.ToString();
        }

    }
}
