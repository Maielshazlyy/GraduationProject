# ──────────────────────────────────────────────
#  Stage 1 – Build & Publish
# ──────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution file
COPY ["digital employee.sln", "./"]

# Copy project files (names with spaces must be quoted)
COPY ["digital employee/digital employee.csproj",   "digital employee/"]
COPY ["DAL/DAL.csproj",                             "DAL/"]
COPY ["Domain layer/Domain layer.csproj",           "Domain layer/"]
COPY ["Service layer/Service layer.csproj",         "Service layer/"]
COPY ["Infrastructure Layer/Infrastructure Layer.csproj", "Infrastructure Layer/"]

# Restore NuGet packages (cached layer)
RUN dotnet restore "digital employee/digital employee.csproj"

# Copy all remaining source files
COPY . .

# Publish release build
RUN dotnet publish "digital employee/digital employee.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# ──────────────────────────────────────────────
#  Stage 2 – Runtime image (smaller)
# ──────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Render injects PORT at runtime; default to 10000
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "digital_employee.dll"]
