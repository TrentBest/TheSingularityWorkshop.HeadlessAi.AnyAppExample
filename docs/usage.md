# Run the example

## Requirements

- .NET 8 SDK.
- Access to restore NuGet test dependencies on the first build.
- No API key is required for the default deterministic run or local interactive simulation.
- Until the alpha package is published, clone the HeadlessAi source into the expected sibling folder; CI does this automatically.

## Build and run

```bash
mkdir -p dependencies
git clone --branch development https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi.git dependencies/HeadlessAi
dotnet restore TheSingularityWorkshop.HeadlessAi.AnyAppExample.slnx
dotnet build TheSingularityWorkshop.HeadlessAi.AnyAppExample.slnx --configuration Release --no-restore
dotnet test TheSingularityWorkshop.HeadlessAi.AnyAppExample.slnx --configuration Release
dotnet run --project src/TheSingularityWorkshop.HeadlessAi.AnyAppExample
```

The default run routes one ordinary HeadlessAi request through a fake `HttpMessageHandler`. This exercises profile configuration, adapter request construction, and response normalization without contacting a real provider.

## Play with the integration locally

Set `HEADLESSAI_PLAYGROUND=true` to enter a prompt loop:

**PowerShell**
```powershell
$env:HEADLESSAI_PLAYGROUND = "true"
dotnet run --project src/TheSingularityWorkshop.HeadlessAi.AnyAppExample
```

**Bash**
```bash
HEADLESSAI_PLAYGROUND=true dotnet run --project src/TheSingularityWorkshop.HeadlessAi.AnyAppExample
```

Type prompts and see them pass through the real HeadlessAi profile/agent/adapter pipeline. The local handler echoes the prompt with a clear simulation label. It does **not** generate an AI answer, call a provider, require a key, or incur a charge. Type `/exit` to quit.

## Try a real provider (optional)

To turn the same prompt loop into a live OpenAI Responses call, read [Live mode](live-mode.md) first, set `HEADLESSAI_LIVE=true` and `OPENAI_API_KEY`, then also set `HEADLESSAI_PLAYGROUND=true`. Live calls can incur charges and are subject to provider limits. Never commit credentials or paste them into source files.

## What to look for

- The host constructs and owns `HttpClient`.
- `HeadlessAiProfile` describes endpoint configuration and non-secret settings.
- `OpenAiResponsesAdapter` translates normalized input to the provider protocol.
- `HeadlessAiAgentTemplate` creates a caller from reusable configuration.
- The host receives normalized output and remains responsible for decisions and actions.
