using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FLM_LobbyDisplay.Pages.acc.LobbyDisplay;

public class DisplayMstModel : PageModel
{
    public void OnGet() { }

    public IActionResult OnPostDisp1()
    {
        return Content("<script>window.open('lobby_mainDisplay', '_blank');</script>", "text/html");
    }

    public IActionResult OnPostDisp2()
    {
        return Content("<script>window.open('lobby_2ndDisplay', '_blank');</script>", "text/html");
    }
}
