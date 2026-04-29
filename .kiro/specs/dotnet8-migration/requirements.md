# Requirements Document

## Introduction

This document defines the requirements for migrating the FLM LobbyDisplay application from ASP.NET Web Forms on .NET Framework 4.8 to .NET 8. The application is a lobby/display system that renders video playlists, scrolling text, and multi-screen display layouts across several physical display areas (Lobby, Lobby2, Pantry). It uses SQL Server for display content data and Oracle for business data, and depends on a set of custom internal library DLLs (Library.Root, Library.Common, Library.Control.Base, Library.Oraclecls, ACL) written in VB.NET.

Because ASP.NET Web Forms is not supported on .NET 8, the migration strategy is to re-platform the UI layer to ASP.NET Core (Razor Pages or MVC), while preserving all existing business logic, database access patterns, and display behaviour. The internal library DLLs must also be rebuilt to target .NET 8.

---

## Glossary

- **Application**: The FLM LobbyDisplay ASP.NET Web Forms system being migrated.
- **Migration_Tool**: The automated and manual process used to convert the Application from .NET 4.8 to .NET 8.
- **Library_Projects**: The four VB.NET class library projects (Library.Root, Library.Common, Library.Control.Base, Library.Oraclecls) and the ACL project whose source code is available in the `Backup/Library` folder.
- **Web_Project**: The ASP.NET web application project containing `.aspx`, `.ascx`, `App_Code`, `App_GlobalResources`, and `App_Module` artefacts.
- **Display_Page**: Any `.aspx` page that renders a lobby or pantry display screen (e.g., `lobby_mainDisplay.aspx`, `lobby_2ndDisplay.aspx`, `pantry_mainDisplay.aspx`).
- **Master_Page**: The `.aspx` page that acts as a navigation/launcher for opening Display_Pages (e.g., `Display_Mst.aspx`).
- **User_Control**: Any `.ascx` component in `App_Module` (Controller, Error, GridFooter, GridHeader, Search, Title).
- **App_Code_Class**: Any C# class in the `App_Code` folder (Base, BaseForm, BaseUC, Binding, Component_Class, FileServerTransfer, HtmlStriper, Info, LogBase, PluginMediaFlash, RegExp, Resource).
- **Oracle_Connection**: The Oracle database access layer provided by `Library.Oraclecls.dll`, which uses `Oracle.ManagedDataAccess`.
- **SQL_Connection**: The SQL Server database access used directly in Display_Pages via `System.Data.SqlClient`.
- **Global_Resource**: `.resx` files in `App_GlobalResources` used for localisation and configuration lookup.
- **Target_Framework**: .NET 8 (net8.0).
- **Razor_Page**: An ASP.NET Core Razor Page (`.cshtml` + `.cshtml.cs`) that replaces an `.aspx` page.
- **Session**: The HTTP session used to store user state (e.g., `Session["Checkpoint"]`, `Session["gstrUserID"]`).
- **FTP_Transfer**: The file upload mechanism in `FileServerTransfer.cs` that uses `FtpWebRequest`.
- **Media_Player_Control**: The `Media-Player-ASP.NET-Control.dll` used for video playback in display pages.

---

## Requirements

### Requirement 1: Upgrade Library Projects to .NET 8

**User Story:** As a developer, I want all internal library projects rebuilt to target .NET 8, so that the Web_Project can reference them without compatibility shims.

#### Acceptance Criteria

1. THE Migration_Tool SHALL retarget Library.Root, Library.Common, Library.Control.Base, Library.Oraclecls, and ACL from .NET Framework 4.8 to net8.0.
2. WHEN a Library_Project references `System.Web`, THE Migration_Tool SHALL replace that reference with the equivalent ASP.NET Core or BCL API.
3. THE Migration_Tool SHALL replace `Oracle.ManagedDataAccess` (full-framework) with `Oracle.ManagedDataAccess.Core` (NuGet package) in Library.Oraclecls.
4. WHEN a Library_Project uses `System.Configuration.ConfigurationManager`, THE Migration_Tool SHALL replace it with `Microsoft.Extensions.Configuration` or retain `ConfigurationManager` via the `System.Configuration.ConfigurationManager` NuGet package.
5. THE Migration_Tool SHALL produce compiled Library_Project assemblies that pass all existing unit tests without modification to test logic.
6. WHEN a Library_Project uses VB.NET-specific runtime helpers (e.g., `Microsoft.VisualBasic` module functions), THE Migration_Tool SHALL verify that the `Microsoft.VisualBasic.Core` NuGet package is referenced so those helpers remain available.

