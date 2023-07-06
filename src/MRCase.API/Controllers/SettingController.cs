using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using MRCase.API.Utilities;
using MRCase.Application.Extensions;
using MRCase.Application.Localization;
using MRCase.Core.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MRCase.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly IStringLocalizer<Resource> localizer;
        private readonly IMemoryCache memoryCache;


        public SettingController(IStringLocalizer<Resource> localizer, IMemoryCache memoryCache)
        {
            this.localizer = localizer;
            this.memoryCache = memoryCache;
        }

        //Get language data
        [HttpGet]
        [ProducesResponseType(typeof(LangPair), 200)]
        [ProducesErrorResponseType(typeof(ResponseDetails))]
        public ActionResult<List<LangPair>> Get()
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;

            if (cultureInfo.Name == "it-IT")
            {
                const string cacheKey = "languageListIT";

                //Checking In-Memory cache for existing data
                var response = GetCacheData.GetData(cacheKey, memoryCache, localizer);
                return Ok(response);
            }
            else
            {
                const string cacheKey = "languageListTR";

                //Checking In-Memory cache for existing data
                var response = GetCacheData.GetData(cacheKey, memoryCache, localizer);
                return Ok(response);
            }

        }

    }
}
