# Implementation Plan: .NET 8 Migration — FLM LobbyDisplay

## Overview

Migrate the FLM LobbyDisplay application from ASP.NET Web Forms (.NET 4.8) to ASP.NET Core Razor Pages (.NET 8). The plan proceeds in layers: library projects first, then the new web project scaffold, then page-by-page conversion, then services, then tests. Each task builds on the previous so there is no orphaned code.

## Tasks

- [x] 1. Migrate Library Projects to net8.0 SDK-style
  - [x] 1.1 Retarget Library.Root to net8.0
    - Replace the existing `.vbproj` with an SDK-style project file targeting `net8.0`
    - Add NuGet packages: `System.Configuration.ConfigurationManager 8.x`, `Microsoft.VisualBasic.Core`
    - Remove `System.Web.UI.Page` inheritance from `Control/Base.vb`; extract non-UI properties into a plain abstract `Base` class as specified in the design
    - Extract `IPageContext` interface for `Response.Redirect`, `ResolveUrl`, `Server.UrlDecode`
    - Update `MessageCenter.ShowAJAXMessageBox` to `BuildAlertScript` returning a `<script>alert(...);</script>` string (removes `ScriptManager` dependency)
    - _Requirements: 1.1, 1.2, 1.6, 2.5, 12.2_

  - [x] 1.2 Retarget Library.Common to net8.0
    - Replace `.vbproj` with SDK-style file targeting `net8.0`
    - Add `System.Configuration.ConfigurationManager 8.x` and `Microsoft.VisualBasic.Core`
    - Remove or replace any `System.Web` references with BCL equivalents
    - _Requirements: 1.1, 1.2, 1.4, 1.6_

  - [x] 1.3 Retarget Library.Control.Base to net8.0
    - Replace `.vbproj` with SDK-style file targeting `net8.0`
    - Add `System.Configuration.ConfigurationManager 8.x` and `Microsoft.VisualBasic.Core`
    - Remove or replace any `System.Web` references
    - _Requirements: 1.1, 1.2, 1.4, 1.6_

  - [x] 1.4 Retarget Library.Oraclecls to net8.0
    - Replace `.vbproj` with SDK-style file targeting `net8.0`
    - Replace `Oracle.ManagedDataAccess` (full-framework) with `Oracle.ManagedDataAccess.Core 23.6.0`
    - Add `System.Configuration.ConfigurationManager 8.x` and `Microsoft.VisualBasic.Core`
    - Preserve the `Connection` class public API exactly: constructor, `Commit`, `RollBack`, `Dispose`, `Status`
    - Preserve the Oracle transaction pattern (begin on construction, commit/rollback explicitly)
    - _Requirements: 1.1, 1.3, 5.1, 5.2, 5.4_

  - [x] 1.5 Retarget ACL to net8.0
    - Replace `.vbproj` with SDK-style file targeting `net8.0`
    - Add `System.Configuration.ConfigurationManager 8.x` and `Microsoft.VisualBasic.Core`
    - Remove or replace any `System.Web` references
    - _Requirements: 1.1, 1.2, 1.4, 1.6_

  - [x] 1.6 Verify all library projects build cleanly
    - Run `dotnet build` on each library project and confirm zero errors
    - Confirm no `System.Web` assembly references remain in any library `.vbproj`
    - Confirm `Oracle.ManagedDataAccess.Core` is referenced in Library.Oraclecls
    - _Requirements: 1.1, 1.5, 11.2_

