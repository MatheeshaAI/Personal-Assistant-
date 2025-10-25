using AlfredGPT.Common;

namespace AlfredGPT.Chat;

public record ChatContextHistory(
    HumanizedDate Date,
    IReadOnlyList<ChatContext> Contexts
);