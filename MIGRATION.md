# Migration Guide — Web Forms → ASP.NET Core 8 Razor Pages

This repository is in the middle of a migration from a .NET Framework 4.5
**Web Forms** application to **ASP.NET Core 8 Razor Pages**. Phase 1 of the
migration (this PR) sets up the new project alongside the old one so the two
can coexist while pages are ported one folder at a time. The original
application — `LobbyDisplay.sln`, `Default.aspx`, `App_Code/`, `master/`,
`acc/`, `pages/`, `Bin/`, `plugin/`, `web.config` — is **left untouched** and
remains the reference implementation until every page has been ported.

---

## Repository layout

```
/                                   ← old Web Forms app (unchanged, keep running in parallel)
  LobbyDisplay.sln                  ← old solution
  Default.aspx, SessionExpired.aspx, web.config, App_Code/, ...

LobbyDisplay.NET8.sln               ← NEW .NET 8 solution
src/
  Library.Root/                     ← portable POCOs / business logic (net8.0)
    Object/                         (Base, Binder, Log, UserProfile)
    Other/                          (BusinessLogicBase, GenericCollection)
    Control/                        (Convertion<T>)
  Library.Common/                   ← FieldSet, Generator, EnumLib (net8.0)
  Library.Oracle/                   ← Connection wrapper using Oracle.ManagedDataAccess.Core (net8.0)
  FLM_LobbyDisplay.Web/             ← NEW Razor Pages app (net8.0)
    Program.cs
    appsettings.json
    Infrastructure/
      AppSettings.cs                ← typed binding for the legacy <appSettings> block
      BasePageModel.cs              ← replaces Library.Control.Base.Page
      IUserAuthenticator.cs         ← login abstraction (real ACL impl is TODO)
      StubUserAuthenticator.cs      ← placeholder until ACL.* is ported
    Pages/
      Index.cshtml(.cs)             ← replaces Default.aspx (login)
      SessionExpired.cshtml(.cs)    ← replaces SessionExpired.aspx
      Shared/_Layout.cshtml         ← replaces master/Main.master
      Shared/_LoginLayout.cshtml    ← replaces master/FrontPage.master
    wwwroot/                        ← copied as-is from /css, /js, /image, /fonts
```

---

## What changed in the libraries

| Library | What was kept | What was dropped / changed |
|---|---|---|
| **Library.Oracle** | `Connection` ADO.NET wrapper, `Commit`/`Rollback`, `IDisposable`. | Switched from `Oracle.ManagedDataAccess` (Framework) to **`Oracle.ManagedDataAccess.Core`**. The constructor now takes a fully-resolved connection string instead of a `ConfigurationManager.ConnectionStrings[name]` lookup — the host (`Program.cs`) injects the string from `IConfiguration`. |
| **Library.Common** | `EnumLib`, `FieldSet`, `Generator` (settings + `Project()`). | `Generator.ToString()` (which rendered an HTML table by binding to `System.Web.UI.WebControls.DataGrid`) is gone — render the projected `DataTable` from a Razor view instead. |
| **Library.Root** | `Object/Base`, `Object/Binder`, `Object/Log`, `Object/UserProfile`, `Other/BusinessLogicBase`, `Other/GenericCollection`/`TotalCollection`, `Control/Convertion<T>`. | **Dropped**: `Control/Base.cs`, `Control/LogBase.cs`, `Control/MessageCenter.cs`, `Component/LocalLabel*`, all `TemplateField/*`. These are Web Forms controls with no Razor Pages equivalent — they are conceptually replaced by `_Layout.cshtml`, tag helpers, and `TempData` flash messages. **Changed**: `Object/Base.cs` no longer reads `HttpContext.Current.Session` in its constructor (defaults to empty / `DateTime.Now`); call `BasePageModel.StampAudit(entity, isNew)` before persisting. `BusinessLogicBase.MaxQuantityPerPage` is now a settable static populated from `AppSettings.MaxRowPerPage` at startup. `Convertion<T>` uses `System.Text.Json` instead of `JavaScriptSerializer`. |
| **Library.Control.Base** | — | **Dropped entirely.** Replaced by `FLM_LobbyDisplay.Web.Infrastructure.BasePageModel`. |

---

## Configuration mapping (`web.config` → `appsettings.json`)

