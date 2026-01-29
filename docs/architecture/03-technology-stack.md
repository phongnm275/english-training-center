# Technology Stack & Deployment Guide

## 1. Technology Stack

### 1.1 Backend
- **Runtime**: .NET Core 8.0 LTS
- **Framework**: ASP.NET Core Web API
- **Language**: C# 12.0
- **Database ORM**: Entity Framework Core 8.0

### 1.2 Database
- **DBMS**: SQL Server 2019+
- **Version**: Enterprise / Standard Edition
- **Backup**: Automated daily backups
- **Connection Pooling**: Yes

### 1.3 Frontend (Optional)
- **Framework**: React.js / Angular
- **UI Library**: Material-UI / Bootstrap
- **State Management**: Redux / NgRx
- **HTTP Client**: Axios / HttpClient

### 1.4 Development Tools
- **IDE**: Visual Studio 2022 / Visual Studio Code
- **Version Control**: Git (GitHub/GitLab)
- **Build Tool**: MSBuild / dotnet CLI
- **Package Manager**: NuGet
- **Testing**: XUnit / NUnit, Moq, xBehave

### 1.5 CI/CD
- **Pipeline**: GitHub Actions / Azure DevOps
- **Container**: Docker
- **Orchestration**: Kubernetes (optional)
- **Deployment**: Azure App Service / AWS EC2

### 1.6 Infrastructure
- **Cloud Platform**: Azure / AWS
- **Monitoring**: Application Insights / DataDog
- **Logging**: Serilog / ELK Stack
- **Caching**: Redis (optional)
- **Message Queue**: RabbitMQ (optional)

### 1.7 External Services
- **Email**: SendGrid / SMTP
- **Payment Gateway**: VNPay / Stripe
- **SMS**: Twilio / Local provider
- **Cloud Storage**: Azure Blob Storage / AWS S3

## 2. Development Environment Setup

### 2.1 Prerequisites
```bash
# Required Software
- Visual Studio 2022 (Community/Professional)
- .NET Core 8.0 SDK
- SQL Server 2019 Express/Developer Edition
- Git 2.40+
- Docker (optional)
```

### 2.2 Local Development Setup
```bash
# Clone repository
git clone https://github.com/your-org/english-training-center.git
cd english-training-center

# Install dependencies
dotnet restore

# Create local database
# Update appsettings.Development.json with connection string
dotnet ef database update

# Run application
dotnet run

# API runs on https://localhost:5001
```

### 2.3 Database Setup
```sql
-- Create database
CREATE DATABASE EnglishTrainingCenter;

-- Run migrations
dotnet ef database update --context ETCContext
```

## 3. Project Structure

```
EnglishTrainingCenter.sln
│
├── src/
│   ├── EnglishTrainingCenter.API/           # API Layer
│   │   ├── Controllers/                     # API Controllers
│   │   ├── Middleware/                      # Custom Middleware
│   │   ├── Startup.cs
│   │   ├── appsettings.json
│   │   └── Program.cs
│   │
│   ├── EnglishTrainingCenter.Application/   # Application Layer
│   │   ├── Services/                        # Business Services
│   │   │   ├── StudentService.cs
│   │   │   ├── CourseService.cs
│   │   │   ├── PaymentService.cs
│   │   │   └── ...
│   │   ├── DTOs/                            # Data Transfer Objects
│   │   ├── Validators/                      # FluentValidation
│   │   └── Mappers/                         # AutoMapper profiles
│   │
│   ├── EnglishTrainingCenter.Domain/        # Domain Layer
│   │   ├── Entities/                        # Domain Entities
│   │   │   ├── Student.cs
│   │   │   ├── Course.cs
│   │   │   ├── Class.cs
│   │   │   └── ...
│   │   ├── Interfaces/                      # Repository Interfaces
│   │   ├── ValueObjects/                    # Value Objects
│   │   └── Exceptions/                      # Domain Exceptions
│   │
│   ├── EnglishTrainingCenter.Infrastructure/ # Infrastructure Layer
│   │   ├── Data/                            # Database Context
│   │   │   ├── ETCContext.cs
│   │   │   └── Migrations/
│   │   ├── Repositories/                    # Repository Implementations
│   │   ├── Services/                        # External Services
│   │   └── Configuration/                   # EF Core Configurations
│   │
│   └── EnglishTrainingCenter.Common/        # Shared Utilities
│       ├── Constants/
│       ├── Extensions/
│       ├── Helpers/
│       └── Utilities/
│
├── tests/
│   ├── EnglishTrainingCenter.Tests.Unit/    # Unit Tests
│   ├── EnglishTrainingCenter.Tests.Integration/ # Integration Tests
│   └── EnglishTrainingCenter.Tests.API/     # API Tests
│
├── docs/
│   ├── architecture/
│   ├── database/
│   ├── api/
│   └── management/
│
├── .github/workflows/                       # GitHub Actions
│   ├── ci-build.yml
│   ├── tests.yml
│   └── deploy.yml
│
└── docker/
    ├── Dockerfile
    └── docker-compose.yml
```

