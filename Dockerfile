FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 80
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Presentation/WebAPI/WebAPI.csproj", "src/Presentation/WebAPI/"]
RUN dotnet restore src/Presentation/WebAPI/WebAPI.csproj
COPY . .
WORKDIR /src
RUN dotnet build "src/Presentation/WebAPI/WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Presentation/WebAPI/WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WebAPI.dll"]