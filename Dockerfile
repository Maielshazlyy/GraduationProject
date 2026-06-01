FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# Publish as self-contained
RUN dotnet publish "digital employee/digital employee.csproj" \
    -c Release \
    -o /app/publish \
    -r linux-x64 \
    --self-contained true

# Final stage — use runtime-deps for self-contained binaries
FROM mcr.microsoft.com/dotnet/runtime-deps:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 10000
ENV ASPNETCORE_URLS=http://+:10000

ENTRYPOINT ["./digital_employee"]
