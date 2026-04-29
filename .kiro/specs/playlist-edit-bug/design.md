# Playlist Edit Bug — Bugfix Design

## Overview

The video playlist detail pages have four interrelated bugs affecting all 6 page pairs (MstMain, MstMainPan, MstMainLobby2 × FullMainScreen_Dtl and Lower2ndScreen_Dtl). Bug 1: the edit form always shows "0" for Seek Start/End because `[BindProperty]` defaults override the Razor `value` expression. Bug 2: SQL statements wrap numeric SeekStart/SeekEnd in single quotes, causing a varchar-to-numeric conversion error. Bug 3: all SQL uses string interpolation, creating SQL injection vulnerabilities. Bug 4: an unvalidated `id` query parameter is interpolated into raw SQL, causing a `@row` scalar variable error when `id` is empty or malformed.

The fix strategy is: (a) populate `[BindProperty]` values from `VideoRow` in `OnGetAsync` when editing, (b) convert all SQL to parameterized queries (which also resolves the type error and injection issues), and (c) validate the `id` parameter before use.

## Glossary

- **Bug_Condition (C)**: The set of conditions that trigger the bugs — editing an existing video entry (display bug), or submitting any form with user-provided values via string-interpolated SQL (type error and injection bugs)
- **Property (P)**: The desired correct behavior — form fields display actual database values when editing; SQL uses parameterized queries with correct types; `id` is validated before use
- **Preservation**: Existing behaviors that must remain unchanged — Add form defaults, file upload flow, redirect behavior, audit field display, time field formatting
- **BindProperty**: ASP.NET Core attribute that enables two-way model binding between form inputs and PageModel properties; during GET requests the property's initialized value takes precedence over the Razor `value` attribute when the input `name` matches the property name
- **VideoRow**: A `DataRow` from the `MM_VIDEOS` table containing the current video entry's database values, loaded during `OnGetAsync` for edit mode (action=3)
- **OnGetAsync**: The Razor Page handler for HTTP GET requests; loads data for display
- **OnPostAsync**: The Razor Page handler for HTTP POST requests; processes form submissions
- **SCR_ID**: A per-page constant identifying which screen/location the video belongs to (e.g., `'1'` for MstMain/Full, `'7'` for MstMainLobby2/Full)
- **Parameterized Query**: A SQL query that uses `@paramName` placeholders and `SqlCommand.Parameters` instead of string interpolation, preventing SQL injection and ensuring correct type handling

## Bug Details

### Bug Condition

The bugs manifest across two dimensions: display (GET) and submission (POST).

**Display Bug**: When a user opens the edit form (action=3), the `[BindProperty]` properties `SeekStart` and `SeekEnd` are initialized to `"0"`. ASP.NET Core's model binding system sees that the form input `name="SeekStart"` matches the `[BindProperty]` property name, so it uses the property value (`"0"`) instead of the Razor expression `Model.VideoRow?["SEEK_START"]`. The `VideoRow` data is loaded correctly but never reaches the rendered HTML.

**SQL Type Bug**: When any form is submitted, the `OnPostAsync` handler constructs SQL via string interpolation with `SEEK_START='{SeekStart}'` and `SEEK_END='{SeekEnd}'`. The single quotes cause SQL Server to treat these as varchar literals, but the database columns `SEEK_START` and `SEEK_END` are numeric, causing an implicit conversion error.

**SQL Injection Bug**: All SQL statements (SELECT in `OnGetAsync`, INSERT and UPDATE in `OnPostAsync`) use C# string interpolation (`$"..."`) to embed user-provided values directly into SQL text. This allows arbitrary SQL injection through any form field or the `id` query parameter.

**ID Validation Bug**: The `id` query parameter from `Request.Query["id"]` is interpolated directly into the SELECT and UPDATE SQL without validation. When `id` is empty or contains non-numeric characters, the resulting SQL is malformed (e.g., `WHERE ID_MM_VIDEOS=` with nothing after it), producing errors like `Must declare the scalar variable "@row"`.

