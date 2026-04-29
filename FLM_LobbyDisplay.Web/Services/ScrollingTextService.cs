namespace FLM_LobbyDisplay.Services;

public class ScrollingTextService
{
    private readonly IWebHostEnvironment _env;

    public ScrollingTextService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> ReadAsync(string area)
    {
        var path = Path.Combine(_env.ContentRootPath, "acc", area, "scrollingtext.txt");
        if (!File.Exists(path)) return string.Empty;
        return await File.ReadAllTextAsync(path);
    }
}
