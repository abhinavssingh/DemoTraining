# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the files needed for restore first (maximize cache)
# If you also have Directory.Build.props/targets at root, copy them too.
COPY DemoTraining.csproj ./
COPY nuget.config ./

# Restore using the explicit NuGet config (Linux is case-sensitive)
RUN dotnet restore DemoTraining.csproj --configfile ./nuget.config

# Copy the rest of the source
COPY . .

# Publish
RUN dotnet publish DemoTraining.csproj -c Release -o /app/publish
COPY ./docker/build-script/wait_sqlserver_start_and_attachdb.sh /app/publish/wait_sqlserver_start_and_attachdb.sh

# ---- Runtime image ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish/ .

# Expose port 80 (Kestrel)
EXPOSE 80

# Default entrypoint; adjust DLL name if your AssemblyName differs
ENTRYPOINT ["./wait_sqlserver_start_and_attachdb.sh"]
