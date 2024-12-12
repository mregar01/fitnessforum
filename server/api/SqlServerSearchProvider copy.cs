using fitnessapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fitnessapi
{
	public class SqlServerSearchProvider : ISearchProvider
	{

        private readonly FitnessContext _context;
        private readonly Parser _parser;
        private readonly SqlServerQueryBuilder _builder;

        public SqlServerSearchProvider(FitnessContext context, Parser parser, SqlServerQueryBuilder builder)
        {
            _context = context;
            _parser = parser;
            _builder = builder;
        }

        [HttpGet("search")]
        public List<SearchResult> PerformSearch(string query, int page)
        {
            int pageSize = 15;

            int skipCount = (page - 1) * pageSize;

            var searchItems = _parser.Parse(query);

            var queryItems = _builder.Build(searchItems);


            // Execute the SQL query using FromSql and pass the parameters
            var posts = _context.SearchResults
                .FromSqlRaw(queryItems.Query, queryItems.Parameters.ToArray())
                .ToList()
                .Skip(skipCount)
                .Take(pageSize)
                .ToList();

            return posts;
        }
    }
}

