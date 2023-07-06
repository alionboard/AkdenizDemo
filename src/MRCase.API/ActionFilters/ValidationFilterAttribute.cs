using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using MRCase.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRCase.API.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        private readonly IStringLocalizer<Resource> localizer;

        public ValidationFilterAttribute(IStringLocalizer<Resource> localizer)
        {
            this.localizer = localizer;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var param = context.ActionArguments.SingleOrDefault(p => p.Value is IFormFile);
            if (param.Value == null)
            {
                context.Result =  new BadRequestObjectResult(localizer["NotValidFile"].Value);
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
