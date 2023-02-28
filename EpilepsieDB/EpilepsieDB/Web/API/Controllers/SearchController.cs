using EpilepsieDB.Authorization;
using EpilepsieDB.Services;
using EpilepsieDB.Web.API.APIModels;
using EpilepsieDB.Web.API.Extensions;
using EpilepsieDB.Web.View.Parser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EpilepsieDB.Web.API.Controllers
{
    [Authorize(Roles = RoleSet.AllowRead, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly ISearchInputParser _inputParser;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
            _inputParser = new SearchInputParser();
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ScanApiDto>>> Get(string name = "", string rec = "", string scan = "", string ver = "", string pinfo = "", string recInfo = "", string date = "", string time = "", string label = "", string type = "", string dim = "", string annot = "")
        {
            var query = new SearchQuery()
            {
                Name = name,
                Rec = rec,
                Scan = scan,
                Ver = ver,
                PInfo = pinfo,
                RecInfo = recInfo,
                Date = date,
                Time = time,
                Label = label,
                Type = type,
                Dim = dim,
                Annot = annot
            };
            var result = await _searchService.Search(query);

            return Ok(result.ToDtos());
        }
    }
}
