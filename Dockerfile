# Use Microsoft's official build .NET image.
# https://hub.docker.com/_/microsoft-dotnet-core-sdk/
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# Install production dependencies.
# Copy csproj and restore as distinct layers.
COPY codecomp.player/*.csproj ./
RUN dotnet restore

# Copy local code to the container image.
RUN ls
COPY codecomp.player/ ./
# WORKDIR /app
RUN ls
ENTRYPOINT [ "dotnet", "run" ]

# # Build a release artifact.
# RUN dotnet publish codecomp.player/player.sln -c Release -o out


# # Use Microsoft's official runtime .NET image.
# # https://hub.docker.com/_/microsoft-dotnet-core-aspnet/
# FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
# WORKDIR /app
# COPY --from=build /app/out ./

# # Run the web service on container startup.
# ENTRYPOINT ["dotnet", "codecomp.player.dll"]