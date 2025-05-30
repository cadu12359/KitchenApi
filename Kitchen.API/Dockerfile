FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Kitchen.API/Kitchen.API.csproj", "Kitchen.API/"]
COPY ["Kitchen.Application/Kitchen.Application.csproj", "Kitchen.Application/"]
COPY ["Kitchen.Domain/Kitchen.Domain.csproj", "Kitchen.Domain/"]
COPY ["Kitchen.Infrastructure/Kitchen.Infrastructure.csproj", "Kitchen.Infrastructure/"]
RUN dotnet restore "Kitchen.API/Kitchen.API.csproj"
COPY . .
WORKDIR "/src/Kitchen.API"
RUN dotnet build "Kitchen.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Kitchen.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kitchen.API.dll"]
