# Testing

Tests use an in-memory `HttpMessageHandler` and a fixed Responses-shaped JSON fixture.

They verify:
- a configured agent uses the injected transport and returns normalized response text;
- the profile identity remains associated with the agent;
- HTTPS is required unless insecure HTTP is explicitly enabled.

These tests do not contact providers, require credentials, measure model quality, or prove provider uptime. The CI workflow restores, builds, runs tests with coverage, and uploads the report when Codecov credentials are configured. Tests are deterministic; performance measurement belongs in the companion benchmark repository.