- [x] 2. Create ASP.NET Core Web Project Scaffold
  - [x] 2.1 Create new SDK-style web project and solution file
    - Create `FLM_LobbyDisplay.Web/FLM_LobbyDisplay.Web.csproj` targeting `net8.0` with `Microsoft.NET.Sdk.Web`
    - Add NuGet packages: `Microsoft.Data.SqlClient`, `Oracle.ManagedDataAccess.Core 23.6.0`, `FluentFTP`, `FsCheck.Xunit` (test project)
    - Create `LobbyDisplay.sln` referencing the web project and all five library projects
    - Create the directory structure: `Pages/`, `Services/`, `Models/`, `Resources/`, `Api/`, `wwwroot/`
    - _Requirements: 2.1, 11.1_

  - [x] 2.2 Configure Program.cs with all middleware
    - Implement `Program.cs` exactly as specified in the design: `AddRazorPages`, `AddControllers`, `AddDistributedMemoryCache`, `AddSession` (20-minute timeout, HttpOnly, IsEssential), `AddLocalization` (ResourcesPath = "Resources"), `Configure<RequestLocalizationOptions>` with `en-US` / `ms-MY` and `CookieRequestCultureProvider` reading `MalaysiaTorayNaviLanguage`, `AddScoped<PlaylistBuilderService>`, `AddScoped<ScrollingTextService>`, `AddSingleton<FileServerTransferService>`, `AddHttpContextAccessor`, `AddLogging`
    - Wire middleware pipeline: `UseExceptionHandler("/Error")`, `UseStaticFiles`, `UseRouting`, `UseRequestLocalization`, `UseSession`, `MapRazorPages`, `MapControllers`
    - _Requirements: 2.1, 3.4, 3.5, 6.1, 6.3, 10.3, 12.3_

  - [x] 2.3 Migrate configuration to appsettings.json
    - Extract all `<connectionStrings>` from `web.config` into `appsettings.json` `ConnectionStrings` section (`filmDisplay`, `ORCL_ACL`)
    - Extract all `<appSettings>` key-value pairs into `appsettings.json` `AppSettings` section (`FILESERVER_KEY`, `FILESERVER_PATH`, `FILESERVER_URL`, `FLASHINSTALLER_FILE`, `FLASHINSTALLER_IMAGE`)
    - Create `appsettings.Production.json` with `${VAR_NAME}` placeholders for passwords (no plain-text credentials in source)
    - Document use of environment variables / .NET Secret Manager for sensitive values in a `README.md` note
    - _Requirements: 10.1, 10.2, 10.3, 10.4, 10.5_

  - [x] 2.4 Copy static assets to wwwroot
    - Copy `acc/LobbyDisplay/css/` → `wwwroot/acc/LobbyDisplay/css/`
    - Copy `acc/LobbyDisplay/mainscr/` → `wwwroot/acc/LobbyDisplay/mainscr/`
    - Repeat for `LobbyDisplay2`, `MstMain`, `MstMainLobby2`, `MstMainPan`, `PantryDisplay` (all `css/` and `mainscr/` subdirectories)
    - Verify all relative URL references in existing HTML/JavaScript remain valid after copy
    - _Requirements: 9.6, 9.7_

- [x] 3. Checkpoint — Verify scaffold builds
  - Ensure `dotnet build LobbyDisplay.sln` produces zero errors before proceeding to page conversion. Ask the user if questions arise.