| `web.config`                                         | `appsettings.json`                            |
|------------------------------------------------------|------------------------------------------------|
| `<appSettings><add key="MaxRowPerPage" .../>`        | `AppSettings:MaxRowPerPage`                   |
| `... key="title"`                                    | `AppSettings:Title`                           |
| `... key="SystemName"`                               | `AppSettings:SystemName`                      |
| `... key="MISSignout"` / `... key="MISHome"`         | `AppSettings:MISSignout` / `AppSettings:MISHome` |
| `<connectionStrings><add name="filmDisplay" .../>`   | `ConnectionStrings:filmDisplay` (SQL Server)  |
| `<add name="ORCL_ACL" .../>`                         | `ConnectionStrings:ORCL_ACL` (Oracle)         |
| `<authentication mode="Windows"/>` etc.              | Cookie auth in `Program.cs`                   |
| `<httpRuntime maxRequestLength="409600" .../>`       | `IIS/Kestrel` request limits in `Program.cs` (add when needed) |
| `<sessionState .../>`                                | `AddSession()` + `Session:IdleTimeoutMinutes` |

> Connection-string passwords are committed as `REPLACE_ME` placeholders.
> Set them via **user secrets** (`dotnet user-secrets`) or environment
> variables (`ConnectionStrings__filmDisplay`, `ConnectionStrings__ORCL_ACL`)
> in production. Do **not** commit real credentials.

---

## Authentication & session

The legacy app used Web Forms session keys (`Session["gstrUserID"]`,
`Session["gettemp"]`, `Session["gstrUsername"]`, `Session["gstrUserCompCode"]`,
`Session["com"]`, `Session["LoginHis"]`, `Session["system"]`) populated by
`Default.aspx.cs` after a successful ACL lookup.

Phase 1 keeps this contract intact for compatibility while pages are ported:

* On successful login `IndexModel` populates **both** the cookie principal
  (`ClaimsPrincipal` with `NameIdentifier`, `Name`, `EmployeeName`,
  `CompanyCode`) **and** the legacy session keys.
* `BasePageModel` exposes the same values via friendly properties
  (`CurrentUserId`, `CurrentUsername`, `CurrentEmployeeName`,
  `CurrentCompanyCode`) — new pages should read those, not `HttpContext.Session`
  directly.

> The actual user-validation backend (`ACL.OracleClass.User` from the legacy
> `/Bin` folder) targets .NET Framework 4.5 and **cannot** be referenced from
> .NET 8. A stub authenticator (`StubUserAuthenticator`) is wired in so the
> app starts; replace it with a real Oracle-backed implementation in a
> Phase 2 follow-up PR (either by porting the ACL source to .NET 8 or by
> writing a direct ADO.NET query against the ACL schema).

---

## Build & run

```bash
# From the repo root
dotnet build LobbyDisplay.NET8.sln
dotnet run --project src/FLM_LobbyDisplay.Web
# → http://localhost:5099 (or whatever Kestrel chooses)
```

Phase 1 was verified by:
* `dotnet build LobbyDisplay.NET8.sln` → 0 errors, 0 warnings.
* App starts; `/Index`, `/SessionExpired`, and static assets (`/css/...`,
  `/image/...`) all return 200; protected URLs redirect to `/SessionExpired`.

---

## Page-by-page porting checklist (Phase 2+)

Each follow-up PR ports **one functional area** at a time, keeping each PR
reviewable (≈200–600 LOC):

- [ ] `Menu/` (one PR) — landing menu after login
- [ ] `acc/PantryDisplay/` (one PR)
- [ ] `acc/LobbyDisplay/` (one PR)
- [ ] `acc/LobbyDisplay2/` (one PR)
- [ ] `acc/MstMain/` (one PR)
- [ ] `acc/MstMainPan/` (one PR)
- [ ] `acc/MstMainLobby2/` (one PR)
- [ ] `pages/Article/` (one PR)
- [ ] `plugin/FileManager/` — replace with a small ASP.NET Core upload page (do **not** port the TinyMCE-era ASPX file manager line-by-line)
- [ ] **Final PR**: delete the old project (`LobbyDisplay.sln`, `Default.aspx`, `SessionExpired.aspx`, `web.config`, `App_Code/`, `Bin/`, `master/`, `App_Module/`, `App_GlobalResources/`, `Backup/`, `packages/`, `plugin/FileManager/`) once every functional area is verified in a test environment.

For each ported page:
1. Identify any `App_Code` or `App_Module` (`.ascx`) helpers used by the page.
   Port the helpers as Razor partials or inject as services. **Do not** port
   `MessageCenter` / `LocalLabel` / `TemplateField/*` — use the Razor
   replacements below.
