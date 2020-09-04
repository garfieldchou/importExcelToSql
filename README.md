# Import Steam Data to MS SQL Server

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
