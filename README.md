# Export Steam Data to MS SQL Server

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

## [Entity Framework Core CLI](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet)

[Add a Tool Manifest File](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools#install-a-local-tool)

```
$ dotnet new tool-manifest
```

[Install dotnet-ef tool](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-install)

```
$ dotnet tool install dotnet-ef
```

## Database Migration

[Create a migration file](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli#create-your-first-migration)

```
dotnet ef migrations add InitialCreate
```

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

Run the console app with Release configuration

```
$ dotnet run -c Release
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

Publish release version for Windows 10 / macOS / RHEL

```
$ dotnet publish -c Release -r win10-x64
$ dotnet publish -c Release -r osx-x64
$ dotnet publish -c Release -r rhel.7.4-x64
```
