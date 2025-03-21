FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /source

# copy csproj and restore as distinct layers
COPY [".", "/src/"]
WORKDIR "/src/CompanyService.Api"
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5193
ENTRYPOINT ["dotnet", "CompanyService.Api.dll"]
