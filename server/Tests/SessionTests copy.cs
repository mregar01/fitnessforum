using fitnessapi.Models;
using Tests.Fixtures;
using System.Text;
using System.Text.Json;

namespace Tests;

public class SessionTests : IntegrationTests
{

    readonly IntegrationFixture _fixture;

    public SessionTests(IntegrationFixture fixture) : base(fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _fixture = fixture;

    }

    [Fact]
    public async Task SetCurrentUser_Should_Return_OkResponse()
    {
        // Arrange
        var currentUser = new CurrentUser
        {
            UserId = 1,
            DisplayName = "John Doe"
        };

        var json = JsonSerializer.Serialize(currentUser);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Fixture.Client.PostAsync("api/session/user", content);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        _fixture.Handler.Container.GetAllCookies().Should().NotBeNull();

        CookieCollection allcookies = _fixture.Handler.Container.GetAllCookies();
        allcookies["Id"]?.Value.Should().Be(currentUser.UserId.ToString());

        // Sends this down encoded, so must decode to check agaisnt value
        var decodedNameValue = WebUtility.UrlDecode(allcookies["Name"]?.Value);

        decodedNameValue.Should().Be(currentUser.DisplayName);

        // Part 2, tests 'get' as well. These two functions are only meant
        // to work together, so another test doesnt make sense       
        var getResponse = await Fixture.Client.GetAsync("api/session/user");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

    }
}



