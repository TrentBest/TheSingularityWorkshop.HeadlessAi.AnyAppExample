# Run the example

## Requirements

- .NET 8 SDK.
- Access to restore NuGet test dependencies on the first build.
- No API key is required for the default deterministic run.
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

The default run routes an ordinary HeadlessAi request through a fake `HttpMessageHandler`. This exercises the profile, adapter, request construction, and response normalization without contacting a real provider.

## What to look for

- The host constructs and owns `HttpClient`.
- `HeadlessAiProfile` describes endpoint configuration and non-secret settings.
- `OpenAiResponsesAdapter` translates normalized input to the provider protocol.
- `HeadlessAiAgentTemplate` lets the host create a lightweight caller.
- The host receives normalized output and decides what to do with it.

For the real provider path, read [Live mode](live-mode.md) before setting environment variables.
