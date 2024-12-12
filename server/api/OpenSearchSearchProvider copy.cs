using fitnessapi.Models;
using Microsoft.AspNetCore.Mvc;
using OpenSearch.Client;
using OpenSearch.Net;

namespace fitnessapi
{
    public class OpenSearchSearchProvider : ISearchProvider
    {
        private readonly Parser _parser;
        private readonly OpenSearchQueryBuilder _builder;

        public OpenSearchSearchProvider(Parser parser, OpenSearchQueryBuilder builder)
        {
            _parser = parser;
            _builder = builder;
        }

        [HttpGet]
        public List<SearchResult> PerformSearch(string query, int page)
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DefaultFieldNameInferrer(f => f);

            IOpenSearchClient client = new OpenSearchClient(settings);

            var lowLevelClient = client.LowLevel;


            var pageSize = 15;
            var skipCount = (page - 1) * pageSize;

            var searchItems = _parser.Parse(query);

            var searchQuery = _builder.Build(searchItems, pageSize, skipCount);



            var searchResponseLow = lowLevelClient.Search<SearchResponse<SearchResult>>("fitnessidx", PostData.Serializable(searchQuery));


            var searchResults = searchResponseLow.Hits.Select(hit => new SearchResult
            {
                Id = hit.Source.Id,
                Body = hit.Source.Body,
                Title = hit.Source.Title,
                Tags = hit.Source.Tags,
                VoteCount = hit.Source.VoteCount,
                ViewCount = hit.Source.ViewCount,
                QuestionId = hit.Source.QuestionId,
                PostTypeId = (int)hit.Source.PostTypeId,
                AcceptedAnswerId = hit.Source.AcceptedAnswerId,
                AnswerCount = hit.Source.AnswerCount,
                IsAcceptedAnswer = hit.Source.IsAcceptedAnswer
            }).ToList();


            return searchResults;
        }
    }
}