**Formal Specification:**
```
FUNCTION isBugCondition_Display(input)
  INPUT: input of type HttpGetRequest
  OUTPUT: boolean

  RETURN input.Action = 3
         AND input.VideoRow IS NOT NULL
         AND (input.VideoRow["SEEK_START"] <> 0 OR input.VideoRow["SEEK_END"] <> 0)
         AND BindProperty.SeekStart = "0"
         AND BindProperty.SeekEnd = "0"
END FUNCTION

FUNCTION isBugCondition_SqlInjection(input)
  INPUT: input of type HttpPostRequest (or HttpGetRequest for SELECT)
  OUTPUT: boolean

  RETURN sqlCommandText CONTAINS user-provided values directly
         AND sqlCommand.Parameters.Count = 0
END FUNCTION

FUNCTION isBugCondition_SqlType(input)
  INPUT: input of type HttpPostRequest
  OUTPUT: boolean

  RETURN input.Action IN {1, 3}
         AND sqlCommandText CONTAINS "SEEK_START='" OR "SEEK_END='"
         AND database column SEEK_START is numeric type
END FUNCTION

FUNCTION isBugCondition_IdValidation(input)
  INPUT: input of type HttpRequest with query parameter "id"
  OUTPUT: boolean

  RETURN (input.Query["id"] IS NULL OR input.Query["id"] = ""
          OR NOT isNumeric(input.Query["id"]))
         AND input.Action = 3
END FUNCTION
```

### Examples

- **Display Bug**: User edits video ID 42 which has `SEEK_START=15, SEEK_END=120` in the database. The form renders `<input name="SeekStart" value="0">` instead of `value="15"` because the `[BindProperty] SeekStart = "0"` default wins over the Razor expression.
- **SQL Type Bug**: User submits edit form with SeekStart=30. The generated SQL is `UPDATE MM_VIDEOS SET SEEK_START='30',...` — the `'30'` is treated as varchar, causing "Error converting data type varchar to numeric".
- **SQL Injection Bug**: User submits SeekStart value of `0; DROP TABLE MM_VIDEOS;--`. The generated SQL becomes `...SEEK_START='0; DROP TABLE MM_VIDEOS;--',...` which could execute arbitrary SQL.
- **ID Validation Bug**: A request to `?action=3&id=` (empty id) generates `SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND ID_MM_VIDEOS=` which is malformed SQL producing unexpected errors.

## Expected Behavior

### Preservation Requirements

**Unchanged Behaviors:**
- Add form (action=1) must continue to display "0" as default for Seek Start and Seek End fields
- Add form must continue to display "00:00" and "23:59" as defaults for Start Time and End Time
- File upload validation (.mp4 only) and file saving logic must remain unchanged
- Successful Add/Edit submissions must continue to redirect to the list page with a success alert
- Edit form must continue to display audit fields (Created By, Created Date, Created Loc, Updated By, Updated Date, Updated Loc) from the database
- Edit form must continue to display the PERIOD_START and PERIOD_END values trimmed to HH:mm format
- Edit form must continue to display the ATTACH_FILE name as a read-only span
- Session validation (redirect to SessionExpired) must remain unchanged
- Each page's SCR_ID constant and file save path must remain unchanged
- The `RECORD_TYP` values used in INSERT (`'1'`) and UPDATE (`'3'`) must remain unchanged

**Scope:**
All inputs that do NOT involve the four bug conditions should be completely unaffected by this fix. This includes:
- Add form rendering (no VideoRow to populate from)
- File upload mechanics and path construction
- Session checking and redirect logic
- Page routing and URL patterns
- CSS styling and HTML structure of the forms

## Hypothesized Root Cause

Based on the bug description and code analysis, the confirmed root causes are:

