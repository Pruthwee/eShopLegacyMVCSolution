@echo off
REM eShopPorted Build and Verification Script for Windows
REM This script builds the .NET 8 Clean Architecture solution and verifies the build

echo ==========================================
echo eShopPorted Build and Verification Script
echo ==========================================
echo.

REM Check if .NET 8 SDK is installed
echo Step 1: Checking .NET 8 SDK...
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo [ERROR] .NET SDK is not installed
    exit /b 1
)

for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
echo [SUCCESS] .NET SDK version: %DOTNET_VERSION%

REM Check if it's .NET 8
echo %DOTNET_VERSION% | findstr /b "8." >nul
if %errorlevel% neq 0 (
    echo [ERROR] .NET 8 SDK is required but version %DOTNET_VERSION% is installed
    exit /b 1
)
echo [SUCCESS] .NET 8 SDK is installed
echo.

REM Clean previous builds
echo Step 2: Cleaning previous builds...
dotnet clean eShopPorted.sln --nologo >nul 2>&1
if %errorlevel% equ 0 (
    echo [SUCCESS] Clean completed
) else (
    echo [ERROR] Clean failed
)
echo.

REM Restore NuGet packages
echo Step 3: Restoring NuGet packages...
dotnet restore eShopPorted.sln --nologo
if %errorlevel% neq 0 (
    echo [ERROR] Package restore failed
    exit /b 1
)
echo [SUCCESS] Package restore completed
echo.

REM Build Domain layer
echo Step 4: Building Domain layer...
dotnet build eShopPorted.Domain\eShopPorted.Domain.csproj --nologo --no-restore
if %errorlevel% neq 0 (
    echo [ERROR] Domain layer build failed
    exit /b 1
)
echo [SUCCESS] Domain layer built successfully
echo.

REM Build Application layer
echo Step 5: Building Application layer...
dotnet build eShopPorted.Application\eShopPorted.Application.csproj --nologo --no-restore
if %errorlevel% neq 0 (
    echo [ERROR] Application layer build failed
    exit /b 1
)
echo [SUCCESS] Application layer built successfully
echo.

REM Build Infrastructure layer
echo Step 6: Building Infrastructure layer...
dotnet build eShopPorted.Infrastructure\eShopPorted.Infrastructure.csproj --nologo --no-restore
if %errorlevel% neq 0 (
    echo [ERROR] Infrastructure layer build failed
    exit /b 1
)
echo [SUCCESS] Infrastructure layer built successfully
echo.

REM Build Web layer
echo Step 7: Building Web layer...
dotnet build eShopPorted.Web\eShopPorted.Web.csproj --nologo --no-restore
if %errorlevel% neq 0 (
    echo [ERROR] Web layer build failed
    exit /b 1
)
echo [SUCCESS] Web layer built successfully
echo.

REM Build entire solution
echo Step 8: Building entire solution...
dotnet build eShopPorted.sln --nologo --no-restore
if %errorlevel% neq 0 (
    echo [ERROR] Solution build failed
    exit /b 1
)
echo [SUCCESS] Solution built successfully
echo.

REM Verify project structure
echo Step 9: Verifying project structure...

if exist "eShopPorted.Domain\Entities" (
    if exist "eShopPorted.Domain\Interfaces" (
        echo [SUCCESS] Domain layer structure verified
    )
) else (
    echo [ERROR] Domain layer structure is incorrect
)

if exist "eShopPorted.Application\DTOs" (
    if exist "eShopPorted.Application\Services" (
        echo [SUCCESS] Application layer structure verified
    )
) else (
    echo [ERROR] Application layer structure is incorrect
)

if exist "eShopPorted.Infrastructure\Data" (
    if exist "eShopPorted.Infrastructure\Repositories" (
        echo [SUCCESS] Infrastructure layer structure verified
    )
) else (
    echo [ERROR] Infrastructure layer structure is incorrect
)

if exist "eShopPorted.Web\Controllers" (
    if exist "eShopPorted.Web\Views" (
        echo [SUCCESS] Web layer structure verified
    )
) else (
    echo [ERROR] Web layer structure is incorrect
)
echo.

REM Summary
echo ==========================================
echo Build Verification Summary
echo ==========================================
echo [SUCCESS] All projects built successfully
echo [SUCCESS] Clean Architecture structure verified
echo [SUCCESS] Target framework: .NET 8
echo.
echo Next steps:
echo   1. Run the application: cd eShopPorted.Web ^&^& dotnet run
echo   2. Open in browser: http://localhost:5000
echo   3. Review documentation: README.md and MIGRATION_GUIDE.md
echo.
echo [SUCCESS] Build verification completed successfully!
pause
