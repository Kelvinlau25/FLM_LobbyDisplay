using FLM_LobbyDisplay.Services;
using Microsoft.AspNetCore.Mvc;

namespace FLM_LobbyDisplay.Api;

[ApiController]
[Route("api/playlist")]
public class PlaylistController : ControllerBase
{
    private readonly PlaylistBuilderService _playlist;

    // Screen ID to media path mapping
    private static readonly Dictionary<int, string> ScreenPaths = new()
    {
        { 1, "/acc/LobbyDisplay/mainscr/" },
        { 2, "/acc/LobbyDisplay/secscrtop/" },
        { 3, "/acc/LobbyDisplay/secscrbtm/" },
        { 4, "/acc/PantryDisplay/mainscr/" },
        { 5, "/acc/PantryDisplay/secscrtop/" },
        { 6, "/acc/PantryDisplay/secscrbtm/" },
        { 7, "/acc/LobbyDisplay2/mainscr/" },
        { 8, "/acc/LobbyDisplay2/secscrtop/" },
        { 9, "/acc/LobbyDisplay2/secscrbtm/" },
    };

    public PlaylistController(PlaylistBuilderService playlist)
    {
        _playlist = playlist;
    }

    [HttpGet("{screenId:int}")]
    public async Task<IActionResult> Get(int screenId)
    {
        if (!ScreenPaths.TryGetValue(screenId, out var mediaPath))
            return BadRequest($"Unknown screenId: {screenId}");

        var data = await _playlist.BuildAsync(screenId, mediaPath);
        return Ok(data);
    }
}
