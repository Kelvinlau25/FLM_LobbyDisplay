# Configuration — Sensitive Values

Connection string passwords are NOT stored in source-controlled files.

## Development
Use .NET Secret Manager:
```
dotnet user-secrets set "ConnectionStrings:filmDisplay" "...full connection string with real password..."
dotnet user-secrets set "ConnectionStrings:ORCL_ACL" "...full connection string with real password..."
```

## Production / IIS
Set environment variables before starting the application:
- `FILM_DB_PASSWORD` — SQL Server password for the filmdisplay user
- `ORACLE_ACL_PASSWORD` — Oracle password for the pfracl user

Or use IIS application pool environment variables / Windows Credential Manager.
