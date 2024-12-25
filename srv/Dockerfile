FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["srv/srv.csproj", "srv/"]
RUN dotnet restore "srv/srv.csproj"
COPY . .
WORKDIR "/src/srv"
RUN dotnet build "srv.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "srv.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
EXPOSE 5000
WORKDIR /app

RUN apt-get update \
&& apt-get install -y wget curl htop procps vim telnet dnsutils iproute2 mc postgresql-client netcat-openbsd tcpdump

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "srv.dll"]
