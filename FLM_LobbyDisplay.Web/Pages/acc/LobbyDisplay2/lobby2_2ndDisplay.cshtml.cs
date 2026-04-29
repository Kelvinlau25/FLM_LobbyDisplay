using FLM_LobbyDisplay.Models;
using FLM_LobbyDisplay.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FLM_LobbyDisplay.Pages.acc.LobbyDisplay2;

public class Lobby22ndDisplayModel : PageModel
{
    private readonly PlaylistBuilderService _playlist;
    private readonly ScrollingTextService _scrolling;
    private readonly ILogger<Lobby22ndDisplayModel> _logger;

    public PlaylistData DataTop { get; private set; } = PlaylistData.Empty;
    public PlaylistData DataBottom { get; private set; } = PlaylistData.Empty;
    public string ScrollingText { get; private set; } = string.Empty;
    public bool IsRefresh { get; private set; }

    public Lobby22ndDisplayModel(PlaylistBuilderService playlist, ScrollingTextService scrolling, ILogger<Lobby22ndDisplayModel> logger)
    {
        _playlist = playlist;
        _scrolling = scrolling;
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        IsRefresh = Request.Query.ContainsKey("refresh");
        DataTop = await _playlist.BuildAsync(8, "/acc/LobbyDisplay2/secscrtop/");
        DataBottom = await _playlist.BuildAsync(9, "/acc/LobbyDisplay2/secscrbtm/");
        ScrollingText = await _scrolling.ReadAsync("MstMainLobby2");
        HttpContext.Session.SetString("Checkpoint", "1");
    }
}
