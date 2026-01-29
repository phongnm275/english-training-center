# 🚀 QUICK START - Build & Test Guide

**English Training Center LMS**  
**.NET Core 8.0 | SQL Server 2019+**

---

## ⚡ 5-MINUTE SETUP

### Prerequisites Check
```powershell
# 1. Verify .NET 8.0
dotnet --version

# 2. Verify SQL Server running
sqlcmd -S localhost -Q "SELECT @@VERSION"

# 3. Navigate to project
cd C:\Projects\english-training-center
```

### Quick Build
```powershell
# Using automated script (recommended)
.\scripts\build.ps1 -Configuration Release

# OR manual steps
dotnet clean
dotnet restore
dotnet build -c Release
```

### Quick Test
```powershell
# Using automated script
.\scripts\test.ps1 -TestType Unit -Verbose

# OR manual command
dotnet test
```

### Quick Database Setup
```powershell
# Setup development database
.\scripts\setup-database.ps1 -Environment Development -CreateIfNotExists

# OR manual command
dotnet ef database update --project src/EnglishTrainingCenter.Infrastructure --startup-project src/EnglishTrainingCenter.API
```

---

## 📋 COMPLETE WORKFLOW

### 1️⃣ Initial Setup (First Time)

```powershell
# Step 1: Clone & navigate
git clone <repo-url>
cd english-training-center

# Step 2: Verify prerequisites
.\scripts\build.ps1 -Verbose  # Checks .NET SDK

# Step 3: Setup database
.\scripts\setup-database.ps1 -Environment Development -CreateIfNotExists

# Step 4: Run all tests
.\scripts\test.ps1 -TestType All -Verbose
```

**Expected Output**: ✅ Build succeeded, Tests passed

### 2️⃣ Daily Development

```powershell
# Each morning
cd C:\Projects\english-training-center

# 1. Update code
git pull origin main

# 2. Build project
.\scripts\build.ps1

# 3. Run tests
.\scripts\test.ps1 -TestType Unit

# 4. Start API
dotnet run --project src/EnglishTrainingCenter.API

# API will be available at:
# https://localhost:5001/swagger (Swagger UI)
# https://localhost:5001/api/v1/... (API endpoints)
```

### 3️⃣ Before Committing

```powershell
# Full test suite
.\scripts\test.ps1 -TestType All -Coverage

# Build for release
.\scripts\build.ps1 -Configuration Release

# Check for warnings
dotnet build --no-restore --no-incremental -warnAsError
```

### 4️⃣ Deployment Preparation

```powershell
# Publish application
.\scripts\build.ps1 -Configuration Release -Publish

# Output in: ./publish/

# Package for deployment
Compress-Archive -Path ./publish -DestinationPath english-training-center-release.zip
```

---

## 🧪 TESTING SCENARIOS

### Unit Tests Only
```powershell
.\scripts\test.ps1 -TestType Unit

# Expected: ~45 tests pass in ~5 seconds
```

### Integration Tests
```powershell
.\scripts\test.ps1 -TestType Integration

# Expected: ~20 tests pass in ~15 seconds
# Requires database setup
```

### With Code Coverage
```powershell
.\scripts\test.ps1 -TestType All -Coverage

# Generates coverage report:
# coverage/coverage.opencover.xml
```

### Specific Test Category
```powershell
# Test only StudentService
dotnet test --filter "Category=Services"

# Test only Validators
dotnet test --filter "Category=Validators"

# Test only Utilities
dotnet test --filter "Category=Utilities"
```

---

## 🔧 COMMON COMMANDS CHEAT SHEET

### Build Commands
| Command | Purpose |
|---------|---------|
| `dotnet build` | Build in Debug mode |
| `dotnet build -c Release` | Build in Release mode |
| `dotnet clean` | Remove build artifacts |
| `dotnet publish -c Release -o ./publish` | Publish for deployment |

### Test Commands
| Command | Purpose |
|---------|---------|
| `dotnet test` | Run all tests |
| `dotnet test --filter "TestClass=StudentTests"` | Run specific test class |
| `dotnet test /p:CollectCoverage=true` | Run with code coverage |
| `dotnet test --logger "console;verbosity=detailed"` | Detailed output |

