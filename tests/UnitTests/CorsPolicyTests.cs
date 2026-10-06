using API.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ApiCorsOptions = API.Options.CorsOptions;
using AspNetCorsOptions = Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions;

namespace PersonalFinance.Tests;

public sealed class CorsPolicyTests
{
    [Fact]
    public void AddCorsPolicy_WhenCorsSectionIsMissing_ShouldThrow()
    {
        var configuration = new ConfigurationBuilder().Build();
        var services = new ServiceCollection();

        var act = () => services.AddCorsPolicy(configuration);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("CORS configuration must define at least one allowed origin at 'Cors:AllowedOrigins'.");
    }

    [Fact]
    public void AddCorsPolicy_WhenAllowedOriginsIsBlank_ShouldThrow()
    {
        var configuration = BuildConfiguration(
            new KeyValuePair<string, string?>("Cors:AllowedOrigins:0", " ")
        );
        var services = new ServiceCollection();

        var act = () => services.AddCorsPolicy(configuration);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("CORS configuration must define at least one allowed origin at 'Cors:AllowedOrigins'.");
    }

    [Fact]
    public void AddCorsPolicy_WhenAllowedOriginsAreConfigured_ShouldRegisterPolicy()
    {
        var configuration = BuildConfiguration(
            new KeyValuePair<string, string?>("Cors:AllowedOrigins:0", "https://app.example.com")
        );
        var services = new ServiceCollection();

        services.AddCorsPolicy(configuration);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<AspNetCorsOptions>>().Value;
        var policy = options.GetPolicy(ApiCorsOptions.DefaultPolicyName);

        policy.Should().NotBeNull();
        policy!.Origins.Should().ContainSingle().Which.Should().Be("https://app.example.com");
        policy.AllowAnyHeader.Should().BeTrue();
        policy.AllowAnyMethod.Should().BeTrue();
        policy.SupportsCredentials.Should().BeTrue();
    }

    private static IConfiguration BuildConfiguration(params KeyValuePair<string, string?>[] values)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }
}
