namespace FLM_LobbyDisplay.Models;

public record VideoEntry(
    string AttachFile,
    string SeekStart,
    string SeekEnd,
    string PeriodStart,
    string PeriodEnd
);
