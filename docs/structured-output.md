# Structured output: parse proposals, not permissions

HeadlessAi returns normalized text. A provider may be instructed to format that text as JSON, but JSON syntax does not make the content trustworthy, safe, or true. The host must parse it, validate the shape and values it accepts, and decide what—if anything—to do with it.

This guide is a host-side pattern. It does **not** claim that every provider enforces a JSON schema or that the current example application automatically executes structured output.

## Example contract

Suppose a task asks the model to suggest one of two labels:

```json
{
  "label": "summarize",
  "reason": "The request asks for a concise overview."
}
```

Parse the returned `HeadlessAiOutput.Content` with the standard JSON APIs and enforce the host's own constraints:

```csharp
using System.Text.Json;

public sealed record LabelProposal(string Label, string Reason);

public static bool TryReadLabelProposal(string content, out LabelProposal? proposal)
{
    proposal = null;

    try
    {
        using var document = JsonDocument.Parse(content);
        var root = document.RootElement;

        if (root.ValueKind != JsonValueKind.Object ||
            !root.TryGetProperty("label", out var labelElement) ||
            labelElement.ValueKind != JsonValueKind.String ||
            !root.TryGetProperty("reason", out var reasonElement) ||
            reasonElement.ValueKind != JsonValueKind.String)
        {
            return false;
        }

        var label = labelElement.GetString();
        var reason = reasonElement.GetString();

        // This host accepts only this explicit, finite set.
        if (label is not ("summarize" or "classify") ||
            string.IsNullOrWhiteSpace(reason) ||
            reason.Length > 500)
        {
            return false;
        }

        proposal = new LabelProposal(label, reason);
        return true;
    }
    catch (JsonException)
    {
        return false;
    }
}
```

## What this does—and does not—prove

- Parsing proves only that the response is syntactically valid JSON.
- Shape and allow-list checks prove only that the value fits this small host-defined contract.
- They do not prove the model's explanation is correct or that the label is appropriate for the user's request.
- Unknown labels, missing fields, wrong JSON types, oversized reasons, and malformed JSON are rejected by this example.
- No command, tool, file operation, permission change, or external action is performed by parsing the proposal.

If a proposal later influences consequential behavior, add task-specific validation, authorization, and—where appropriate—human approval at the point of use. Never treat model output as a permission grant. Keep provider-native structured-output features behind explicit capability checks; a prompt asking for JSON is not the same as a provider-enforced schema.

## Further reading

- [HeadlessAi usage and adapter contracts](https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi/blob/development/docs/usage.md)
- [HeadlessAi security boundary](https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi/blob/development/docs/security.md)
- [Example testing guide](testing.md)
