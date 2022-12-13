FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
RUN apt-get update && apt-get install -y \
    tzdata \
    && rm -rf /var/lib/apt/lists/*

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN apt-get update && apt-get install -y \
    openssl \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["Exentials.ReCache/Exentials.ReCache.csproj", "Exentials.ReCache/"]
COPY ["Exentials.ReCache.Abstractions/Exentials.ReCache.Abstractions.csproj", "Exentials.ReCache.Abstractions/"]
COPY ["Exentials.ReCache.Client/Exentials.ReCache.Client.csproj", "Exentials.ReCache.Client/"]
COPY ["Exentials.ReCache.Server/Exentials.ReCache.Server.csproj", "Exentials.ReCache.Server/"]
RUN dotnet restore "Exentials.ReCache.Server/Exentials.ReCache.Server.csproj"
COPY . .
WORKDIR "/src/Exentials.ReCache.Server"
RUN dotnet build "Exentials.ReCache.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Exentials.ReCache.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false \
    && mkdir /app/publish/certificates \
    && openssl req -newkey rsa:4096 -x509 -sha256 -days 3650 -nodes -out certificate.crt -keyout certificate.key -subj "/CN=localhost" \
    && openssl pkcs12 -export -out /app/publish/certificates/certificate.pfx -inkey certificate.key -in certificate.crt -passout pass:


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS="https://+:443;http://+:80"
ENV ASPNETCORE_HTTPS_PORT=443
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/certificates/certificate.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=""
ENTRYPOINT ["dotnet", "Exentials.ReCache.Server.dll"]
