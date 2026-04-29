using System.Globalization;
using Microsoft.AspNetCore.Localization;
using FLM_LobbyDisplay.Services;

var builder = WebApplication.CreateBuilder(args);

// Allow large video file uploads (original web.config: maxRequestLength=409600 KB, ~400 MB)
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 4_294_967_295L; // ~4 GB (matches original requestLimits maxAllowedContentLength)
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 4_294_967_295L;
});

builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Localisation
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supported = new[] { new CultureInfo("en-US"), new CultureInfo("ms-MY") };
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supported;
    options.SupportedUICultures = supported;
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider
    {
        CookieName = "MalaysiaTorayNaviLanguage"
    });
});

// Application services
builder.Services.AddScoped<PlaylistBuilderService>();
builder.Services.AddScoped<ScrollingTextService>();
builder.Services.AddSingleton<FileServerTransferService>();
builder.Services.AddScoped<IMssqlAuthService, MssqlAuthService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseRequestLocalization();
app.UseSession();
app.MapRazorPages();
app.MapControllers();

app.Run();