2. Add the page under `src/FLM_LobbyDisplay.Web/Pages/<Area>/<Name>.cshtml`
   with a `<Name>.cshtml.cs` `PageModel` inheriting from `BasePageModel`.
3. Run `dotnet build LobbyDisplay.NET8.sln` to verify.
4. Smoke-test the page by hand against the test database.

---

## Pattern cookbook — replacing Web Forms idioms

### `<asp:GridView>` / `<asp:Repeater>` → `foreach` in Razor
The legacy app binds `DataTable`s to a `GridView` with custom
`TemplateField` subclasses (`checkboxfield`, `deletefield`, `historyfield`,
`radiobuttonfield`, `viewfield`). In Razor:

```cshtml
@foreach (DataRow row in Model.Rows)
{
    <tr>
        <td><input type="checkbox" name="selected" value="@row["ID"]" /></td>
        <td>@row["Name"]</td>
        <td><a asp-page="./Edit" asp-route-id="@row["ID"]">edit</a></td>
        <td>
            <form method="post" asp-page-handler="Delete" asp-route-id="@row["ID"]">
                <button type="submit">delete</button>
            </form>
        </td>
    </tr>
}
```

Pagination uses `BusinessLogicBase.FromRowNo(page)` /
`BusinessLogicBase.ToRowNo(page)` (unchanged).

### Postback (`Page.IsPostBack`, `OnClick`) → handler methods
Web Forms button click handlers become **named handlers** on the `PageModel`:

```csharp
public IActionResult OnPostSave(...)   { /* btnSave_Click */ }
public IActionResult OnPostDelete(int id) { /* btnDelete_Click */ }
```

```cshtml
<button type="submit" asp-page-handler="Save">Save</button>
<form method="post" asp-page-handler="Delete" asp-route-id="@id">…</form>
```

### `ViewState` → bound properties + hidden fields
Use `[BindProperty]` on the `PageModel`. Anything that needs to round-trip
through the form goes in a hidden input that's bound on POST. For things
that should survive only the redirect after a successful POST, use
`TempData` (PRG pattern).

### `<asp:Validator>` / `ValidationSummary` → DataAnnotations + tag helpers
Annotate input properties with `[Required]`, `[StringLength]`, `[RegularExpression]`,
etc. Render errors with `<span asp-validation-for="..."></span>` and a
summary with `<div asp-validation-summary="All"></div>`.

### `MessageCenter.Show(...)` → `BasePageModel.SetFlashMessage(...)`
Sets `TempData["FlashMessage"]` / `TempData["FlashMessageType"]`; the
shared `_Layout.cshtml` renders it as a JS `alert()` after the redirect.

### `Response.Redirect("~/Menu/Menu.aspx")` → `RedirectToPage("/Menu/Menu")`
Note no `.aspx` suffix.

### `HttpContext.Current.Session["gstrUserID"]` → `BasePageModel.CurrentUserId`
And the same for `CurrentUsername`, `CurrentEmployeeName`,
`CurrentCompanyCode`. The underlying `ISession` keys are still populated for
backwards compatibility while the migration is in flight.

### Audit fields on a `Library.Root.Object.Base` subclass
Before persisting, call:

```csharp
StampAudit(entity, isNew: true);   // sets CreatedBy/Loc + UpdatedBy/Loc
// or
StampAudit(entity, isNew: false);  // updates UpdatedBy/Loc only
```

This replaces the constructor's old reliance on `HttpContext.Current.Session`.

---

## Final phase — decommission

Once every functional area listed above has been ported and verified in a
test environment, a final PR removes the legacy artefacts:

* `LobbyDisplay.sln`
* `Default.aspx`, `SessionExpired.aspx` (and their `.cs`)
* `web.config`, `web_bk.config`
* `App_Code/`, `App_Module/`, `App_GlobalResources/`
* `Bin/`, `packages/`
* `master/`
* `Library/` (the old non-SDK projects — superseded by `src/Library.*`)
* `Backup/`, `plugin/FileManager/`, `js/tiny_mce/FileManager/`
* `Menu/`, `acc/`, `pages/`, `resources/` (legacy `.aspx` siblings of the
  ported pages)

At that point the repository will contain only `LobbyDisplay.NET8.sln`,
`src/`, and supporting files (`README.md`, `MIGRATION.md`, `.gitignore`).
