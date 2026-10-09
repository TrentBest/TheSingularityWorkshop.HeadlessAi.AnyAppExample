using System.Net;
using System.Text;
using TheSingularityWorkshop.HeadlessAi;

namespace TheSingularityWorkshop.HeadlessAi.AnyAppExample;

internal static class Program
{
    private const string DemoResponse = "Hello from the local HeadlessAi demonstration.";
    private const string DemoJson = """{"output":[{"type":"message","content":[{"type":"output_text","text":"Hello from the local HeadlessAi demonstration."}]}]}""";

    private static async Task<int> Main()
    {
        var live = string.Equals(Environment.GetEnvironmentVariable("HEADLESSAI_LIVE"), "true", StringComparison.OrdinalIgnoreCase);
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (live && string.IsNullOrWhiteSpace(apiKey))
        {
            Console.Error.WriteLine("Live mode was requested, but OPENAI_API_KEY is not set.");
            return 2;
        }

        using var httpClient = live
            ? new HttpClient()
            : new HttpClient(new DemonstrationHandler(DemoJson));

        var headers = live
            ? new Dictionary<string, string> { ["Authorization"] = $"Bearer {apiKey}" }
            : new Dictionary<string, string>();

        var profile = new HeadlessAiProfile(
            id: live ? "openai-live-demo" : "local-demo",
            endpoint: new Uri(Environment.GetEnvironmentVariable("HEADLESSAI_ENDPOINT") ?? "https://api.openai.com/v1/responses"),
            headers: headers,
            settings: new Dictionary<string, string> { ["model"] = Environment.GetEnvironmentVariable("HEADLESSAI_MODEL") ?? "gpt-4.1-mini" },
            timeout: TimeSpan.FromSeconds(30),
            maxResponseBytes: 1024 * 1024);

        var template = new HeadlessAiAgentTemplate(profile, new OpenAiResponsesAdapter());
        var agent = template.CreateAgent(httpClient);

        Console.WriteLine(live ? "Mode: live provider call" : "Mode: deterministic local HTTP demonstration");
        var response = await agent.SendAsync(new HeadlessAiInput("Say hello in one short sentence."));
        Console.WriteLine($"Profile: {agent.ProfileId}");
        Console.WriteLine($"HTTP: {(int)response.StatusCode} {response.StatusCode}");
        Console.WriteLine($"Response: {response.Content}");
        if (!live && response.Content != DemoResponse)
            return 1;
        return 0;
    }

    private sealed class DemonstrationHandler(string responseJson) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (request.RequestUri is null || request.RequestUri.Scheme != Uri.UriSchemeHttps)
                throw new InvalidOperationException("The demonstration expects an HTTPS-shaped endpoint URI.");
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });
        }
    }
}
