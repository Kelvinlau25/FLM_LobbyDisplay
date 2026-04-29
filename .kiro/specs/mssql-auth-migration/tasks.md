# Implementation Plan: MSSQL Auth Migration

## Overview

Migrate the FLM LobbyDisplay authentication subsystem from Oracle to MSSQL. Implementation proceeds bottom-up: models first, then the password hasher, then the auth service, then page integration, and finally Oracle removal. Each step builds on the previous and ends with wiring into the existing application.

## Tasks

- [x] 1. Create data models and relocate MenuResource
  - [x] 1.1 Create AuthenticatorModel in Models folder
    - Create `FLM_LobbyDisplay.Web/Models/AuthenticatorModel.cs`
    - Define all 15 properties: ID_ACL_USER, ID_ACL_ROLE, ID_ACL_RESOURCE, USER_ID, USR_EMAIL, COMPANY, EMP_NO, EMP_NAME, ROLE_NAME, ROLE_DESC, RESOURCE_NAME, RESOURCE_DESC, VALID_USER, LOGIN_ID, PASSWORD
    - Namespace: `FLM_LobbyDisplay.Models`
    - _Requirements: 1.2_

  - [x] 1.2 Relocate MenuResource to Models folder
    - Create `FLM_LobbyDisplay.Web/Models/MenuResource.cs` with the existing MenuResource class
    - Namespace: `FLM_LobbyDisplay.Models`
    - Keep same properties: ResourceID, ResourceName, ResourceURL, ResourceDesc, ParentID, AppID
    - Update `using` statements in `Menu.cshtml.cs` and `OracleAuthService.cs` to reference new namespace
    - _Requirements: 6.2_

- [x] 2. Implement Pbkdf2PasswordHasher
  - [x] 2.1 Create Pbkdf2PasswordHasher static utility
    - Create `FLM_LobbyDisplay.Web/Services/Pbkdf2PasswordHasher.cs`
    - Implement `HashPassword(string password)`: generate 16-byte random salt via `RandomNumberGenerator`, derive 32-byte key using `Rfc2898DeriveBytes` (1000 iterations, HMAC-SHA256), return Base64 of 49-byte array (0x00 header + salt + key)
    - Implement `VerifyHashedPassword(string hashedPassword, string providedPassword)`: decode Base64, validate length is 49 bytes, extract salt from bytes[1..16], recompute derived key, compare bytes[17..48] using fixed-time comparison (`CryptographicOperations.FixedTimeEquals`)
    - _Requirements: 2.1, 2.2, 2.3_

  - [ ]* 2.2 Write property test: PBKDF2 hash-then-verify round-trip
    - **Property 1: PBKDF2 hash-then-verify round-trip**
    - **Validates: Requirements 2.1, 2.2, 2.4**

  - [ ]* 2.3 Write property test: PBKDF2 unique salt per hash
    - **Property 2: PBKDF2 unique salt per hash**
    - **Validates: Requirements 2.3**

  - [ ]* 2.4 Write property test: PBKDF2 rejects wrong passwords
    - **Property 3: PBKDF2 rejects wrong passwords**
    - **Validates: Requirements 2.5**

- [x] 3. Implement IMssqlAuthService and MssqlAuthService
  - [x] 3.1 Create IMssqlAuthService interface
    - Create `FLM_LobbyDisplay.Web/Services/IMssqlAuthService.cs`
    - Define methods: `ValidateUser(string loginId, string systemName)`, `ValidateAdUser(string adUsername, string systemName)`, `GetMenuResources(int roleId, string systemName)`, `ChangePassword(string loginId, string oldPassword, string newPassword)`
    - Return types: `AuthenticatorModel`, `AuthenticatorModel`, `List<MenuResource>`, `bool`
    - _Requirements: 1.1, 6.1, 8.2, 9.1_

  - [x] 3.2 Create MssqlAuthService implementation
    - Create `FLM_LobbyDisplay.Web/Services/MssqlAuthService.cs`
    - Constructor: inject `IConfiguration` and `ILogger<MssqlAuthService>`
    - Read `ConnectionStrings:MSSQL_ACL` from config; throw `InvalidOperationException` if missing/empty
    - Implement `ValidateUser`: query ACL_USER + ACL_USER_ROLE + ACL_ROLE tables by loginId and systemName, verify password via `Pbkdf2PasswordHasher.VerifyHashedPassword`, populate and return `AuthenticatorModel`
    - Implement `ValidateAdUser`: query ACL_USER by USER_AD column + systemName join, no password check, populate `AuthenticatorModel`
    - Implement `GetMenuResources`: query ACL_RESOURCE + ACL_ROLE_RESOURCE tables by roleId, filter by systemName, return `List<MenuResource>`
    - Implement `ChangePassword`: verify old password hash, generate new hash via `Pbkdf2PasswordHasher.HashPassword`, update PASSWORD column, log event
    - All methods use `try/catch` with structured logging; return safe defaults on failure
    - Include static helper `ExtractAdUsername(string identityName)` for "DOMAIN\username" parsing
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 3.1, 3.2, 3.3, 6.1, 6.2, 6.4, 8.1, 8.2, 8.3, 9.1, 9.2, 9.3, 9.4_

  - [ ]* 3.3 Write property test: AuthenticatorModel field mapping
    - **Property 4: AuthenticatorModel field mapping preserves all fields**
    - **Validates: Requirements 1.2**

  - [ ]* 3.4 Write property test: MenuResource field mapping
    - **Property 5: MenuResource field mapping preserves all fields**
    - **Validates: Requirements 6.2**

  - [ ]* 3.5 Write property test: AD username extraction
    - **Property 6: AD username extraction**
    - **Validates: Requirements 8.1**