---

### Requirement 2: Replace Web Forms Project with ASP.NET Core

**User Story:** As a developer, I want the Web_Project converted to an ASP.NET Core project targeting .NET 8, so that the application can run on a modern, supported runtime.

#### Acceptance Criteria

1. THE Migration_Tool SHALL create a new ASP.NET Core project file (`.csproj`) targeting net8.0 to replace the existing Web Forms project.
2. THE Migration_Tool SHALL convert each `.aspx` / `.aspx.cs` pair to an equivalent Razor_Page (`.cshtml` / `.cshtml.cs`), preserving all HTML structure, inline expressions, and code-behind logic.
3. THE Migration_Tool SHALL convert each `.ascx` / `.ascx.cs` User_Control to an equivalent ASP.NET Core Partial View or View Component, preserving all properties, events, and rendering logic.
4. THE Migration_Tool SHALL migrate all App_Code_Classes to the `Services` or `Models` folder of the new project, retaining their namespaces and public APIs.
5. WHEN an App_Code_Class inherits from `System.Web.UI.Page` or `System.Web.UI.UserControl`, THE Migration_Tool SHALL replace the base class with the appropriate ASP.NET Core base class (`PageModel` or `ViewComponent`).
6. THE Migration_Tool SHALL preserve the existing URL structure so that all Display_Page URLs remain reachable at the same relative paths after migration.
7. WHEN the existing project uses `Page.ClientScript.RegisterStartupScript` or `ScriptManager.RegisterClientScriptBlock`, THE Migration_Tool SHALL replace those calls with equivalent inline `<script>` injection in the Razor view or a TempData-based mechanism.

---

### Requirement 3: Migrate Global Resources and Localisation

**User Story:** As a developer, I want all App_GlobalResources `.resx` files preserved and accessible in the new project, so that localisation and resource lookups continue to work without code changes.

#### Acceptance Criteria

1. THE Migration_Tool SHALL copy all `.resx` files from `App_GlobalResources` into the new project under a `Resources` folder.
2. WHEN code calls `GetGlobalResourceObject(className, key)`, THE Migration_Tool SHALL replace that call with the equivalent `IStringLocalizer` or strongly-typed resource class lookup.
3. THE Migration_Tool SHALL preserve all existing resource keys and values without modification.
4. WHEN the application reads the `MalaysiaTorayNaviLanguage` cookie to set culture, THE Migration_Tool SHALL implement the same culture-selection logic using ASP.NET Core request localisation middleware.
5. THE Migration_Tool SHALL support at least the `en-US` and `ms-MY` cultures that are currently handled by `BaseForm.SetCulture`.

---

### Requirement 4: Migrate Database Access — SQL Server

**User Story:** As a developer, I want all SQL Server data access code to work correctly under .NET 8, so that Display_Pages continue to retrieve video playlist and display content data.

#### Acceptance Criteria

1. THE Migration_Tool SHALL replace `System.Data.SqlClient` references with `Microsoft.Data.SqlClient` (NuGet package) throughout the Web_Project.
2. WHEN a Display_Page reads the `filmDisplay` connection string via `ConfigurationManager.ConnectionStrings`, THE Migration_Tool SHALL replace that with `IConfiguration` injection reading from `appsettings.json`.
3. THE Migration_Tool SHALL preserve all existing SQL query strings and `DataTable`-based result handling without modification to query logic.
4. WHEN a SQL query returns zero rows, THE Migration_Tool SHALL ensure the Display_Page renders an empty playlist without throwing an exception, matching current behaviour.

---

### Requirement 5: Migrate Database Access — Oracle

**User Story:** As a developer, I want all Oracle database access to work correctly under .NET 8, so that business data queries continue to function.

#### Acceptance Criteria

