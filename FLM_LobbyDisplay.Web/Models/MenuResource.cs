namespace FLM_LobbyDisplay.Models;

public class MenuResource
{
    public int    ResourceID   { get; set; }
    public string ResourceName { get; set; } = string.Empty;
    public string ResourceURL  { get; set; } = string.Empty;
    public string ResourceDesc { get; set; } = string.Empty;
    public int    ParentID     { get; set; }
    public int    AppID        { get; set; }
}
