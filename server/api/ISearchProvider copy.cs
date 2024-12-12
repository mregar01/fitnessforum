using fitnessapi.Models;
namespace fitnessapi
{
	public interface ISearchProvider
    {
		List<SearchResult> PerformSearch(string query, int page);
	}

	public interface ISearchProviderFactory
	{
		ISearchProvider Create(string name);		
	}

	public class SearchProviderFactory : ISearchProviderFactory
	{
        readonly Func<string, ISearchProvider> _func;

		public SearchProviderFactory(Func<string, ISearchProvider> func)
		{
			_func = func;
		}

        public ISearchProvider Create(string name)
		{
			return _func(name);
		}
	}
}