1. THE Migration_Tool SHALL replace the `Oracle.ManagedDataAccess` full-framework package with `Oracle.ManagedDataAccess.Core` in all projects that reference it.
2. THE Migration_Tool SHALL preserve the `Oracle_Connection` base class API (constructor, `Commit`, `RollBack`, `Dispose`) so that all derived data-access classes compile without modification.
3. WHEN the Oracle connection string name `ORCL_ACL` is referenced, THE Migration_Tool SHALL ensure that connection string is defined in `appsettings.json` and resolved via `IConfiguration`.
4. THE Migration_Tool SHALL preserve the Oracle transaction pattern (begin on construction, commit/rollback explicitly) used throughout Library.Oraclecls.

---

### Requirement 6: Migrate Session and Cookie Handling

**User Story:** As a developer, I want session and cookie usage preserved under ASP.NET Core, so that display checkpoint tracking and language selection continue to work.

#### Acceptance Criteria

1. THE Migration_Tool SHALL configure ASP.NET Core session middleware so that `Session["Checkpoint"]` and `Session["gstrUserID"]` reads and writes behave identically to the Web Forms session.
2. WHEN the application reads the `MalaysiaTorayNaviLanguage` cookie, THE Migration_Tool SHALL use `IHttpContextAccessor` or the Razor Page `Request.Cookies` API to read the cookie value.
3. THE Migration_Tool SHALL configure session with a timeout that is equal to or greater than the current Web Forms session timeout defined in `web.config`.
4. WHEN a session key is absent, THE Migration_Tool SHALL return the same default value as the current code (e.g., `0` for `gstrUserID`, `null` for absent keys).

---

### Requirement 7: Migrate File System Operations

**User Story:** As a developer, I want all file system operations (scrolling text reads, file uploads, FTP transfers, archive operations) to work correctly under .NET 8, so that media content management is unaffected.

#### Acceptance Criteria

1. WHEN a Display_Page reads `scrollingtext.txt` using `Server.MapPath`, THE Migration_Tool SHALL replace `Server.MapPath` with `IWebHostEnvironment.WebRootPath` or `ContentRootPath` to resolve the physical path.
2. THE Migration_Tool SHALL preserve the `FileServerTransfer` class logic for file upload, archive, and removal, replacing `HttpPostedFile` with `IFormFile` where required by ASP.NET Core.
3. WHEN `FileServerTransfer` reads `FILESERVER_KEY`, `FILESERVER_PATH`, and `FILESERVER_URL` from `ConfigurationManager.AppSettings`, THE Migration_Tool SHALL replace those reads with `IConfiguration` lookups from `appsettings.json`.
4. THE Migration_Tool SHALL preserve the FTP upload logic in `FileServerTransfer.TransferFile`, replacing the deprecated `FtpWebRequest` with `FluentFTP` or `WinSCP` .NET library if `FtpWebRequest` is unavailable on .NET 8.
5. WHEN a directory does not exist during a file save operation, THE Migration_Tool SHALL create the directory, matching the current `Directory.CreateDirectory` behaviour.

---

### Requirement 8: Replace or Remove Incompatible Third-Party Controls

**User Story:** As a developer, I want all third-party Web Forms controls replaced with compatible alternatives, so that the migrated application renders correctly without runtime errors.

#### Acceptance Criteria

1. THE Migration_Tool SHALL identify all references to `Media-Player-ASP.NET-Control.dll` in Display_Pages and replace them with an HTML5 `<video>` element or a compatible JavaScript video player (e.g., Video.js).
2. WHEN `PluginMediaFlash.cs` generates Flash plugin markup, THE Migration_Tool SHALL replace that markup with an HTML5 `<video>` or `<audio>` element, as Flash is no longer supported.
3. THE Migration_Tool SHALL verify that no remaining DLL reference targets .NET Framework only (i.e., no `net4x` TFM-only packages remain after migration).
4. WHEN a third-party control has no .NET 8-compatible equivalent, THE Migration_Tool SHALL document the gap and propose a replacement strategy before implementation.

---

### Requirement 9: Preserve Display Page Behaviour and Rendering

**User Story:** As a display system operator, I want all lobby and pantry display screens to render identically after migration, so that the physical display experience is unaffected.