1. **BindProperty Default Override (Bug 1)**: The `[BindProperty]` attribute on `SeekStart` and `SeekEnd` with initializer `= "0"` causes ASP.NET Core's tag helper / model binding system to use the property value when rendering `<input name="SeekStart" ...>`. Even though the Razor view writes `value="@(Model.VideoRow?["SEEK_START"] ?? "0")"`, the model binding framework sees the `name` attribute matches a `[BindProperty]` and substitutes the property's current value. The fix is to populate `SeekStart` and `SeekEnd` from `VideoRow` in `OnGetAsync` when editing, so the `[BindProperty]` value matches the database value.

2. **String-Quoted Numeric Values (Bug 2)**: The SQL statements use `'{SeekStart}'` with single quotes around the interpolated value. Since `SEEK_START` and `SEEK_END` are numeric columns in SQL Server, wrapping the value in quotes forces an implicit varchar-to-numeric conversion that fails. Switching to parameterized queries with `SqlDbType.Decimal` (or `Int`) eliminates this entirely.

3. **String Interpolation for SQL (Bug 3)**: Every SQL statement in all 6 code-behind files uses C# string interpolation (`$"SELECT ... WHERE ID={id}"`) to embed user input directly into SQL text. This is the textbook SQL injection pattern. The fix is to use `SqlCommand.Parameters.AddWithValue()` or typed `SqlParameter` objects for all user-provided values.

4. **Unvalidated ID Parameter (Bug 4)**: `Request.Query["id"].ToString()` returns an empty string when the query parameter is missing or empty. This empty string is interpolated into `WHERE ID_MM_VIDEOS=` producing malformed SQL. The fix is to parse `id` as an integer and return an error page if it's invalid.

## Correctness Properties

Property 1: Bug Condition — Edit Form Displays Database Values

_For any_ edit request (action=3) where a valid VideoRow is loaded from the database with SEEK_START and SEEK_END values, the fixed `OnGetAsync` SHALL populate the `SeekStart` and `SeekEnd` BindProperty values from the VideoRow, so that the rendered form inputs display the actual database values instead of the default "0".

**Validates: Requirements 2.1**

Property 2: Bug Condition — SQL Uses Parameterized Queries With Correct Types

_For any_ form submission (action=1 or action=3) with user-provided SeekStart, SeekEnd, PeriodStart, PeriodEnd, and other values, the fixed `OnPostAsync` SHALL construct SQL commands using `SqlCommand.Parameters` with appropriate types (numeric for SEEK_START/SEEK_END, varchar for string fields), and the SQL command text SHALL NOT contain any user-provided values directly.

**Validates: Requirements 2.2, 2.3, 2.4**

Property 3: Bug Condition — ID Parameter Validated Before SQL Use

_For any_ request where the `id` query parameter is used (action=3 for both GET and POST), the fixed code SHALL validate that `id` is a valid positive integer before using it in any SQL query. If `id` is missing, empty, or non-numeric, the page SHALL return an error or redirect without executing SQL.

**Validates: Requirements 2.2, 2.4**

Property 4: Preservation — Add Form Defaults and Non-Edit Behavior Unchanged

_For any_ request where the bug conditions do NOT hold (action=1 Add form rendering, or any non-edit interaction), the fixed code SHALL produce the same behavior as the original code, preserving default field values ("0" for seek fields, "00:00"/"23:59" for time fields), file upload validation, redirect URLs, and session checking.

**Validates: Requirements 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 3.7**

## Fix Implementation

### Changes Required

Assuming our root cause analysis is correct, the same pattern of changes applies to all 6 code-behind files. The Razor views (`.cshtml`) do NOT need changes — the `value="@(Model.VideoRow?["SEEK_START"] ?? "0")"` expression will work correctly once the BindProperty values are populated.

