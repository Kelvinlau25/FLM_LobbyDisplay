# Implementation Plan: Library .csproj .NET 8 Migration

## Overview

Migrate three C# library projects from .NET Framework 4.5 old-style MSBuild to .NET 8.0 SDK-style format, removing all System.Web dependencies by mirroring the already-migrated VB.NET reference implementations. Build order: Library.Root first, then Library.Common + Library.Control.Base, then update web project references. Verification is via `dotnet build`.

## Tasks

- [x] 1. Convert Library.Root to SDK-style .csproj and migrate all C# source files
  - [x] 1.1 Replace Library.Root.csproj with SDK-style format targeting net8.0
    - Overwrite `Library/Library.Root/Library.Root.csproj` with minimal SDK-style project file
    - Set `<TargetFramework>net8.0</TargetFramework>`, `<RootNamespace>Library.Root</RootNamespace>`, `<AssemblyName>Library.Root</AssemblyName>`, `<Nullable>disable</Nullable>`
    - Add PackageReference for `System.Configuration.ConfigurationManager` version 8.0.0
    - Remove all explicit `<Compile Include>`, `<Reference Include>`, and old-style PropertyGroup conditions
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5_

  - [x] 1.2 Create IPageContext interface in C#
    - Create new file `Library/Library.Root/Control/IPageContext.cs` mirroring `IPageContext.vb`
    - Define in `Library.Root.Control` namespace with `Redirect`, `ResolveUrl`, `UrlDecode`, `UrlEncode` methods
    - _Requirements: 5.1, 5.2, 5.3, 5.4, 5.5_

  - [x] 1.3 Migrate Control/Base.cs — remove System.Web.UI.Page inheritance
    - Rewrite `Library/Library.Root/Control/Base.cs` following `Base.vb` reference
    - Remove `System.Web.UI.Page` inheritance, make abstract class
    - Add constructor accepting `IPageContext`, `NameValueCollection`, `bool isPostBack`
    - Constructor calls `BindAction`, `BindSort`, `CheckURL`, `BindKey` in original lifecycle order
    - Replace `Response.Redirect` → `_pageContext.Redirect`, `ResolveUrl` → `_pageContext.ResolveUrl`
    - Replace `Server.UrlEncode`/`UrlDecode` → `Uri.EscapeDataString`/`Uri.UnescapeDataString`
    - Replace `Request.QueryString` → injected `NameValueCollection`
    - Replace `ViewState` → `Dictionary<string, object>`
    - Remove all GridView-related properties, event handlers, and Web Forms UI logic
    - Remove `using System.Web.UI.WebControls`
    - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.5, 4.6, 4.7, 4.8, 4.9, 4.10_

  - [x] 1.4 Migrate Control/LogBase.cs — remove System.Web.UI.Page inheritance
    - Rewrite `Library/Library.Root/Control/LogBase.cs` following `LogBase.vb` reference
    - Remove `System.Web.UI.Page` inheritance, make abstract class
    - Add constructor accepting `NameValueCollection`
    - Constructor calls `BindKey()` then `BindData()`
    - Replace `Server.UrlDecode`/`UrlEncode` → `Uri.UnescapeDataString`/`Uri.EscapeDataString`
    - Replace `Request.QueryString` → injected `NameValueCollection`
    - Replace `ResolveUrl` with direct string concatenation in `GenerateList`
    - _Requirements: 6.1, 6.2, 6.3, 6.4, 6.5, 6.6_

  - [x] 1.5 Migrate Control/Convertion.cs — replace JavaScriptSerializer with System.Text.Json
    - Rewrite `Library/Library.Root/Control/Convertion.cs` following `Convertion.vb` reference
    - Replace `JavaScriptSerializer` with `System.Text.Json.JsonSerializer`
    - `Serializer` → `JsonSerializer.Serialize(list)`
    - `Deserializer` → `JsonSerializer.Deserialize<List<T>>(StringFormat)`
    - Remove `using System.Web.Script.Serialization`
    - _Requirements: 7.1, 7.2, 7.3_

  - [x] 1.6 Migrate Control/MessageCenter.cs — remove System.Web dependencies
    - Rewrite `Library/Library.Root/Control/MessageCenter.cs` following `MessageCenter.vb` reference
    - Replace `ShowAJAXMessageBox` and `ShowJqueryMessageBox` with single `BuildAlertScript(string message)` returning script string
    - Use `System.Net.WebUtility.HtmlEncode` for encoding
    - Remove references to `System.Web.UI.Page`, `ScriptManager`, `HttpContext`, `ClientScript`
    - _Requirements: 9.1, 9.2, 9.3_

  - [x] 1.7 Migrate Object/Base.cs — remove System.Web.HttpContext dependency
    - Rewrite `Library/Library.Root/Object/Base.cs` following `Base.vb` reference
    - Initialize `CreatedBy`, `CreatedLoc`, `UpdatedBy`, `UpdatedLoc` to `string.Empty`
    - Remove `using System.Web` and all `HttpContext.Current` references
    - _Requirements: 8.1, 8.2_

  - [x] 1.8 Migrate remaining Library.Root files that have no System.Web dependencies
    - Review and update `Library/Library.Root/Object/Binder.cs` following `Binder.vb`
    - Review and update `Library/Library.Root/Object/Log.cs` following `Log.vb`
    - Review and update `Library/Library.Root/Object/UserProfile.cs` following `UserProfile.vb`
    - Review and update `Library/Library.Root/Control/GenericCollection.cs` following `GenericCollection.vb`
    - Review and update `Library/Library.Root/Other/BusinessLogicBase.cs` following `BusinessLogicBase.vb`
    - Ensure no System.Web references remain in any of these files
    - _Requirements: 1.5, 17.1_

  - [x] 1.9 Convert TemplateField classes to stubs
    - Rewrite each of `checkboxfield.cs`, `deletefield.cs`, `historyfield.cs`, `radiobuttonfield.cs`, `viewfield.cs` following their `.vb` counterparts
    - Each class becomes a minimal stub with only a constructor accepting `int type`
    - Remove `ITemplate` implementation, `InstantiateIn`, all Web Forms control creation
    - Remove `using System.Web.UI` and `using System.Web.UI.WebControls`
    - _Requirements: 10.1, 10.2, 10.3_

  - [x] 1.10 Convert LocalLabel to a stub and remove Designer file
    - Rewrite `Library/Library.Root/Component/LocalLabel.cs` following `LocalLabel.vb`
    - Remove `WebControl` inheritance, retain `Key` property and `Text` property returning `string.Empty`
    - Remove `RenderBeginTag`, `RenderContents`, `OnInit`, `ResourceManager`, `ViewState`, `HtmlTextWriter`
    - Empty or remove `Library/Library.Root/Component/LocalLabel.Designer.cs`
    - _Requirements: 11.1, 11.2, 11.3, 11.4_

  - [x] 1.11 Remove Library.Root Properties/AssemblyInfo.cs
    - Delete `Library/Library.Root/Properties/AssemblyInfo.cs`
    - SDK-style projects auto-generate assembly attributes; keeping the old file causes duplicate attribute errors
    - _Requirements: 16.1_

  - [x] 1.12 Build verification for Library.Root
    - Run `dotnet build Library/Library.Root/Library.Root.csproj`
    - Fix any build errors until zero errors achieved
    - _Requirements: 17.1_

