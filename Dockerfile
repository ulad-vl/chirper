#docker build --no-cache -t chirper .

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./Chirper.Server/Chirper.Server.csproj", "/Chirper.Server/"]
COPY ["./Chirper.Grains/Chirper.Grains.csproj", "/Chirper.Grains/"]
COPY ["./Chirper.Grains.Interfaces/Chirper.Grains.Interfaces.csproj", "/Chirper.Grains.Interfaces/"]
RUN dotnet restore "/Chirper.Server/Chirper.Server.csproj"
COPY . .
WORKDIR "/src/Chirper.Server"
RUN dotnet build "./Chirper.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Chirper.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chirper.Server.dll"]