- [x] 4. Checkpoint - Ensure all tests pass
  - Ensure all tests pass, ask the user if questions arise.

- [x] 5. Integrate with Login page (Index.cshtml.cs)
  - [x] 5.1 Update Index.cshtml.cs to use IMssqlAuthService
    - Replace `OracleAuthService` dependency with `IMssqlAuthService`
    - Update `OnPost`: call `_auth.ValidateUser(Username.Trim(), systemName)` instead of Oracle flow
    - Check `VALID_USER` on returned `AuthenticatorModel`; redirect to Menu on success, show error on failure
    - Store session keys: `gstrUserID` (USER_ID), `gettemp` (EMP_NAME), `gstrUsername` (LOGIN_ID), `system` (systemName), `roleId` (ID_ACL_ROLE.ToString()), `LoginHis` (formatted date)
    - Keep whitespace validation unchanged (already correct)
    - Remove `GetSystemId` call (no longer needed — systemName passed directly to service)
    - _Requirements: 5.1, 5.2, 5.3, 5.4, 7.1, 7.3_

  - [ ]* 5.2 Write property test: Whitespace-only input rejection
    - **Property 7: Whitespace-only input rejection**
    - **Validates: Requirements 5.3**

- [x] 6. Integrate with Menu page (Menu.cshtml.cs)
  - [x] 6.1 Update Menu.cshtml.cs to use IMssqlAuthService
    - Replace `OracleAuthService` dependency with `IMssqlAuthService`
    - Update `BuildMenu`: read `roleId` from session, parse to int, call `_auth.GetMenuResources(roleId, systemName)`
    - Keep existing `BuildMenuFromResources` and `BuildStaticMenu` logic (fallback on empty list)
    - Keep session guard redirect to SessionExpired
    - _Requirements: 6.1, 6.3, 7.2_

- [x] 7. Update configuration and DI registration
  - [x] 7.1 Update appsettings.json
    - Add `ConnectionStrings:MSSQL_ACL` with placeholder connection string
    - Remove `ConnectionStrings:ORCL_ACL`
    - _Requirements: 3.1, 10.2_

  - [x] 7.2 Update Program.cs DI registration
    - Remove `builder.Services.AddScoped<OracleAuthService>()`
    - Add `builder.Services.AddScoped<IMssqlAuthService, MssqlAuthService>()`
    - Add `using FLM_LobbyDisplay.Models;` if needed
    - _Requirements: 4.1, 4.2, 4.3_

- [x] 8. Create ChangePassword page
  - [x] 8.1 Create ChangePassword Razor Page
    - Create `FLM_LobbyDisplay.Web/Pages/ChangePassword.cshtml` with form: old password, new password, confirm new password fields
    - Create `FLM_LobbyDisplay.Web/Pages/ChangePassword.cshtml.cs` with `IMssqlAuthService` injection
    - Implement `OnPost`: validate inputs, call `_auth.ChangePassword(loginId, oldPassword, newPassword)`, display success/error message
    - Read `loginId` from session (`gstrUsername`)
    - Include session guard (redirect to SessionExpired if not authenticated)
    - _Requirements: 9.1, 9.2, 9.3, 9.4_

- [x] 9. Checkpoint - Ensure all tests pass
  - Ensure all tests pass, ask the user if questions arise.

- [x] 10. Remove Oracle dependencies
  - [x] 10.1 Delete OracleAuthService.cs
    - Delete `FLM_LobbyDisplay.Web/Services/OracleAuthService.cs`
    - Verify no remaining references to `OracleAuthService` in the project
    - _Requirements: 10.1, 10.3_

  - [x] 10.2 Remove Library.Oracle project reference
    - Remove `<ProjectReference Include="..\Library\Library.Oracle\Library.Oracle.vbproj" />` from `FLM_LobbyDisplay.Web.csproj`
    - Verify no remaining `Oracle.ManagedDataAccess.Client` using statements in auth-related code
    - _Requirements: 4.4, 10.1, 10.4_

- [x] 11. Final checkpoint - Ensure all tests pass
  - Ensure all tests pass, ask the user if questions arise.

## Notes

- Tasks marked with `*` are optional and can be skipped for faster MVP
- Each task references specific requirements for traceability
- Checkpoints ensure incremental validation
- Property tests use FsCheck (NuGet: `FsCheck` + `FsCheck.Xunit`) for C#/.NET 8
- The `Microsoft.Data.SqlClient` package is already referenced in the project — no additional NuGet install needed
- The existing `MenuResource` class shape is preserved; only the namespace and file location change
