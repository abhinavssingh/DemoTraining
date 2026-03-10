## Table of Contents
- [Assumptions](#assumptions)
- [Introduction](#introduction)
- [Understanding the project structure](#understanding-the-project-structure)
- [Dotnet commands to create Content Types, Controllers, Components and Jobs](#dotnet-commands-to-create-content-types-controllers-components-and-jobs)
- [Add-ons what are used in this project](#add-ons-what-are-used-in-this-project)
- [How to run](#how-to-run)
  - [Windows](#windows)
  - [How to deploy to IIS](#how-to-deploy-to-iis)
  - [Any OS with Docker](#any-os-with-docker)
  - [Any OS with external database server](#any-os-with-external-database-server)

## Assumptions
This is assumed that you have installed Optimizely Templates for .NET 8.0, if not you can install it using the following command:
```powershell
dotnet tool install EPiServer.Net.Cli --global --add-source 'https://nuget.optimizely.com/feed/packages.svc'
 
dotnet new -i EPiServer.Templates  # install Optimizely tempaltes

dotnet new --list epi # to view the installed templates
```

## Introduction
- This is a sample Optimizely CMS 12 project created using the official Optimizely CMS empty template for .NET 8.0.
- This project is built on vertical architecture, which is a software design pattern that organizes code into vertical slices based on features or functionality. Each vertical slice contains all the necessary components (e.g., controllers, views, models) to implement a specific feature or functionality of the application.
- It serves as a starting point for developers looking to build web applications using Optimizely CMS.
- This project is build on latest veriosn of `EPiServer.CMS 12.34.2`.
- Sql Server 2025 is required, if you would like to attach db on Sql Server.
- If you would like to run the prject using localhost then you can use Sql Server Express LocalDB which is a lightweight version of Sql Server and is included with Visual Studio. It allows you to create and manage databases without the need for a full Sql Server installation.


## Understanding the project structure
- The project is organized into several folders, each serving a specific purpose:
  - `Business`: This folder contains the business logic of the application, which is shared across different parts of the application. Business folder has many sub folder also as per the functionality, e.g. `Rendering` folder contains logics to extende the rendering of the content.
  - `Components`: This folder contains reusable components logic that can be used to render Block Componentsin the application. As Block components are View Components.
  - `Controllers`: This folder contains the default controllers for the application, which are responsible for handling incoming requests and returning appropriate responses. Default controller which is used to route the most of the requests where dierct page controller is not assigned.
  - `Extensions`: This folder contains the Initialization logic for the application, which is executed when the application starts. It is used to configure services, middleware, and other components of the application, e.g. `AddDemoTraining` is registered in ConfigureServices method of the application.
  - `Features`: This folder contains the feature-specific logic of the application, which is organized into subfolders based on the features of the application. Each feature folder contains its own controllers, views, and models that are specific to that feature.
  - `Resources`: This folder contains the resource files for localization, which are used to localize the CMS interface in targeted language. This folder conatains many `xml` files, each file has `languages` and `language` node. If we have enabled any language in CMS, then we must need to add new `language` node in the `languages` node of the resource file, otherwise that language will not work in CMS interface. 

## Dotnet commands to create Content Types, Controllers, Components and Jobs

- Create a new Content Type
```powershell
dotnet new epi-cms-contenttype -p:na <your-name-space> --name <your-page-name> # first create the content type and then assign it to the controller
```

- Create a new Controller
```powershell
dotnet new epi-cms-pagecontroller -p:na <your-name-space> -ct <your-page-type>--name <your-controller-name> # create page controller and assign the content type
```

- Create a new Component
```powershell
dotnet new epi-cms-contentcomponent -p:na <your-name-space> --name <your-component-name>
```

- Create a Job
```powershell
dotnet new epi-cms-job -p:na <your-name-space> --name <your-job-name>
```

## Add-ons what are used in this project
- `EPiServer.CMS.AspNetCore.TagHelpers` - This package provides a set of tag helpers for ASP.NET Core applications that are built on top of the Optimizely CMS. Tag helpers are a feature in ASP.NET Core that allow developers to create reusable components and enhance the HTML markup in their views. This package includes tag helpers specifically designed for working with Optimizely CMS content, making it easier to render and manage content within ASP.NET Core applications.
- `EPiServer.Find.Cms` - This package integrates the Episerver Find search engine with the Optimizely CMS, allowing developers to implement powerful search functionality within their CMS applications. It provides tools and features for indexing and searching content stored in the CMS, making it easier for users to find relevant information quickly and efficiently.
- `EPiServer.Labs.GridView` - This package provides a grid-based layout system for Optimizely CMS. It offers a visual interface for arranging content blocks in a grid format.
- `Wangkanai.Detection` - This package provides device detection capabilities for ASP.NET Core applications. It allows developers to identify the type of device making a request (e.g., mobile, tablet, desktop) and tailor the response accordingly. This can be useful for optimizing the user experience and delivering content that is appropriate for the specific device being used.

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