### Database Commands
| Command | Purpose |
|---------|---------|
| `dotnet ef migrations add <Name>` | Create migration |
| `dotnet ef database update` | Apply migrations |
| `dotnet ef migrations remove` | Remove last migration |
| `dotnet ef database drop` | Delete database |

### Run Commands
| Command | Purpose |
|---------|---------|
| `dotnet run --project src/EnglishTrainingCenter.API` | Start API |
| `dotnet run --project src/EnglishTrainingCenter.API -- --urls "http://localhost:5000"` | Start on specific port |

---

## ⚠️ TROUBLESHOOTING

### "Build failed" ❌
```powershell
# Clean and restore
dotnet clean
dotnet restore
dotnet build

# Or clear NuGet cache
dotnet nuget locals all --clear
```

### "Database connection failed" ❌
```powershell
# Check SQL Server running
sqlcmd -S localhost -Q "SELECT @@VERSION"

# If not running:
# Windows: Start SQL Server from Services
# Docker: docker start <container-id>

# Verify connection string
cat appsettings.json | grep -A2 ConnectionStrings
```

### "Port already in use" ❌
```powershell
# Find what's using port 5000/5001
netstat -ano | findstr :5000

# Kill the process (replace PID)
taskkill /PID <process-id> /F

# Or use different port
# Edit: appsettings.json or launchSettings.json
```

### "Tests timeout" ❌
```powershell
# Increase timeout (add to test project)
# [Fact(Timeout = 30000)] // 30 seconds

# Or use fresh test database
.\scripts\setup-database.ps1 -Environment Test -CreateIfNotExists
```

---

## 📊 EXPECTED BUILD ARTIFACTS

After successful build:
```
bin/
├── Debug/
│   └── net8.0/
│       ├── EnglishTrainingCenter.API.dll
│       ├── EnglishTrainingCenter.Application.dll
│       ├── EnglishTrainingCenter.Domain.dll
│       ├── EnglishTrainingCenter.Infrastructure.dll
│       ├── EnglishTrainingCenter.Common.dll
│       └── appsettings.json
│
└── Release/
    └── net8.0/
        └── (same files, optimized)

publish/
├── EnglishTrainingCenter.API.exe
├── EnglishTrainingCenter.*.dll
├── appsettings.json
└── (all dependencies)
```

---

## 🔐 SECURITY CHECKLIST

Before deploying:

```
✅ JWT Secret Key
  [ ] Change secret in appsettings.Production.json
  [ ] Minimum 32 characters
  [ ] Store securely (not in version control)

✅ Database Connection
  [ ] Use production SQL Server instance
  [ ] Change sa password
  [ ] Use encrypted connection string

✅ CORS Configuration
  [ ] Update allowed origins for production
  [ ] Remove localhost origins in production

✅ Logging
  [ ] Set to Error level in production
  [ ] Configure log file location
  [ ] Enable log rotation

✅ API Configuration
  [ ] Change API version if needed
  [ ] Update description/title
  [ ] Configure rate limiting
```

---

## 📞 SUPPORT

**For issues, check:**

1. **BUILD_AND_TEST_GUIDE.md** - Comprehensive guide
2. **Database setup issues** - See database/README.md
3. **API issues** - See src/EnglishTrainingCenter.API/README.md
4. **Test issues** - See tests/README.md

**Script Help:**
```powershell
# Get help for scripts
.\scripts\build.ps1 -Help
.\scripts\test.ps1 -Help
.\scripts\setup-database.ps1 -Help
```

---

## ✅ VERIFICATION CHECKLIST

### After Setup
- [ ] .NET 8.0 SDK installed
- [ ] SQL Server running
- [ ] Project builds without errors
- [ ] All tests pass
- [ ] API starts without errors
- [ ] Swagger UI accessible (https://localhost:5001/swagger)

### Before Commit
- [ ] Unit tests pass
- [ ] Code builds with no warnings
- [ ] Database migrations applied
- [ ] No debug breakpoints left

### Before Deployment
- [ ] Release build succeeds
- [ ] All tests pass including integration
- [ ] Code coverage > 70%
- [ ] Security checklist completed
- [ ] Documentation updated

---

**Ready to start?** Run this:
```powershell
.\scripts\build.ps1 -Configuration Release && .\scripts\test.ps1 -TestType All -Coverage
```

**Good luck! 🚀**

