# Empty CMS template

## How to run

Chose one of the following options to get started. 

### Windows

Prerequisities
- .NET SDK 8+
- SQL Server 2016 Express LocalDB (or later)

```bash
$ dotnet run
````

### How to deploy to IIS
- Follow the [official Optimizely guide](https://docs.developers.optimizely.com/content-management-system/docs/deploying-to-windows-servers) to deploy the application to IIS.
- Copy all the configuration from environment json file to the default appsettings.json file and make sure to update the connection string to point to the correct database server.
- configuartion should be like below:

```json
{
  "ConnectionStrings": {
	"EPiServerDB": "Data Source=<your_server>;Initial Catalog=<your_database>;User ID=<sql_user>;Password=<sql_password>;Encrypt=True;TrustServerCertificate=True;Connect Timeout=30"
  },
  "CMS": {
	"AdminUsername": "admin",
	"AdminPassword": "admin",
	"AdminEmail": "
	}
}
```

### Any OS with Docker

Prerequisities
- Docker
- Enable Docker support when applying the template
- Review the .env file and make changes where necessary to the Docker-related variables

```bash
$ docker-compose up
````

> Note that this Docker setup is just configured for local development. Follow this [guide to enable HTTPS](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-aspnetcore-https-development.md).

#### Reclaiming Docker Image Space

1. Backup the App_Data/\${DB_NAME}.mdf and App_Data/\${DB_NAME}.ldf DB restoration files for safety
2. Run `docker compose down --rmi all` to remove containers, networks, and images associated with the specific project instance
3. In the future, run `docker compose up` anytime you want to recreate the images and containers

### Any OS with external database server

Prerequisities
- .NET SDK 8+
- SQL Server 2016 (or later) on a external server, e.g. Azure SQL

Create an empty database on the external database server and update the connection string accordingly.

```bash
$ dotnet run
````