## 4. Configuration

### 4.1 appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EnglishTrainingCenter;Trusted_Connection=true;"
  },
  "Jwt": {
    "Secret": "your-secret-key-here",
    "Issuer": "EnglishTrainingCenter",
    "Audience": "EnglishTrainingCenterUsers",
    "ExpiryMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### 4.2 Dependency Injection Setup
```csharp
// Program.cs
public void ConfigureServices(IServiceCollection services)
{
    // Database
    services.AddDbContext<ETCContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
    
    // Repositories
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    
    // Services
    services.AddScoped<IStudentService, StudentService>();
    services.AddScoped<ICourseService, CourseService>();
    services.AddScoped<IPaymentService, PaymentService>();
    // ... other services
    
    // Authentication
    var jwtSettings = Configuration.GetSection("Jwt");
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["Secret"]))
            };
        });
    
    // AutoMapper
    services.AddAutoMapper(typeof(Program));
    
    // Validation
    services.AddFluentValidation();
    
    // API
    services.AddControllers();
    services.AddSwaggerGen();
}
```

## 5. Deployment

### 5.1 Docker Deployment

**Dockerfile**:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY publish/ .
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "EnglishTrainingCenter.API.dll"]
```

**docker-compose.yml**:
```yaml
version: '3.8'

services:
  api:
    build:
      context: .
      dockerfile: docker/Dockerfile
    ports:
      - "5000:80"
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=EnglishTrainingCenter;User Id=sa;Password=YourPassword123!
    depends_on:
      - sqlserver

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword123!
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql

volumes:
  sqlserver_data:
```

### 5.2 Azure App Service Deployment

```bash
# Build application
dotnet publish -c Release -o ./publish

# Create Azure App Service
az appservice plan create \
  --name english-training-center-plan \
  --resource-group your-rg \
  --sku B1

# Deploy
az webapp up \
  --name english-training-center-api \
  --resource-group your-rg \
  --plan english-training-center-plan
```

## 6. CI/CD Pipeline

### 6.1 GitHub Actions Workflow (.github/workflows/ci-build.yml)

```yaml
name: CI Build

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  build:
    runs-on: ubuntu-latest

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
    
    - name: Run unit tests
      run: dotnet test --configuration Release --no-build
    
    - name: SonarCloud Scan
      uses: SonarSource/sonarcloud-github-action@master
      env:
        GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
        SONAR_TOKEN: ${{ secrets.SONAR_TOKEN }}
    
    - name: Publish
      run: dotnet publish -c Release -o ./publish
    
    - name: Build Docker image
      run: docker build -t english-training-center:latest .
    
    - name: Push to Docker Hub
      run: docker push english-training-center:latest
```

## 7. Monitoring & Logging

### 7.1 Application Insights Setup

```csharp
services.AddApplicationInsightsTelemetry();

// In appsettings.json
"ApplicationInsights": {
    "InstrumentationKey": "your-key-here"
}
```

### 7.2 Serilog Configuration

```csharp
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.ApplicationInsights(new TelemetryClient(), TelemetryConverter.Traces)
    .CreateLogger();
```

## 8. Security Checklist

- ✅ HTTPS enabled on all endpoints
- ✅ JWT token validation implemented
- ✅ SQL injection prevention (parameterized queries)
- ✅ CORS policy configured
- ✅ Rate limiting implemented
- ✅ Input validation & sanitization
- ✅ OWASP Top 10 compliance
- ✅ Secrets management (Azure Key Vault)
- ✅ Encrypted connections to database
- ✅ Regular security audits

## 9. Performance Optimization

- Connection pooling (SQL Server)
- Async/await throughout
- Database indexing strategy
- Redis caching for frequently accessed data
- API response compression
- Load testing & stress testing
- Query optimization
- Lazy loading vs Eager loading considerations

---

**Version**: 1.0  
**Last Updated**: January 28, 2026
