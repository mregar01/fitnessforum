using fitnessapi.Controllers;
using fitnessapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tests.Fixtures;

namespace Tests;

public class CommentTests : IntegrationTests
{

    readonly FitnessContext _context;
    readonly IntegrationFixture _fixture;

    public CommentTests(IntegrationFixture fixture) : base(fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _context = new FitnessContext(Fixture.DbOptions);
        _fixture = fixture;

    }

    [Fact]
    public async Task AddComment_Should_Return_OkResult()
    {
        // Arrange

        var postDto = new PostDto
        {
            PostTitle = "add comment 1",
            PostBody = "add comment 1 body",
            PostTags = "<commenttest>"
        };

        var commentDto = new CommentDto
        {
            PostId = 1,
            CommentBody = "new comment body here"
        };


        // Act

        var postLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PostController>();

        var postController = new PostController(_context, postLogger);

        var commentLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<CommentController>();

        var commentController = new CommentController(_context, commentLogger);

        var postResponse = await postController.AddPost(postDto);
        var commentResponse = await commentController.AddComment(commentDto);

        var actionResult = await postController.GetPostWithResponses(id: 1);
        var post = actionResult.Value; // Extract the value from the ActionResult

        // Assert

        postResponse.Should().BeOfType<OkResult>();
        commentResponse.Should().BeOfType<OkResult>();

        post?.PostItem?.Comments?.First().Text.Should().BeEquivalentTo(commentDto.CommentBody);
        post?.PostItem?.Comments.Should().HaveCount(1);

        _fixture.ClearDatabase();
    }

}