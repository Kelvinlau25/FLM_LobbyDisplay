# Requirements Document

## Introduction

The FLM LobbyDisplay application currently authenticates users via Oracle stored procedures through `OracleAuthService.cs`. This feature replaces the entire Oracle-based authentication and authorization (ACL) subsystem with an MSSQL-based implementation, following the reference architecture from the FILM_Sparepart_MVC system. The migration covers user credential validation, password hashing (MD5 → PBKDF2), menu/resource retrieval, session management, change-password flow, and Windows AD authentication support. Upon completion, the Oracle ACL dependency is fully removed from the application.

## Glossary

- **Auth_Service**: The new MSSQL-based authentication and authorization service that replaces `OracleAuthService`
- **Authenticator_Model**: The data model carrying user identity and role information after successful authentication (ID_ACL_USER, USER_ID, EMP_NAME, ROLE_NAME, RESOURCE_NAME, VALID_USER, etc.)
- **PBKDF2_Hasher**: The password hashing component using `Rfc2898DeriveBytes` (PBKDF2 with HMAC-SHA256) to replace MD5 hashing
- **Menu_Builder**: The component that queries MSSQL for sidebar/menu resources based on role ID and system name
- **Connection_Manager**: The configuration component that provides the MSSQL ACL connection string to the Auth_Service
- **Session_Store**: The ASP.NET Core session used to persist authenticated user details (ACL_UserObj equivalent)
- **AD_Resolver**: The component that resolves the current Windows AD identity from `HttpContext.User.Identity.Name`
- **System_Name**: The application identifier string used to look up the system in the ACL database (e.g., "PFR Film Display Signage System")
- **Role_ID**: The numeric identifier for a user's ACL role, used to retrieve permitted menu resources

## Requirements

### Requirement 1: MSSQL User Credential Validation

**User Story:** As a system administrator, I want user credentials validated against the MSSQL ACL database, so that Oracle is no longer required for authentication.

#### Acceptance Criteria

1. WHEN a user submits a username and password on the login page, THE Auth_Service SHALL query the MSSQL ACL database to validate the credentials and return an Authenticator_Model on success.
2. WHEN the MSSQL ACL database returns a matching user record with a valid hashed password, THE Auth_Service SHALL populate the Authenticator_Model with ID_ACL_USER, ID_ACL_ROLE, USER_ID, USR_EMAIL, COMPANY, EMP_NO, EMP_NAME, ROLE_NAME, ROLE_DESC, RESOURCE_NAME, RESOURCE_DESC, VALID_USER, LOGIN_ID, and PASSWORD fields.
3. WHEN the supplied username does not exist in the MSSQL ACL database, THE Auth_Service SHALL return an Authenticator_Model with VALID_USER set to false.
4. WHEN the supplied password does not match the stored hash, THE Auth_Service SHALL return an Authenticator_Model with VALID_USER set to false.
5. WHEN a database connection error occurs during validation, THE Auth_Service SHALL log the error and return an Authenticator_Model with VALID_USER set to false.

### Requirement 2: PBKDF2 Password Hashing

**User Story:** As a security engineer, I want passwords hashed using PBKDF2 (Rfc2898DeriveBytes) instead of MD5, so that stored credentials are resistant to brute-force attacks.

#### Acceptance Criteria

1. THE PBKDF2_Hasher SHALL hash passwords using `Rfc2898DeriveBytes` with HMAC-SHA256 and a cryptographically random salt.
2. WHEN verifying a password, THE PBKDF2_Hasher SHALL extract the salt from the stored hash, recompute the derived key, and compare it to the stored derived key.
3. WHEN creating a new password hash, THE PBKDF2_Hasher SHALL generate a unique random salt for each password.
4. FOR ALL valid plaintext passwords, hashing then verifying the same plaintext against the resulting hash SHALL return true (round-trip property).
5. FOR ALL distinct plaintext passwords, hashing each SHALL produce distinct hash outputs with high probability.

### Requirement 3: MSSQL Connection Configuration

**User Story:** As a DevOps engineer, I want the MSSQL ACL connection string configured in appsettings.json, so that the database endpoint is environment-configurable without code changes.

#### Acceptance Criteria

1. THE Connection_Manager SHALL read the MSSQL ACL connection string from the `ConnectionStrings:MSSQL_ACL` key in appsettings.json.
2. WHEN the `ConnectionStrings:MSSQL_ACL` key is missing or empty, THE Auth_Service SHALL log an error at startup and throw a descriptive configuration exception.
3. THE Connection_Manager SHALL use `Microsoft.Data.SqlClient` (already referenced in the project) to connect to the MSSQL ACL database.

### Requirement 4: DI Registration and Oracle Removal

