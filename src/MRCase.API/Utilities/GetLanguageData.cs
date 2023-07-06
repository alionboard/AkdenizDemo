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
    //Get Language data from resx files
    public static class GetLanguageData
    {
        public static List<LangPair> GetList(IStringLocalizer<Resource> localizer)
        {
            List<LangPair> list = new List<LangPair>();

            var resourceSet = localizer.GetAllStrings().Select(x => x.Name);
            foreach (var item in resourceSet)
                list.Add(new LangPair { Key = item, Value = localizer[item].Value });

            return list;
        }
    }
}
