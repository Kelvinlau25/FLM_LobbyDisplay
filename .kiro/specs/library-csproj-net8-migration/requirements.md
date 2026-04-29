# Requirements Document

## Introduction

Migrate the three C# library projects (Library.Root, Library.Common, Library.Control.Base) from .NET Framework 4.5 old-style MSBuild format to .NET 8.0 SDK-style format. The C# source files contain System.Web dependencies (System.Web.UI, System.Web.UI.WebControls, System.Web.Script.Serialization, HttpContext) that do not exist in .NET 8. The VB.NET versions of these libraries have already been successfully migrated to net8.0 and serve as the reference implementation for how System.Web dependencies were removed. After migration, the FLM_LobbyDisplay.Web project should reference the C# .csproj files instead of the VB.NET .vbproj files and build successfully.

## Glossary

- **Library.Root**: The foundational C# class library project located at `Library/Library.Root/`. Contains base page classes, object models, template fields, serialization helpers, and utility classes.
- **Library.Common**: The shared C# class library project located at `Library/Library.Common/`. Contains HTML generator, field set definitions, and enum definitions. Depends on Library.Root.
- **Library.Control.Base**: The C# class library project located at `Library/Library.Control.Base/`. Contains abstract base classes for pages and user controls. References System.Web.UI.Page and System.Web.UI.UserControl.
- **Web_Project**: The ASP.NET Core Razor Pages web application at `FLM_LobbyDisplay.Web/`. Currently references the VB.NET .vbproj versions of the libraries.
- **SDK_Style_Project_File**: The modern .NET project file format introduced with .NET Core that uses `<Project Sdk="Microsoft.NET.Sdk">` and does not require explicit file listings or assembly references.
- **Old_Style_Project_File**: The legacy MSBuild project file format that uses `ToolsVersion`, explicit `<Compile Include>` items, and `<Reference Include>` for framework assemblies.
- **IPageContext**: An interface defined in Library.Root that abstracts Redirect, ResolveUrl, UrlDecode, and UrlEncode operations, replacing direct System.Web.UI.Page dependency.
- **System.Web_Dependencies**: APIs from the System.Web namespace including System.Web.UI.Page, System.Web.UI.WebControls, System.Web.UI.UserControl, System.Web.Script.Serialization.JavaScriptSerializer, System.Web.HttpContext, and System.Web.UI.ScriptManager.
- **VB_Reference_Implementation**: The already-migrated VB.NET (.vbproj) versions of the three libraries that target net8.0 and demonstrate the correct migration patterns.
- **TemplateField_Classes**: Web Forms GridView template field classes (checkboxfield, deletefield, historyfield, radiobuttonfield, viewfield) that implement System.Web.UI.ITemplate.
- **Migration_Build**: A successful `dotnet build` of the solution after all migration changes are applied.

## Requirements

### Requirement 1: Convert Library.Root.csproj to SDK-Style Format

**User Story:** As a developer, I want the Library.Root.csproj converted from old-style MSBuild format to SDK-style format targeting net8.0, so that the project uses the modern .NET build system.

#### Acceptance Criteria

1. THE Library.Root.csproj SHALL use the `<Project Sdk="Microsoft.NET.Sdk">` format with `<TargetFramework>net8.0</TargetFramework>`.
2. THE Library.Root.csproj SHALL set `<RootNamespace>Library.Root</RootNamespace>` and `<AssemblyName>Library.Root</AssemblyName>`.
3. THE Library.Root.csproj SHALL set `<Nullable>disable</Nullable>` to match the VB_Reference_Implementation.
4. THE Library.Root.csproj SHALL include a PackageReference for `System.Configuration.ConfigurationManager` version 8.0.0.
5. THE Library.Root.csproj SHALL NOT contain explicit `<Compile Include>` items, `<Reference Include>` items for framework assemblies, or old-style PropertyGroup conditions.

### Requirement 2: Convert Library.Common.csproj to SDK-Style Format

**User Story:** As a developer, I want the Library.Common.csproj converted from old-style MSBuild format to SDK-style format targeting net8.0, so that the project uses the modern .NET build system.

#### Acceptance Criteria