**Files** (all 6 code-behind files):
- `FLM_LobbyDisplay.Web/Pages/acc/MstMain/FullMainScreen_Dtl.cshtml.cs`
- `FLM_LobbyDisplay.Web/Pages/acc/MstMain/Lower2ndScreen_Dtl.cshtml.cs`
- `FLM_LobbyDisplay.Web/Pages/acc/MstMainPan/FullMainScreen_Dtl.cshtml.cs`
- `FLM_LobbyDisplay.Web/Pages/acc/MstMainPan/Lower2ndScreen_Dtl.cshtml.cs`
- `FLM_LobbyDisplay.Web/Pages/acc/MstMainLobby2/FullMainScreen_Dtl.cshtml.cs`
- `FLM_LobbyDisplay.Web/Pages/acc/MstMainLobby2/Lower2ndScreen_Dtl.cshtml.cs`

**Specific Changes per file:**

1. **Populate BindProperty from VideoRow in OnGetAsync (Fix Bug 1)**:
   After loading `VideoRow` in the `if (Action == 3)` block, add:
   ```csharp
   SeekStart = VideoRow["SEEK_START"].ToString() ?? "0";
   SeekEnd = VideoRow["SEEK_END"].ToString() ?? "0";
   PeriodStart = VideoRow["PERIOD_START"].ToString()!.Length >= 5
       ? VideoRow["PERIOD_START"].ToString()![..5] : "00:00";
   PeriodEnd = VideoRow["PERIOD_END"].ToString()!.Length >= 5
       ? VideoRow["PERIOD_END"].ToString()![..5] : "23:59";
   ```
   This ensures the `[BindProperty]` values match the database, so Razor renders the correct values.

2. **Validate `id` parameter (Fix Bug 4)**:
   Replace `var id = Request.Query["id"].ToString();` with:
   ```csharp
   if (!int.TryParse(Request.Query["id"], out var id))
   {
       TempData["Alert"] = "Invalid video ID.";
       return Page(); // or redirect to list
   }
   ```
   Apply this in both `OnGetAsync` and `OnPostAsync` where `id` is used.

3. **Parameterize SELECT in OnGetAsync (Fix Bug 3)**:
   Replace the string-interpolated SELECT:
   ```csharp
   // Before:
   var dt = await QueryAsync($"SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND ID_MM_VIDEOS={id}");
   // After:
   var dt = await QueryAsync(
       "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND ID_MM_VIDEOS=@id",
       new SqlParameter("@id", SqlDbType.Int) { Value = id });
   ```

4. **Parameterize INSERT in OnPostAsync (Fix Bugs 2 & 3)**:
   Replace the string-interpolated INSERT with a parameterized version:
   ```csharp
   var sql = @"INSERT INTO MM_VIDEOS
       (ATTACH_FILE,SEEK_START,SEEK_END,PERIOD_START,PERIOD_END,SCR_ID,
        RECORD_TYP,CREATED_BY,CREATED_DATE,CREATED_LOC,UPDATED_BY,UPDATED_DATE,UPDATED_LOC)
       VALUES
       (@file,@seekStart,@seekEnd,@periodStart,@periodEnd,@scrId,
        @recTyp,@user,GETDATE(),@loc,@user,GETDATE(),@loc)";
   // Add SqlParameters with correct types
   ```

5. **Parameterize UPDATE in OnPostAsync (Fix Bugs 2 & 3)**:
   Replace the string-interpolated UPDATE with a parameterized version:
   ```csharp
   var sql = @"UPDATE MM_VIDEOS SET
       SEEK_START=@seekStart, SEEK_END=@seekEnd,
       PERIOD_START=@periodStart, PERIOD_END=@periodEnd,
       RECORD_TYP=@recTyp, UPDATED_BY=@user,
       UPDATED_DATE=GETDATE(), UPDATED_LOC=@loc
       WHERE ID_MM_VIDEOS=@id";
   // Add SqlParameters with correct types
   ```

6. **Update QueryAsync helper to accept parameters**:
   Change the signature to accept optional `SqlParameter[]`:
   ```csharp
   private async Task<DataTable> QueryAsync(string sql, params SqlParameter[] parameters)
   ```
   And add `cmd.Parameters.AddRange(parameters);` before execution.

