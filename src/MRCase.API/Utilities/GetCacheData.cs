using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using MRCase.Application.Localization;
using MRCase.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRCase.API.Utilities
{
    //In-Memory Cache
    public static class GetCacheData
    {
        public static List<LangPair> GetData(string cacheKey, IMemoryCache memoryCache, IStringLocalizer<Resource> localizer)
        {
            if (!memoryCache.TryGetValue(cacheKey, out List<LangPair> response))
            {
                response = GetLanguageData.GetList(localizer);

                var cacheExpirationOptions =
                    new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddHours(1),
                        Priority = CacheItemPriority.Normal
                    };
                memoryCache.Set(cacheKey, response, cacheExpirationOptions);
            }
            return response;
        }
    }
}