- [x] 4. Migrate Data Models and Core Services
  - [x] 4.1 Create VideoEntry and PlaylistData models
    - Create `Models/VideoEntry.cs` as a C# `record` with properties: `AttachFile`, `SeekStart`, `SeekEnd`, `PeriodStart`, `PeriodEnd`
    - Create `Models/PlaylistData.cs` with `VideoLists`, `SeekStarts`, `SeekEnds`, `PeriodStarts`, `PeriodEnds`, `VideoListsCsv`, `SeekStartsCsv`, `SeekEndsCsv`, `PeriodStartsCsv`, `PeriodEndsCsv` properties and a static `Empty` factory
    - _Requirements: 4.3, 9.1_

  - [x] 4.2 Implement PlaylistBuilderService
    - Create `Services/PlaylistBuilderService.cs`
    - Inject `IConfiguration` and `ILogger<PlaylistBuilderService>`
    - Implement `BuildAsync(int screenId)`: open `SqlConnection` using `_config.GetConnectionString("filmDisplay")`, execute `SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID={screenId}`, fill `DataTable`, build JavaScript array literal strings and CSV variants, return `PlaylistData`
    - Return `PlaylistData.Empty` (no exception) when query returns zero rows
    - Replace `System.Data.SqlClient` with `Microsoft.Data.SqlClient`
    - Log SQL errors with connection name and exception message; do not log the connection string value
    - _Requirements: 4.1, 4.2, 4.3, 4.4, 9.1, 12.1, 12.4_

  - [ ]* 4.3 Write property test — Property 1: Playlist builder produces valid JS array literals
    - **Property 1: Playlist builder produces valid JavaScript array literals for any non-empty video row set**
    - **Validates: Requirements 9.1, 9.2, 9.3**
    - Use FsCheck `[Property]` attribute; generate `NonEmptyArray<VideoEntry>` inputs; assert all five array strings start with `[` and end with `]` and contain one quoted entry per row

  - [ ]* 4.4 Write property test — Property 2: Playlist builder produces empty arrays for zero-row input
    - **Property 2: Playlist builder produces empty arrays for zero-row input**
    - **Validates: Requirements 4.4, 9.1**
    - Pass an empty `VideoEntry[]` to `PlaylistBuilderService.Build`; assert all array strings equal `[]` and no exception is thrown

  - [x] 4.5 Implement ScrollingTextService
    - Create `Services/ScrollingTextService.cs`
    - Inject `IWebHostEnvironment`
    - Implement `ReadAsync(string area)`: resolve path via `Path.Combine(_env.ContentRootPath, "acc", area, "scrollingtext.txt")`, return file contents or `string.Empty` if file does not exist
    - _Requirements: 7.1_

  - [x] 4.6 Implement FileServerTransferService
    - Create `Services/FileServerTransferService.cs`
    - Inject `IConfiguration` and `IWebHostEnvironment`
    - Implement `SaveAsync(IFormFile file, string destDirectory, string destFileName)`: resolve destination path, call `Directory.CreateDirectory`, delete existing file if present, copy `IFormFile` stream to destination
    - Implement `TransferFileAsync(string filename, byte[] content)`: read `FILESERVER_KEY`, `FILESERVER_PATH`, `FILESERVER_URL` from `IConfiguration`; use `FluentFTP AsyncFtpClient` to upload bytes (replaces `FtpWebRequest`)
    - Wrap all I/O in try/catch; log errors via `ILogger`; return `false` on failure (preserves original boolean contract)
    - _Requirements: 7.2, 7.3, 7.4, 7.5_

  - [x] 4.7 Implement Html5MediaConverter (replaces PluginMediaFlash)
    - Create `Services/Html5MediaConverter.cs` with a static `Convert(string html)` method
    - Replace Flash plugin markup (`.swf` references, `<object>`, `<embed>`, `<param name="flashvars">`) with an HTML5 `<video>` element
    - Migrate `App_Code/PluginMediaFlash.cs` logic into this converter
    - _Requirements: 8.1, 8.2_

  - [ ]* 4.8 Write property test — Property 7: Flash markup replacement produces no SWF references
    - **Property 7: Flash markup replacement produces no SWF references**
    - **Validates: Requirements 8.2**
    - Generate arbitrary HTML strings containing `.swf` references and Flash markup; assert `Html5MediaConverter.Convert` output contains no `.swf` and contains at least one `<video` or `<audio` element

- [-] 5. Migrate App_Code Classes to Services/Models
  - [-] 5.1 Migrate Base, BaseForm, BaseUC
    - Move `App_Code/Base.cs`, `BaseForm.cs`, `BaseUC.cs` to `Services/` or `Models/` as appropriate
    - Replace `System.Web.UI.Page` / `System.Web.UI.UserControl` base classes with `PageModel` / plain class as specified in the design
    - Implement `IPageContext` interface for web-project-level injection of `Response.Redirect`, `ResolveUrl`, `Server.UrlDecode`
    - Retain all non-UI public APIs and namespaces
    - _Requirements: 2.4, 2.5_

  - [-] 5.2 Migrate Binding, Component_Class, Info, RegExp, Resource, HtmlStriper, LogBase
    - Move each class from `App_Code/` to `Services/` or `Models/`
    - Replace any `System.Web` references with BCL or ASP.NET Core equivalents
    - Replace `ConfigurationManager.AppSettings` reads with `IConfiguration` injection where the class is instantiated by DI; retain `ConfigurationManager` via NuGet for classes not in the DI graph
    - _Requirements: 2.4, 1.4_

