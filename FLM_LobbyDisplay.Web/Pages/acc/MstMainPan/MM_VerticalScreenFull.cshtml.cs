using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FLM_LobbyDisplay.Pages.acc.MstMainPan;

public class MMVerticalScreenFullModel : PageModel
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<MMVerticalScreenFullModel> _logger;

    public List<DataRow> Grid1Rows { get; private set; } = new();
    public List<DataRow> Grid2Rows { get; private set; } = new();
    public List<DataRow> Grid3Rows { get; private set; } = new();
    public string ScrollingText { get; private set; } = string.Empty;

    public MMVerticalScreenFullModel(IConfiguration config, IWebHostEnvironment env, ILogger<MMVerticalScreenFullModel> logger)
    {
        _config = config;
        _env = env;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("gstrUserID") == null)
            return Redirect(Url.Content("~/SessionExpired"));

        await LoadDataAsync();
        LoadScrollingText();
        return Page();
    }

    private async Task LoadDataAsync()
    {
        Grid1Rows = await QueryRowsAsync("SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID=4 ORDER BY ID_MM_VIDEOS DESC");
        Grid2Rows = await QueryRowsAsync("SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID=5 ORDER BY ID_MM_VIDEOS DESC");
        Grid3Rows = await QueryRowsAsync("SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID=6 ORDER BY ID_MM_VIDEOS DESC");
    }

    private async Task<List<DataRow>> QueryRowsAsync(string sql)
    {
        var dt = new DataTable();
        try
        {
            var connStr = _config.GetConnectionString("filmDisplay")!;
            await using var con = new SqlConnection(connStr);
            await con.OpenAsync();
            await using var cmd = new SqlCommand(sql, con);
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SQL query failed: {Message}", ex.Message);
        }
        return dt.Rows.Cast<DataRow>().ToList();
    }

    private void LoadScrollingText()
    {
        try
        {
            var path = Path.Combine(_env.ContentRootPath, "acc", "MstMainPan", "scrollingtext.txt");
            if (System.IO.File.Exists(path)) ScrollingText = System.IO.File.ReadAllText(path);
        }
        catch (Exception ex) { _logger.LogError(ex, "Failed to read scrollingtext.txt"); }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id, int scrId)
    {
        if (HttpContext.Session.GetString("gstrUserID") == null)
            return Redirect(Url.Content("~/SessionExpired"));

        var user = HttpContext.Session.GetString("gstrUserID") ?? "unknown";
        var loc = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var countRows = await QueryRowsAsync($"SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID={scrId}");
        if (countRows.Count < 2)
        {
            TempData["Alert"] = "Unable to Delete - Video are being used in Pantry Screen";
            return RedirectToPage();
        }

        try
        {
            var connStr = _config.GetConnectionString("filmDisplay")!;
            await using var con = new SqlConnection(connStr);
            await con.OpenAsync();
            var sql = $"UPDATE MM_VIDEOS SET UPDATED_by='{user}', UPDATED_Loc='{loc}', UPDATED_DATE=GETDATE(),RECORD_TYP='5' WHERE RECORD_TYP<>'5' AND ID_MM_VIDEOS={id}";
            await using var cmd = new SqlCommand(sql, con);
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delete failed: {Message}", ex.Message);
            TempData["Alert"] = "Unable to Delete - Error";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostSaveScrollingTextAsync(string footerText)
    {
        try
        {
            var path = Path.Combine(_env.ContentRootPath, "acc", "MstMainPan", "scrollingtext.txt");
            await System.IO.File.WriteAllTextAsync(path, footerText ?? string.Empty);
            TempData["Alert"] = "Text Saved.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save scrollingtext.txt");
            TempData["Alert"] = ex.Message;
        }
        return RedirectToPage();
    }
}