- [x] 2. Convert Library.Common to SDK-style .csproj and migrate C# source files
  - [x] 2.1 Replace Library.Common.csproj with SDK-style format targeting net8.0
    - Overwrite `Library/Library.Common/Library.Common.csproj` with minimal SDK-style project file
    - Set `<TargetFramework>net8.0</TargetFramework>`, `<RootNamespace>Library.common</RootNamespace>`, `<AssemblyName>Library.common</AssemblyName>`, `<Nullable>disable</Nullable>`
    - Add PackageReference for `System.Configuration.ConfigurationManager` version 8.0.0
    - Add ProjectReference to `..\Library.Root\Library.Root.csproj`
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5, 2.6_

  - [x] 2.2 Migrate Component/Generator.cs — remove System.Web.UI dependencies
    - Rewrite `Library/Library.Common/Component/Generator.cs` following `Generator.vb` reference
    - Replace `DataGrid`/`HtmlTextWriter` rendering with `StringBuilder`-based manual HTML table construction
    - Use `System.Net.WebUtility.HtmlEncode` for encoding cell values
    - Remove `using System.Web.UI`, `using System.Web.UI.WebControls`, `using System.IO` (StringWriter)
    - _Requirements: 14.1, 14.2, 14.3_

  - [x] 2.3 Review remaining Library.Common files
    - Review `Library/Library.Common/Component/EnumLib.cs` against `EnumLib.vb` for any System.Web references
    - Review `Library/Library.Common/Object/FieldSet.cs` against `FieldSet.vb` for any System.Web references
    - Update if needed to remove any legacy dependencies
    - _Requirements: 2.6, 17.2_

  - [x] 2.4 Remove Library.Common Properties/AssemblyInfo.cs
    - Delete `Library/Library.Common/Properties/AssemblyInfo.cs`
    - _Requirements: 16.2_

  - [x] 2.5 Build verification for Library.Common
    - Run `dotnet build Library/Library.Common/Library.Common.csproj`
    - Fix any build errors until zero errors achieved
    - _Requirements: 17.2_

