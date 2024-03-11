FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Deathwrap.GerageGenius.FrontAPI/Deathwrap.GerageGenius.FrontAPI.csproj", "Deathwrap.GerageGenius.FrontAPI/"]
RUN dotnet restore "Deathwrap.GerageGenius.FrontAPI/Deathwrap.GerageGenius.FrontAPI.csproj"
COPY . .
WORKDIR "/src/Deathwrap.GerageGenius.FrontAPI"
RUN dotnet build "Deathwrap.GerageGenius.FrontAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Deathwrap.GerageGenius.FrontAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Deathwrap.GerageGenius.FrontAPI.dll"]
