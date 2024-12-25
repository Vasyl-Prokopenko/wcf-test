FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["client/client.csproj", "client/"]
RUN dotnet restore "client/client.csproj"
COPY . .
WORKDIR "/src/client"
RUN dotnet build "client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

RUN apt-get update \
&& apt-get install -y wget curl htop procps vim telnet dnsutils iproute2 mc postgresql-client netcat-openbsd tcpdump

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "client.dll"]
