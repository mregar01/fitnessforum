using fitnessapi.Controllers;
using fitnessapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tests.Fixtures;

namespace Tests;

public class ResponseTests : IntegrationTests
{

    readonly FitnessContext _context;
    readonly IntegrationFixture _fixture;

    public ResponseTests(IntegrationFixture fixture) : base(fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _context = new FitnessContext(Fixture.DbOptions);
        _fixture = fixture;

    }

    [Fact]
    public async Task EditResponse_Should_Return_OkResult()
    {
        // Arrange

        var postDto = new PostDto
        {
            PostTitle = "add response 1",
            PostBody = "add response 1 body",
            PostTags = "<diet>"
        };


        var responseDtoInitial = new ResponseDto
        {
            ParentId = 1,
            ResponseBody = "Reponse test initial",
        };

        var responseDtoEdited = new ResponseDto
        {
            ParentId = 1,
            ResponseBody = "Reponse test after edit",
        };

        // Act

        var postLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PostController>();

        var postController = new PostController(_context, postLogger);

        var responseLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ResponseController>();

        var responseController = new ResponseController(_context, responseLogger);

        var postResponse = await postController.AddPost(postDto);
        var answerResponse = await postController.AddResponse(responseDtoInitial, 1);

        var editResponse = await responseController.EditResponse(2, responseDtoEdited);


        var actionResult = await postController.GetPostWithResponses(id: 1);
        var post = actionResult.Value; // Extract the value from the ActionResult

        // Assert

        postResponse.Should().BeOfType<OkResult>();
        answerResponse.Should().BeOfType<OkResult>();
        editResponse.Should().BeOfType<OkResult>();

        post?.Responses?.First().Body.Should().BeEquivalentTo(responseDtoEdited.ResponseBody);

        _fixture.ClearDatabase();
    }

}