- [ ] 6. Migrate Global Resources
  - [ ] 6.1 Copy .resx files to Resources folder
    - Copy all `.resx` files from `App_GlobalResources/` to `FLM_LobbyDisplay.Web/Resources/` without modifying any keys or values
    - Add each `.resx` file to the project with `EmbeddedResource` build action
    - _Requirements: 3.1, 3.3_

  - [ ] 6.2 Wire IStringLocalizer for resource lookups
    - Register `AddLocalization(options => options.ResourcesPath = "Resources")` in `Program.cs` (already done in task 2.2)
    - Replace all `GetGlobalResourceObject(className, key)` calls in migrated page models and services with `IStringLocalizer<T>[key].Value` or strongly-typed resource class lookups
    - _Requirements: 3.2, 3.3_

  - [ ]* 6.3 Write property test — Property 4: Resource key-value preservation
    - **Property 4: Resource key-value preservation**
    - **Validates: Requirements 3.3**
    - For each key in the original `.resx` files, assert that `IStringLocalizer` returns the same string value as the original resource; parameterise over all keys using FsCheck generators seeded from the `.resx` file contents

  - [ ]* 6.4 Write property test — Property 5: Culture selection from cookie
    - **Property 5: Culture selection from cookie**
    - **Validates: Requirements 3.4, 6.2**
    - For each supported culture string (`en-US`, `ms-MY`), simulate a request with `MalaysiaTorayNaviLanguage` cookie set to that value; assert `Thread.CurrentThread.CurrentCulture.Name` equals the cookie value after middleware runs

- [ ] 7. Migrate User Controls to Partial Views
  - [ ] 7.1 Convert App_Module user controls to Razor partial views
    - Create `Pages/Shared/_Controller.cshtml`, `_Error.cshtml`, `_GridFooter.cshtml`, `_GridHeader.cshtml`, `_Search.cshtml`, `_Title.cshtml`
    - Preserve all HTML structure and rendering logic from the corresponding `.ascx` files
    - Replace the `Controller.ascx` event model (`AddAction`, `EditAction`, etc.) with Razor Page handler methods (`OnPostAdd`, `OnPostEdit`, etc.) and render buttons that POST to named handlers
    - Move `.ascx.cs` code-behind logic into the partial view or a backing `ViewComponent` as appropriate
    - _Requirements: 2.3_

- [x] 8. Convert Display Pages to Razor Pages
  - [x] 8.1 Convert LobbyDisplay pages
    - Create `Pages/acc/LobbyDisplay/Display_Mst.cshtml` / `.cshtml.cs` from `Display_Mst.aspx` / `.aspx.cs`
    - Create `Pages/acc/LobbyDisplay/lobby_mainDisplay.cshtml` / `.cshtml.cs` from `lobby_mainDisplay.aspx` / `.aspx.cs`
      - Inject `PlaylistBuilderService`, `ScrollingTextService`, `ILogger`
      - Implement `OnGetAsync`: set `IsRefresh` from `Request.Query["refresh"]`, call `BuildAsync(screenId: 1)`, call `ScrollingTextService.ReadAsync("MstMain")`, write `Session["Checkpoint"]`
      - Emit `video_list`, `seek_starts`, `seek_ends`, `period_starts`, `period_ends` JavaScript arrays via `@Html.Raw(Model.Data.*)`
      - Emit `loaded(...); onPageLoad();` when `IsRefresh`, else `loaded2();`
      - Replace `__doPostBack` timer with `fetch('/api/playlist/1')` call
    - Create `Pages/acc/LobbyDisplay/lobby_2ndDisplay.cshtml` / `.cshtml.cs` from `lobby_2ndDisplay.aspx` / `.aspx.cs`
    - Add `@page` directive with route matching original URL for each page
    - _Requirements: 2.2, 2.6, 9.1, 9.2, 9.4, 9.5_

  - [x] 8.2 Convert LobbyDisplay2 pages
    - Create `Pages/acc/LobbyDisplay2/Display2_Mst.cshtml` / `.cshtml.cs`
    - Create `Pages/acc/LobbyDisplay2/lobby2_mainDisplay.cshtml` / `.cshtml.cs`
    - Create `Pages/acc/LobbyDisplay2/lobby2_2ndDisplay.cshtml` / `.cshtml.cs`
    - Follow the same conversion pattern as task 8.1
    - _Requirements: 2.2, 2.6, 9.2_

  - [x] 8.3 Convert MstMain pages
    - Create `Pages/acc/MstMain/FullMainScreen_Dtl.cshtml` / `.cshtml.cs`
    - Create `Pages/acc/MstMain/Lower2ndScreen_Dtl.cshtml` / `.cshtml.cs`
    - Create `Pages/acc/MstMain/Upper2ndScreen_Dtl.cshtml` / `.cshtml.cs`
    - Create `Pages/acc/MstMain/MM_VerticalScreenFull.cshtml` / `.cshtml.cs`
    - _Requirements: 2.2, 2.6_

  - [x] 8.4 Convert MstMainLobby2 and MstMainPan pages
    - Create equivalent Razor Pages for all `.aspx` files under `acc/MstMainLobby2/` and `acc/MstMainPan/`
    - _Requirements: 2.2, 2.6_

  - [x] 8.5 Convert PantryDisplay pages
    - Create `Pages/acc/PantryDisplay/Display_Mst.cshtml` / `.cshtml.cs`
    - Create `Pages/acc/PantryDisplay/pantry_mainDisplay.cshtml` / `.cshtml.cs`
    - Create `Pages/acc/PantryDisplay/pantry_2ndDisplay.cshtml` / `.cshtml.cs`
    - Inject `PlaylistBuilderService` with the correct `screenId` for pantry
    - _Requirements: 2.2, 2.6, 9.3_

  - [x] 8.6 Convert PopUp pages
    - Create `Pages/acc/PopUp/ImgVideoPreview.cshtml` / `.cshtml.cs`
    - Create `Pages/acc/PopUpLobby2/ImgVideoPreview.cshtml` / `.cshtml.cs`
    - Create `Pages/acc/PopUpPan/ImgVideoPreview.cshtml` / `.cshtml.cs`
    - Replace `Media-Player-ASP.NET-Control.dll` usage with HTML5 `<video>` element
    - _Requirements: 2.2, 8.1, 8.2_

  - [x] 8.7 Replace all ScriptManager and Server.MapPath calls
    - Search the entire web project for `ScriptManager.RegisterClientScriptBlock`, `Page.ClientScript.RegisterStartupScript`, and `Server.MapPath`; replace each with the Razor inline `<script>` or `IWebHostEnvironment` equivalent
    - Confirm zero occurrences remain
    - _Requirements: 2.7, 7.1_

