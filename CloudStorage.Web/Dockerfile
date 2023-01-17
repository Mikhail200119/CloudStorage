FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CloudStorage.Web/CloudStorage.Web.csproj", "CloudStorage.Web/"]
COPY ["CloudStorage.BLL/CloudStorage.BLL.csproj", "CloudStorage.BLL/"]
COPY ["CloudStorage.Common/CloudStorage.Common.csproj", "CloudStorage.Common/"]
COPY ["CloudStorage.DAL/CloudStorage.DAL.csproj", "CloudStorage.DAL/"]
RUN dotnet restore "CloudStorage.Web/CloudStorage.Web.csproj"
COPY . .
WORKDIR "/src/CloudStorage.Web"
RUN dotnet build "CloudStorage.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudStorage.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p wwwroot/Files

ENV DEBIAN_FRONTEND=nointeractive
RUN apt-get update && apt-get install -y vim


ENTRYPOINT ["dotnet", "CloudStorage.Web.dll"]
