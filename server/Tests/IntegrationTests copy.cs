using Tests.Fixtures;

namespace Tests;

[Collection("Integration")]
public abstract class IntegrationTests
{
    protected IntegrationTests(IntegrationFixture fixture)
    {
        Fixture = fixture;
    }

    protected IntegrationFixture Fixture { get; }
    
}