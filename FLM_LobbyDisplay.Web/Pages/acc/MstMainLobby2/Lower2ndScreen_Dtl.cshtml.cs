using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FLM_LobbyDisplay.Pages.acc.MstMainLobby2;

public class Lower2ndScreenDtlModel : PageModel
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<Lower2ndScreenDtlModel> _logger;

    public int Action { get; private set; }
    public string MainTitle { get; private set; } = string.Empty;
    public DataRow? VideoRow { get; private set; }

    [BindProperty] public IFormFile? FileUpload { get; set; }
    [BindProperty] public string SeekStart { get; set; } = "0";
    [BindProperty] public string SeekEnd { get; set; } = "0";
    [BindProperty] public string PeriodStart { get; set; } = "00:00";
    [BindProperty] public string PeriodEnd { get; set; } = "23:59";

    public Lower2ndScreenDtlModel(IConfiguration config, IWebHostEnvironment env, ILogger<Lower2ndScreenDtlModel> logger)
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
            MainTitle = "Master Maintenance Second Screen Lower Video - TV 2 - Edit";

            if (!int.TryParse(Request.Query["id"], out var id))
            {
                TempData["Alert"] = "Invalid video ID.";
                return Page();
            }

            var dt = await QueryAsync(
                "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND ID_MM_VIDEOS=@id",
                new SqlParameter("@id", SqlDbType.Int) { Value = id });

            if (dt.Rows.Count > 0)
            {
                VideoRow = dt.Rows[0];
                SeekStart = VideoRow["SEEK_START"]?.ToString() ?? "0";
                SeekEnd = VideoRow["SEEK_END"]?.ToString() ?? "0";
                var ps = VideoRow["PERIOD_START"]?.ToString() ?? "";
                PeriodStart = ps.Length >= 5 ? ps[..5] : "00:00";
                var pe = VideoRow["PERIOD_END"]?.ToString() ?? "";
                PeriodEnd = pe.Length >= 5 ? pe[..5] : "23:59";
            }
        }
        else
        {
            MainTitle = "Master Maintenance Second Screen Lower Video - TV 2 - Add";
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

        if (!decimal.TryParse(SeekStart, out var seekStartVal)) seekStartVal = 0;
        if (!decimal.TryParse(SeekEnd, out var seekEndVal)) seekEndVal = 0;

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
                var savePath = Path.Combine(_env.WebRootPath, "acc", "LobbyDisplay2", "secscrbtm", name);
                Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
                await using var stream = new FileStream(savePath, FileMode.Create);
                await FileUpload.CopyToAsync(stream);

                var sql = @"INSERT INTO MM_VIDEOS (ATTACH_FILE,SEEK_START,SEEK_END,PERIOD_START,PERIOD_END,SCR_ID,RECORD_TYP,CREATED_BY,CREATED_DATE,CREATED_LOC,UPDATED_BY,UPDATED_DATE,UPDATED_LOC)
                            VALUES (@file,@seekStart,@seekEnd,@periodStart,@periodEnd,@scrId,@recTyp,@user,GETDATE(),@loc,@user2,GETDATE(),@loc2)";
                await using var con = new SqlConnection(connStr);
                await con.OpenAsync();
                await using var cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@file", SqlDbType.VarChar) { Value = name });
                cmd.Parameters.Add(new SqlParameter("@seekStart", SqlDbType.Decimal) { Value = seekStartVal });
                cmd.Parameters.Add(new SqlParameter("@seekEnd", SqlDbType.Decimal) { Value = seekEndVal });
                cmd.Parameters.Add(new SqlParameter("@periodStart", SqlDbType.VarChar) { Value = startTime });
                cmd.Parameters.Add(new SqlParameter("@periodEnd", SqlDbType.VarChar) { Value = endTime });
                cmd.Parameters.Add(new SqlParameter("@scrId", SqlDbType.VarChar) { Value = "9" });
                cmd.Parameters.Add(new SqlParameter("@recTyp", SqlDbType.VarChar) { Value = "1" });
                cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar) { Value = user });
                cmd.Parameters.Add(new SqlParameter("@loc", SqlDbType.VarChar) { Value = loc });
                cmd.Parameters.Add(new SqlParameter("@user2", SqlDbType.VarChar) { Value = user });
                cmd.Parameters.Add(new SqlParameter("@loc2", SqlDbType.VarChar) { Value = loc });
                await cmd.ExecuteNonQueryAsync();
                TempData["Alert"] = "Video Successfully Added.";
                return Redirect(Url.Content("~/acc/MstMainLobby2/MM_VerticalScreenFull"));
            }
            else if (Action == 3) // Edit
            {
                if (!int.TryParse(Request.Query["id"], out var id))
                {
                    TempData["Alert"] = "Invalid video ID.";
                    return Page();
                }

                var sql = @"UPDATE MM_VIDEOS SET SEEK_START=@seekStart,SEEK_END=@seekEnd,PERIOD_START=@periodStart,PERIOD_END=@periodEnd,
                            RECORD_TYP=@recTyp,UPDATED_BY=@user,UPDATED_DATE=GETDATE(),UPDATED_LOC=@loc
                            WHERE ID_MM_VIDEOS=@id";
                await using var con = new SqlConnection(connStr);
                await con.OpenAsync();
                await using var cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@seekStart", SqlDbType.Decimal) { Value = seekStartVal });
                cmd.Parameters.Add(new SqlParameter("@seekEnd", SqlDbType.Decimal) { Value = seekEndVal });
                cmd.Parameters.Add(new SqlParameter("@periodStart", SqlDbType.VarChar) { Value = startTime });
                cmd.Parameters.Add(new SqlParameter("@periodEnd", SqlDbType.VarChar) { Value = endTime });
                cmd.Parameters.Add(new SqlParameter("@recTyp", SqlDbType.VarChar) { Value = "3" });
                cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar) { Value = user });
                cmd.Parameters.Add(new SqlParameter("@loc", SqlDbType.VarChar) { Value = loc });
                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });
                await cmd.ExecuteNonQueryAsync();
                TempData["Alert"] = "Edits Successfully Updated.";
                return Redirect(Url.Content("~/acc/MstMainLobby2/MM_VerticalScreenFull"));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Submit failed: {Message}", ex.Message);
            TempData["Alert"] = ex.Message;
        }
        return Page();
    }

    private async Task<DataTable> QueryAsync(string sql, params SqlParameter[] parameters)
    {
        var dt = new DataTable();
        try
        {
            await using var con = new SqlConnection(_config.GetConnectionString("filmDisplay")!);
            await con.OpenAsync();
            await using var cmd = new SqlCommand(sql, con);
            if (parameters.Length > 0) cmd.Parameters.AddRange(parameters);
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
        }
        catch (Exception ex) { _logger.LogError(ex, "Query failed: {Message}", ex.Message); }
        return dt;
    }
}
