using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FLM_LobbyDisplay.Services;

namespace FLM_LobbyDisplay.Pages;

public class ChangePasswordModel : PageModel
{
    private readonly IMssqlAuthService _auth;
    private readonly ILogger<ChangePasswordModel> _logger;

    [BindProperty] public string OldPassword { get; set; } = string.Empty;
    [BindProperty] public string NewPassword { get; set; } = string.Empty;
    [BindProperty] public string ConfirmPassword { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }

    public ChangePasswordModel(IMssqlAuthService auth, ILogger<ChangePasswordModel> logger)
    {
        _auth = auth;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("gstrUsername") == null)
            return Redirect(Url.Content("~/SessionExpired"));

        return Page();
    }

    public IActionResult OnPost()
    {
        if (HttpContext.Session.GetString("gstrUsername") == null)
            return Redirect(Url.Content("~/SessionExpired"));

        if (string.IsNullOrWhiteSpace(OldPassword) ||
            string.IsNullOrWhiteSpace(NewPassword) ||
            string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            Message = "All fields are required.";
            return Page();
        }

        if (NewPassword != ConfirmPassword)
        {
            Message = "New password and confirm password do not match.";
            return Page();
        }

        try
        {
            var loginId = HttpContext.Session.GetString("gstrUsername")!;
            var result = _auth.ChangePassword(loginId, OldPassword, NewPassword);

            if (result)
            {
                IsSuccess = true;
                Message = "Password is successfully changed. Please relogin again.";
            }
            else
            {
                Message = "Incorrect old password. Please try again.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Change password failed");
            Message = "Failed change your password. Please try again.";
        }

        return Page();
    }
}
