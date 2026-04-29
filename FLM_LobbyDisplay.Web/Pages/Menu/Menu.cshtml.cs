using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using FLM_LobbyDisplay.Services;
using FLM_LobbyDisplay.Models;

namespace FLM_LobbyDisplay.Pages.Menu;

public class MenuModel : PageModel
{
    private readonly IConfiguration _config;
    private readonly IMssqlAuthService _auth;
    private readonly ILogger<MenuModel> _logger;

    public string AppTitle { get; private set; } = string.Empty;
    public string Words { get; private set; } = string.Empty;
    public string SignOutURL { get; private set; } = "/SessionExpired";
    public string HomeURL { get; private set; } = "/";
    public string MenuItemsHtml { get; private set; } = string.Empty;
    public string MenuListJson { get; private set; } = "[]";
    public string IframeSrc { get; private set; } = string.Empty;
    public string UserDisplayName { get; private set; } = string.Empty;
    public string LoginDate { get; private set; } = string.Empty;

    public MenuModel(IConfiguration config, IMssqlAuthService auth, ILogger<MenuModel> logger)
    {
        _config = config;
        _auth = auth;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        // Session guard — matches original redirect to SessionExpired
        if (HttpContext.Session.GetString("gstrUserID") == null)
            return Redirect(Url.Content("~/SessionExpired"));

        AppTitle = _config["AppSettings:title"] ?? "Film Signage";
        SignOutURL = Url.Content("~/SessionExpired");
        HomeURL = Url.Content("~/");

        // iframe src from query string (matches original Page_Init)
        if (Request.Query.ContainsKey("id"))
            IframeSrc = $"../acc/Entry/{Request.Query["page"]}{Request.QueryString}";

        // Greeting
        var hour = DateTime.Now.Hour;
        Words = hour < 12 ? "Good Morning" : hour <= 17 ? "Good Afternoon" : "Good Evening";

        UserDisplayName = HttpContext.Session.GetString("gettemp") ?? string.Empty;
        LoginDate = HttpContext.Session.GetString("LoginHis") ?? DateTime.Now.ToString("dd MMMM yyyy");

        // Build menu from ACL (stub returns empty list — real menu when ACL source available)
        BuildMenu();

        return Page();
    }

