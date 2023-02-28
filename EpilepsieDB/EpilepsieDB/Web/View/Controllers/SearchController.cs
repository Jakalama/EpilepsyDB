using EpilepsieDB.Authorization;
using EpilepsieDB.Models;
using EpilepsieDB.Services;
using EpilepsieDB.Web.View.Parser;
using EpilepsieDB.Web.View.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpilepsieDB.Web.View.Controllers
{
    [Controller]
    [Authorize(Roles = RoleSet.AllowRead)]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[action]", Order = 0)]
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;
        private readonly ISearchInputParser _inputParser;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
            _inputParser = new SearchInputParser();
        }

        public async Task<IActionResult> Index(string currentFilter, string searchString, int? pageNumber)
        {
            if (currentFilter != null)
                currentFilter = currentFilter.Trim();
            if (searchString != null)
                searchString = searchString.Trim();

            if (searchString == null)
                searchString = currentFilter;
            else
                pageNumber = 1;

            // apply searchstring first
            ViewData["CurrentFilter"] = searchString;

            var query = _inputParser.Parse(searchString);
            var result = await _searchService.Search(query);

            return View(GetSearchResultFromScans(result));
        }

        private SearchResult GetSearchResultFromScans(IEnumerable<Scan> scans)
        {
            SearchResult result = new SearchResult();

            foreach (Scan scan in scans)
            {
                if (!result.Results.ContainsKey(scan.Recording.Patient))
                {
                    result.Results.Add(scan.Recording.Patient, new Dictionary<Recording, IEnumerable<Scan>>());
                }

                if (!result.Results[scan.Recording.Patient].ContainsKey(scan.Recording))
                {
                    result.Results[scan.Recording.Patient].Add(scan.Recording, new List<Scan>());
                }

                result.Results[scan.Recording.Patient][scan.Recording] = result.Results[scan.Recording.Patient][scan.Recording].Append(scan);
            }

            return result;
        }
    }
}