7. **Parse SeekStart/SeekEnd as numeric before SQL use**:
   In `OnPostAsync`, parse the string values to `decimal` (or `int`) to ensure type safety:
   ```csharp
   if (!decimal.TryParse(SeekStart, out var seekStartVal)) seekStartVal = 0;
   if (!decimal.TryParse(SeekEnd, out var seekEndVal)) seekEndVal = 0;
   ```
   Then use `seekStartVal` and `seekEndVal` as parameter values with `SqlDbType.Decimal`.

## Testing Strategy

### Validation Approach

The testing strategy follows a two-phase approach: first, surface counterexamples that demonstrate the bugs on unfixed code, then verify the fix works correctly and preserves existing behavior. Since all 6 files share the identical pattern, testing one representative file (e.g., `MstMain/FullMainScreen_Dtl.cshtml.cs`) validates the pattern, and the remaining 5 files are verified by code inspection.

### Exploratory Bug Condition Checking

**Goal**: Surface counterexamples that demonstrate the bugs BEFORE implementing the fix. Confirm or refute the root cause analysis. If we refute, we will need to re-hypothesize.

**Test Plan**: Write unit tests that instantiate the PageModel, mock the database query to return a VideoRow with known SEEK_START/SEEK_END values, call `OnGetAsync` with action=3, and assert that the BindProperty values match the database. For the SQL bugs, inspect the generated SqlCommand to verify it uses string interpolation. Run these tests on the UNFIXED code to observe failures.

**Test Cases**:
1. **Display Bug Test**: Load edit form for a video with SEEK_START=45, SEEK_END=120. Assert `SeekStart == "45"` and `SeekEnd == "120"` — will fail on unfixed code because BindProperty defaults to "0".
2. **SQL Type Test**: Submit edit form with SeekStart="30". Inspect the SQL command text — will show `SEEK_START='30'` with quotes on unfixed code.
3. **SQL Injection Test**: Submit form with SeekStart value containing `'; DROP TABLE MM_VIDEOS;--`. Inspect SQL command text — will contain the injected SQL on unfixed code.
4. **ID Validation Test**: Call OnGetAsync with action=3 and id="" (empty). Observe that the SQL query is malformed — will produce an error on unfixed code.

**Expected Counterexamples**:
- SeekStart/SeekEnd BindProperty values remain "0" regardless of VideoRow content
- SQL command text contains user-provided values directly (no parameters)
- Numeric values wrapped in single quotes in SQL text
- Empty `id` produces malformed SQL

### Fix Checking

**Goal**: Verify that for all inputs where the bug conditions hold, the fixed functions produce the expected behavior.

**Pseudocode:**
```
// Fix Check — Display Bug
FOR ALL input WHERE isBugCondition_Display(input) DO
  result := OnGetAsync_fixed(input)
  ASSERT result.SeekStart = input.VideoRow["SEEK_START"].ToString()
  ASSERT result.SeekEnd = input.VideoRow["SEEK_END"].ToString()
END FOR

// Fix Check — SQL Parameterization
FOR ALL input WHERE isBugCondition_SqlInjection(input) DO
  sqlCmd := buildSqlCommand_fixed(input)
  ASSERT sqlCmd.Parameters.Count > 0
  ASSERT sqlCmd.CommandText NOT CONTAINS input.userValues
END FOR

// Fix Check — SQL Type
FOR ALL input WHERE isBugCondition_SqlType(input) DO
  sqlCmd := buildSqlCommand_fixed(input)
  ASSERT sqlCmd.Parameters["@seekStart"].SqlDbType = SqlDbType.Decimal
  ASSERT sqlCmd.Parameters["@seekEnd"].SqlDbType = SqlDbType.Decimal
END FOR

// Fix Check — ID Validation
FOR ALL input WHERE isBugCondition_IdValidation(input) DO
  result := OnGetAsync_fixed(input)
  ASSERT result IS ErrorPage OR RedirectResult
  ASSERT no SQL executed
END FOR
```

