using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace HospitalManagement.IntegrationTests;

public class PagesTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public PagesTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_IndexPage_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/");

        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Get_LoginPage_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/Login");

        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Get_HealthEndpoint_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/health");

        response.IsSuccessStatusCode.Should().BeTrue();
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("healthy");
    }
}
