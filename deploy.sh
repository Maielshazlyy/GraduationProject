#!/usr/bin/env bash
# ──────────────────────────────────────────────────────────────
#  Digital Employee — One-Command Deploy to Fly.io
#  Usage: bash deploy.sh
#  Requires: Internet access + browser for one-time Fly.io login
# ──────────────────────────────────────────────────────────────
set -euo pipefail

# ── Colours ────────────────────────────────────────────────────
GREEN='\033[0;32m'; YELLOW='\033[1;33m'; RED='\033[0;31m'; NC='\033[0m'
info()  { echo -e "${GREEN}[✔]${NC} $*"; }
warn()  { echo -e "${YELLOW}[!]${NC} $*"; }
error() { echo -e "${RED}[✘]${NC} $*"; exit 1; }

# ── Config ─────────────────────────────────────────────────────
APP_API="digital-employee-api"
APP_DB="digital-employee-db"
REGION="iad"              # iad=US-East | ams=Europe | sin=Singapore | syd=Australia
SA_PASS="${SA_PASSWORD:-Admin@Secure999!}"   # override with: SA_PASSWORD=xxx bash deploy.sh
JWT_SECRET="${JWT_KEY:-$(openssl rand -base64 48 | tr -dc 'A-Za-z0-9' | head -c 64)}"
AI_URL="${AI_BASE_URL:-https://anyway-remix-puzzling.ngrok-free.dev}"

echo ""
echo "╔══════════════════════════════════════════════╗"
echo "║  Digital Employee — Fly.io Deployment        ║"
echo "╚══════════════════════════════════════════════╝"
echo ""

# ── 1. Install flyctl ───────────────────────────────────────────
if ! command -v flyctl &>/dev/null && ! command -v fly &>/dev/null; then
    warn "Installing fly.io CLI..."
    curl -L https://fly.io/install.sh | sh
    export FLYCTL_INSTALL="$HOME/.fly"
    export PATH="$FLYCTL_INSTALL/bin:$PATH"
    info "flyctl installed"
fi
FLY="$(command -v flyctl 2>/dev/null || command -v fly)"

# ── 2. Auth ─────────────────────────────────────────────────────
if ! $FLY auth whoami &>/dev/null; then
    warn "Opening browser for Fly.io login (free account)..."
    $FLY auth login
fi
info "Logged in as: $($FLY auth whoami)"

# ── 3. Deploy SQL Server ────────────────────────────────────────
echo ""
info "Deploying SQL Server 2022 Express..."

$FLY apps list | grep -q "$APP_DB" \
    || $FLY apps create "$APP_DB" --org personal
info "App '$APP_DB' ready"

$FLY volumes list --app "$APP_DB" 2>/dev/null | grep -q "sqlserver_data" \
    || $FLY volumes create sqlserver_data --app "$APP_DB" --region "$REGION" --size 5
info "Volume ready"

$FLY secrets set \
    ACCEPT_EULA=Y \
    SA_PASSWORD="$SA_PASS" \
    MSSQL_PID=Express \
    MSSQL_MEMORY_LIMIT_MB=512 \
    --app "$APP_DB" --stage
info "SQL Server secrets set"

$FLY deploy --config fly-db.toml --app "$APP_DB" --remote-only
info "SQL Server deployed ✔"

# ── 4. Deploy .NET API ──────────────────────────────────────────
echo ""
info "Deploying .NET 10 API..."

$FLY apps list | grep -q "$APP_API" \
    || $FLY apps create "$APP_API" --org personal
info "App '$APP_API' ready"

DB_CONN="Server=${APP_DB}.internal,1433;Database=DigitalEmployeeDB;User Id=sa;Password=${SA_PASS};MultipleActiveResultSets=True;TrustServerCertificate=True;Connection Timeout=60;"

$FLY secrets set \
    ConnectionStrings__DefaultConnection="$DB_CONN" \
    JWT__Key="$JWT_SECRET" \
    JWT__Issuer="https://${APP_API}.fly.dev" \
    JWT__Audience="https://${APP_API}.fly.dev" \
    AI__BaseUrl="$AI_URL" \
    --app "$APP_API" --stage
info "API secrets set"

$FLY deploy --config fly.toml --app "$APP_API" --remote-only
info "API deployed ✔"

# ── 5. Done ─────────────────────────────────────────────────────
echo ""
echo "╔══════════════════════════════════════════════════════════╗"
echo "║  ✅  DEPLOYMENT COMPLETE                                 ║"
echo "╠══════════════════════════════════════════════════════════╣"
printf "║  🌐  API:     https://%-36s║\n" "${APP_API}.fly.dev"
printf "║  📖  Swagger: https://%-36s║\n" "${APP_API}.fly.dev/swagger"
echo "╚══════════════════════════════════════════════════════════╝"
echo ""
echo "First startup takes ~2 min (SQL Server cold start)."
echo "Migrations run automatically on first API launch."
