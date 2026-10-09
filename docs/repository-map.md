# Repository map

- README.md: orientation, quick start, and boundaries.
- DOCUMENTATION_STANDARD.md: local application of Workshop documentation rules.
- DOCUMENTATION_INDEX.md: guided document directory.
- TheSingularityWorkshop.HeadlessAi.AnyAppExample.slnx: application and tests.
- src/: .NET console application and deterministic HTTP handler.
- tests/: fixture-backed integration contract tests.
- docs/usage.md: build/run instructions.
- docs/architecture.md: responsibility and data flow.
- docs/live-mode.md: external-provider opt-in and credential guidance.
- docs/testing.md: test scope and guarantees.
- .github/workflows/build.yml: build, test, coverage; no publishing.

HeadlessAi owns endpoint invocation contracts. This repository owns the demonstration host and integration tests.
