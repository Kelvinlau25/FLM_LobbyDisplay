using FLM_LobbyDisplay.Models;
using FLM_LobbyDisplay.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FLM_LobbyDisplay.Pages.acc.LobbyDisplay2;

public class Lobby2MainDisplayModel : PageModel
{
    private readonly PlaylistBuilderService _playlist;
    private readonly ScrollingTextService _scrolling;
    private readonly ILogger<Lobby2MainDisplayModel> _logger;

    public PlaylistData Data { get; private set; } = PlaylistData.Empty;
    public string ScrollingText { get; private set; } = string.Empty;
    public bool IsRefresh { get; private set; }

    public Lobby2MainDisplayModel(PlaylistBuilderService playlist, ScrollingTextService scrolling, ILogger<Lobby2MainDisplayModel> logger)
    {
        _playlist = playlist;
        _scrolling = scrolling;
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        IsRefresh = Request.Query.ContainsKey("refresh");
        Data = await _playlist.BuildAsync(7, "/acc/LobbyDisplay2/mainscr/");
        ScrollingText = await _scrolling.ReadAsync("MstMainLobby2");
        HttpContext.Session.SetString("Checkpoint", "1");
    }
}
