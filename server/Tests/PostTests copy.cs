using fitnessapi.Controllers;
using fitnessapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tests.Fixtures;

namespace Tests;

public class PostTests : IntegrationTests
{

    readonly FitnessContext _context;
    readonly IntegrationFixture _fixture;

    public PostTests(IntegrationFixture fixture) : base(fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _context = new FitnessContext(Fixture.DbOptions);
        _fixture = fixture;

    }

    [Fact]
    public async Task AddPost_Should_Return_OkResult()
    {
        // Arrange
        var postDto = new PostDto
        {
            PostTitle = "add post 1",
            PostBody = "add post 1 body",
            PostTags = "<diet>"
        };

        // Act
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PostController>();

        var controller = new PostController(_context, logger);

        var response = await controller.AddPost(postDto);

        var actionResult = await controller.GetPostWithResponses(id: 1);
        var firstpost = actionResult.Value; // Extract the value from the ActionResult

        // Assert
        response.Should().BeOfType<OkResult>();
        firstpost.Should().BeOfType<PostItemWithResponses>();

        firstpost?.PostItem?.Title.Should().BeEquivalentTo(postDto.PostTitle);
        firstpost?.PostItem?.Body.Should().BeEquivalentTo(postDto.PostBody);
        firstpost?.PostItem?.Tags.Should().BeEquivalentTo(postDto.PostTags);

        _fixture.ClearDatabase();

    }


    [Fact]
    public async Task AddResponse_Should_Return_OkResult()
    {
        // Arrange
        var postDto = new PostDto
        {
            PostTitle = "add response 1",
            PostBody = "add response 1 body",
            PostTags = "<diet>"
        };


        var responseDto = new ResponseDto
        {
            ParentId = 1,
            ResponseBody = "Reponse test",
        };

        // Act
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PostController>();

        var controller = new PostController(_context, logger);

        var response = await controller.AddPost(postDto);

        var secondresponse = await controller.AddResponse(responseDto, 1);

        var actionResult = await controller.GetPostWithResponses(id: 1);
        var firstpost = actionResult.Value; // Extract the value from the ActionResult


        // Assert
        response.Should().BeOfType<OkResult>();
        secondresponse.Should().BeOfType<OkResult>();
        firstpost?.Responses?.First().Body.Should().BeEquivalentTo(responseDto.ResponseBody);

        _fixture.ClearDatabase();
    }

    [Fact]
    public async Task GetPosts_Should_Return_OkResult()
    {
        // Arrange
        var postDto1 = new PostDto
        {
            PostTitle = "get posts 1",
            PostBody = "get posts 1 body",
            PostTags = "<diet>"
        };

        var postDto2 = new PostDto
        {
            PostTitle = "get posts 2",
            PostBody = "get posts 2 body",
            PostTags = "<squats>"
        };

        // Act
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PostController>();

        var controller = new PostController(_context, logger);

        var response1 = await controller.AddPost(postDto1);
        var response2 = await controller.AddPost(postDto2);

        var actionResult = await controller.GetPosts();
        var posts = actionResult.Value;

        // Assert
        response1.Should().BeOfType<OkResult>();
        response2.Should().BeOfType<OkResult>();

        posts.Should().NotBeNull().And.BeAssignableTo<IEnumerable<PostItem>>();
        posts.Should().HaveCount(2);


        // putting postdto2 first because getposts returns post list with
        // most recent post first

        // also no testing for body as getposts doesnt add body because
        // body is not displayed on main page
        posts?.ElementAt(0).Title.Should().BeEquivalentTo(postDto2.PostTitle);
        posts?.ElementAt(0).Tags.Should().BeEquivalentTo(postDto2.PostTags);

        posts?.ElementAt(1).Title.Should().BeEquivalentTo(postDto1.PostTitle);
        posts?.ElementAt(1).Tags.Should().BeEquivalentTo(postDto1.PostTags);

        _fixture.ClearDatabase();

    }

