using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FLM_LobbyDisplay.Services;

namespace FLM_LobbyDisplay.Pages;

public class IndexModel : PageModel
{
    private readonly IConfiguration _config;
    private readonly IMssqlAuthService _auth;
    private readonly ILogger<IndexModel> _logger;

    public string AppTitle { get; private set; } = "Film Signage";
    public string ErrorMessage { get; private set; } = string.Empty;

    [BindProperty] public string Username { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;

    public IndexModel(IConfiguration config, IMssqlAuthService auth, ILogger<IndexModel> logger)
    {
        _config = config;
        _auth = auth;
        _logger = logger;
    }

    public void OnGet()
    {
        AppTitle = _config["AppSettings:title"] ?? "Film Signage";
        HttpContext.Session.Clear();
    }

    public IActionResult OnPost()
    {
        AppTitle = _config["AppSettings:title"] ?? "Film Signage";

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please enter User Id and Password.";
            return Page();
        }

        try
        {
            var systemName = _config["AppSettings:SystemName"] ?? string.Empty;

            var model = _auth.ValidateUser(Username.Trim(), systemName);

            if (model.VALID_USER &&
                Pbkdf2PasswordHasher.VerifyHashedPassword(model.PASSWORD, Password.Trim()))
            {
                HttpContext.Session.SetString("gstrUserID", model.USER_ID);
                HttpContext.Session.SetString("gettemp", model.EMP_NAME);
                HttpContext.Session.SetString("gstrUsername", model.LOGIN_ID);
                HttpContext.Session.SetString("system", systemName);
                HttpContext.Session.SetString("roleId", model.ID_ACL_ROLE.ToString());
                HttpContext.Session.SetString("LoginHis", DateTime.Now.ToString("dd MMMM yyyy"));
                return Redirect(Url.Content("~/Menu/Menu"));
            }
            else
            {
                ErrorMessage = "Invalid username and password.";
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login error for user {Username}", Username);
            ErrorMessage = "Login failed. Please try again.";
            return Page();
        }
    }
}
