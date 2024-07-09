
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebScrapingAPI/WebScrapingAPI.csproj", "WebScrapingAPI/"]
RUN dotnet restore "WebScrapingAPI/WebScrapingAPI.csproj"
COPY . .
WORKDIR "/src/WebScrapingAPI"
RUN dotnet build "WebScrapingAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebScrapingAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "WebScrapingAPI.dll"]