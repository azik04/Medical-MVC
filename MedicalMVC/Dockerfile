# Use the official ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official ASP.NET Core SDK for build and publish stages
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MedicalMVC/MedicalMVC.csproj", "MedicalMVC/"]
RUN dotnet restore "./MedicalMVC/MedicalMVC.csproj"
COPY . .
WORKDIR "/src/MedicalMVC"
RUN dotnet build "./MedicalMVC.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MedicalMVC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables for Kestrel configuration
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "MedicalMVC.dll"]
