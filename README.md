# Import Steam Data to MS SQL Server

## Use environment variables to configure connection string

### Create an .env file for development

For username/password authentication

```
DOTNET_ENV=dev
DB_INTEGRATED_SECURITY=False
DB_SERVER=<your_db_host_name>
DB_NAME=Steam
DB_SERVER_USER=<db_user>
DB_SERVER_PASSWORD=<YourStrong@Passw0rd>
DB_MULTI_ACTIVE_RES_SET=True
```

For windows authentication

```
DOTNET_ENV=prod
DB_INTEGRATED_SECURITY=True
DB_SERVER=<your_db_host_name>
DB_NAME=Steam
DB_MULTI_ACTIVE_RES_SET=True
```

## Database Migration

Reset the database

```
$ dotnet ef database update 0
```

Setup the database

```
$ dotnet ef database update
```

See [documentation](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli) for more details about using EF Core for database migration.

## Usage

Build

```
$ dotnet build
```

Build & Run

```
$ dotnet run
```

Import multiple xlsx files in current directory to db

```
$ dotnet run --no-build
```

Import one Steam xlsx file to db

```
$ dotnet run --no-build DownloadedStatistics_20200601.xlsx
```

Import multiple xlsx files under some directory

```
$ dotnet run --no-build ../spec/Steam_20200826
```
