using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FLM_LobbyDisplay.Pages.acc.MstMainPan;

public class Upper2ndScreenDtlModel : PageModel
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<Upper2ndScreenDtlModel> _logger;

    public int Action { get; private set; }
    public string MainTitle { get; private set; } = string.Empty;
    public DataRow? VideoRow { get; private set; }

    [BindProperty] public IFormFile? FileUpload { get; set; }
    [BindProperty] public string SeekStart { get; set; } = "0";
    [BindProperty] public string SeekEnd { get; set; } = "0";
    [BindProperty] public string PeriodStart { get; set; } = "00:00";
    [BindProperty] public string PeriodEnd { get; set; } = "23:59";

    public Upper2ndScreenDtlModel(IConfiguration config, IWebHostEnvironment env, ILogger<Upper2ndScreenDtlModel> logger)
    {
        _config = config;
        _env = env;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("gstrUserID") == null)
            return Redirect(Url.Content("~/SessionExpired"));

        Action = int.TryParse(Request.Query["action"], out var a) ? a : 0;

        if (Action == 3) // Edit
        {
            MainTitle = "Master Maintenance Second Screen Upper Video - Pantry - Edit";
            var id = Request.Query["id"].ToString();
            var dt = await QueryAsync($"SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND ID_MM_VIDEOS={id}");
            if (dt.Rows.Count > 0) VideoRow = dt.Rows[0];
        }
        else
        {
            MainTitle = "Master Maintenance Second Screen Upper Video - Pantry - Add";
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("gstrUserID") == null)
            return Redirect(Url.Content("~/SessionExpired"));

        Action = int.TryParse(Request.Query["action"], out var a) ? a : 0;
        var user = HttpContext.Session.GetString("gstrUserID") ?? "unknown";
        var loc = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var startTime = string.IsNullOrEmpty(PeriodStart) ? "00:00:00" : PeriodStart + ":00";
        var endTime = string.IsNullOrEmpty(PeriodEnd) ? "23:59:59" : PeriodEnd + ":59";

        try
        {
            var connStr = _config.GetConnectionString("filmDisplay")!;

            if (Action == 1) // Add
            {
                if (FileUpload == null || FileUpload.Length == 0)
                { TempData["Alert"] = "Please Select Video"; return Page(); }

                var ext = Path.GetExtension(FileUpload.FileName).ToLower();
                if (ext != ".mp4") { TempData["Alert"] = "Upload Only .mp4 Video"; return Page(); }

                var name = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(FileUpload.FileName).ToLower();
                var savePath = Path.Combine(_env.WebRootPath, "acc", "PantryDisplay", "secscrtop", name);
                Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
                await using var stream = new FileStream(savePath, FileMode.Create);
                await FileUpload.CopyToAsync(stream);

                var sql = $"INSERT INTO MM_VIDEOS (ATTACH_FILE,SEEK_START,SEEK_END,PERIOD_START,PERIOD_END,SCR_ID,RECORD_TYP,CREATED_BY,CREATED_DATE,CREATED_LOC,UPDATED_BY,UPDATED_DATE,UPDATED_LOC) VALUES ('{name}','{SeekStart}','{SeekEnd}','{startTime}','{endTime}','5','1','{user}',GETDATE(),'{loc}','{user}',GETDATE(),'{loc}')";
                await using var con = new SqlConnection(connStr);
                await con.OpenAsync();
                await using var cmd = new SqlCommand(sql, con);
                await cmd.ExecuteNonQueryAsync();
                TempData["Alert"] = "Video Successfully Added.";
                return Redirect(Url.Content("~/acc/MstMainPan/MM_VerticalScreenFull"));
            }
            else if (Action == 3) // Edit
            {
                var id = Request.Query["id"].ToString();
                var sql = $"UPDATE MM_VIDEOS SET SEEK_START='{SeekStart}',SEEK_END='{SeekEnd}',PERIOD_START='{startTime}',PERIOD_END='{endTime}',RECORD_TYP='3',UPDATED_BY='{user}',UPDATED_DATE=GETDATE(),UPDATED_LOC='{loc}' WHERE ID_MM_VIDEOS='{id}'";
                await using var con = new SqlConnection(connStr);
                await con.OpenAsync();
                await using var cmd = new SqlCommand(sql, con);
                await cmd.ExecuteNonQueryAsync();
                TempData["Alert"] = "Edits Successfully Updated.";
                return Redirect(Url.Content("~/acc/MstMainPan/MM_VerticalScreenFull"));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Submit failed: {Message}", ex.Message);
            TempData["Alert"] = ex.Message;
        }
        return Page();
    }

    private async Task<DataTable> QueryAsync(string sql)
    {
        var dt = new DataTable();
        try
        {
            await using var con = new SqlConnection(_config.GetConnectionString("filmDisplay")!);
            await con.OpenAsync();
            await using var cmd = new SqlCommand(sql, con);
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
        }
        catch (Exception ex) { _logger.LogError(ex, "Query failed: {Message}", ex.Message); }
        return dt;
    }
}
