# DevinSite
ASP.NET project for CS276

## Running the App
- open a bash terminal, or if your on windows, follow instructions to add a new migration before installing the database. See [managing migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing?tabs=dotnet-core-cli)
- install the database with 

```bsh

dotnet ef database update
```

- run project.
- Register User.
  - No email provider has been set up for email verification, 
    so in order for a new user to be verified, 
    you need to click the verification link after registering a new user.
    
- Login with registerted user.
