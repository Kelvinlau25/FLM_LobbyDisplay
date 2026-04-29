using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FLM_LobbyDisplay.Pages.acc.LobbyDisplay2;

public class Display2MstModel : PageModel
{
    public void OnGet() { }

    public IActionResult OnPostDisp1()
    {
        return Content("<script>window.open('lobby2_mainDisplay', '_blank');</script>", "text/html");
    }

    public IActionResult OnPostDisp2()
    {
        return Content("<script>window.open('lobby2_2ndDisplay', '_blank');</script>", "text/html");
    }
}
