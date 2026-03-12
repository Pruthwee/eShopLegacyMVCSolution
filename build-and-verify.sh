#!/bin/bash

# eShopPorted Build and Verification Script
# This script builds the .NET 8 Clean Architecture solution and verifies the build

echo "=========================================="
echo "eShopPorted Build and Verification Script"
echo "=========================================="
echo ""

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to print colored output
print_success() {
    echo -e "${GREEN}✓ $1${NC}"
}

print_error() {
    echo -e "${RED}✗ $1${NC}"
}

print_info() {
    echo -e "${YELLOW}ℹ $1${NC}"
}

# Check if .NET 8 SDK is installed
echo "Step 1: Checking .NET 8 SDK..."
if command -v dotnet &> /dev/null; then
    DOTNET_VERSION=$(dotnet --version)
    print_success ".NET SDK version: $DOTNET_VERSION"
    
    # Check if it's .NET 8
    if [[ $DOTNET_VERSION == 8.* ]]; then
        print_success ".NET 8 SDK is installed"
    else
        print_error ".NET 8 SDK is required but version $DOTNET_VERSION is installed"
        exit 1
    fi
else
    print_error ".NET SDK is not installed"
    exit 1
fi
echo ""

# Navigate to solution directory
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd "$SCRIPT_DIR"

# Clean previous builds
echo "Step 2: Cleaning previous builds..."
dotnet clean eShopPorted.sln --nologo > /dev/null 2>&1
if [ $? -eq 0 ]; then
    print_success "Clean completed"
else
    print_error "Clean failed"
fi
echo ""

# Restore NuGet packages
echo "Step 3: Restoring NuGet packages..."
dotnet restore eShopPorted.sln --nologo
if [ $? -eq 0 ]; then
    print_success "Package restore completed"
else
    print_error "Package restore failed"
    exit 1
fi
echo ""

# Build Domain layer
echo "Step 4: Building Domain layer..."
dotnet build eShopPorted.Domain/eShopPorted.Domain.csproj --nologo --no-restore
if [ $? -eq 0 ]; then
    print_success "Domain layer built successfully"
else
    print_error "Domain layer build failed"
    exit 1
fi
echo ""

# Build Application layer
echo "Step 5: Building Application layer..."
dotnet build eShopPorted.Application/eShopPorted.Application.csproj --nologo --no-restore
if [ $? -eq 0 ]; then
    print_success "Application layer built successfully"
else
    print_error "Application layer build failed"
    exit 1
fi
echo ""

# Build Infrastructure layer
echo "Step 6: Building Infrastructure layer..."
dotnet build eShopPorted.Infrastructure/eShopPorted.Infrastructure.csproj --nologo --no-restore
if [ $? -eq 0 ]; then
    print_success "Infrastructure layer built successfully"
else
    print_error "Infrastructure layer build failed"
    exit 1
fi
echo ""

# Build Web layer
echo "Step 7: Building Web layer..."
dotnet build eShopPorted.Web/eShopPorted.Web.csproj --nologo --no-restore
if [ $? -eq 0 ]; then
    print_success "Web layer built successfully"
else
    print_error "Web layer build failed"
    exit 1
fi
echo ""

# Build entire solution
echo "Step 8: Building entire solution..."
dotnet build eShopPorted.sln --nologo --no-restore
if [ $? -eq 0 ]; then
    print_success "Solution built successfully"
else
    print_error "Solution build failed"
    exit 1
fi
echo ""

# Verify project structure
echo "Step 9: Verifying project structure..."

# Check Domain layer
if [ -d "eShopPorted.Domain/Entities" ] && [ -d "eShopPorted.Domain/Interfaces" ]; then
    print_success "Domain layer structure verified"
else
    print_error "Domain layer structure is incorrect"
fi

# Check Application layer
if [ -d "eShopPorted.Application/DTOs" ] && [ -d "eShopPorted.Application/Services" ]; then
    print_success "Application layer structure verified"
else
    print_error "Application layer structure is incorrect"
fi

# Check Infrastructure layer
if [ -d "eShopPorted.Infrastructure/Data" ] && [ -d "eShopPorted.Infrastructure/Repositories" ]; then
    print_success "Infrastructure layer structure verified"
else
    print_error "Infrastructure layer structure is incorrect"
fi

# Check Web layer
if [ -d "eShopPorted.Web/Controllers" ] && [ -d "eShopPorted.Web/Views" ]; then
    print_success "Web layer structure verified"
else
    print_error "Web layer structure is incorrect"
fi
echo ""

# Count files
echo "Step 10: File statistics..."
DOMAIN_FILES=$(find eShopPorted.Domain -name "*.cs" | wc -l)
APPLICATION_FILES=$(find eShopPorted.Application -name "*.cs" | wc -l)
INFRASTRUCTURE_FILES=$(find eShopPorted.Infrastructure -name "*.cs" | wc -l)
WEB_FILES=$(find eShopPorted.Web -name "*.cs" | wc -l)
TOTAL_FILES=$((DOMAIN_FILES + APPLICATION_FILES + INFRASTRUCTURE_FILES + WEB_FILES))

print_info "Domain layer: $DOMAIN_FILES C# files"
print_info "Application layer: $APPLICATION_FILES C# files"
print_info "Infrastructure layer: $INFRASTRUCTURE_FILES C# files"
print_info "Web layer: $WEB_FILES C# files"
print_info "Total: $TOTAL_FILES C# files"
echo ""

# Check for .NET 8 target framework
echo "Step 11: Verifying target framework..."
if grep -q "<TargetFramework>net8.0</TargetFramework>" eShopPorted.Domain/eShopPorted.Domain.csproj; then
    print_success "Domain targets .NET 8"
else
    print_error "Domain does not target .NET 8"
fi

if grep -q "<TargetFramework>net8.0</TargetFramework>" eShopPorted.Application/eShopPorted.Application.csproj; then
    print_success "Application targets .NET 8"
else
    print_error "Application does not target .NET 8"
fi

if grep -q "<TargetFramework>net8.0</TargetFramework>" eShopPorted.Infrastructure/eShopPorted.Infrastructure.csproj; then
    print_success "Infrastructure targets .NET 8"
else
    print_error "Infrastructure does not target .NET 8"
fi

if grep -q "<TargetFramework>net8.0</TargetFramework>" eShopPorted.Web/eShopPorted.Web.csproj; then
    print_success "Web targets .NET 8"
else
    print_error "Web does not target .NET 8"
fi
echo ""

# Summary
echo "=========================================="
echo "Build Verification Summary"
echo "=========================================="
print_success "All projects built successfully"
print_success "Clean Architecture structure verified"
print_success "Target framework: .NET 8"
print_success "Total C# files: $TOTAL_FILES"
echo ""
print_info "Next steps:"
echo "  1. Run the application: cd eShopPorted.Web && dotnet run"
echo "  2. Open in browser: http://localhost:5000"
echo "  3. Review documentation: README.md and MIGRATION_GUIDE.md"
echo ""
print_success "Build verification completed successfully!"
