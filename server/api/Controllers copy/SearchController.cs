using fitnessapi.Models;
using Microsoft.AspNetCore.Mvc;


namespace fitnessapi.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        readonly ISearchProviderFactory _provider;

        public SearchController(ISearchProviderFactory provider)
        {
            _provider = provider;
        }


        [HttpGet]
        public List<SearchResult> PerformSearch(string query, int page, string name)
        {

            if (name != "sql" && name != "opensearch" )
            {
                name = "sql";
            }

            var provider = _provider.Create(name);
            return provider.PerformSearch(query, page);
        }

    }
}
