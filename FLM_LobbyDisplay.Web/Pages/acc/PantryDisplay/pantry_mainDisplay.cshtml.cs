using FLM_LobbyDisplay.Models;
using FLM_LobbyDisplay.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FLM_LobbyDisplay.Pages.acc.PantryDisplay;

public class PantryMainDisplayModel : PageModel
{
    private readonly PlaylistBuilderService _playlist;
    private readonly ScrollingTextService _scrolling;
    private readonly ILogger<PantryMainDisplayModel> _logger;

    public PlaylistData Data { get; private set; } = PlaylistData.Empty;
    public string ScrollingText { get; private set; } = string.Empty;
    public bool IsRefresh { get; private set; }

    public PantryMainDisplayModel(PlaylistBuilderService playlist, ScrollingTextService scrolling, ILogger<PantryMainDisplayModel> logger)
    {
        _playlist = playlist;
        _scrolling = scrolling;
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        IsRefresh = Request.Query.ContainsKey("refresh");
        Data = await _playlist.BuildAsync(4, "/acc/PantryDisplay/mainscr/");
        ScrollingText = await _scrolling.ReadAsync("MstMainPan");
        HttpContext.Session.SetString("Checkpoint", "1");
    }
}