    private void BuildMenu()
    {
        try
        {
            var roleIdStr = HttpContext.Session.GetString("roleId") ?? "0";
            var roleId = int.TryParse(roleIdStr, out var rid) ? rid : 0;
            var systemName = _config["AppSettings:SystemName"] ?? string.Empty;

            var resources = _auth.GetMenuResources(roleId, systemName);

            if (resources.Count > 0)
            {
                BuildMenuFromResources(resources);
                return;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MSSQL menu build failed, falling back to static menu");
        }

        // Fallback: static menu
        BuildStaticMenu();
    }

    private void BuildMenuFromResources(List<MenuResource> resources)
    {
        var menuListSb = new StringBuilder();
        var itemsHtmlSb = new StringBuilder();
        int counter = 0;

        // Group by ParentID: parent groups are resources whose children exist in the list.
        // Identify parents: resources whose ResourceID appears as another resource's ParentID.
        var parentIds = new HashSet<int>(resources.Where(r => resources.Any(c => c.ParentID == r.ResourceID))
                                                   .Select(r => r.ResourceID));

        // If no hierarchy detected (all flat), treat items with empty URL as parents
        if (parentIds.Count == 0)
        {
            parentIds = new HashSet<int>(resources.Where(r => string.IsNullOrEmpty(r.ResourceURL))
                                                   .Select(r => r.ResourceID));
        }

        var parents = resources.Where(r => parentIds.Contains(r.ResourceID)).ToList();
        var children = resources.Where(r => !parentIds.Contains(r.ResourceID)).ToList();

        foreach (var parent in parents)
        {
            var childItems = children.Where(c => c.ParentID == parent.ResourceID).ToList();

            if (counter > 0) menuListSb.Append(",");
            menuListSb.Append($"{{contentEl:'left_menu_{counter}',title:'{EscapeJs(parent.ResourceDesc.Trim())}',autoHeight:true}}");

            var liSb = new StringBuilder();
            for (int j = 0; j < childItems.Count; j++)
            {
                var child = childItems[j];
                var cssClass = (j + 1) % 2 == 0 ? "alt" : "nor";

                // Resolve URL: use DB value if available, otherwise map by resource name + parent context
                var url = ResolveResourceUrl(child, parent);
                liSb.Append($"<li class='{cssClass}'><a target='page' href='{System.Web.HttpUtility.HtmlEncode(url)}'>{System.Web.HttpUtility.HtmlEncode(child.ResourceDesc.Trim())}</a></li>");
            }
            itemsHtmlSb.Append($"<div class='bar_itms' id='left_menu_{counter}'><ul>{liSb}</ul></div>");
            counter++;
        }

        MenuListJson = $"[{menuListSb}]";
        MenuItemsHtml = itemsHtmlSb.ToString();
    }

    /// <summary>
    /// Resolves the URL for a menu resource. If the DB has a controller/view URL, use it.
    /// Otherwise, map by resource name and parent context for the Film Display system.
    /// </summary>
    private string ResolveResourceUrl(MenuResource child, MenuResource parent)
    {
        // If the DB provides a real URL (controller/view), use it
        if (!string.IsNullOrEmpty(child.ResourceURL))
            return Url.Content(child.ResourceURL.Replace(".aspx", ""));

        // Film Display system: map by parent resource ID and child resource name
        // Parent groups map to display areas; child items map to specific pages
        var parentName = parent.ResourceName.Trim().ToLower();
        var childName = child.ResourceName.Trim().ToLower();

        // Determine the display area from the parent
        string displayArea;
        string masterArea;
        if (parentName.Contains("pantry"))
        {
            displayArea = "PantryDisplay";
            masterArea = "MstMainPan";
        }
        else if (parentName.Contains("tv2") || parentName.Contains("lobby2") || parentName.Contains("room - tv2"))
        {
            displayArea = "LobbyDisplay2";
            masterArea = "MstMainLobby2";
        }
        else // TV1 / default
        {
            displayArea = "LobbyDisplay";
            masterArea = "MstMain";
        }

        // Map child resource name to page
        if (childName.Contains("display master") || childName.Contains("display control"))
        {
            var pageName = displayArea == "LobbyDisplay2" ? "Display2_Mst" : "Display_Mst";
            return Url.Content($"~/acc/{displayArea}/{pageName}");
        }
        if (childName.Contains("content maintenance") || childName.Contains("main screen"))
        {
            return Url.Content($"~/acc/{masterArea}/MM_VerticalScreenFull");
        }

        // Fallback: no URL
        _logger.LogWarning("No URL mapping for resource '{ResourceName}' under parent '{ParentName}'",
            child.ResourceName, parent.ResourceName);
        return "#";
    }

    private void BuildStaticMenu()
    {
        // Static menu matching the known pages of this Film Signage Display system
        var sections = new[]
        {
            new {
                Title = "Lobby Display",
                Items = new[] {
                    new { Label = "Display Control",    Url = Url.Content("~/acc/LobbyDisplay/Display_Mst") },
                    new { Label = "Main Screen Video",  Url = Url.Content("~/acc/MstMain/MM_VerticalScreenFull") },
                }
            },
            new {
                Title = "Lobby Display 2",
                Items = new[] {
                    new { Label = "Display Control",    Url = Url.Content("~/acc/LobbyDisplay2/Display2_Mst") },
                    new { Label = "Main Screen Video",  Url = Url.Content("~/acc/MstMainLobby2/MM_VerticalScreenFull") },
                }
            },
            new {
                Title = "Pantry Display",
                Items = new[] {
                    new { Label = "Display Control",    Url = Url.Content("~/acc/PantryDisplay/Display_Mst") },
                    new { Label = "Main Screen Video",  Url = Url.Content("~/acc/MstMainPan/MM_VerticalScreenFull") },
                }
            },
        };

        var menuListSb = new StringBuilder();
        var itemsHtmlSb = new StringBuilder();

        for (int i = 0; i < sections.Length; i++)
        {
            var section = sections[i];
            if (i > 0) menuListSb.Append(",");
            menuListSb.Append($"{{contentEl:'left_menu_{i}',title:'{EscapeJs(section.Title)}',autoHeight:true}}");
            var liSb = new StringBuilder();
            for (int j = 0; j < section.Items.Length; j++)
            {
                var item = section.Items[j];
                var cssClass = (j + 1) % 2 == 0 ? "alt" : "nor";
                liSb.Append($"<li class='{cssClass}'><a target='page' href='{item.Url}'>{System.Web.HttpUtility.HtmlEncode(item.Label)}</a></li>");
            }
            itemsHtmlSb.Append($"<div class='bar_itms' id='left_menu_{i}'><ul>{liSb}</ul></div>");
        }

        MenuListJson = $"[{menuListSb}]";
        MenuItemsHtml = itemsHtmlSb.ToString();
    }

    private static string EscapeJs(string s) =>
        s.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"");
}
