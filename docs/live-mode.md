# Optional live mode

The default demonstration is local and deterministic. Live mode is a deliberate opt-in because it can transmit input to an external service and incur provider charges.

## PowerShell

```powershell
$env:HEADLESSAI_LIVE = "true"
$env:OPENAI_API_KEY = "your-key-from-a-secret-manager"
$env:HEADLESSAI_MODEL = "gpt-4.1-mini"
dotnet run --project src/TheSingularityWorkshop.HeadlessAi.AnyAppExample
```

## Bash

```bash
export HEADLESSAI_LIVE=true
export OPENAI_API_KEY='your-key-from-a-secret-manager'
export HEADLESSAI_MODEL='gpt-4.1-mini'
dotnet run --project src/TheSingularityWorkshop.HeadlessAi.AnyAppExample
```

Optional `HEADLESSAI_ENDPOINT` overrides the endpoint URL. Use only an endpoint you are authorized to call and understand. The code defaults to the OpenAI Responses URL and reads the model from `HEADLESSAI_MODEL`.

## Safety notes

- Do not commit credentials or put them in appsettings tracked by Git.
- Environment variables are a simple demonstration, not a complete production secret-management strategy.
- The live endpoint receives the prompt. Do not submit confidential or personal information unless your application and provider policies permit it.
- Provider requests may cost money and are subject to authentication, quota, rate, and data-handling terms.
- The sample prints response text to the console; production hosts should apply their own sensitive-data and logging policy.
- The sample does not retry automatically or execute output.
