# Deploying Digital Employee API on Render.com

## Overview
This guide walks you through deploying the .NET 10 API to [Render.com](https://render.com) using Docker,
with an external SQL Server database (Azure SQL free tier recommended).

---

## Step 1 – Get a Free SQL Server Database (Azure SQL)

1. Go to [portal.azure.com](https://portal.azure.com) and sign in (free account is fine).
2. Search for **Azure SQL** → click **Create**.
3. Choose **SQL Database** → fill in:
   - **Resource group**: create new, e.g. `digital-employee-rg`
   - **Database name**: `DigitalEmployeeDB`
   - **Server**: create new → set admin login + password
   - **Compute + storage**: click *Configure database* → choose **Free tier** (100K vCore-seconds/month, 32 GB)
4. On the **Networking** tab:
   - Set *Allow Azure services and resources to access this server* → **Yes**
   - Add your IP for local testing
5. Click **Review + Create** → **Create**.
6. Once deployed, go to the database → **Connection strings** → copy the **ADO.NET** string.
   It looks like:
   ```
   Server=tcp:<yourserver>.database.windows.net,1433;Initial Catalog=DigitalEmployeeDB;
   Persist Security Info=False;User ID=<user>;Password=<password>;
   MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;
   Connection Timeout=30;
   ```
7. **Add** `MultipleActiveResultSets=True;` to the string (required by the app).

---

## Step 2 – Push Project to GitHub

The project must be in a GitHub repository for Render to pull it.

```bash
cd /path/to/GraduationProject
git init          # skip if already a repo
git add .
git commit -m "Add Render deployment files"
git remote add origin https://github.com/<your-username>/<repo-name>.git
git push -u origin main
```

> Make sure `.gitignore` excludes `appsettings.Development.json` and any secrets.

---

## Step 3 – Create the Web Service on Render

1. Go to [dashboard.render.com](https://dashboard.render.com) → **New +** → **Web Service**.
2. Connect your GitHub account and select the repository.
3. Configure:
   - **Name**: `digital-employee-api`
   - **Region**: choose closest to your users
   - **Branch**: `main`
   - **Runtime**: **Docker** (Render auto-detects the `Dockerfile`)
   - **Plan**: Free (or Starter for always-on)
4. Click **Create Web Service** – Render will start the first build.

---

## Step 4 – Set Environment Variables

In Render dashboard → your service → **Environment** tab, add these variables:

| Key | Value |
|-----|-------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ConnectionStrings__DefaultConnection` | *(your Azure SQL connection string from Step 1)* |
| `JWT__Key` | *(a long random secret, e.g. 64-char string)* |
| `JWT__Issuer` | `https://digital-employee-api.onrender.com` |
| `JWT__Audience` | `https://digital-employee-api.onrender.com` |
| `Google__ClientId` | *(your Google OAuth client ID)* |

> **Tip:** Generate a strong JWT key with:
> ```bash
> openssl rand -base64 64
> ```

After saving, Render will **automatically redeploy**.

---

## Step 5 – Verify the Deployment

1. Wait for the build to finish (5–10 minutes first time).
2. Visit: `https://digital-employee-api.onrender.com/swagger`
   - You should see the Swagger UI with all endpoints listed.
3. The app **automatically runs EF migrations** on startup — your database tables will be created on first launch.

---

## Step 6 – Update Your Frontend / AI Team

Replace `http://localhost:5157` with your Render URL in all API calls:
```
https://digital-employee-api.onrender.com
```

Update the Postman environment variable `baseUrl` to the new URL.

---

## Troubleshooting

| Problem | Fix |
|---------|-----|
| Build fails: SDK version | The Dockerfile uses `mcr.microsoft.com/dotnet/sdk:10.0` – ensure .NET 10 packages are available |
| DB connection error | Double-check Azure SQL firewall allows Azure services; verify connection string in Render env vars |
| 401 on all endpoints | Ensure `JWT__Key`, `JWT__Issuer`, `JWT__Audience` are set and match what your frontend sends |
| App sleeps on free tier | Free plan spins down after 15 min inactivity. Upgrade to Starter ($7/mo) for always-on |
| Migrations fail | Check Azure SQL user has `db_owner` role on `DigitalEmployeeDB` |

---

## Files Added by This Setup

| File | Purpose |
|------|---------|
| `Dockerfile` | Multi-stage Docker build for the .NET 10 solution |
| `.dockerignore` | Excludes bin/obj/docs from Docker build context |
| `render.yaml` | Render infrastructure-as-code (optional, for Blueprint deploys) |
| `digital employee/Program.cs` | Updated: Swagger enabled in prod, auto-migration on startup |
| `digital employee/digital employee.csproj` | Updated: `AssemblyName` set to `digital_employee` (no spaces) |
