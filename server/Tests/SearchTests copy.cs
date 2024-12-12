using System.Text;
using fitnessapi;
using fitnessapi.Models;
using Tests.Fixtures;

namespace Tests;

public class SearchTests : IntegrationTests
{

    readonly FitnessContext _context;
    readonly IntegrationFixture _fixture;

    public SearchTests(IntegrationFixture fixture) : base(fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _context = new FitnessContext(Fixture.DbOptions);
        _fixture = fixture;
    }

    [Fact]
    public void Parser_Should_Work()
    {
        // Arrange

        string query = "bench user:10 score:50 [diet] answers:5 isaccepted:yes \"bench press\"";

        var Parser = new Parser();

        // Act

        var SearchItems = Parser.Parse(query);

        // Assert

        SearchItems.IsAccepted.Should().BeTrue();
        SearchItems.UserId.Should().Be(10);
        SearchItems.MinScore.Should().Be(50);
        SearchItems.NumAnswers.Should().Be(5);
        SearchItems.SearchWords.Should().Contain("bench");
        SearchItems.SearchPhrases.Should().Contain("bench press");
        SearchItems.Tags.Should().Contain("diet");
    }

    [Fact]
    public void Query_Builder_Should_Work()
    {
        // Arrange

        var SearchItems = new SearchItems
        {
            UserId = 5,
            MinScore = 10,
            NumAnswers = 15,
            IsAccepted = false,
            SearchWords = new List<string> { "bench", "boss" },
            SearchPhrases = new List<string> { "bench press"},
            Tags = new List<string> { "diet" }
        };

        var expectedQuery = new StringBuilder();
        expectedQuery.Append("""
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
                            WHERE 1=1 AND EXISTS (SELECT 1 FROM NotAcceptedAnswers AS answer WHERE answer.Id = p1.Id)AND p1.OwnerUserId = @UserId AND p1.Score >= @Score AND p1.AnswerCount >= @Answers AND ISNULL(p2.Tags, p1.Tags) LIKE '%' + @Tag0 + '%'AND p1.Body IS NOT NULL AND (p1.Body LIKE '% ' + @SearchWord0 + ' %' OR p1.Body LIKE @SearchWord0 + ' %' OR p1.Body LIKE '% ' + @SearchWord0)AND p1.Body IS NOT NULL AND (p1.Body LIKE '% ' + @SearchWord1 + ' %' OR p1.Body LIKE @SearchWord1 + ' %' OR p1.Body LIKE '% ' + @SearchWord1)AND p1.Body IS NOT NULL AND p1.Body LIKE '%' + @SearchPhrase0 + '%'
                            """);


        var Builder = new SqlServerQueryBuilder();

        // Act

        var queryItems = Builder.Build(SearchItems);

        // Assert

        queryItems.Query.Should().Be(expectedQuery.ToString());

        queryItems.Parameters.Should().Contain(param => param.ParameterName == "@UserId" && param.Value.Equals(SearchItems.UserId))
          .And.Contain(param => param.ParameterName == "@Score" && param.Value.Equals(SearchItems.MinScore))
          .And.Contain(param => param.ParameterName == "@SearchWord0" && param.Value.Equals("%bench%"))
          .And.Contain(param => param.ParameterName == "@SearchWord1" && param.Value.Equals("%boss%"))
          .And.Contain(param => param.ParameterName == "@Answers" && param.Value.Equals(SearchItems.NumAnswers))
          .And.Contain(param => param.ParameterName == "@Tag0" && param.Value.Equals("%<diet>%"))
          .And.Contain(param => param.ParameterName == "@SearchPhrase0" && param.Value.Equals("%bench press%"));

    }     
}
