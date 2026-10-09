# Continuation brief

## Purpose

Maintain a trustworthy, runnable example of a host application integrating HeadlessAi.

## Current baseline

- .NET 8 console application.
- Default deterministic in-memory response; no provider call or key required.
- Optional live OpenAI Responses path gated by HEADLESSAI_LIVE=true.
- Fixture-backed tests for normalized output and HTTPS-by-default profile validation.
- CI checks out HeadlessAi development source and builds/tests this repository.
- Pull request #1 is open against master; do not merge without explicit approval.

## Boundaries

Do not move library behavior into this repository. Do not execute model output, add live network dependencies to tests, commit credentials, or publish packages from this project.

## Next work

1. Confirm the build/test/coverage workflow passes.
2. Keep usage docs aligned with current HeadlessAi contracts.
3. Add a second host shape only if it teaches a distinct integration lesson.
4. Add structured output only alongside explicit validation and a clear statement that schema conformance is not truth.
