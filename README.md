# inventory-api

This is the API for the [WellPartner Inventory App](https://github.com/OptiCorp/inventory-app). 
The system keeps track of their items: **Units**, **Assemblies**, **Subassemblies** and **Parts**.

- Units consists of Assemblies
- Assemblies consists of Subassemblies and/or Parts
- Subassemblies consists of Parts

## Key Features
This section contains information about the API endpoints.

### Category

- CRUD for item template categories such as bolt, electrical etc.

### Document

- Add documents to an item template or item using Azure Blob Storage
- Get documents for an item template or item

### Document type

- CRUD for document types to describe documents

### Item

- CRUD for items
- Get items containing search string

### Item template

- CRUD for item templates containing common data for items

### List

- CRUD for lists containing items
- Add/remove items in lists

### Location

- CRUD for item locations to keep track of where they are located

### PreCheck

- CRUD for pre checks for when adding an item

### Size

- CRUD for sizes belonging to an item template

### User

- CRUD for users of the system

### Vendor

- CRUD for vendors to know where the items are bought from

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

## Issues

Issues should have the following format:

1. **Type**: Use one of the following prefixes to specify the nature of the change:
    - **feat** for new features or enhancements.
    - **fix** for bug fixes.
    - **chore** for routine tasks or maintenance.
    - **refactor** for code restructuring.
2. **Descriptive Text**: Add a concise descriptive text.

If applicable, also give the issue a label.

**Example**
```
fix: Failed to Login BrowserAuthError
```

## Formatting

Make sure to run 
```
dotnet format inventory-api.sln
```
before creating pull requests.

## Branch name convention
Branches should have the auto-generated name given to branches created from issues.

**Example**
```
205-fix-failed-to-login-browserautherror
```

### Commit Messages

-   [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/)


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
