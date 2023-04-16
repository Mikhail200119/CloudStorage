FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CloudStorage.Api/CloudStorage.Api.csproj", "CloudStorage.Api/"]
RUN dotnet restore "CloudStorage.Api/CloudStorage.Api.csproj"
COPY . .
WORKDIR "/src/CloudStorage.Api"
RUN dotnet build "CloudStorage.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudStorage.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudStorage.Api.dll"]
