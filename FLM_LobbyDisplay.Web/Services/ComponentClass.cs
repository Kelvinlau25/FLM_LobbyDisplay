namespace FLM_LobbyDisplay.Services;

/// <summary>Utility helpers. Replaces Component_Class from App_Code.</summary>
public class ComponentClass
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ComponentClass(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Rectify_Date — normalises a date string based on whether the request is local or remote.
    /// </summary>
    /// <param name="value">Input date/datetime string.</param>
    /// <param name="format">Target date format; defaults to "dd/MM/yyyy".</param>
    /// <param name="spliter_format">Separator character(s) between day, month, year.</param>
    /// <param name="type">When empty, treats value as date-only; any other value includes time portion.</param>
    public string Rectify_Date(string value, string format = "dd/MM/yyyy", string spliter_format = "/", string type = "")
    {
        string d = value;
        try
        {
            bool isLocal = _httpContextAccessor.HttpContext?.Request.IsLocal() ?? false;

            if (string.IsNullOrEmpty(type))
            {
                d = isLocal
                    ? value.Substring(0, 2) + spliter_format + value.Substring(3, 2) + spliter_format + value.Substring(6, 4)
                    : value.Substring(3, 2) + spliter_format + value.Substring(0, 2) + spliter_format + value.Substring(6, 4);
            }
            else
            {
                d = isLocal
                    ? value.Substring(0, 2) + spliter_format + value.Substring(3, 2) + spliter_format + value.Substring(6)
                    : value.Substring(3, 2) + spliter_format + value.Substring(0, 2) + spliter_format + value.Substring(6);
            }

            if (format != "dd/MM/yyyy")
                d = DateTime.Parse(d).ToString(format);
        }
        catch
        {
            d = value;
        }
        return d;
    }

    public static string fn_convertMth(string mth) => mth switch
    {
        "JAN" => "01", "FEB" => "02", "MAR" => "03", "APR" => "04",
        "MAY" => "05", "JUN" => "06", "JUL" => "07", "AUG" => "08",
        "SEP" => "09", "OCT" => "10", "NOV" => "11", "DEC" => "12",
        _ => mth
    };
}

/// <summary>Extension method to check if a request is local. Replaces HttpContext.Current.Request.IsLocal.</summary>
public static class HttpRequestExtensions
{
    public static bool IsLocal(this HttpRequest request)
    {
        var connection = request.HttpContext.Connection;
        if (connection.RemoteIpAddress is null) return true;
        if (connection.LocalIpAddress is null)
            return System.Net.IPAddress.IsLoopback(connection.RemoteIpAddress);
        return connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
            || System.Net.IPAddress.IsLoopback(connection.RemoteIpAddress);
    }
}
