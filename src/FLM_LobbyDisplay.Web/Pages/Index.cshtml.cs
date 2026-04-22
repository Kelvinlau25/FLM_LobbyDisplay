using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using FLM_LobbyDisplay.Web.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FLM_LobbyDisplay.Web.Pages;

/// <summary>
/// Login page. Replaces <c>Default.aspx</c> / <c>Default.aspx.cs</c>.
/// </summary>
[AllowAnonymous]
public class IndexModel : BasePageModel
{
    private readonly IUserAuthenticator _authenticator;
    private readonly AppSettings _appSettings;

    public IndexModel(IUserAuthenticator authenticator, AppSettings appSettings)
    {
        _authenticator = authenticator;
        _appSettings = appSettings;
    }

    [BindProperty]
    public LoginInput? Input { get; set; }

    public bool ShowCompanySelection => _appSettings.CrossCompany == "1";

    public IList<SelectListItem> Companies { get; private set; } = new List<SelectListItem>();

    public string? LoginError { get; private set; }

    public async Task<IActionResult> OnGetAsync(string? userid, string? returnUrl)
    {
        // Mirror legacy behaviour: a fresh GET clears any existing session.
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();

        Input = new LoginInput { Username = userid ?? string.Empty };
        ViewData["ReturnUrl"] = returnUrl;
        await PopulateCompaniesAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl)
    {
        await PopulateCompaniesAsync();

        if (!ModelState.IsValid || Input is null)
        {
            LoginError = "Username and password are required.";
            return Page();
        }

        // Resolve the system id by name from the ACL store, mirroring the
        // legacy systemCheck() helper.
        var systemId = await _authenticator.ResolveSystemIdAsync(_appSettings.SystemName, HttpContext.RequestAborted);
        HttpContext.Session.SetInt32("system", systemId);

        if (systemId == 0)
        {
            LoginError = "Invalid System.";
            return Page();
        }

        var result = await _authenticator.ValidateAsync(
            Input.Company,
            Input.Username?.Trim() ?? string.Empty,
            Input.Password?.Trim() ?? string.Empty,
            systemId,
            HttpContext.RequestAborted);

        if (!result.Success)
        {
            LoginError = result.FailureReason ?? "Invalid username and password.";
            return Page();
        }

        // Store legacy session keys so the per-page port can read them
        // unchanged while the migration is in progress.
        HttpContext.Session.SetString("gstrUserID", result.UserId ?? string.Empty);
        HttpContext.Session.SetString("gettemp", result.EmployeeName ?? string.Empty);
        HttpContext.Session.SetString("gstrUsername", Input.Username?.Trim() ?? string.Empty);
        HttpContext.Session.SetString("LoginHis", DateTime.Now.ToString("dd MMMM yyyy"));
        HttpContext.Session.SetString("gstrUserCompCode", result.CompanyCode ?? string.Empty);
        HttpContext.Session.SetString("com", result.CompanyCode ?? string.Empty);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.UserId ?? string.Empty),
            new(ClaimTypes.Name, result.Username ?? Input.Username ?? string.Empty),
            new("EmployeeName", result.EmployeeName ?? string.Empty),
            new("CompanyCode", result.CompanyCode ?? string.Empty),
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        // Phase 2 will introduce /Menu — until then, send the user back to a
        // simple landing page (Index acts as both login and "home").
        return RedirectToPage("/Index");
    }

    private Task PopulateCompaniesAsync()
    {
        // Phase 2 will populate from the ACL store (the legacy
        // ACL.Control.Binding.BindCompany helper). For now, provide a single
        // placeholder so the markup renders when CrossCompany="1".
        Companies = new List<SelectListItem>
        {
            new("(default)", "")
        };
        return Task.CompletedTask;
    }

    public class LoginInput
    {
        public string? Company { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
