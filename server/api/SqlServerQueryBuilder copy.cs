using Microsoft.Data.SqlClient;
using System.Text;
using fitnessapi.Models;

namespace fitnessapi
{
	public class SqlServerQueryBuilder
	{		
		public QueryItems Build(SearchItems searchItems)
		{
            var sqlQuery = new StringBuilder();
            sqlQuery.Append("""
                            WITH AcceptedAnswers AS (
                                SELECT * 
                                FROM Posts AS answer 
                                WHERE answer.PostTypeId = 2 
                                AND EXISTS (
                                    SELECT 1 
                                    FROM Posts AS parent 
                                    WHERE parent.Id = answer.ParentId 
                                    AND parent.AcceptedAnswerId = answer.Id 
                                )
                            ), NotAcceptedAnswers AS (
                                SELECT *FROM Posts AS answer 
                                WHERE answer.PostTypeId = 2 
                                AND NOT EXISTS (
                                    SELECT 1 
                                    FROM Posts AS parent 
                                    WHERE parent.Id = answer.ParentId 
                                    AND parent.AcceptedAnswerId = answer.Id
                                )
                            ) SELECT 
                                p1.Body as Body, 
                                p1.Id as Id, 
                                ISNULL(p2.Id, p1.Id) as QuestionId, 
                                ISNULL(p2.Title, p1.Title) as Title, 
                                ISNULL(p2.Tags, p1.Tags) as Tags, 
                                (SELECT SUM(CASE WHEN VoteTypeId = 2 then 1 else -1 END) FROM Votes WHERE PostId = p1.Id) as VoteCount, 
                                p1.ViewCount as ViewCount, 
                                p1.PostTypeId as PostTypeId, 
                                ISNULL(p2.AcceptedAnswerId, p1.AcceptedAnswerId) as AcceptedAnswerId,
                                CASE WHEN p1.PostTypeId = 2 AND EXISTS (SELECT 1 FROM Posts WHERE PostTypeId = 1 AND AcceptedAnswerId = p1.Id) THEN 1 ELSE 0 END as IsAcceptedAnswer,
                                p1.AnswerCount as AnswerCount,
                                p1.OwnerUserId as OwnerUserId 
                            FROM Posts p1 
                            LEFT OUTER JOIN Posts p2 ON p2.Id = p1.ParentId AND p1.PostTypeId = 2 
                            WHERE 1=1 
                            """);

            if (searchItems.IsAccepted.HasValue)
            {
                if (searchItems.IsAccepted == true)
                {
                    sqlQuery.Append("AND EXISTS (SELECT 1 FROM AcceptedAnswers AS answer WHERE answer.Id = p1.Id)");
                }
                else
                {
                    sqlQuery.Append("AND EXISTS (SELECT 1 FROM NotAcceptedAnswers AS answer WHERE answer.Id = p1.Id)");
                }                
            }            


            if (searchItems?.UserId.HasValue == true)
            {
                sqlQuery.Append("AND p1.OwnerUserId = @UserId ");                
            }

            if (searchItems?.MinScore.HasValue == true)
            {
                sqlQuery.Append("AND p1.Score >= @Score ");                
            }

            if (searchItems?.NumAnswers.HasValue == true)
            {
                sqlQuery.Append("AND p1.AnswerCount >= @Answers ");                
            }

            // Add conditions for each tag in the tags using parameterized queries
            for (int i = 0; i < searchItems?.Tags?.Count; i++)
            {
                sqlQuery.Append("AND ISNULL(p2.Tags, p1.Tags) LIKE '%' + @Tag" + i + " + '%'");                
            }          

            // Add conditions for each word in the searchWords using parameterized queries
            for (int i = 0; i < searchItems?.SearchWords?.Count; i++)
            {
                sqlQuery.Append("AND p1.Body IS NOT NULL AND (p1.Body LIKE '% ' + @SearchWord" + i + " + ' %' OR p1.Body LIKE @SearchWord" + i + " + ' %' OR p1.Body LIKE '% ' + @SearchWord" + i + ")");                
            }
            
            // Add conditions for each phrase using parameterized queries
            for (int i = 0; i < searchItems?.SearchPhrases?.Count; i++)
            {
                sqlQuery.Append("AND p1.Body IS NOT NULL AND p1.Body LIKE '%' + @SearchPhrase" + i + " + '%'");                
            }

            // Create a SqlParameter array for the search word and phrase parameters
            var parameters = new List<SqlParameter>();

            if (searchItems?.UserId.HasValue == true)
            {
                parameters.Add(new SqlParameter("@UserId", searchItems.UserId));
            }

            if (searchItems?.MinScore.HasValue == true)
            {
                parameters.Add(new SqlParameter("@Score", searchItems.MinScore));
            }

            if (searchItems?.NumAnswers.HasValue == true)
            {
                parameters.Add(new SqlParameter("@Answers", searchItems.NumAnswers));
            }

            for (int i = 0; i < searchItems?.SearchWords?.Count; i++)
            {
                parameters.Add(new SqlParameter("@SearchWord" + i, "%" + searchItems.SearchWords[i] + "%"));
            }

            for (int i = 0; i < searchItems?.SearchPhrases?.Count; i++)
            {
                parameters.Add(new SqlParameter("@SearchPhrase" + i, "%" + searchItems.SearchPhrases[i] + "%"));
            }

            for (int i = 0; i < searchItems?.Tags?.Count; i++)
            {
                parameters.Add(new SqlParameter("@Tag" + i, "%<" + searchItems.Tags[i] + ">%"));
            }

            // Convert the StringBuilder to a formattable string using ToString()
            string finalQuery = sqlQuery.ToString();

            QueryItems queryItems = new()
            {
                Query = finalQuery,
                Parameters = parameters
            };

            foreach (var param in parameters)
            {
                Console.WriteLine(param.Value);
            }

            return queryItems;
            
        }
	}
}