**User Story:** As a developer, I want the new MSSQL Auth_Service registered in the DI container and the Oracle auth service removed, so that all consuming pages use the new implementation.

#### Acceptance Criteria

1. THE Program.cs SHALL register the Auth_Service as a scoped service in the ASP.NET Core dependency injection container.
2. THE Program.cs SHALL remove the `OracleAuthService` registration.
3. WHEN the application starts, THE Auth_Service SHALL be injectable into Razor Page models that previously depended on OracleAuthService.
4. THE FLM_LobbyDisplay.Web.csproj SHALL remove the `Library.Oracle` project reference after migration is complete.

### Requirement 5: Login Page Integration

**User Story:** As a user, I want to log in through the existing login page using MSSQL-backed authentication, so that my login experience is unchanged.

#### Acceptance Criteria

1. WHEN a user submits valid credentials on the Index page, THE Index page model SHALL call the Auth_Service to validate the user and redirect to the Menu page on success.
2. WHEN the Auth_Service returns VALID_USER as false, THE Index page model SHALL display "Invalid username and password." on the login page.
3. WHEN the username or password field is empty, THE Index page model SHALL display "Please enter User Id and Password." without calling the Auth_Service.
4. WHEN authentication succeeds, THE Index page model SHALL store the user ID, employee name, username, system identifier, and login date in the Session_Store.

### Requirement 6: Menu and Resource Retrieval from MSSQL

**User Story:** As a user, I want the sidebar menu built from MSSQL ACL resources, so that my permitted pages are displayed after login.

#### Acceptance Criteria

1. WHEN the Menu page loads for an authenticated user, THE Menu_Builder SHALL query the MSSQL ACL database using the user's Role_ID and System_Name to retrieve permitted menu resources.
2. THE Menu_Builder SHALL return a list of menu resources containing resource ID, resource name, resource URL, resource description, parent ID, and application ID for each entry.
3. WHEN the MSSQL query returns zero resources, THE Menu page model SHALL fall back to the existing static menu.
4. WHEN a database error occurs during resource retrieval, THE Menu_Builder SHALL log the error and return an empty resource list.

### Requirement 7: Session Management

**User Story:** As a user, I want my authenticated session to persist user details, so that subsequent pages can identify me without re-authentication.

#### Acceptance Criteria

1. WHEN authentication succeeds, THE Session_Store SHALL persist the Authenticator_Model fields: user ID (gstrUserID), employee name (gettemp), username (gstrUsername), system identifier (system), role ID, and login date (LoginHis).
2. WHEN the Menu page is accessed without a valid session, THE Menu page model SHALL redirect the user to the SessionExpired page.
3. WHEN the user navigates to the Index page, THE Index page model SHALL clear the existing session.

### Requirement 8: Windows AD Authentication Support

**User Story:** As a domain user, I want the system to resolve my Windows AD identity, so that the application can support AD-based login in addition to form-based login.

#### Acceptance Criteria

1. WHEN Windows Authentication is enabled, THE AD_Resolver SHALL extract the user's AD username from `HttpContext.User.Identity.Name`.
2. WHEN an AD username is resolved, THE Auth_Service SHALL validate the AD username against the MSSQL ACL database using the `ValidateUserInfo(userAD, systemName)` query pattern.
3. IF the AD username is not found in the MSSQL ACL database, THEN THE Auth_Service SHALL return an Authenticator_Model with VALID_USER set to false.

### Requirement 9: Change Password Flow

**User Story:** As a user, I want to change my password through the application, so that I can update my credentials securely.

#### Acceptance Criteria

1. WHEN a user submits a change-password request with the correct old password and a new password, THE Auth_Service SHALL verify the old password against the stored PBKDF2 hash.
2. WHEN old password verification succeeds, THE Auth_Service SHALL hash the new password using the PBKDF2_Hasher and update the stored hash in the MSSQL ACL database.
3. IF the old password does not match the stored hash, THEN THE Auth_Service SHALL reject the change request and return an error indication.
4. WHEN the new password is successfully stored, THE Auth_Service SHALL log the password change event for the user.

### Requirement 10: Oracle Dependency Removal

**User Story:** As a developer, I want all Oracle authentication dependencies removed from the project, so that the application no longer requires Oracle client libraries for ACL.

#### Acceptance Criteria

1. THE application SHALL not reference `Oracle.ManagedDataAccess.Client` for authentication or ACL purposes after migration.
2. THE appsettings.json SHALL remove the `ORCL_ACL` connection string after migration is complete.
3. THE `OracleAuthService.cs` file SHALL be deleted or archived after the Auth_Service is fully operational.
4. THE FLM_LobbyDisplay.Web.csproj SHALL remove the `Library.Oracle` project reference after migration is complete.
