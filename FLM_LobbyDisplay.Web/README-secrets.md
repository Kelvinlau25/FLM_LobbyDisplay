# Sensitive Configuration

Connection string passwords are NOT stored in source-controlled files.

## Development
Use .NET Secret Manager:
```
dotnet user-secrets set "ConnectionStrings:filmDisplay" "...full connection string with real password..."
dotnet user-secrets set "ConnectionStrings:ORCL_ACL" "...full connection string with real password..."
```

## Production
Set environment variables before starting the application:
- `FILM_DB_PASSWORD` — SQL Server password for filmdisplay user
- `ORACLE_ACL_PASSWORD` — Oracle password for pfracl user

Or use `ConnectionStrings__filmDisplay` and `ConnectionStrings__ORCL_ACL` environment variables with the full connection string.
