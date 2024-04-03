FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Yet Another Traceback Tracker/Yet Another Traceback Tracker.csproj", "Yet Another Traceback Tracker/"]
RUN dotnet restore "Yet Another Traceback Tracker/Yet Another Traceback Tracker.csproj"
COPY . .
WORKDIR "/src/Yet Another Traceback Tracker"
RUN dotnet build "Yet Another Traceback Tracker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Yet Another Traceback Tracker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Yet Another Traceback Tracker.dll"]
