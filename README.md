# Signage_Display_System-19C
Signage_Display_System-19C

## Status — migrating to ASP.NET Core 8 Razor Pages

This repository is in the middle of a migration from Web Forms (.NET
Framework 4.5) to **ASP.NET Core 8 Razor Pages**. Both applications
currently coexist:

* **Old (reference)**: `LobbyDisplay.sln` — Web Forms app, untouched.
* **New (in progress)**: `LobbyDisplay.NET8.sln` — `src/FLM_LobbyDisplay.Web`
  Razor Pages app + ported `src/Library.*` libraries.

See [`MIGRATION.md`](MIGRATION.md) for the full migration plan, page-by-page
porting checklist, configuration mapping (`web.config` → `appsettings.json`),
and Web Forms → Razor pattern cookbook.

### Build & run the new app

```bash
dotnet build LobbyDisplay.NET8.sln
dotnet run --project src/FLM_LobbyDisplay.Web
```
