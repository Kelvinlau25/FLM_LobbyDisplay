using FLM_LobbyDisplay.Models;
using FLM_LobbyDisplay.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FLM_LobbyDisplay.Pages.acc.LobbyDisplay;

public class LobbyMainDisplayModel : PageModel
{
    private readonly PlaylistBuilderService _playlist;
    private readonly ScrollingTextService _scrolling;
    private readonly ILogger<LobbyMainDisplayModel> _logger;

    public PlaylistData Data { get; private set; } = PlaylistData.Empty;
    public string ScrollingText { get; private set; } = string.Empty;
    public bool IsRefresh { get; private set; }

    public LobbyMainDisplayModel(PlaylistBuilderService playlist, ScrollingTextService scrolling, ILogger<LobbyMainDisplayModel> logger)
    {
        _playlist = playlist;
        _scrolling = scrolling;
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        IsRefresh = Request.Query.ContainsKey("refresh");
        Data = await _playlist.BuildAsync(1, "/acc/LobbyDisplay/mainscr/");
        ScrollingText = await _scrolling.ReadAsync("MstMain");
        HttpContext.Session.SetString("Checkpoint", "1");
    }
}
