using Microsoft.Data.SqlClient;
using System.Data;
using FLM_LobbyDisplay.Models;

namespace FLM_LobbyDisplay.Services;

public class PlaylistBuilderService
{
    private readonly IConfiguration _config;
    private readonly ILogger<PlaylistBuilderService> _logger;

    public PlaylistBuilderService(IConfiguration config, ILogger<PlaylistBuilderService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<PlaylistData> BuildAsync(int screenId, string mediaPath)
    {
        var sql = $"SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID={screenId}";
        var dt = new DataTable();

        try
        {
            var connStr = _config.GetConnectionString("filmDisplay")
                ?? throw new InvalidOperationException("filmDisplay connection string not configured.");

            await using var con = new SqlConnection(connStr);
            await con.OpenAsync();
            await using var cmd = new SqlCommand(sql, con);
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SQL query failed on connection {ConnectionName}: {Message}", "filmDisplay", ex.Message);
            return PlaylistData.Empty;
        }

        return BuildPlaylistData(dt, mediaPath);
    }

    public static PlaylistData BuildPlaylistData(DataTable dt, string mediaPath)
    {
        if (dt.Rows.Count == 0)
            return PlaylistData.Empty;

        var videoList = new List<string>();
        var seekStarts = new List<string>();
        var seekEnds = new List<string>();
        var periodStarts = new List<string>();
        var periodEnds = new List<string>();

        foreach (DataRow row in dt.Rows)
        {
            videoList.Add($"'{mediaPath}{row["ATTACH_FILE"]}'");
            seekStarts.Add($"'{row["SEEK_START"]}'");
            seekEnds.Add($"'{row["SEEK_END"]}'");
            periodStarts.Add($"'{row["PERIOD_START"]}'");
            periodEnds.Add($"'{row["PERIOD_END"]}'");
        }

        var vl = $"[{string.Join(",", videoList)}]";
        var ss = $"[{string.Join(",", seekStarts)}]";
        var se = $"[{string.Join(",", seekEnds)}]";
        var ps = $"[{string.Join(",", periodStarts)}]";
        var pe = $"[{string.Join(",", periodEnds)}]";

        return new PlaylistData
        {
            VideoLists = vl,
            SeekStarts = ss,
            SeekEnds = se,
            PeriodStarts = ps,
            PeriodEnds = pe,
            VideoListsCsv = ToCsv(vl),
            SeekStartsCsv = ToCsv(ss),
            SeekEndsCsv = ToCsv(se),
            PeriodStartsCsv = ToCsv(ps),
            PeriodEndsCsv = ToCsv(pe)
        };
    }

    private static string ToCsv(string arrayLiteral)
    {
        if (arrayLiteral == "[]") return string.Empty;
        // Strip the surrounding [ and ] — the JS loaded() function receives
        // the inner comma-separated quoted values e.g. 'path/a.mp4','path/b.mp4'
        return arrayLiteral.TrimStart('[').TrimEnd(']');
    }
}