- [x] 9. Implement Playlist JSON API Endpoint
  - [x] 9.1 Create PlaylistController
    - Create `Api/PlaylistController.cs` with `[ApiController]` and `[Route("api/playlist")]`
    - Implement `[HttpGet("{screenId:int}")]` action: inject `PlaylistBuilderService`, call `BuildAsync(screenId)`, return `Ok(data)`
    - Register `AddControllers()` and `MapControllers()` in `Program.cs` (already done in task 2.2)
    - _Requirements: 2.1, 9.4_

- [ ] 10. Implement Session and Cookie Handling
  - [ ] 10.1 Verify session middleware and usage
    - Confirm `AddSession` / `UseSession` are configured in `Program.cs` (task 2.2)
    - Replace all `Session["key"]` reads/writes in migrated page models with `HttpContext.Session.GetString` / `SetString`
    - Ensure absent keys return the same defaults as the original code (`null` for absent string keys, `"0"` for `gstrUserID` default)
    - _Requirements: 6.1, 6.3, 6.4_

  - [ ]* 10.2 Write property test — Property 6: Session round-trip
    - **Property 6: Session round-trip**
    - **Validates: Requirements 6.1, 6.4**
    - For any `NonEmptyString` key and value, write to `ISession` and read back; assert the returned value equals the written value

- [ ] 11. Implement Logging and Error Handling
  - [ ] 11.1 Replace silent catch blocks with ILogger calls
    - Search all migrated C# files for empty or comment-only `catch` blocks; replace each with `_logger.LogError(ex, ...)` calls
    - Ensure log messages include the connection string name and exception message but never the password value
    - _Requirements: 12.1, 12.4_

  - [ ] 11.2 Configure exception handling middleware and Error page
    - Confirm `app.UseExceptionHandler("/Error")` is in `Program.cs` (task 2.2)
    - Create `Pages/Error.cshtml` / `.cshtml.cs` rendering a user-friendly error message
    - Add `app.UseDeveloperExceptionPage()` for the Development environment
    - _Requirements: 12.3_

  - [ ] 11.3 Verify MessageCenter.BuildAlertScript
    - Confirm `MessageCenter.BuildAlertScript` in Library.Root encodes single quotes and `||` newline markers as specified in the design
    - Add inline Razor injection pattern `@Html.Raw(MessageCenter.BuildAlertScript(Model.AlertMessage))` to all page models that previously called `ShowAJAXMessageBox`
    - _Requirements: 12.2_

  - [ ]* 11.4 Write property test — Property 8: MessageCenter alert encoding preserves message content
    - **Property 8: MessageCenter alert encoding preserves message content**
    - **Validates: Requirements 12.2**
    - For any `NonEmptyString` message (including single quotes, HTML special characters, newlines), assert `BuildAlertScript` output contains `alert(` and that the decoded argument is semantically equivalent to the original message

  - [ ]* 11.5 Write property test — Property 9: Connection failure log contains name but not password
    - **Property 9: Connection failure log contains name but not password**
    - **Validates: Requirements 12.4**
    - For any connection string name, password, and exception, assert the log entry built by the error handler contains the name and exception message and does not contain the password

