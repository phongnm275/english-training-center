# .NET Core Project Setup Guide

## 📁 Project Structure

```
english-training-center/
├── src/
│   ├── EnglishTrainingCenter.API/                 # API Layer
│   │   ├── Controllers/
│   │   ├── Middleware/
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   └── Program.cs
│   │
│   ├── EnglishTrainingCenter.Application/         # Application Layer
│   │   ├── Services/
│   │   ├── DTOs/
│   │   ├── Validators/
│   │   ├── Mappers/
│   │   └── ApplicationDependencyInjection.cs
│   │
│   ├── EnglishTrainingCenter.Domain/              # Domain Layer
│   │   ├── Entities/
│   │   ├── Interfaces/
│   │   └── Exceptions/
│   │
│   ├── EnglishTrainingCenter.Infrastructure/      # Infrastructure Layer
│   │   ├── Data/
│   │   ├── Repositories/
│   │   ├── Services/
│   │   └── InfrastructureDependencyInjection.cs
│   │
│   └── EnglishTrainingCenter.Common/              # Shared Utilities
│       ├── Constants/
│       ├── Utilities/
│       └── Exceptions/
│
├── tests/
│   └── EnglishTrainingCenter.Tests.Unit/
│
├── database/                                       # SQL Scripts
│   └── scripts/
│
└── EnglishTrainingCenter.sln                      # Visual Studio Solution
```

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- SQL Server 2019+ (or SQL Server Express)
- Visual Studio 2022 or VS Code

### Step 1: Clone & Open Project
```bash
cd c:\BMAD\english-training-center
```

### Step 2: Restore Dependencies
```bash
dotnet restore
```

### Step 3: Setup Database

#### Option A: Using SQL Server Management Studio
1. Open SSMS
2. Connect to your SQL Server
3. Run the database scripts:
   - `database/scripts/01-create-tables.sql`
   - `database/scripts/02-insert-sample-data.sql`
   - `database/stored-procedures/01-stored-procedures.sql`

#### Option B: Using EF Core (Future)
```bash
cd src/EnglishTrainingCenter.API
dotnet ef database update
```

### Step 4: Update Connection String
Edit `src/EnglishTrainingCenter.API/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=EnglishTrainingCenter;Trusted_Connection=true;"
  }
}
```

### Step 5: Run Application
```bash
cd src/EnglishTrainingCenter.API
dotnet run
```

**Output**:
```
Application running at: https://localhost:5001
Swagger UI: https://localhost:5001/swagger/index.html
```

### Step 6: Run Tests
```bash
dotnet test
```

## 📦 Project Layers

### API Layer (EnglishTrainingCenter.API)
- **Controllers**: API endpoints
- **Middleware**: Custom middleware (exception handling, logging)
- **Program.cs**: Application entry point & configuration

**Dependencies**:
- AspNetCore
- Serilog
- Swagger

### Application Layer (EnglishTrainingCenter.Application)
- **Services**: Business logic (e.g., StudentService, CourseService)
- **DTOs**: Data transfer objects
- **Validators**: FluentValidation rules
- **Mappers**: AutoMapper profiles

**Dependencies**:
- AutoMapper
- FluentValidation

### Domain Layer (EnglishTrainingCenter.Domain)
- **Entities**: Domain models (Student, Course, Instructor, etc.)
- **Interfaces**: Repository interfaces
- **Exceptions**: Domain-specific exceptions

**No external dependencies** (only .NET)

### Infrastructure Layer (EnglishTrainingCenter.Infrastructure)
- **Data**: EF Core DbContext
- **Repositories**: Generic repository implementation
- **Services**: External service integrations

**Dependencies**:
- EntityFrameworkCore
- SqlServer

### Common Layer (EnglishTrainingCenter.Common)
- **Utilities**: ApiResponse, PagedResult, etc.
- **Constants**: Application constants
- **Helpers**: Common helper functions

## 🔧 Key Components

### Generic Repository
```csharp
// Usage in any service
public class StudentService
{
    private readonly IRepository<Student> _studentRepository;
    
    public StudentService(IRepository<Student> studentRepository)
    {
        _studentRepository = studentRepository;
    }
    
    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        return await _studentRepository.GetByIdAsync(id);
    }
}
```

### AutoMapper Configuration
```csharp
// In MappingProfiles.cs
CreateMap<Student, StudentDto>().ReverseMap();
```

### Dependency Injection
```csharp
// In Program.cs
builder.Services.AddInfrastructureServices(connectionString);
builder.Services.AddApplicationServices();
```

### FluentValidation
```csharp
// In Validators
RuleFor(x => x.Email)
    .NotEmpty().WithMessage("Email is required")
    .EmailAddress().WithMessage("Invalid email format");
```

## 📝 Configuration Files

### appsettings.json (Production)
- Connection strings
- JWT settings
- API settings
- CORS policies

### appsettings.Development.json (Development)
- Development database
- Debug logging
- Relaxed CORS policies

## 🔐 Dependency Injection Setup

All layers are registered in Program.cs:
1. **Infrastructure**: DbContext, Repositories
2. **Application**: Services, Validators, AutoMapper
3. **API**: Controllers, Middleware

## 📊 EF Core Setup

### DbContext (ETCContext)
- All entities are registered as DbSets
- Fluent API configuration in OnModelCreating
- Automatic ModifiedDate tracking

### Entities
- All entities inherit from BaseEntity
- Navigation properties configured
- Foreign keys properly defined

## 🧪 Testing Setup

### Unit Tests
- XUnit test framework
- Moq for mocking
- FluentAssertions for assertions

```csharp
[Fact]
public async Task GetStudentById_WithValidId_ReturnsStudent()
{
    // Arrange
    var mockRepository = new Mock<IRepository<Student>>();
    
    // Act
    var result = await mockRepository.Object.GetByIdAsync(1);
    
    // Assert
    result.Should().NotBeNull();
}
```

## 🚀 Building & Publishing

### Build
```bash
dotnet build
```

### Publish
```bash
dotnet publish -c Release -o ./publish
```

### Run Published App
```bash
cd ./publish
dotnet EnglishTrainingCenter.API.dll
```

## 🐳 Docker Support (Optional)

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY publish/ .
EXPOSE 80
ENTRYPOINT ["dotnet", "EnglishTrainingCenter.API.dll"]
```

Build & Run:
```bash
docker build -t etc-api:latest .
docker run -p 5000:80 etc-api:latest
```

## ✅ Checklist

- [ ] Database scripts executed
- [ ] Connection string updated
- [ ] Application runs without errors
- [ ] Swagger UI accessible
- [ ] Tests pass
- [ ] API endpoints responding

## 📚 Next Steps

1. **Implement Services**: Create StudentService, CourseService, etc.
2. **Create Controllers**: Implement API endpoints
3. **Add Authentication**: JWT bearer token authentication
4. **Payment Integration**: Integrate payment gateway
5. **Notifications**: Email/SMS services
6. **Deployment**: Deploy to Azure/AWS

## 🆘 Troubleshooting

### Connection String Error
- Verify SQL Server is running
- Check server name and database name
- Ensure authentication is correct

### EF Core Migration Issues
```bash
# Add migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

### Dependency Errors
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore
```

### Port Already In Use
```bash
# Use different port
dotnet run --launch-profile https
```

---

**Version**: 1.0  
**Last Updated**: January 28, 2026
