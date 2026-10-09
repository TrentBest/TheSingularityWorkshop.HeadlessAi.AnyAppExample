using System.Net;
using System.Text;
using System.Text.Json;
using TheSingularityWorkshop.HeadlessAi;

namespace TheSingularityWorkshop.HeadlessAi.AnyAppExample;

internal static class Program
{
    private const string DemoResponse = "Hello from the local HeadlessAi demonstration.";
    private const string DemoJson = """{"output":[{"type":"message","content":[{"type":"output_text","text":"Hello from the local HeadlessAi demonstration."}]}]}""";

    private static async Task<int> Main()
    {
        var live = string.Equals(Environment.GetEnvironmentVariable("HEADLESSAI_LIVE"), "true", StringComparison.OrdinalIgnoreCase);
        var playground = string.Equals(Environment.GetEnvironmentVariable("HEADLESSAI_PLAYGROUND"), "true", StringComparison.OrdinalIgnoreCase);
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (live && string.IsNullOrWhiteSpace(apiKey))
        {
            Console.Error.WriteLine("Live mode was requested, but OPENAI_API_KEY is not set.");
            return 2;
        }

        using var httpClient = live
            ? new HttpClient()
            : new HttpClient(new DemonstrationHandler(DemoJson, playground));

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

        var agent = new HeadlessAiAgentTemplate(profile, new OpenAiResponsesAdapter()).CreateAgent(httpClient);

        Console.WriteLine(live
            ? "Mode: live provider call (requests may incur charges)"
            : playground
                ? "Mode: local interactive simulation (no AI provider or network call)"
                : "Mode: deterministic local HTTP demonstration");
        Console.WriteLine($"Profile: {agent.ProfileId}");

        if (playground)
        {
            Console.WriteLine("Enter a prompt, or type /exit to quit.");
            while (true)
            {
                Console.Write("> ");
                var prompt = Console.ReadLine();
                if (prompt is null || string.Equals(prompt.Trim(), "/exit", StringComparison.OrdinalIgnoreCase))
                    break;
                if (string.IsNullOrWhiteSpace(prompt))
                    continue;

                try
                {
                    var response = await agent.SendAsync(new HeadlessAiInput(prompt));
                    Console.WriteLine(response.Content);
                    if (live)
                        Console.WriteLine($"HTTP: {(int)response.StatusCode} {response.StatusCode}");
                }
                catch (OperationCanceledException)
                {
                    Console.Error.WriteLine("The request was cancelled or timed out.");
                }
                catch (HeadlessAiHttpException ex)
                {
                    Console.Error.WriteLine($"Endpoint returned HTTP {(int)ex.StatusCode}. Response excerpts can contain sensitive data; inspect them deliberately.");
                }
                catch (HttpRequestException)
                {
                    Console.Error.WriteLine("The endpoint request failed. Check connectivity, endpoint configuration, and credentials without printing secrets.");
                }
            }

            return 0;
        }

        var result = await agent.SendAsync(new HeadlessAiInput("Say hello in one short sentence."));
        Console.WriteLine($"HTTP: {(int)result.StatusCode} {result.StatusCode}");
        Console.WriteLine($"Response: {result.Content}");
        if (!live && result.Content != DemoResponse)
            return 1;
        return 0;
    }

    private sealed class DemonstrationHandler(string responseJson, bool interactive) : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (request.RequestUri is null || request.RequestUri.Scheme != Uri.UriSchemeHttps)
                throw new InvalidOperationException("The demonstration expects an HTTPS-shaped endpoint URI.");

            var response = responseJson;
            if (interactive && request.Content is not null)
            {
                using var requestJson = JsonDocument.Parse(await request.Content.ReadAsStringAsync(cancellationToken));
                var prompt = requestJson.RootElement.TryGetProperty("input", out var input)
                    ? input.GetString() ?? string.Empty
                    : string.Empty;
                var simulatedText = $"Local simulation received your prompt: {prompt}\n\nThis is a deterministic demo response, not a response generated by an AI model.";
                response = JsonSerializer.Serialize(new
                {
                    output = new[]
                    {
                        new
                        {
                            type = "message",
                            content = new[] { new { type = "output_text", text = simulatedText } }
                        }
                    }
                });
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(response, Encoding.UTF8, "application/json")
            };
        }
    }
}