    [Fact]
    public async Task GetTags_Should_Return_OkResult()
    {
        // Arrange
        var postDto1 = new PostDto
        {
            PostTitle = "tags 1",
            PostBody = "tags 1 body",
            PostTags = "<tagtest>"
        };

        var postDto2 = new PostDto
        {
            PostTitle = "tags 2",
            PostBody = "tags 2 body",
            PostTags = "<tagtest>"
        };

        // Act
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PostController>();

        var controller = new PostController(_context, logger);

        var response1 = await controller.AddPost(postDto1);
        var response2 = await controller.AddPost(postDto2);

        var actionResult = await controller.GetTags(tagName: "tagtest", page: 1);

        var taggedposts = actionResult.Value;

        // Assert
        response1.Should().BeOfType<OkResult>();
        response2.Should().BeOfType<OkResult>();

        taggedposts.Should().NotBeNull().And.BeAssignableTo<IEnumerable<PostItem>>();
        taggedposts.Should().HaveCount(2);

        // putting postdto2 first because getposts returns post list with
        // most recent post first

        // also no testing for body as getposts doesnt add body because
        // body is not displayed on main page
        taggedposts?.ElementAt(0).Title.Should().BeEquivalentTo(postDto2.PostTitle);
        taggedposts?.ElementAt(0).Tags.Should().BeEquivalentTo(postDto2.PostTags);

        taggedposts?.ElementAt(1).Title.Should().BeEquivalentTo(postDto1.PostTitle);
        taggedposts?.ElementAt(1).Tags.Should().BeEquivalentTo(postDto1.PostTags);

        _fixture.ClearDatabase();
    }

    [Fact]
    public async Task EditPost_Should_Return_OkResult()
    {
        // Arrange
        var postDtoOriginal = new PostDto
        {
            PostTitle = "edit 1",
            PostBody = "edit 1 body",
            PostTags = "<fitness>"
        };

        var postDtoEdited = new PostDto
        {
            PostTitle = "post edit 1",
            PostBody = "post edit 1 body",
            PostTags = "<editedtag>"
        };

        // Act
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PostController>();

        var controller = new PostController(_context, logger);

        var response = await controller.AddPost(postDtoOriginal);

        var editResponse = await controller.EditPost(postId: 1, postDtoEdited);

        var actionResult = await controller.GetPostWithResponses(id: 1);
        var editedpost = actionResult.Value; // Extract the value from the ActionResult

        // Assert
        response.Should().BeOfType<OkResult>();
        editResponse.Should().BeOfType<OkResult>();

        editedpost?.PostItem?.Title.Should().BeEquivalentTo(postDtoEdited.PostTitle);
        editedpost?.PostItem?.Body.Should().BeEquivalentTo(postDtoEdited.PostBody);
        editedpost?.PostItem?.Tags.Should().BeEquivalentTo(postDtoEdited.PostTags);

        _fixture.ClearDatabase();

    }

    [Fact]
    public async Task Increment_Should_Return_OkResult()
    {
        // Arrange
        var postDto = new PostDto
        {
            PostTitle = "upvote 1",
            PostBody = "upvote 1 body",
            PostTags = "<upvote>"
        };

        // Act
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PostController>();

        var controller = new PostController(_context, logger);

        var response = await controller.AddPost(postDto);

        var upvoteResponse = await controller.Upvote(postId: 1);

        var actionResult = await controller.GetPostWithResponses(id: 1);
        var post = actionResult.Value; // Extract the value from the ActionResult

        // Assert
        response.Should().BeOfType<OkResult>();
        upvoteResponse.Should().BeOfType<OkResult>();

        post?.PostItem?.Votes.Should().Be(1);

        _fixture.ClearDatabase();

    }

    [Fact]
    public async Task Decrement_Should_Return_OkResult()
    {
        // Arrange
        var postDto = new PostDto
        {
            PostTitle = "downvote 1",
            PostBody = "downvote 1 body",
            PostTags = "<upvote>"
        };

        // Act
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PostController>();

        var controller = new PostController(_context, logger);

        var response = await controller.AddPost(postDto);

        var upvoteResponse = await controller.DecrementVote(postId: 1);

        var actionResult = await controller.GetPostWithResponses(id: 1);
        var post = actionResult.Value; // Extract the value from the ActionResult

        // Assert
        response.Should().BeOfType<OkResult>();
        upvoteResponse.Should().BeOfType<OkResult>();

        post?.PostItem?.Votes.Should().Be(-1);

        _fixture.ClearDatabase();
    }
}
