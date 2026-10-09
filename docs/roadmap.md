# Roadmap

The goal is a small but complete teaching example, not a second implementation of HeadlessAi.

## Initial baseline
- [x] Deterministic local demonstration without credentials or external network.
- [x] Optional explicitly enabled live endpoint path.
- [x] Separate test project with fixture-backed contract checks.
- [x] Guided docs for use, boundaries, live mode, and testing.
- [x] CI build/test/coverage workflow with package publishing absent.

## Next improvements
- [ ] Add a second host shape (for example, a worker service) only when it clarifies a distinct integration lesson.
- [ ] Add a request-time credential provider example once the public contract is stable.
- [x] Add a safe structured-output host example with explicit parsing, field validation, and no automatic execution; see [Structured output](structured-output.md).
- [x] Keep every documented command verified by CI; the deterministic example command is executed by the build workflow.
- [x] Add an opt-in interactive prompt loop for a no-credential local simulation, with an explicitly separate live-provider mode.