- [x] 12. Checkpoint — Full solution build and smoke tests
  - Run `dotnet build LobbyDisplay.sln` and confirm zero errors and zero migration-related warnings.
  - Verify: no `System.Web` references in library projects, `Oracle.ManagedDataAccess.Core` present in Library.Oraclecls, `filmDisplay` and `ORCL_ACL` connection strings in `appsettings.json`, no `ScriptManager` calls, no `Server.MapPath` calls, no empty `catch` blocks.
  - Ask the user if questions arise.

- [ ] 13. Write Remaining Property-Based Tests
  - [ ]* 13.1 Write property test — Property 3: Postback script injection round-trip
    - **Property 3: Postback script injection round-trip**
    - **Validates: Requirements 9.4**
    - For any `PlaylistData` value, build the postback script string via `PageModelHelper.BuildPostbackScript`; extract the `loaded(...)` arguments; assert that splitting on `","` reconstructs the original CSV values exactly

- [ ] 14. Write Unit and Integration Tests
  - [ ]* 14.1 Write unit tests for PlaylistBuilderService
    - Test correct JavaScript array format for known video row sets
    - Test empty result for zero rows (no exception)
    - _Requirements: 4.3, 4.4, 9.1_

  - [ ]* 14.2 Write unit tests for ScrollingTextService
    - Test correct path resolution using a mock `IWebHostEnvironment`
    - Test returns `string.Empty` when file does not exist
    - _Requirements: 7.1_

  - [ ]* 14.3 Write unit tests for FileServerTransferService
    - Test directory creation when destination does not exist
    - Test file overwrite behaviour
    - Test archive and removal behaviour
    - _Requirements: 7.2, 7.5_

  - [ ]* 14.4 Write unit tests for MessageCenter.BuildAlertScript
    - Test output for known inputs including single quotes, HTML special characters, and newlines
    - _Requirements: 12.2_

  - [ ]* 14.5 Write integration tests for URL routing
    - Verify each known display page URL returns HTTP 200 using `WebApplicationFactory<Program>`
    - _Requirements: 2.6, 9.1, 9.2, 9.3_

  - [ ]* 14.6 Write integration tests for static file serving
    - Verify CSS and media files are accessible at their original relative paths
    - _Requirements: 9.6, 9.7_

- [x] 15. Verify Build and Publish Pipeline
  - [x] 15.1 Verify dotnet build produces zero errors
    - Run `dotnet build LobbyDisplay.sln --configuration Release` and confirm zero errors and zero warnings related to the migration
    - _Requirements: 11.2_

  - [x] 15.2 Verify dotnet publish produces a deployable artefact
    - Run `dotnet publish FLM_LobbyDisplay.Web/FLM_LobbyDisplay.Web.csproj --configuration Release --output ./publish`
    - Confirm the output contains a `web.config` shim for IIS hosting (generated automatically by the ASP.NET Core module)
    - Confirm the artefact is framework-dependent (or self-contained if required) and can be hosted on IIS or Kestrel
    - _Requirements: 11.3, 11.4, 11.5_

- [x] 16. Final Checkpoint — Ensure all tests pass
  - Run `dotnet test LobbyDisplay.sln` and confirm all unit, property-based, and integration tests pass. Ask the user if questions arise.

## Notes

- Tasks marked with `*` are optional and can be skipped for a faster MVP
- Each task references specific requirements for traceability
- Property tests use FsCheck (`FsCheck.Xunit`) with a minimum of 100 iterations per property
- Checkpoints (tasks 3, 12, 16) are gates — do not proceed past a checkpoint with build errors
- Sensitive configuration values (DB passwords) must never be committed in plain text; use environment variables or .NET Secret Manager
- The `Backup/Library` folder contains the VB.NET source for all five library projects