### Preservation Checking

**Goal**: Verify that for all inputs where the bug conditions do NOT hold, the fixed functions produce the same result as the original functions.

**Pseudocode:**
```
// Preservation — Add form defaults
FOR ALL input WHERE input.Action = 1 DO
  ASSERT OnGetAsync_fixed(input).SeekStart = "0"
  ASSERT OnGetAsync_fixed(input).SeekEnd = "0"
  ASSERT OnGetAsync_fixed(input).PeriodStart = "00:00"
  ASSERT OnGetAsync_fixed(input).PeriodEnd = "23:59"
END FOR

// Preservation — Edit form with zero values
FOR ALL input WHERE input.Action = 3 AND input.VideoRow["SEEK_START"] = 0 DO
  ASSERT OnGetAsync_fixed(input).SeekStart = "0"
END FOR

// Preservation — Successful submissions still redirect
FOR ALL input WHERE input.Action IN {1, 3} AND input.HasValidData DO
  ASSERT OnPostAsync_original(input).RedirectUrl = OnPostAsync_fixed(input).RedirectUrl
END FOR
```

**Testing Approach**: Property-based testing is recommended for preservation checking because:
- It generates many combinations of valid form inputs to verify the submission path works
- It catches edge cases in numeric parsing (e.g., SeekStart="0.5", SeekStart="999999")
- It provides strong guarantees that the Add form path is completely unaffected

**Test Plan**: Observe behavior on UNFIXED code first for Add form rendering and valid submissions, then write property-based tests capturing that behavior.

**Test Cases**:
1. **Add Form Defaults Preservation**: Verify that OnGetAsync with action=1 still sets SeekStart="0", SeekEnd="0", PeriodStart="00:00", PeriodEnd="23:59" after the fix
2. **File Upload Preservation**: Verify that .mp4 validation, file naming, and save path logic are unchanged
3. **Redirect URL Preservation**: Verify that successful Add/Edit submissions redirect to the same URLs as before
4. **Session Check Preservation**: Verify that requests without a valid session still redirect to SessionExpired
5. **Audit Fields Preservation**: Verify that VideoRow audit fields are still accessible in the Razor view after the fix

### Unit Tests

- Test that `OnGetAsync` with action=3 populates SeekStart/SeekEnd from VideoRow
- Test that `OnGetAsync` with action=3 and invalid `id` returns error without executing SQL
- Test that `OnGetAsync` with action=1 leaves SeekStart/SeekEnd at default "0"
- Test that `OnPostAsync` INSERT uses parameterized queries (inspect SqlCommand)
- Test that `OnPostAsync` UPDATE uses parameterized queries (inspect SqlCommand)
- Test that SeekStart/SeekEnd are parsed as numeric before being added as SQL parameters
- Test edge cases: SeekStart="", SeekStart="-1", SeekStart="abc", SeekStart="99999"

### Property-Based Tests

- Generate random valid numeric SeekStart/SeekEnd values and verify they round-trip correctly through OnGetAsync (loaded from VideoRow) and OnPostAsync (sent as parameters)
- Generate random string inputs for all form fields and verify no SQL injection is possible (SqlCommand.CommandText never contains the raw input)
- Generate random valid/invalid `id` values and verify the validation gate works correctly
- Generate random combinations of action values (1, 3, other) and verify correct code path selection

### Integration Tests

- Test full edit flow: load edit form → verify fields show database values → submit → verify parameterized UPDATE executes → verify redirect
- Test full add flow: load add form → verify default values → submit with .mp4 file → verify parameterized INSERT executes → verify redirect
- Test that all 6 page pairs produce consistent behavior after the fix (same pattern applied uniformly)
- Test error handling: submit with invalid data and verify appropriate error messages without SQL errors
