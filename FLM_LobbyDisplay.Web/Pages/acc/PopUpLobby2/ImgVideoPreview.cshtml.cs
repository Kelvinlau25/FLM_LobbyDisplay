using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FLM_LobbyDisplay.Pages.acc.PopUpLobby2;

public class ImgVideoPreviewModel : PageModel
{
    public string VideoPath { get; private set; } = string.Empty;

    public void OnGet()
    {
        var id = Request.Query["id"].ToString();
        if (id.Length <= 5) return;

        var ext = id[^3..].ToLower()[..^1]; // last 3 chars minus last 1
        var file = id[1..^1]; // strip first and last char

        if (ext is "mp4" or "wmv")
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var scr = Request.Query["scr"].ToString();
            VideoPath = scr switch
            {
                "1" => $"{baseUrl}/acc/LobbyDisplay2/mainscr/{file}",
                "2" => $"{baseUrl}/acc/LobbyDisplay2/secscrtop/{file}",
                _   => $"{baseUrl}/acc/LobbyDisplay2/secscrbtm/{file}"
            };
        }
    }
}
