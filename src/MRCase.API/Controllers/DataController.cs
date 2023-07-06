using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using MRCase.API.ActionFilters;
using MRCase.Application.Data;
using MRCase.Application.Data.Dtos;
using MRCase.Application.Pagination;
using MRCase.Core.Authorization;
using MRCase.Core.Data;
using MRCase.Core.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MRCase.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IStringLocalizer<Resource> localizer;
        private readonly IDataService dataService;
        private readonly IMapper mapper;

        public DataController(IStringLocalizer<Resource> localizer, IDataService dataService, IMapper mapper)
        {
            this.localizer = localizer;
            this.dataService = dataService;
            this.mapper = mapper;
        }

        public string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        //Get User Data
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DatumResponseDto>>> Get([FromQuery] PagingParameters pagingParameters)
        {
            var data = await dataService.GetPagedDataAsync(pagingParameters, UserId);

            var metadata = new
            {
                data.TotalCount,
                data.PageSize,
                data.CurrentPage,
                data.HasNext,
                data.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(mapper.Map<PagedList<Datum>, DatumResponseDto[]>(data));
        }

        //Import Json File
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Post([FromForm] IFormFile formFile)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;

            //Reading Json File

            //https://stackoverflow.com/questions/40045147/how-to-read-into-memory-the-lines-of-a-text-file-from-an-iformfile-in-asp-net-co/40045456
            //var result = new StringBuilder();

            //using (var reader = new StreamReader(formFile.OpenReadStream()))
            //{
            //    while (reader.Peek() >= 0)
            //        result.AppendLine(reader.ReadLine());
            //}

            string result;
            using (var reader = new StreamReader(formFile.OpenReadStream()))
            {
                result = await reader.ReadToEndAsync();
            }

            if (cultureInfo.Name == "it-IT")
            {
                var data = JsonConvert.DeserializeObject<List<ImportDataITDto>>(result.ToString());

                //Checking valid language
                if (data.Any(x => x.dc_Evento == null))
                    throw new ArgumentException(localizer["NotValidLangFile"].Value);

                dataService.ImportData(mapper.Map<Datum[]>(data), UserId);

                if (await dataService.SaveChangesAsync())
                    return Ok(localizer["Created"].Value);
            }

            else if (cultureInfo.Name == "tr-TR")
            {
                var data = JsonConvert.DeserializeObject<List<ImportDataTRDto>>(result.ToString());

                //Checking valid language
                if (data.Any(x => x.dc_Olay == null))
                    throw new ArgumentException(localizer["NotValidLangFile"].Value);

                dataService.ImportData(mapper.Map<Datum[]>(data), UserId);

                if (await dataService.SaveChangesAsync())
                    return Ok(localizer["Created"].Value);
            }

            else
            {
                throw new ArgumentException(localizer["NotValidLangFile"].Value);
            }

            throw new Exception(localizer["Error"].Value);
        }

        //Delete all
        [HttpDelete]
        public async Task<IActionResult> DeleteAllData()
        {
            var userData = await dataService.GetAllAsync(UserId);
            if (!userData.Any())
                throw new FileNotFoundException(localizer["NotFoundData"].Value);

            dataService.DeleteAll(userData);

            if (await dataService.SaveChangesAsync())
                return Ok(localizer["Deleted"].Value);

            throw new Exception(localizer["Error"].Value);
        }

        //Delete by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = dataService.GetByIdAsync(id);
            if (data == null)
                throw new FileNotFoundException(localizer["NotFoundData"].Value);

            dataService.DeleteOne(data);
            if (await dataService.SaveChangesAsync())
            {
                return Ok(localizer["Deleted"].Value);
            }

            throw new Exception(localizer["Error"].Value);

        }
    }
}