1. THE Library.Common.csproj SHALL use the `<Project Sdk="Microsoft.NET.Sdk">` format with `<TargetFramework>net8.0</TargetFramework>`.
2. THE Library.Common.csproj SHALL set `<RootNamespace>Library.common</RootNamespace>` and `<AssemblyName>Library.common</AssemblyName>` to preserve the original casing.
3. THE Library.Common.csproj SHALL set `<Nullable>disable</Nullable>` to match the VB_Reference_Implementation.
4. THE Library.Common.csproj SHALL include a PackageReference for `System.Configuration.ConfigurationManager` version 8.0.0.
5. THE Library.Common.csproj SHALL include a ProjectReference to `..\Library.Root\Library.Root.csproj`.
6. THE Library.Common.csproj SHALL NOT contain explicit `<Compile Include>` items, `<Reference Include>` items for framework assemblies, or old-style PropertyGroup conditions.

### Requirement 3: Convert Library.Control.Base.csproj to SDK-Style Format

**User Story:** As a developer, I want the Library.Control.Base.csproj converted from old-style MSBuild format to SDK-style format targeting net8.0, so that the project uses the modern .NET build system.

#### Acceptance Criteria

1. THE Library.Control.Base.csproj SHALL use the `<Project Sdk="Microsoft.NET.Sdk">` format with `<TargetFramework>net8.0</TargetFramework>`.
2. THE Library.Control.Base.csproj SHALL set `<RootNamespace>Library.Control.Base</RootNamespace>` and `<AssemblyName>Library.Control.Base</AssemblyName>`.
3. THE Library.Control.Base.csproj SHALL set `<Nullable>disable</Nullable>` to match the VB_Reference_Implementation.
4. THE Library.Control.Base.csproj SHALL include a PackageReference for `System.Configuration.ConfigurationManager` version 8.0.0.
5. THE Library.Control.Base.csproj SHALL NOT contain explicit `<Compile Include>` items, `<Reference Include>` items for framework assemblies, or old-style PropertyGroup conditions.

### Requirement 4: Remove System.Web.UI.Page Inheritance from Control.Base

**User Story:** As a developer, I want the Control.Base class in Library.Root to no longer inherit from System.Web.UI.Page, so that the library compiles under net8.0.

#### Acceptance Criteria

1. THE Control.Base class SHALL NOT inherit from System.Web.UI.Page.
2. THE Control.Base class SHALL accept an IPageContext, a NameValueCollection for query string parameters, and a boolean isPostBack flag via its constructor.
3. WHEN the Control.Base constructor is called, THE Control.Base class SHALL invoke BindAction, BindSort, CheckURL, and BindKey in the same order as the original OnInit and OnLoad lifecycle.
4. THE Control.Base class SHALL replace all calls to `Response.Redirect` with calls to `IPageContext.Redirect`.
5. THE Control.Base class SHALL replace all calls to `ResolveUrl` with calls to `IPageContext.ResolveUrl`.
6. THE Control.Base class SHALL replace all calls to `Server.UrlEncode` and `Server.UrlDecode` with `Uri.EscapeDataString` and `Uri.UnescapeDataString`.
7. THE Control.Base class SHALL replace `Request.QueryString` access with the injected NameValueCollection parameter.
8. THE Control.Base class SHALL replace `ViewState` with an in-memory `Dictionary<string, object>`.
9. THE Control.Base class SHALL remove all GridView-related properties, event handlers, and Web Forms UI logic (OnLoad GridView column setup, Sorting handler, RowCreated handler, RowDataBound handler).
10. THE Control.Base class SHALL remove the `using System.Web.UI.WebControls` directive.

### Requirement 5: Create IPageContext Interface in C#

**User Story:** As a developer, I want an IPageContext interface defined in C# within Library.Root, so that the migrated C# classes can abstract page-level operations without depending on System.Web.

#### Acceptance Criteria

1. THE IPageContext interface SHALL be defined in the `Library.Root.Control` namespace.
2. THE IPageContext interface SHALL declare a `Redirect(string url)` method.
3. THE IPageContext interface SHALL declare a `ResolveUrl(string relativeUrl)` method returning a string.
4. THE IPageContext interface SHALL declare a `UrlDecode(string value)` method returning a string.
5. THE IPageContext interface SHALL declare a `UrlEncode(string value)` method returning a string.

### Requirement 6: Remove System.Web.UI.Page Inheritance from LogBase

**User Story:** As a developer, I want the LogBase class in Library.Root to no longer inherit from System.Web.UI.Page, so that the library compiles under net8.0.

