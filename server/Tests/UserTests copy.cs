using fitnessapi.Controllers;
using fitnessapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tests.Fixtures;

namespace Tests;

public class UserTests : IntegrationTests
{

    readonly FitnessContext _context;
    readonly IntegrationFixture _fixture;

    public UserTests(IntegrationFixture fixture) : base(fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _context = new FitnessContext(Fixture.DbOptions);
        _fixture = fixture;

    }

    [Fact]
    public async Task GetUser_Should_Return_OkResult()
    {
        // Arrange
        var user = new User
        {
            Id = 28201,
            Reputation = 25,
            DisplayName = "Max",
            WebsiteUrl = "DummyURL",
            Location = "home",
            AboutMe = "this is about me"
        };

        _context.Users.Add(user);



        var postDto = new PostDto
        {
            PostTitle = "get user 1",
            PostBody = "get user 1 body",
            PostTags = "<usertest>"
        };

        var goldBadge = new Badge
        {
            Id = 1,
            UserId = 28201,
            Name = "Test badge",
            Class = BadgeType.Gold
        };

        _context.Badges.Add(goldBadge);


        // Act
        var postLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PostController>();

        var postController = new PostController(_context, postLogger);


        var userController = new UserController(_context);

        var postResponse = await postController.AddPost(postDto);

        var actionResult = await userController.GetUser(28201);
        var currUser = actionResult.Value; // Extract the value from the ActionResult

        // Assert
        postResponse.Should().BeOfType<OkResult>();

        currUser.Should().NotBeNull();
        currUser?.Reputation.Should().Be(user.Reputation);
        currUser?.DisplayName.Should().BeEquivalentTo(user.DisplayName);
        currUser?.WebsiteUrl.Should().BeEquivalentTo(user.WebsiteUrl);
        currUser?.Location.Should().BeEquivalentTo(user.Location);
        currUser?.AboutMe.Should().BeEquivalentTo(user.AboutMe);

        currUser?.GoldBadges.Should().HaveCount(1);
        currUser?.GoldBadges?.First().Name.Should().BeEquivalentTo(goldBadge.Name);

        currUser?.Posts.Should().HaveCount(1);
        currUser?.Posts?.First().Title.Should().BeEquivalentTo(postDto.PostTitle);
        // Won't return body or tags as not displayed on website
        

        _fixture.ClearDatabase();
    }

    [Fact]
    public async Task GetTopUsers_Should_Return_OkResult()
    {
        // Arrange
        var userMax = new User
        {
            Id = 1,
            Reputation = 999999,
            DisplayName = "Max",
            WebsiteUrl = "DummyURL",
            Location = "home",
            AboutMe = "this is about Max"
        };

        var userTom = new User
        {
            Id = 2,
            Reputation = 12,
            DisplayName = "Tom",
            WebsiteUrl = "URL",
            Location = "work",
            AboutMe = "this is about Tom"
        };

        var userJeff = new User
        {
            Id = 3,
            Reputation = 11,
            DisplayName = "Jeff",
            WebsiteUrl = "URLhere",
            Location = "office",
            AboutMe = "this is about Jeff"
        };

        _context.Users.Add(userMax);
        _context.Users.Add(userJeff);
        _context.Users.Add(userTom);

        await _context.SaveChangesAsync();


        // Act
        var userController = new UserController(_context);

        var actionResult = await userController.GetTopUsers();
        var topUsers = actionResult.Value;

        // Assert
        topUsers.Should().NotBeNull();

        // First user should be max, ordered by reputation
        topUsers?.Should().HaveCount(3);
        topUsers?.ElementAt(0).DisplayName.Should().BeEquivalentTo(userMax.DisplayName);
        topUsers?.ElementAt(1).DisplayName.Should().BeEquivalentTo(userTom.DisplayName);
        topUsers?.ElementAt(2).DisplayName.Should().BeEquivalentTo(userJeff.DisplayName);

        _fixture.ClearDatabase();
    }
}