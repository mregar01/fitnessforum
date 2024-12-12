using fitnessapi.Models;

namespace fitnessapi
{
	public class OpenSearchQueryBuilder
	{
        public object Build(SearchItems searchItems, int pageSize, int skipCount)
        {


            var queryBody = new
            {
                size = pageSize,
                from = skipCount,
                query = new
                {
                    @bool = new
                    {
                        must = new List<object>()
                    }
                }
            };


            if (searchItems.IsAccepted.HasValue)
            {
                if (searchItems.IsAccepted.Value)
                {
                    queryBody.query.@bool.must.Add(new
                    {
                        term = new
                        {
                            IsAcceptedAnswer = new
                            {
                                value = 1
                            }
                        }
                    });
                }
                else
                {
                    queryBody.query.@bool.must.Add(new
                    {
                        @bool = new
                        {
                            must = new List<object>
                {
                    new { term = new { PostTypeId = new { value = 1 } } },
                    new { @bool = new { must_not = new { exists = new { field = "AcceptedAnswerId" } } } }
                }
                        }
                    });
                }
            }

            if (searchItems.UserId.HasValue)
            {
                queryBody.query.@bool.must.Add(new
                {
                    term = new
                    {
                        OwnerUserId = new
                        {
                            value = searchItems.UserId.Value
                        }
                    }
                });
            }

            if (searchItems.MinScore.HasValue)
            {
                queryBody.query.@bool.must.Add(new
                {
                    range = new
                    {
                        VoteCount = new
                        {
                            gte = searchItems.MinScore.Value
                        }
                    }
                });
            }

            if (searchItems.NumAnswers.HasValue)
            {
                queryBody.query.@bool.must.Add(new
                {
                    range = new
                    {
                        AnswerCount = new
                        {
                            gte = searchItems.NumAnswers.Value
                        }
                    }
                });
            }

            if (searchItems.Tags?.Any() == true)
            {
                foreach (var tag in searchItems.Tags)
                {
                    queryBody.query.@bool.must.Add(new
                    {
                        match = new
                        {
                            Tags = new
                            {
                                query = tag
                            }
                        }
                    });
                }
            }

            if (searchItems.SearchWords?.Any() == true)
            {
                foreach (var word in searchItems.SearchWords)
                {
                    queryBody.query.@bool.must.Add(new
                    {
                        match = new
                        {
                            Body = new
                            {
                                query = word
                            }
                        }
                    });
                }
            }

            if (searchItems.SearchPhrases?.Any() == true)
            {
                foreach (var phrase in searchItems.SearchPhrases)
                {
                    queryBody.query.@bool.must.Add(new
                    {
                        match_phrase = new
                        {
                            Body = new
                            {
                                query = phrase
                            }
                        }
                    });
                }
            }

            return queryBody;
        }
    }
}