- [x] 3. Convert Library.Control.Base to SDK-style .csproj and migrate C# source files
  - [x] 3.1 Replace Library.Control.Base.csproj with SDK-style format targeting net8.0
    - Overwrite `Library/Library.Control.Base/Library.Control.Base.csproj` with minimal SDK-style project file
    - Set `<TargetFramework>net8.0</TargetFramework>`, `<RootNamespace>Library.Control.Base</RootNamespace>`, `<AssemblyName>Library.Control.Base</AssemblyName>`, `<Nullable>disable</Nullable>`
    - Add PackageReference for `System.Configuration.ConfigurationManager` version 8.0.0
    - Add ProjectReference to `..\Library.Root\Library.Root.csproj`
    - _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5_

  - [x] 3.2 Migrate Page.cs — remove System.Web.UI.Page inheritance
    - Rewrite `Library/Library.Control.Base/Page.cs` following `Page.vb` reference
    - Remove `System.Web.UI.Page` inheritance, make abstract class
    - Remove `ControlPanel` property, `Remove` method, `_list` field
    - Retain properties: `Action`, `CurrentStep`, `Worksno`, `CelNo`, `CompCode`, `Reqno`
    - _Requirements: 12.1, 12.2, 12.3_

  - [x] 3.3 Migrate UserControl.cs — remove System.Web.UI.UserControl inheritance
    - Rewrite `Library/Library.Control.Base/UserControl.cs` following `UserControl.vb` reference
    - Remove `System.Web.UI.UserControl` inheritance, make abstract class
    - Retain `EditMode`, `ValidationGroup` properties and all abstract method signatures
    - _Requirements: 13.1, 13.2_

  - [x] 3.4 Remove Library.Control.Base Properties/AssemblyInfo.cs
    - Delete `Library/Library.Control.Base/Properties/AssemblyInfo.cs`
    - _Requirements: 16.3_

  - [x] 3.5 Build verification for Library.Control.Base
    - Run `dotnet build Library/Library.Control.Base/Library.Control.Base.csproj`
    - Fix any build errors until zero errors achieved
    - _Requirements: 17.3_

- [x] 4. Update web project references and final build verification
  - [x] 4.1 Update FLM_LobbyDisplay.Web.csproj project references
    - Replace `Library.Root.vbproj` → `Library.Root.csproj`
    - Replace `Library.Common.vbproj` → `Library.Common.csproj`
    - Replace `Library.Control.Base.vbproj` → `Library.Control.Base.csproj`
    - _Requirements: 15.1, 15.2, 15.3_

  - [x] 4.2 Full solution build verification
    - Run `dotnet build FLM_LobbyDisplay.Web/FLM_LobbyDisplay.Web.csproj`
    - This builds all library dependencies transitively
    - Fix any build errors until zero errors achieved
    - _Requirements: 17.1, 17.2, 17.3, 17.4_

- [x] 5. Final checkpoint
  - Ensure all projects build with zero errors, ask the user if questions arise.

## Notes

- VB.NET files (.vb) are the authoritative reference for every C# migration change
- Build order must be respected: Library.Root → Library.Common + Library.Control.Base → Web project
- No test framework exists; verification is exclusively via `dotnet build`
- SDK-style projects auto-include all .cs files, so no explicit `<Compile Include>` items are needed
- SDK-style projects will NOT accidentally include .vb files (language-specific auto-inclusion)
