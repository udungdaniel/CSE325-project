# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore first so Docker can cache the packages
COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish ThreadLine.csproj -c Release -o /app/publish --no-restore


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

# Render expects the app to listen on port 10000 by default
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Change ThreadLine.dll if your project file has a different name
ENTRYPOINT ["dotnet", "ThreadLine.dll"]
