using FLM_LobbyDisplay.Models;
using FLM_LobbyDisplay.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FLM_LobbyDisplay.Pages.acc.LobbyDisplay;

public class Lobby2ndDisplayModel : PageModel
{
    private readonly PlaylistBuilderService _playlist;
    private readonly ScrollingTextService _scrolling;
    private readonly ILogger<Lobby2ndDisplayModel> _logger;

    public PlaylistData DataTop { get; private set; } = PlaylistData.Empty;
    public PlaylistData DataBottom { get; private set; } = PlaylistData.Empty;
    public string ScrollingText { get; private set; } = string.Empty;
    public bool IsRefresh { get; private set; }

    public Lobby2ndDisplayModel(PlaylistBuilderService playlist, ScrollingTextService scrolling, ILogger<Lobby2ndDisplayModel> logger)
    {
        _playlist = playlist;
        _scrolling = scrolling;
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        IsRefresh = Request.Query.ContainsKey("refresh");
        DataTop = await _playlist.BuildAsync(2, "/acc/LobbyDisplay/secscrtop/");
        DataBottom = await _playlist.BuildAsync(3, "/acc/LobbyDisplay/secscrbtm/");
        ScrollingText = await _scrolling.ReadAsync("MstMain");
        HttpContext.Session.SetString("Checkpoint", "1");
    }
}
