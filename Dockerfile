# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

WORKDIR /src

# Copy solution and project files for dependency resolution
COPY ["ClinicManagement/ClinicManagement.sln", "ClinicManagement/"]
COPY ["ClinicManagement/src/ClinicManagement.Web/ClinicManagement.Web.csproj", "ClinicManagement/src/ClinicManagement.Web/"]
COPY ["ClinicManagement/src/ClinicManagement.Application/ClinicManagement.Application.csproj", "ClinicManagement/src/ClinicManagement.Application/"]
COPY ["ClinicManagement/src/ClinicManagement.Infrastructure/ClinicManagement.Infrastructure.csproj", "ClinicManagement/src/ClinicManagement.Infrastructure/"]
COPY ["ClinicManagement/src/ClinicManagement.Domain/ClinicManagement.Domain.csproj", "ClinicManagement/src/ClinicManagement.Domain/"]

# Restore dependencies
RUN dotnet restore "ClinicManagement/src/ClinicManagement.Web/ClinicManagement.Web.csproj"

# Copy entire source code
COPY ClinicManagement/ ClinicManagement/

# Build the application
WORKDIR /src/ClinicManagement/src/ClinicManagement.Web
RUN dotnet build "ClinicManagement.Web.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "ClinicManagement.Web.csproj" -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Create non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published application from builder
COPY --from=builder /app/publish .

# Set ownership
RUN chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Set environment variables for ASP.NET Core
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    TZ=America/New_York

# Expose application port
EXPOSE 8080

# Health check is handled by ECS service - no curl/wget installation needed

# Entry point
ENTRYPOINT ["dotnet", "ClinicManagement.Web.dll"]