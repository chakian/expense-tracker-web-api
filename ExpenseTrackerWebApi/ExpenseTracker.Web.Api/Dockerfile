FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ExpenseTracker.Web.Api.csproj ./
RUN dotnet restore 

# Copy everything else and build
COPY . ./
RUN dotnet publish /ExpenseTracker.Web.Api.csproj -c Release -o ./out --nologo

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out/ .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080
ENV ASPNETCORE_ENVIRONMENT=production
ENV TAPTAKE_SEED=false
ENTRYPOINT ["dotnet", "ExpenseTracker.Web.Api.dll"]
