using System.Net;
using System.Text;
using TheSingularityWorkshop.HeadlessAi;

namespace TheSingularityWorkshop.HeadlessAi.AnyAppExample.Tests;

public sealed class ExampleContractTests
{
    [Fact]
    public async Task AgentUsesInjectedTransportAndReturnsNormalizedText()
    {
        using var client = new HttpClient(new FixtureHandler());
        var profile = new HeadlessAiProfile(
            "fixture",
            new Uri("https://example.invalid/v1/responses"),
            settings: new Dictionary<string, string> { ["model"] = "fixture-model" });
        var agent = new HeadlessAiAgentTemplate(profile, new OpenAiResponsesAdapter()).CreateAgent(client);

        var output = await agent.SendAsync(new HeadlessAiInput("hello"));

        Assert.Equal("Example fixture response.", output.Content);
        Assert.Equal(HttpStatusCode.OK, output.StatusCode);
        Assert.Equal("fixture", agent.ProfileId);
    }

    [Fact]
    public void ProfileRejectsPlainHttpByDefault()
    {
        Assert.Throws<ArgumentException>(() =>
            new HeadlessAiProfile("unsafe", new Uri("http://example.invalid/v1/responses")));
    }

    private sealed class FixtureHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("example.invalid", request.RequestUri!.Host);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    """{"output":[{"type":"message","content":[{"type":"output_text","text":"Example fixture response."}]}]}""",
                    Encoding.UTF8,
                    "application/json")
            });
        }
    }
}
