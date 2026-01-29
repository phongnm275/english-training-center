# 🔨 BUILD & TEST GUIDE - English Training Center LMS

**Date**: January 29, 2026  
**Framework**: .NET Core 8.0  
**Database**: SQL Server 2019+  
**Status**: Complete Guide

---

## 📋 TABLE OF CONTENTS

1. [Prerequisites & Setup](#1-prerequisites--setup)
2. [Project Structure](#2-project-structure)
3. [Building the Project](#3-building-the-project)
4. [Test Environment Setup](#4-test-environment-setup)
5. [Running Tests](#5-running-tests)
6. [Database Management](#6-database-management)
7. [Troubleshooting](#7-troubleshooting)
8. [CI/CD Integration](#8-cicd-integration)

---

## 1. PREREQUISITES & SETUP

### System Requirements
```
✅ Operating System: Windows 10/11, macOS, or Linux
✅ .NET 8.0 SDK (minimum version 8.0.0)
✅ SQL Server 2019+ or SQL Server Express
✅ Git (for version control)
✅ Visual Studio 2022 OR VS Code with C# extension
✅ 4GB+ RAM (8GB+ recommended for testing)
✅ 5GB+ disk space for build artifacts & databases
```

### Installation Steps

#### Step 1: Install .NET 8.0 SDK
```powershell
# Windows - Using Chocolatey
choco install dotnet-sdk-8.0

# Or download from:
# https://dotnet.microsoft.com/en-us/download/dotnet/8.0

# Verify installation
dotnet --version
# Output: 8.0.x
```

#### Step 2: Install SQL Server
```powershell
# Option A: SQL Server 2022 Express (Free)
# Download from: https://www.microsoft.com/sql-server/sql-server-express

# Option B: LocalDB (lightweight, for development)
# Installed with Visual Studio 2022

# Option C: Docker Container
docker pull mcr.microsoft.com/mssql/server:2022-latest
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123!" `
  -p 1433:1433 `
  -d mcr.microsoft.com/mssql/server:2022-latest

# Verify connection
sqlcmd -S localhost -U sa -P YourPassword123! -Q "SELECT @@VERSION"
```

#### Step 3: Clone Project
```powershell
# Navigate to your desired directory
cd C:\Projects

# Clone the repository
git clone <repository-url> english-training-center
cd english-training-center

# Verify structure
dir

# Output should show:
# src/
# tests/
# database/
# docs/
# EnglishTrainingCenter.sln
```

#### Step 4: Install Dependencies
```powershell
# Navigate to project root
cd C:\Projects\english-training-center

# Restore NuGet packages
dotnet restore

# Output: Restored 50+ packages
```

---

## 2. PROJECT STRUCTURE

```
english-training-center/
├── src/
│   ├── EnglishTrainingCenter.API/          [ASP.NET Core API Layer]
│   │   ├── Controllers/
│   │   ├── Middleware/
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   ├── Program.cs
│   │   └── EnglishTrainingCenter.API.csproj
│   │
│   ├── EnglishTrainingCenter.Application/  [Business Logic Layer]
│   │   ├── Services/
│   │   ├── DTOs/
│   │   ├── Validators/
│   │   └── EnglishTrainingCenter.Application.csproj
│   │
│   ├── EnglishTrainingCenter.Domain/       [Domain Layer]
│   │   ├── Entities/
│   │   ├── Interfaces/
│   │   └── EnglishTrainingCenter.Domain.csproj
│   │
│   ├── EnglishTrainingCenter.Infrastructure/ [Data Access Layer]
│   │   ├── Data/
│   │   ├── Repositories/
│   │   └── EnglishTrainingCenter.Infrastructure.csproj
│   │
│   └── EnglishTrainingCenter.Common/       [Shared Utilities]
│       ├── Utilities/
│       └── EnglishTrainingCenter.Common.csproj
│
├── tests/
│   └── EnglishTrainingCenter.Tests.Unit/   [Unit Tests]
│       ├── Services/
│       ├── Validators/
│       ├── Utilities/
│       └── EnglishTrainingCenter.Tests.Unit.csproj
│
├── database/
│   ├── scripts/                             [DB Scripts]
│   ├── migrations/                          [EF Core Migrations]
│   └── seeds/                               [Seed Data]
│
├── docs/                                     [Documentation]
├── EnglishTrainingCenter.sln                [Solution File]
├── .gitignore
└── README.md
```

---

## 3. BUILDING THE PROJECT

### Step 1: Development Build
```powershell
# Navigate to project root
cd C:\Projects\english-training-center

# Clean previous builds
dotnet clean

# Restore packages
dotnet restore

# Build solution (Debug mode)
dotnet build --configuration Debug

# Output:
# Build succeeded. 0 Warning(s)
```

### Step 2: Production Build
```powershell
# Build solution (Release mode)
dotnet build --configuration Release

# Output:
# Build succeeded. 0 Warning(s)
```

### Step 3: Publish for Deployment
```powershell
# Publish to folder
dotnet publish -c Release -o ./publish

# Output folder structure:
# publish/
# ├── EnglishTrainingCenter.API.exe
# ├── EnglishTrainingCenter.Application.dll
# ├── appsettings.json
# ├── appsettings.Production.json
# └── ... (all dependencies)
```

### Build Command Reference
```powershell
# Full build command with options
dotnet build `
  --configuration Release `
  --verbosity normal `
  --no-restore

# Build specific project
dotnet build src/EnglishTrainingCenter.API/EnglishTrainingCenter.API.csproj

# Build and check for warnings
dotnet build --no-restore --no-incremental -warnAsError
```

---

## 4. TEST ENVIRONMENT SETUP

### Step 1: Configure Test Database

#### Create Test Connection String
```powershell
# Edit: appsettings.Development.json

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EnglishTrainingCenter_Dev;Trusted_Connection=true;TrustServerCertificate=true;",
    "TestConnection": "Server=localhost;Database=EnglishTrainingCenter_Test;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

#### Create Test Database
```powershell
# Using SQL Server Management Studio (SSMS)
# 1. Connect to: localhost
# 2. Right-click on "Databases"
# 3. New Database
# 4. Name: EnglishTrainingCenter_Test
# 5. Click OK

# OR use SQL command:
sqlcmd -S localhost -U sa -P YourPassword123! -Q "CREATE DATABASE EnglishTrainingCenter_Test"

# Verify
sqlcmd -S localhost -U sa -P YourPassword123! -Q "SELECT name FROM sys.databases WHERE name LIKE 'EnglishTrainingCenter%'"
```

### Step 2: Setup Test Data

```powershell
# Navigate to tests folder
cd tests/EnglishTrainingCenter.Tests.Unit

# Create appsettings.json for tests
@"
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EnglishTrainingCenter_Test;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Jwt": {
    "Secret": "test-secret-key-very-long-for-testing-purposes",
    "Issuer": "EnglishTrainingCenter",
    "Audience": "EnglishTrainingCenterUsers",
    "ExpiryMinutes": 60
  }
}
"@ | Out-File -FilePath appsettings.json
```

### Step 3: Install Test Frameworks

#### Check Test Project Configuration
```xml
<!-- EnglishTrainingCenter.Tests.Unit.csproj -->

<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <IsTestProject>true</IsTestProject>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <!-- Unit Testing Framework -->
    <PackageReference Include="xunit" Version="2.6.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.3" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.2" />
    
    <!-- Mocking & Assertions -->
    <PackageReference Include="Moq" Version="4.20.69" />
    <PackageReference Include="FluentAssertions" Version="6.12.0" />
    
    <!-- Fixtures & Helpers -->
    <PackageReference Include="xunit.runner.console" Version="2.6.3" />
    <PackageReference Include="Respawn" Version="5.0.1" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\src\EnglishTrainingCenter.API\EnglishTrainingCenter.API.csproj" />
    <ProjectReference Include="..\..\src\EnglishTrainingCenter.Application\EnglishTrainingCenter.Application.csproj" />
  </ItemGroup>

</Project>
```

#### Restore Test Dependencies
```powershell
# Restore test project packages
cd tests/EnglishTrainingCenter.Tests.Unit
dotnet restore

# Output: Successfully restored...
```

---

## 5. RUNNING TESTS

### Unit Tests

#### Run All Tests
```powershell
cd C:\Projects\english-training-center

# Run all unit tests
dotnet test

# Output:
# Test Session started...
# Test run with 45 test(s), 45 Passed...
# Total tests: 45, Passed: 45, Failed: 0, Skipped: 0
```

#### Run Tests with Options
```powershell
# Run tests with detailed output
dotnet test --verbosity normal

# Run tests with logging
dotnet test --logger "console;verbosity=detailed"

# Run tests and generate report
dotnet test --logger "trx;LogFileName=TestResults.trx"

# Run specific test file
dotnet test tests/EnglishTrainingCenter.Tests.Unit/Services/StudentServiceTests.cs

# Run tests matching pattern
dotnet test --filter "Category=Services"

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

#### Visual Studio Test Explorer
```
1. Open Solution in Visual Studio 2022
2. Menu: Test → Test Explorer
3. Click "Run All Tests"
4. View results in Test Explorer window
5. Double-click failed tests for details
```

#### Run Tests for Each Layer

```powershell
# Test Application Layer Services
dotnet test tests/EnglishTrainingCenter.Tests.Unit/Services/ `
  --logger "console;verbosity=detailed"

# Test Validators
dotnet test tests/EnglishTrainingCenter.Tests.Unit/Validators/ `
  --logger "console;verbosity=detailed"

# Test Utilities
dotnet test tests/EnglishTrainingCenter.Tests.Unit/Utilities/ `
  --logger "console;verbosity=detailed"
```

### Integration Tests

#### Create Integration Test Project
```powershell
# Create new test project
dotnet new xunit -n EnglishTrainingCenter.Tests.Integration `
  -o tests/EnglishTrainingCenter.Tests.Integration

# Add project references
cd tests/EnglishTrainingCenter.Tests.Integration
dotnet add reference ../../src/EnglishTrainingCenter.API/EnglishTrainingCenter.API.csproj
```

#### Run Integration Tests
```powershell
# Run integration tests
dotnet test tests/EnglishTrainingCenter.Tests.Integration/ `
  --logger "console;verbosity=detailed"

# Run both unit and integration tests
dotnet test --logger "console;verbosity=detailed"
```

### API Tests (Postman/Thunder Client)

#### Setup API Testing
```powershell
# 1. Start the API server
dotnet run --project src/EnglishTrainingCenter.API

# 2. Server running at:
# https://localhost:5001
# http://localhost:5000

# 3. Swagger UI available at:
# https://localhost:5001/swagger/index.html

# 4. Import Postman collection from:
# docs/postman/EnglishTrainingCenter.postman_collection.json
```

#### Sample API Test (PowerShell)
```powershell
# Login endpoint
$loginUrl = "https://localhost:5001/api/v1/auth/login"
$loginBody = @{
    email = "admin@example.com"
    password = "Password123!"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri $loginUrl `
  -Method Post `
  -ContentType "application/json" `
  -Body $loginBody `
  -SkipCertificateCheck

# Extract token
$token = $response.data.accessToken
Write-Host "Token: $token"

# Use token in subsequent requests
$studentUrl = "https://localhost:5001/api/v1/students"
$headers = @{
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
}

$students = Invoke-RestMethod -Uri $studentUrl `
  -Headers $headers `
  -SkipCertificateCheck

Write-Host ($students | ConvertTo-Json)
```

### Load Testing

#### Using Apache JMeter
```powershell
# Download JMeter from: https://jmeter.apache.org/download_jmeter.html

# Extract and run
cd C:\apache-jmeter\bin
jmeter.bat

# Create Test Plan:
# 1. Thread Group: 100 users, ramp-up 10 seconds, loop count 10
# 2. HTTP Request: POST /api/v1/students
# 3. Assertions: Response code 200/201
# 4. Listeners: View Results Tree, Summary Report
```

#### Using Load Testing Script
```powershell
# Create load test script
$apiUrl = "https://localhost:5001/api/v1/students"
$token = "your-jwt-token"
$headers = @{"Authorization" = "Bearer $token"}

# Simulate 100 concurrent requests
1..100 | ForEach-Object -Parallel {
    $body = @{
        fullName = "Student $_"
        email = "student$_@example.com"
        phone = "555-000-$_"
    } | ConvertTo-Json
    
    $response = Invoke-RestMethod -Uri $apiUrl `
      -Method Post `
      -Headers $headers `
      -Body $body `
      -SkipCertificateCheck `
      -ErrorAction SilentlyContinue
    
    Write-Host "Request $_: Status $(if($response) {'Success'} else {'Failed'})"
} -ThrottleLimit 50
```

---

## 6. DATABASE MANAGEMENT

### Database Migrations

#### Create Database
```powershell
cd src/EnglishTrainingCenter.Infrastructure

# Create migration
dotnet ef migrations add InitialCreate `
  --project ../EnglishTrainingCenter.Infrastructure `
  --startup-project ../EnglishTrainingCenter.API

# Update database
dotnet ef database update `
  --project ../EnglishTrainingCenter.Infrastructure `
  --startup-project ../EnglishTrainingCenter.API
```

#### Add New Migration
```powershell
# Create migration for new feature
dotnet ef migrations add AddStudentGradeField `
  --project ../EnglishTrainingCenter.Infrastructure `
  --startup-project ../EnglishTrainingCenter.API

# Update database
dotnet ef database update
```

#### Rollback Migration
```powershell
# Revert to previous migration
dotnet ef database update PreviousMigrationName `
  --project ../EnglishTrainingCenter.Infrastructure `
  --startup-project ../EnglishTrainingCenter.API

# Remove last migration
dotnet ef migrations remove `
  --project ../EnglishTrainingCenter.Infrastructure `
  --startup-project ../EnglishTrainingCenter.API
```

### Seed Test Data
```powershell
# Create seed script
@"
-- Create test users
INSERT INTO Users (Username, Email, PasswordHash, FullName, RoleId, IsActive, CreatedDate)
VALUES ('admin', 'admin@example.com', 'hash_value', 'Administrator', 1, 1, GETUTCDATE());

-- Create test students
INSERT INTO Students (UserId, FullName, Email, Phone, EnrollmentDate, IsActive, CreatedDate)
VALUES (1, 'John Doe', 'john@example.com', '555-0001', GETUTCDATE(), 1, GETUTCDATE());

-- Create test courses
INSERT INTO Courses (CourseName, Description, Duration, Level, MaxCapacity, IsActive, CreatedDate)
VALUES ('English Basics', 'Introduction to English', 30, 1, 20, 1, GETUTCDATE());
"@ | Out-File -FilePath database/seeds/test-data.sql

# Run seed script
sqlcmd -S localhost -U sa -P YourPassword123! `
  -d EnglishTrainingCenter_Test `
  -i database/seeds/test-data.sql
```

---

## 7. TROUBLESHOOTING

### Common Issues & Solutions

#### Issue 1: "Connection String Not Found"
```
Error: Connection string 'DefaultConnection' not found.

Solution:
1. Check appsettings.json in API project
2. Verify ConnectionStrings section exists
3. For tests, check appsettings in test project
4. Ensure file is not excluded from build
```

#### Issue 2: "Database Does Not Exist"
```
Error: Cannot open database 'EnglishTrainingCenter'

Solution:
1. Verify SQL Server is running
2. Create database manually
3. Run migrations: dotnet ef database update
4. Check server name in connection string
```

#### Issue 3: "Port Already In Use"
```
Error: Unable to bind to http://localhost:5000 on address 127.0.0.1 for IPv4.

Solution:
# Find process using port
netstat -ano | findstr :5000

# Kill process
taskkill /PID <process-id> /F

# Or use different port in launchSettings.json
```

#### Issue 4: "Build Fails with NuGet Error"
```
Error: Unable to restore dependencies

Solution:
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore again
dotnet restore --force

# Update NuGet
dotnet tool update -g NuGet.CommandLine
```

#### Issue 5: "Test Fails with Timeout"
```
Error: Test timed out after 30 seconds

Solution:
1. Increase timeout in test settings
2. Check for database locks
3. Use separate test database
4. Implement test fixtures for isolation
```

### Debug Mode Execution

```powershell
# Run API with debugging
dotnet run --project src/EnglishTrainingCenter.API --debug

# Run tests with debugging
dotnet test --logger "console;verbosity=detailed" --debug

# Attach debugger in Visual Studio
# Debug → Attach to Process → Choose dotnet.exe process
```

---

## 8. CI/CD INTEGRATION

### GitHub Actions Workflow

```yaml
# .github/workflows/build-and-test.yml

name: Build and Test

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    
    services:
      mssql:
        image: mcr.microsoft.com/mssql/server:2022-latest
        env:
          SA_PASSWORD: 'TestPassword123!'
          ACCEPT_EULA: 'Y'
        options: >-
          --health-cmd "/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TestPassword123! -Q 'SELECT 1' || exit 1"
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5
        ports:
          - 1433:1433

    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --configuration Release --no-restore
    
    - name: Run Unit Tests
      run: dotnet test --configuration Release --no-build --logger "console;verbosity=detailed"
    
    - name: Upload Coverage
      uses: codecov/codecov-action@v3
      with:
        files: ./coverage/coverage.xml
```

### Azure DevOps Pipeline

```yaml
# azure-pipelines.yml

trigger:
  - main
  - develop

pool:
  vmImage: 'windows-latest'

variables:
  buildConfiguration: 'Release'
  dotnetVersion: '8.0.x'

steps:
- task: UseDotNet@2
  inputs:
    version: $(dotnetVersion)
    packageType: sdk

- task: DotNetCoreCLI@2
  displayName: 'Restore NuGet packages'
  inputs:
    command: 'restore'
    projects: '**/*.csproj'

- task: DotNetCoreCLI@2
  displayName: 'Build solution'
  inputs:
    command: 'build'
    arguments: '--configuration $(buildConfiguration)'

- task: DotNetCoreCLI@2
  displayName: 'Run tests'
  inputs:
    command: 'test'
    arguments: '--configuration $(buildConfiguration) --logger trx --collect:"XPlat Code Coverage"'

- task: PublishTestResults@2
  displayName: 'Publish test results'
  inputs:
    testResultsFormat: 'VSTest'
    testResultsFiles: '**/*.trx'

- task: PublishCodeCoverageResults@1
  displayName: 'Publish code coverage'
  inputs:
    codeCoverageTool: Cobertura
    summaryFileLocation: '$(Agent.TempDirectory)/**/coverage.cobertura.xml'
```

---

## QUICK REFERENCE COMMANDS

### Build Commands
```powershell
# Clean build
dotnet clean && dotnet build

# Build with specific configuration
dotnet build -c Release

# Build and publish
dotnet publish -c Release -o ./publish
```

### Test Commands
```powershell
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true

# Run specific test
dotnet test --filter "TestClass=StudentServiceTests"

# Run tests with logging
dotnet test --logger "console;verbosity=detailed"
```

### Database Commands
```powershell
# Create migration
dotnet ef migrations add <MigrationName>

# Update database
dotnet ef database update

# Remove migration
dotnet ef migrations remove

# Script database
dotnet ef migrations script
```

### Run Commands
```powershell
# Run API locally
dotnet run --project src/EnglishTrainingCenter.API

# Run with specific configuration
dotnet run --project src/EnglishTrainingCenter.API --configuration Development

# Run on specific port
dotnet run --project src/EnglishTrainingCenter.API -- --urls "https://localhost:5001;http://localhost:5000"
```

---

## ENVIRONMENT CONFIGURATION

### Development Environment
```json
// appsettings.Development.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EnglishTrainingCenter_Dev;..."
  },
  "Jwt": {
    "ExpiryMinutes": 480  // 8 hours for development
  }
}
```

### Test Environment
```json
// appsettings.Test.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EnglishTrainingCenter_Test;..."
  },
  "Jwt": {
    "ExpiryMinutes": 60
  }
}
```

### Production Environment
```json
// appsettings.Production.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Error"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=EnglishTrainingCenter;..."
  },
  "Jwt": {
    "ExpiryMinutes": 60
  }
}
```

---

## PERFORMANCE TESTING CHECKLIST

```
✅ Before Testing:
  [ ] Database is fresh with test data
  [ ] API is running without errors
  [ ] All services are responding
  [ ] Network connectivity is stable

✅ During Testing:
  [ ] Monitor CPU usage
  [ ] Monitor memory usage
  [ ] Monitor disk I/O
  [ ] Monitor network bandwidth
  [ ] Collect error logs

✅ After Testing:
  [ ] Analyze results
  [ ] Identify bottlenecks
  [ ] Generate reports
  [ ] Plan optimizations
```

---

## SUMMARY

This guide covers:
1. ✅ Complete setup for development environment
2. ✅ Building .NET Core 8.0 project
3. ✅ Setting up test databases
4. ✅ Running unit, integration, and API tests
5. ✅ Database migration management
6. ✅ Troubleshooting common issues
7. ✅ CI/CD pipeline integration
8. ✅ Performance testing procedures

**Next Steps:**
1. Follow setup steps in order
2. Build and test locally
3. Implement CI/CD pipeline
4. Run comprehensive tests before deployment

---

**Guide Status**: ✅ COMPLETE  
**Last Updated**: January 29, 2026  
**Version**: 1.0

