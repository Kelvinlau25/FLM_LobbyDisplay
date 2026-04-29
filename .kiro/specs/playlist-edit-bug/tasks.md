# Implementation Plan

- [ ] 1. Write bug condition exploration test
  - **Property 1: Bug Condition** - Edit Form Display & SQL Injection Bugs
  - **CRITICAL**: This test MUST FAIL on unfixed code - failure confirms the bugs exist
  - **DO NOT attempt to fix the test or the code when it fails**
  - **NOTE**: This test encodes the expected behavior - it will validate the fix when it passes after implementation
  - **GOAL**: Surface counterexamples that demonstrate the bugs exist across the 4 bug conditions
  - **Scoped PBT Approach**: Since there is no test framework set up, write a manual verification script (C# console app or inline code review) that inspects the code-behind files for the bug patterns:
    - **Display Bug**: In `OnGetAsync`, verify that when `Action == 3` and `VideoRow` is loaded, the `SeekStart` and `SeekEnd` BindProperty values are NOT populated from `VideoRow`. On unfixed code, the BindProperty defaults ("0") override the Razor `value` expression. Counterexample: editing video ID 42 with `SEEK_START=15, SEEK_END=120` renders `value="0"` instead of `value="15"`.
    - **SQL Type Bug**: In `OnPostAsync`, verify that INSERT and UPDATE SQL statements wrap `SeekStart`/`SeekEnd` in single quotes (e.g., `SEEK_START='{SeekStart}'`), causing varchar-to-numeric conversion errors on numeric columns. Counterexample: submitting SeekStart=30 generates `SEEK_START='30'` which fails.
    - **SQL Injection Bug**: In both `OnGetAsync` and `OnPostAsync`, verify that SQL statements use string interpolation (`$"..."`) with user-provided values embedded directly. Counterexample: `$"SELECT * FROM MM_VIDEOS WHERE ... ID_MM_VIDEOS={id}"` allows arbitrary SQL injection.
    - **ID Validation Bug**: In `OnGetAsync` and `OnPostAsync`, verify that `Request.Query["id"].ToString()` is used without numeric validation. Counterexample: `id=""` produces `WHERE ID_MM_VIDEOS=` (malformed SQL).
  - Run verification on UNFIXED code — all 4 bug patterns should be confirmed present in all 6 files
  - **EXPECTED OUTCOME**: Test FAILS (confirms bugs exist in all 6 code-behind files)
  - Document counterexamples found to understand root cause
  - Mark task complete when verification is run and failures are documented
  - _Requirements: 1.1, 1.2, 1.3, 1.4_

- [ ] 2. Write preservation property tests (BEFORE implementing fix)
  - **Property 2: Preservation** - Add Form Defaults & Non-Edit Behavior
  - **IMPORTANT**: Follow observation-first methodology
  - Observe behavior on UNFIXED code for non-buggy inputs (Add form rendering, session checks, file upload validation):
    - Observe: `OnGetAsync` with `action=1` sets `SeekStart="0"`, `SeekEnd="0"`, `PeriodStart="00:00"`, `PeriodEnd="23:59"` (BindProperty defaults)
    - Observe: `OnGetAsync` with `action=1` sets `MainTitle` to the page-specific "... - Add" string
    - Observe: `OnGetAsync` without valid session redirects to `~/SessionExpired`
    - Observe: `OnPostAsync` with `action=1` and no file returns `TempData["Alert"] = "Please Select Video"`
    - Observe: `OnPostAsync` with `action=1` and non-.mp4 file returns `TempData["Alert"] = "Upload Only .mp4 Video"`
    - Observe: Each page's file save path, SCR_ID, and redirect URL are unique and must be preserved exactly
  - Write verification that captures these observed behaviors as baseline assertions
  - Verify assertions pass on UNFIXED code (these behaviors are correct and must not regress)
  - **EXPECTED OUTCOME**: Tests PASS (confirms baseline behavior to preserve)
  - Mark task complete when tests are written, run, and passing on unfixed code
  - _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 3.7_

- [-] 3. Fix MstMain/FullMainScreen_Dtl.cshtml.cs (reference implementation)
  - Apply all 7 changes to the first file as the reference pattern for the remaining 5 files
  - This file uses: SCR_ID=`'1'`, save path=`LobbyDisplay/mainscr`, redirect=`~/acc/MstMain/MM_VerticalScreenFull`, MainTitle=`"Master Maintenance Main Screen Video - Edit/Add"`

  - [x] 3.1 Populate BindProperty values from VideoRow in OnGetAsync
    - After `if (dt.Rows.Count > 0) VideoRow = dt.Rows[0];`, add assignments:
      - `SeekStart = VideoRow["SEEK_START"].ToString() ?? "0";`
      - `SeekEnd = VideoRow["SEEK_END"].ToString() ?? "0";`
      - `PeriodStart = VideoRow["PERIOD_START"].ToString()!` trimmed to HH:mm (first 5 chars) or default `"00:00"`
      - `PeriodEnd = VideoRow["PERIOD_END"].ToString()!` trimmed to HH:mm (first 5 chars) or default `"23:59"`
    - _Bug_Condition: isBugCondition_Display(input) where Action=3 AND VideoRow IS NOT NULL AND BindProperty defaults override Razor value expression_
    - _Expected_Behavior: SeekStart/SeekEnd/PeriodStart/PeriodEnd BindProperty values match database VideoRow values_
    - _Preservation: Add form (action=1) defaults remain "0"/"0"/"00:00"/"23:59" since VideoRow is null_
    - _Requirements: 2.1_

  - [x] 3.2 Validate `id` parameter as integer in OnGetAsync and OnPostAsync
    - Replace `var id = Request.Query["id"].ToString();` with `if (!int.TryParse(Request.Query["id"], out var id)) { TempData["Alert"] = "Invalid video ID."; return Page(); }`
    - Apply in both `OnGetAsync` (inside `Action == 3` block) and `OnPostAsync` (inside `Action == 3` block)
    - _Bug_Condition: isBugCondition_IdValidation(input) where id is null, empty, or non-numeric_
    - _Expected_Behavior: Invalid id returns error page without executing SQL_
    - _Preservation: Valid integer id values continue to work as before_
    - _Requirements: 2.2, 2.4_

  - [x] 3.3 Update QueryAsync helper to accept SqlParameter[] params
    - Change signature from `QueryAsync(string sql)` to `QueryAsync(string sql, params SqlParameter[] parameters)`
    - Add `cmd.Parameters.AddRange(parameters);` before `adapter.Fill(dt);`
    - _Requirements: 2.4_

  - [x] 3.4 Parameterize SELECT query in OnGetAsync
    - Replace `$"SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND ID_MM_VIDEOS={id}"` with parameterized query using `@id` placeholder and `new SqlParameter("@id", SqlDbType.Int) { Value = id }`
    - _Bug_Condition: isBugCondition_SqlInjection(input) where user-provided id is interpolated into SQL_
    - _Expected_Behavior: SQL command uses @id parameter, CommandText contains no user values_
    - _Requirements: 2.4_

  - [x] 3.5 Parameterize INSERT query in OnPostAsync (action=1)
    - Replace string-interpolated INSERT with parameterized query using `@file`, `@seekStart`, `@seekEnd`, `@periodStart`, `@periodEnd`, `@scrId`, `@recTyp`, `@user`, `@loc` placeholders
    - Parse SeekStart/SeekEnd as `decimal` before adding as `SqlParameter` with `SqlDbType.Decimal`
    - Preserve SCR_ID=`'1'` and RECORD_TYP=`'1'` as constants
    - _Bug_Condition: isBugCondition_SqlType(input) AND isBugCondition_SqlInjection(input)_
    - _Expected_Behavior: SQL uses parameterized queries; SeekStart/SeekEnd passed as numeric type_
    - _Preservation: File upload, save path (LobbyDisplay/mainscr), redirect URL, SCR_ID='1' unchanged_
    - _Requirements: 2.3, 2.4_

  - [x] 3.6 Parameterize UPDATE query in OnPostAsync (action=3)
    - Replace string-interpolated UPDATE with parameterized query using `@seekStart`, `@seekEnd`, `@periodStart`, `@periodEnd`, `@recTyp`, `@user`, `@loc`, `@id` placeholders
    - Parse SeekStart/SeekEnd as `decimal` before adding as `SqlParameter` with `SqlDbType.Decimal`
    - Use the validated `id` (int) from step 3.2
    - _Bug_Condition: isBugCondition_SqlType(input) AND isBugCondition_SqlInjection(input)_
    - _Expected_Behavior: SQL uses parameterized queries; SeekStart/SeekEnd passed as numeric type; id passed as int_
    - _Preservation: RECORD_TYP='3', redirect URL (~/acc/MstMain/MM_VerticalScreenFull) unchanged_
    - _Requirements: 2.2, 2.4_

  - [ ] 3.7 Verify bug condition exploration test now passes
    - **Property 1: Expected Behavior** - Edit Form Display & SQL Parameterization
    - **IMPORTANT**: Re-run the SAME verification from task 1 - do NOT write a new test
    - The verification from task 1 encodes the expected behavior
    - When this test passes, it confirms the expected behavior is satisfied for MstMain/FullMainScreen_Dtl
    - Run bug condition exploration test from step 1 against the fixed file
    - **EXPECTED OUTCOME**: Test PASSES (confirms bugs are fixed in this file)
    - _Requirements: 2.1, 2.2, 2.3, 2.4_

  - [ ] 3.8 Verify preservation tests still pass
    - **Property 2: Preservation** - Add Form Defaults & Non-Edit Behavior
    - **IMPORTANT**: Re-run the SAME tests from task 2 - do NOT write new tests
    - Run preservation tests from step 2 against the fixed file
    - **EXPECTED OUTCOME**: Tests PASS (confirms no regressions)
    - Confirm Add form defaults, file upload validation, redirect URLs, session checks all unchanged

- [x] 4. Fix MstMain/Lower2ndScreen_Dtl.cshtml.cs
  - Apply the identical pattern from task 3 to this file
  - Preserve per-file constants: SCR_ID=`'3'`, save path=`LobbyDisplay/secscrbtm`, redirect=`~/acc/MstMain/MM_VerticalScreenFull`, MainTitle=`"Master Maintenance Second Screen Lower Video - Edit/Add"`

  - [x] 4.1 Populate BindProperty values from VideoRow in OnGetAsync
    - _Requirements: 2.1_
  - [x] 4.2 Validate `id` parameter as integer in OnGetAsync and OnPostAsync
    - _Requirements: 2.2, 2.4_
  - [x] 4.3 Update QueryAsync helper to accept SqlParameter[] params
    - _Requirements: 2.4_
  - [x] 4.4 Parameterize SELECT query in OnGetAsync
    - _Requirements: 2.4_
  - [x] 4.5 Parameterize INSERT query in OnPostAsync (action=1)
    - Preserve SCR_ID=`'3'`, save path=`LobbyDisplay/secscrbtm`
    - _Requirements: 2.3, 2.4_
  - [x] 4.6 Parameterize UPDATE query in OnPostAsync (action=3)
    - Preserve redirect=`~/acc/MstMain/MM_VerticalScreenFull`
    - _Requirements: 2.2, 2.4_

- [x] 5. Fix MstMainPan/FullMainScreen_Dtl.cshtml.cs
  - Apply the identical pattern from task 3 to this file
  - Preserve per-file constants: SCR_ID=`'4'`, save path=`PantryDisplay/mainscr`, redirect=`~/acc/MstMainPan/MM_VerticalScreenFull`, MainTitle=`"Master Maintenance Main Screen Video - Pantry - Edit/Add"`

  - [x] 5.1 Populate BindProperty values from VideoRow in OnGetAsync
    - _Requirements: 2.1_
  - [x] 5.2 Validate `id` parameter as integer in OnGetAsync and OnPostAsync
    - _Requirements: 2.2, 2.4_
  - [x] 5.3 Update QueryAsync helper to accept SqlParameter[] params
    - _Requirements: 2.4_
  - [x] 5.4 Parameterize SELECT query in OnGetAsync
    - _Requirements: 2.4_
  - [x] 5.5 Parameterize INSERT query in OnPostAsync (action=1)
    - Preserve SCR_ID=`'4'`, save path=`PantryDisplay/mainscr`
    - _Requirements: 2.3, 2.4_
  - [x] 5.6 Parameterize UPDATE query in OnPostAsync (action=3)
    - Preserve redirect=`~/acc/MstMainPan/MM_VerticalScreenFull`
    - _Requirements: 2.2, 2.4_

- [x] 6. Fix MstMainPan/Lower2ndScreen_Dtl.cshtml.cs
  - Apply the identical pattern from task 3 to this file
  - Preserve per-file constants: SCR_ID=`'6'`, save path=`PantryDisplay/secscrbtm`, redirect=`~/acc/MstMainPan/MM_VerticalScreenFull`, MainTitle=`"Master Maintenance Second Screen Lower Video - Pantry - Edit/Add"`

  - [x] 6.1 Populate BindProperty values from VideoRow in OnGetAsync
    - _Requirements: 2.1_
  - [x] 6.2 Validate `id` parameter as integer in OnGetAsync and OnPostAsync
    - _Requirements: 2.2, 2.4_
  - [x] 6.3 Update QueryAsync helper to accept SqlParameter[] params
    - _Requirements: 2.4_
  - [x] 6.4 Parameterize SELECT query in OnGetAsync
    - _Requirements: 2.4_
  - [x] 6.5 Parameterize INSERT query in OnPostAsync (action=1)
    - Preserve SCR_ID=`'6'`, save path=`PantryDisplay/secscrbtm`
    - _Requirements: 2.3, 2.4_
  - [x] 6.6 Parameterize UPDATE query in OnPostAsync (action=3)
    - Preserve redirect=`~/acc/MstMainPan/MM_VerticalScreenFull`
    - _Requirements: 2.2, 2.4_

- [x] 7. Fix MstMainLobby2/FullMainScreen_Dtl.cshtml.cs
  - Apply the identical pattern from task 3 to this file
  - Preserve per-file constants: SCR_ID=`'7'`, save path=`LobbyDisplay2/mainscr`, redirect=`~/acc/MstMainLobby2/MM_VerticalScreenFull`, MainTitle=`"Master Maintenance Main Screen Video - TV 2 - Edit/Add"`

  - [x] 7.1 Populate BindProperty values from VideoRow in OnGetAsync
    - _Requirements: 2.1_
  - [x] 7.2 Validate `id` parameter as integer in OnGetAsync and OnPostAsync
    - _Requirements: 2.2, 2.4_
  - [x] 7.3 Update QueryAsync helper to accept SqlParameter[] params
    - _Requirements: 2.4_
  - [x] 7.4 Parameterize SELECT query in OnGetAsync
    - _Requirements: 2.4_
  - [x] 7.5 Parameterize INSERT query in OnPostAsync (action=1)
    - Preserve SCR_ID=`'7'`, save path=`LobbyDisplay2/mainscr`
    - _Requirements: 2.3, 2.4_
  - [x] 7.6 Parameterize UPDATE query in OnPostAsync (action=3)
    - Preserve redirect=`~/acc/MstMainLobby2/MM_VerticalScreenFull`
    - _Requirements: 2.2, 2.4_

- [x] 8. Fix MstMainLobby2/Lower2ndScreen_Dtl.cshtml.cs
  - Apply the identical pattern from task 3 to this file
  - Preserve per-file constants: SCR_ID=`'9'`, save path=`LobbyDisplay2/secscrbtm`, redirect=`~/acc/MstMainLobby2/MM_VerticalScreenFull`, MainTitle=`"Master Maintenance Second Screen Lower Video - TV 2 - Edit/Add"`

  - [x] 8.1 Populate BindProperty values from VideoRow in OnGetAsync
    - _Requirements: 2.1_
  - [x] 8.2 Validate `id` parameter as integer in OnGetAsync and OnPostAsync
    - _Requirements: 2.2, 2.4_
  - [x] 8.3 Update QueryAsync helper to accept SqlParameter[] params
    - _Requirements: 2.4_
  - [x] 8.4 Parameterize SELECT query in OnGetAsync
    - _Requirements: 2.4_
  - [x] 8.5 Parameterize INSERT query in OnPostAsync (action=1)
    - Preserve SCR_ID=`'9'`, save path=`LobbyDisplay2/secscrbtm`
    - _Requirements: 2.3, 2.4_
  - [x] 8.6 Parameterize UPDATE query in OnPostAsync (action=3)
    - Preserve redirect=`~/acc/MstMainLobby2/MM_VerticalScreenFull`
    - _Requirements: 2.2, 2.4_

- [x] 9. Final verification across all 6 files
  - **Property 1: Expected Behavior** - All Files Fixed
  - **Property 2: Preservation** - All Per-File Constants Preserved
  - Re-run exploration tests from task 1 across all 6 fixed files — all should PASS
  - Re-run preservation tests from task 2 across all 6 fixed files — all should PASS
  - Verify per-file constants are preserved exactly:
    - MstMain/Full: SCR_ID=`'1'`, path=`LobbyDisplay/mainscr`, redirect=`~/acc/MstMain/MM_VerticalScreenFull`
    - MstMain/Lower: SCR_ID=`'3'`, path=`LobbyDisplay/secscrbtm`, redirect=`~/acc/MstMain/MM_VerticalScreenFull`
    - MstMainPan/Full: SCR_ID=`'4'`, path=`PantryDisplay/mainscr`, redirect=`~/acc/MstMainPan/MM_VerticalScreenFull`
    - MstMainPan/Lower: SCR_ID=`'6'`, path=`PantryDisplay/secscrbtm`, redirect=`~/acc/MstMainPan/MM_VerticalScreenFull`
    - MstMainLobby2/Full: SCR_ID=`'7'`, path=`LobbyDisplay2/mainscr`, redirect=`~/acc/MstMainLobby2/MM_VerticalScreenFull`
    - MstMainLobby2/Lower: SCR_ID=`'9'`, path=`LobbyDisplay2/secscrbtm`, redirect=`~/acc/MstMainLobby2/MM_VerticalScreenFull`
  - Verify the project builds successfully: `dotnet build`
  - Ensure all tests pass, ask the user if questions arise.

- [x] 10. Checkpoint - Ensure all tests pass
  - Run `dotnet build` to confirm no compilation errors across all 6 modified files
  - Verify no Razor view (.cshtml) files were modified (only code-behind .cshtml.cs files changed)
  - Ensure all tests pass, ask the user if questions arise.