#### Acceptance Criteria

1. WHEN the migrated `lobby_mainDisplay` Razor_Page loads, THE Application SHALL query the `MM_VIDEOS` table with `RECORD_TYP<>5 AND SCR_ID=1` and produce the same JavaScript `video_lists`, `seek_starts`, `seek_ends`, `period_starts`, and `period_ends` arrays as the current page.
2. WHEN the migrated `lobby_2ndDisplay` Razor_Page loads, THE Application SHALL produce display output equivalent to the current `lobby_2ndDisplay.aspx` page.
3. WHEN the migrated `pantry_mainDisplay` Razor_Page loads, THE Application SHALL produce display output equivalent to the current `pantry_mainDisplay.aspx` page.
4. WHEN a Display_Page is loaded as a PostBack (form re-submission), THE Application SHALL call the equivalent of `loaded()` and `onPageLoad()` JavaScript functions with the current playlist data.
5. WHEN a Display_Page is loaded for the first time (non-PostBack), THE Application SHALL call the equivalent of `loaded2()` JavaScript function.
6. THE Application SHALL serve all CSS files from the existing `css` subdirectories under their current relative URLs.
7. THE Application SHALL serve all media files from the existing `mainscr` subdirectories under their current relative URLs.

---

### Requirement 10: Migrate Configuration from web.config to appsettings.json

**User Story:** As a developer, I want all application configuration migrated from `web.config` to `appsettings.json`, so that the application uses the standard .NET 8 configuration system.

#### Acceptance Criteria

1. THE Migration_Tool SHALL extract all `<connectionStrings>` entries from `web.config` and place them in the `ConnectionStrings` section of `appsettings.json`.
2. THE Migration_Tool SHALL extract all `<appSettings>` key-value pairs from `web.config` and place them in the `AppSettings` section of `appsettings.json`.
3. THE Migration_Tool SHALL extract all `<system.web>` session, authentication, and compilation settings from `web.config` and apply equivalent configuration in `Program.cs` or `appsettings.json`.
4. WHEN a `web.config` transform (e.g., `web.Release.config`) exists, THE Migration_Tool SHALL create equivalent `appsettings.Production.json` overrides.
5. THE Migration_Tool SHALL not store connection string passwords in plain text in source-controlled files; it SHALL document the use of environment variables or .NET Secret Manager for sensitive values.

---

### Requirement 11: Maintain Build and Deployment Pipeline

**User Story:** As a developer, I want the migrated project to build and publish using standard .NET 8 CLI commands, so that the CI/CD pipeline requires minimal changes.

#### Acceptance Criteria

1. THE Migration_Tool SHALL produce a solution file (`.sln`) that includes the migrated Web_Project and all migrated Library_Projects.
2. WHEN `dotnet build` is run against the solution, THE Application SHALL compile with zero errors and zero warnings related to the migration.
3. WHEN `dotnet publish` is run, THE Application SHALL produce a self-contained or framework-dependent deployment artefact that can be hosted on IIS or Kestrel.
4. THE Migration_Tool SHALL include a `web.config` shim (generated by `dotnet publish`) for IIS hosting so that the ASP.NET Core module is correctly configured.
5. WHEN the published application is deployed to IIS, THE Application SHALL start and serve requests without requiring the .NET Framework runtime.

---

### Requirement 12: Logging and Error Handling

**User Story:** As a developer, I want all error handling and logging preserved or improved under .NET 8, so that runtime errors are captured and diagnosable.

#### Acceptance Criteria

1. THE Migration_Tool SHALL replace all `catch (Exception)` blocks that silently swallow exceptions with blocks that log the exception using `Microsoft.Extensions.Logging.ILogger`.
2. WHEN `Library.Root.Control.MessageCenter.ShowAJAXMessageBox` is called, THE Application SHALL display the same JavaScript alert to the user as the current implementation.
3. THE Migration_Tool SHALL configure ASP.NET Core's built-in exception handling middleware to return a user-friendly error page for unhandled exceptions.
4. WHEN an Oracle or SQL Server connection fails, THE Application SHALL log the connection error with sufficient detail (connection string name, exception message) for diagnosis, without exposing credentials in the log output.
