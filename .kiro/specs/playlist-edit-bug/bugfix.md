# Bugfix Requirements Document

## Introduction

The video playlist edit form across all 6 detail pages (MstMain, MstMainPan, MstMainLobby2 — each with FullMainScreen_Dtl and Lower2ndScreen_Dtl) has two related bugs. First, when editing an existing video entry, the Seek Start and Seek End fields display "0" instead of the actual database values because the Razor form input `name` attributes match `[BindProperty]` property names that are initialized to "0", and ASP.NET Core model binding overwrites the rendered value. Second, when submitting the edit form, the SQL UPDATE statement wraps SeekStart and SeekEnd values in single quotes (treating them as varchar), but the database columns SEEK_START and SEEK_END are numeric types, causing a "Error converting data type varchar to numeric" failure. Both bugs exist identically in all 6 code-behind files.

## Bug Analysis

### Current Behavior (Defect)

1.1 WHEN a user opens the Edit form (action=3) for an existing video entry that has non-zero SEEK_START and SEEK_END values in the database THEN the system displays "0" in both the Seek Start and Seek End input fields instead of the actual stored values, because the `[BindProperty]` properties `SeekStart` and `SeekEnd` (initialized to "0") take precedence over the `VideoRow` DataRow values during Razor rendering when the input `name` attribute matches the bound property name.

1.2 WHEN a user submits the Edit form (action=3) with any SeekStart or SeekEnd value THEN the system fails with "Error converting data type varchar to numeric" because the SQL UPDATE statement wraps the values in single quotes (e.g., `SEEK_START='{SeekStart}'`), causing SQL Server to attempt an implicit varchar-to-numeric conversion on the numeric SEEK_START and SEEK_END columns.

1.3 WHEN a user submits the Add form (action=1) with any SeekStart or SeekEnd value THEN the system inserts the values wrapped in single quotes in the SQL INSERT statement (e.g., `'{SeekStart}'`), which may also fail with a type conversion error if the database columns are numeric.

1.4 WHEN any form is submitted with user-provided values for SeekStart, SeekEnd, PeriodStart, PeriodEnd, or other fields THEN the system constructs SQL statements using string interpolation without parameterization, exposing the application to SQL injection attacks.

### Expected Behavior (Correct)

2.1 WHEN a user opens the Edit form (action=3) for an existing video entry that has non-zero SEEK_START and SEEK_END values in the database THEN the system SHALL display the actual SEEK_START and SEEK_END values from the database in the Seek Start and Seek End input fields respectively.

2.2 WHEN a user submits the Edit form (action=3) with valid numeric SeekStart and SeekEnd values THEN the system SHALL successfully update the MM_VIDEOS record without type conversion errors by passing SeekStart and SeekEnd as numeric parameters (without wrapping in single quotes).

2.3 WHEN a user submits the Add form (action=1) with valid numeric SeekStart and SeekEnd values THEN the system SHALL successfully insert the MM_VIDEOS record without type conversion errors by passing SeekStart and SeekEnd as numeric parameters.

2.4 WHEN any form is submitted with user-provided values THEN the system SHALL use parameterized SQL queries instead of string interpolation to prevent SQL injection vulnerabilities.

### Unchanged Behavior (Regression Prevention)

3.1 WHEN a user opens the Add form (action=1) for a new video entry THEN the system SHALL CONTINUE TO display "0" as the default value for Seek Start and Seek End fields.

3.2 WHEN a user opens the Edit form (action=3) for a video entry with SEEK_START=0 and SEEK_END=0 in the database THEN the system SHALL CONTINUE TO display "0" in both fields.

3.3 WHEN a user opens the Add form (action=1) THEN the system SHALL CONTINUE TO display "00:00" and "23:59" as default values for Start Time and End Time fields respectively.

3.4 WHEN a user opens the Edit form (action=3) for an existing video entry THEN the system SHALL CONTINUE TO display the correct PERIOD_START and PERIOD_END values from the database (trimmed to HH:mm format).

3.5 WHEN a user submits the Add form with a valid .mp4 file THEN the system SHALL CONTINUE TO upload the file, insert the record, and redirect with a success message.

3.6 WHEN a user submits the Edit form with valid values THEN the system SHALL CONTINUE TO update the record and redirect with a success message.

3.7 WHEN a user opens the Edit form THEN the system SHALL CONTINUE TO display the audit fields (Created By, Created Date, Created Loc, Updated By, Updated Date, Updated Loc) from the database.

---

### Bug Condition Derivation

**Bug Condition Function** — Identifies inputs that trigger Bug 1 (incorrect display):
```pascal
FUNCTION isBugCondition_Display(X)
  INPUT: X of type EditFormRequest
  OUTPUT: boolean

  // Returns true when editing an existing video (action=3) with a VideoRow loaded
  RETURN X.Action = 3 AND X.VideoRow IS NOT NULL
END FUNCTION
```

**Bug Condition Function** — Identifies inputs that trigger Bug 2 (SQL type error):
```pascal
FUNCTION isBugCondition_SqlType(X)
  INPUT: X of type FormSubmission
  OUTPUT: boolean

  // Returns true when any form submission includes SeekStart/SeekEnd values
  // that get wrapped in quotes for numeric database columns
  RETURN X.Action IN {1, 3} AND (X.SeekStart IS NOT NULL OR X.SeekEnd IS NOT NULL)
END FUNCTION
```

**Property Specification** — Fix Checking for Bug 1:
```pascal
// Property: Fix Checking - Edit form displays actual database values
FOR ALL X WHERE isBugCondition_Display(X) DO
  renderedPage ← OnGetAsync'(X)
  ASSERT renderedPage.SeekStartField.Value = X.VideoRow["SEEK_START"]
  ASSERT renderedPage.SeekEndField.Value = X.VideoRow["SEEK_END"]
END FOR
```

**Property Specification** — Fix Checking for Bug 2:
```pascal
// Property: Fix Checking - SQL statements use proper numeric parameters
FOR ALL X WHERE isBugCondition_SqlType(X) DO
  result ← OnPostAsync'(X)
  ASSERT result.SqlCommand.UsesParameterizedQueries = TRUE
  ASSERT result.SqlCommand.SeekStartParam.Type = SqlDbType.Int
  ASSERT result.SqlCommand.SeekEndParam.Type = SqlDbType.Int
  ASSERT no_sql_error(result)
END FOR
```

**Preservation Goal**:
```pascal
// Property: Preservation Checking - Add form defaults unchanged
FOR ALL X WHERE NOT isBugCondition_Display(X) DO
  ASSERT F(X).SeekStartField.Value = F'(X).SeekStartField.Value
  ASSERT F(X).SeekEndField.Value = F'(X).SeekEndField.Value
END FOR

// Property: Preservation Checking - Successful submissions still work
FOR ALL X WHERE X.Action IN {1, 3} AND X.HasValidData DO
  ASSERT F'(X).Result = SuccessRedirect
END FOR
```