#### Acceptance Criteria

1. THE LogBase class SHALL NOT inherit from System.Web.UI.Page.
2. THE LogBase class SHALL accept a NameValueCollection for query string parameters via its constructor.
3. WHEN the LogBase constructor is called, THE LogBase class SHALL invoke BindKey and BindData in the same order as the original OnInit lifecycle.
4. THE LogBase class SHALL replace all calls to `Server.UrlDecode` and `Server.UrlEncode` with `Uri.UnescapeDataString` and `Uri.EscapeDataString`.
5. THE LogBase class SHALL replace `Request.QueryString` access with the injected NameValueCollection parameter.
6. THE LogBase class SHALL replace `ResolveUrl` calls with direct string concatenation in the GenerateList property, matching the VB_Reference_Implementation pattern.

### Requirement 7: Replace JavaScriptSerializer in Convertion Class

**User Story:** As a developer, I want the Convertion class to use System.Text.Json instead of System.Web.Script.Serialization.JavaScriptSerializer, so that JSON serialization works under net8.0.

#### Acceptance Criteria

1. THE Convertion class SHALL use `System.Text.Json.JsonSerializer` for the Serializer method.
2. THE Convertion class SHALL use `System.Text.Json.JsonSerializer` for the Deserializer method.
3. THE Convertion class SHALL NOT reference `System.Web.Script.Serialization` or `JavaScriptSerializer`.

### Requirement 8: Remove System.Web.HttpContext from Object.Base

**User Story:** As a developer, I want the Object.Base class to no longer depend on System.Web.HttpContext for session and request data, so that the library compiles under net8.0.

#### Acceptance Criteria

1. THE Object.Base constructor SHALL initialize CreatedBy, CreatedLoc, UpdatedBy, and UpdatedLoc to `string.Empty` instead of reading from HttpContext.Current.Session or HttpContext.Current.Request.
2. THE Object.Base class SHALL NOT reference `System.Web.HttpContext` or the `System.Web` namespace.

### Requirement 9: Remove System.Web from MessageCenter

**User Story:** As a developer, I want the MessageCenter class to no longer depend on System.Web.UI.Page, System.Web.HttpContext, or System.Web.UI.ScriptManager, so that the library compiles under net8.0.

#### Acceptance Criteria

1. THE MessageCenter class SHALL replace the `ShowAJAXMessageBox` and `ShowJqueryMessageBox` methods with a single `BuildAlertScript(string message)` method that returns a script string.
2. THE MessageCenter class SHALL use `System.Web.HttpUtility.HtmlEncode` for HTML encoding (available via the `System.Web.HttpUtility` NuGet compatibility package or `System.Net.WebUtility.HtmlEncode`).
3. THE MessageCenter class SHALL NOT reference System.Web.UI.Page, System.Web.UI.ScriptManager, System.Web.HttpContext, or ClientScript.

### Requirement 10: Convert TemplateField Classes to Stubs

**User Story:** As a developer, I want the TemplateField classes (checkboxfield, deletefield, historyfield, radiobuttonfield, viewfield) converted to minimal stubs, so that the library compiles under net8.0 while retaining source compatibility.

#### Acceptance Criteria

1. THE checkboxfield, deletefield, historyfield, radiobuttonfield, and viewfield classes SHALL each be converted to a minimal stub class with only a constructor accepting an integer parameter.
2. THE TemplateField_Classes SHALL NOT implement System.Web.UI.ITemplate or reference any System.Web.UI types.
3. THE TemplateField_Classes SHALL NOT contain InstantiateIn methods or Web Forms control creation logic.

### Requirement 11: Convert LocalLabel to a Stub

**User Story:** As a developer, I want the LocalLabel class converted to a minimal stub without System.Web.UI.WebControls.WebControl inheritance, so that the library compiles under net8.0.

#### Acceptance Criteria

1. THE LocalLabel class SHALL NOT inherit from System.Web.UI.WebControls.WebControl.
2. THE LocalLabel class SHALL retain a Key property and a Text property returning `string.Empty`.
3. THE LocalLabel class SHALL NOT reference System.Web.UI, HtmlTextWriter, ViewState, or ResourceManager with App_GlobalResources.
4. THE LocalLabel.Designer.cs file SHALL be removed or emptied since the designer pattern depends on WebControl.

### Requirement 12: Remove System.Web.UI.Page Inheritance from Library.Control.Base.Page

