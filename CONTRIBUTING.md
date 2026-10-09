# Contributing

Keep this repository focused on host integration, not duplicate HeadlessAi library behavior. Preserve the deterministic offline default; do not add credentials, live provider calls, or billing-dependent tests to CI. Update tests and docs whenever example behavior changes.

## Verify locally

```bash
dotnet restore TheSingularityWorkshop.HeadlessAi.AnyAppExample.slnx
dotnet build TheSingularityWorkshop.HeadlessAi.AnyAppExample.slnx --configuration Release --no-restore
dotnet test TheSingularityWorkshop.HeadlessAi.AnyAppExample.slnx --configuration Release
```

Follow the [FSM_COS documentation standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md). State whether behavior is implemented, tested, conceptual, or planned.
