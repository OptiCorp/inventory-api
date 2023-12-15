# inventory-api

This is the API for the [WellPartner Inventory App](https://github.com/OptiCorp/inventory-app). 
The system keeps track of their items: **Units**, **Assemblies**, **Subassemblies** and **Parts**.

- Units consists of Assemblies
- Assemblies consists of Subassemblies and/or parts
- Subassemblies consists of parts

## Key Features
This section contains information about the API endpoints.

### Category

- CRUD for item categories such as bolt, electrical etc.


### Item

- CRUD for items
- Get items containing search string

### List

- CRUD for lists containing items
- Add/remove items in lists

### Location

- CRUD for item locations to keep track of where they are located

### User

- CRUD for users of the system


## Architecture

- C#
- ASP.NET Core
- Entity Framework Core
- REST API
- Azure App Service with Entra authentication and authorization
- Deployment to Azure using GitHub Actions

## Install & Run

Clone and Run the API Application:

1. Clone the repo
2. Navigate to project folder: `inventory-api`
3. Run the API: `dotnet run`
4. Use Swagger for endpoint documentation: `http://localhost:5026/swagger/index.html`


## Branch name convention
1. type (feat, fix, chore, refactor)
2. Issue number
3. Descriptive text.

### Example:

```
feat/#1/users-endpoint
```

## Database Migration

After changing your database models or model configurations run the following command:

```dotnet
dotnet ef migrations add <NameOfChanges>
dotnet ef database update (or start the solution)
```

To undo migration, run the following command:

```dotnet
dotnet ef migrations remove
dotnet ef database update (or start the solution)
```

or:

```dotnet
dotnet ef database update <NameofMigrationYouWantToRevertTo>
dotnet ef migrations remove (will remove all migration after the one you reverted to)
```
