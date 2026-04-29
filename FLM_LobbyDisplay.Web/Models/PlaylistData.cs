namespace FLM_LobbyDisplay.Models;

public class PlaylistData
{
    public string VideoLists { get; init; } = "[]";
    public string SeekStarts { get; init; } = "[]";
    public string SeekEnds { get; init; } = "[]";
    public string PeriodStarts { get; init; } = "[]";
    public string PeriodEnds { get; init; } = "[]";

    public string VideoListsCsv { get; init; } = string.Empty;
    public string SeekStartsCsv { get; init; } = string.Empty;
    public string SeekEndsCsv { get; init; } = string.Empty;
    public string PeriodStartsCsv { get; init; } = string.Empty;
    public string PeriodEndsCsv { get; init; } = string.Empty;

    public static PlaylistData Empty => new();
}
