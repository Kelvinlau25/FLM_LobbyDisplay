using FLM_LobbyDisplay.Web.Infrastructure;
using Library.Root.Other;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------------------------
// Configuration
// --------------------------------------------------------------------
var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>() ?? new AppSettings();
builder.Services.AddSingleton(appSettings);

// Mirror the legacy ConfigurationManager.AppSettings["MaxRowPerPage"]
// behaviour into the static used by Library.Root.Other.BusinessLogicBase.
BusinessLogicBase.MaxQuantityPerPage = appSettings.MaxRowPerPage;

// Optional: point Oracle.ManagedDataAccess.Core at a TNS_ADMIN folder
// (only needed if you use TNS aliases instead of full descriptors in the
// connection string). Without TNS aliases this is a no-op.
var tnsAdmin = builder.Configuration["Oracle:TnsAdmin"];
if (!string.IsNullOrWhiteSpace(tnsAdmin))
{
    Oracle.ManagedDataAccess.Client.OracleConfiguration.TnsAdmin = tnsAdmin;
    Oracle.ManagedDataAccess.Client.OracleConfiguration.WalletLocation = tnsAdmin;
}

// --------------------------------------------------------------------
// Services
// --------------------------------------------------------------------
builder.Services.AddRazorPages(options =>
{
    // Anonymous-allowed pages. Everything else requires authentication.
    options.Conventions.AllowAnonymousToPage("/Index");
    options.Conventions.AllowAnonymousToPage("/SessionExpired");
    options.Conventions.AllowAnonymousToPage("/Error");
});

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Index";
        options.LogoutPath = "/Index";
        options.AccessDeniedPath = "/Index";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(builder.Configuration.GetValue("Session:IdleTimeoutMinutes", 60));
        options.SlidingExpiration = true;
        options.Cookie.Name = "FLM_LobbyDisplay.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = ctx =>
            {
                // For non-AJAX requests targeting an authorised page, redirect
                // to the dedicated SessionExpired page (which then bounces to
                // Index) so the UX matches the legacy SessionExpired.aspx flow.
                if (!ctx.Request.Path.StartsWithSegments("/Index"))
                {
                    ctx.Response.Redirect("/SessionExpired");
                    return Task.CompletedTask;
                }
                ctx.Response.Redirect(ctx.RedirectUri);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(builder.Configuration.GetValue("Session:IdleTimeoutMinutes", 60));
    options.Cookie.Name = "FLM_LobbyDisplay.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddAntiforgery();

// Application services. The login flow is intentionally delegated to an
// IUserAuthenticator abstraction so the real ACL Oracle implementation can be
// wired in later (the existing ACL.* binaries target .NET Framework 4.5 and
// must themselves be ported before they can be referenced from .NET 8).
builder.Services.AddScoped<IUserAuthenticator, StubUserAuthenticator>();

var app = builder.Build();

// --------------------------------------------------------------------
// Pipeline
// --------------------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