**User Story:** As a developer, I want the Page class in Library.Control.Base to no longer inherit from System.Web.UI.Page, so that the library compiles under net8.0.

#### Acceptance Criteria

1. THE Library.Control.Base.Page class SHALL NOT inherit from System.Web.UI.Page.
2. THE Library.Control.Base.Page class SHALL be an abstract class with the same properties (Action, CurrentStep, Worksno, CelNo, CompCode, Reqno) as the original.
3. THE Library.Control.Base.Page class SHALL NOT contain the ControlPanel property, the Remove method, or the _list field that depend on System.Web.UI types (PlaceHolder, Control).

### Requirement 13: Remove System.Web.UI.UserControl Inheritance from Library.Control.Base.UserControl

**User Story:** As a developer, I want the UserControl class in Library.Control.Base to no longer inherit from System.Web.UI.UserControl, so that the library compiles under net8.0.

#### Acceptance Criteria

1. THE Library.Control.Base.UserControl class SHALL NOT inherit from System.Web.UI.UserControl.
2. THE Library.Control.Base.UserControl class SHALL be an abstract class with the same properties (EditMode, ValidationGroup) and abstract method signatures as the original.

### Requirement 14: Remove System.Web from Library.Common Generator

**User Story:** As a developer, I want the Generator class in Library.Common to no longer depend on System.Web.UI.HtmlTextWriter or System.Web.UI.WebControls.DataGrid, so that the library compiles under net8.0.

#### Acceptance Criteria

1. THE Generator.ToString method SHALL generate HTML using StringBuilder with manual table/row/cell construction instead of System.Web.UI.WebControls.DataGrid.
2. THE Generator class SHALL use `System.Net.WebUtility.HtmlEncode` or `System.Web.HttpUtility.HtmlEncode` for encoding cell values.
3. THE Generator class SHALL NOT reference System.Web.UI.HtmlTextWriter, System.Web.UI.WebControls.DataGrid, or System.Web.UI.Control.

### Requirement 15: Update Web Project References

**User Story:** As a developer, I want the FLM_LobbyDisplay.Web project to reference the C# .csproj files instead of the VB.NET .vbproj files, so that the web application uses the migrated C# libraries.

#### Acceptance Criteria

1. THE Web_Project csproj SHALL replace the ProjectReference to `Library.Root.vbproj` with a ProjectReference to `Library.Root.csproj`.
2. THE Web_Project csproj SHALL replace the ProjectReference to `Library.Common.vbproj` with a ProjectReference to `Library.Common.csproj`.
3. THE Web_Project csproj SHALL replace the ProjectReference to `Library.Control.Base.vbproj` with a ProjectReference to `Library.Control.Base.csproj`.

### Requirement 16: Remove Obsolete AssemblyInfo Files

**User Story:** As a developer, I want the old Properties/AssemblyInfo.cs files removed from each library project, so that they do not conflict with auto-generated assembly attributes in SDK-style projects.

#### Acceptance Criteria

1. THE Properties/AssemblyInfo.cs file in Library.Root SHALL be removed or the project SHALL set `<GenerateAssemblyInfo>false</GenerateAssemblyInfo>` to avoid duplicate attribute errors.
2. THE Properties/AssemblyInfo.cs file in Library.Common SHALL be removed or the project SHALL set `<GenerateAssemblyInfo>false</GenerateAssemblyInfo>` to avoid duplicate attribute errors.
3. THE Properties/AssemblyInfo.cs file in Library.Control.Base SHALL be removed or the project SHALL set `<GenerateAssemblyInfo>false</GenerateAssemblyInfo>` to avoid duplicate attribute errors.

### Requirement 17: Successful Build Verification

**User Story:** As a developer, I want the entire solution to build successfully after migration, so that I can confirm the migration is complete.

#### Acceptance Criteria

1. WHEN `dotnet build` is executed for Library.Root.csproj, THE Migration_Build SHALL complete with zero errors.
2. WHEN `dotnet build` is executed for Library.Common.csproj, THE Migration_Build SHALL complete with zero errors.
3. WHEN `dotnet build` is executed for Library.Control.Base.csproj, THE Migration_Build SHALL complete with zero errors.
4. WHEN `dotnet build` is executed for FLM_LobbyDisplay.Web.csproj, THE Migration_Build SHALL complete with zero errors.
