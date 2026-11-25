FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ProfilesApi.Domain/ProfilesApi.Domain.csproj ProfilesApi.Domain/
COPY ProfilesApi.Application/ProfilesApi.Application.csproj ProfilesApi.Application/
COPY ProfilesApi.Infrastructure/ProfilesApi.Infrastructure.csproj ProfilesApi.Infrastructure/
COPY ProfilesApi.Api/ProfilesApi.Api.csproj ProfilesApi.Api/
RUN dotnet restore ProfilesApi.Api/ProfilesApi.Api.csproj
COPY . .
RUN dotnet publish ProfilesApi.Api/ProfilesApi.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM build AS migrate
RUN dotnet tool install --global dotnet-ef --version 10.0.14
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT ["dotnet", "ef", "database", "update", "--project", "ProfilesApi.Infrastructure", "--startup-project", "ProfilesApi.Api"]

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
USER root
RUN apt-get update \
    && apt-get install -y --no-install-recommends socat \
    && rm -rf /var/lib/apt/lists/* \
    && printf '%s\n' \
        '#!/bin/sh' \
        'socat TCP-LISTEN:8080,fork,bind=127.0.0.1,reuseaddr TCP:keycloak:8080 &' \
        'exec dotnet ProfilesApi.Api.dll' \
        > /entrypoint.sh \
    && chmod +x /entrypoint.sh
USER app
ENV ASPNETCORE_URLS=http://+:5082
EXPOSE 5082
ENTRYPOINT ["/entrypoint.sh"]
