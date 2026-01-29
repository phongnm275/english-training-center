# Phase 6 - .NET Setup & Dependencies Guide

## 📦 NuGet Packages to Add

### Install Redis (Distributed Caching)
```bash
cd src/EnglishTrainingCenter.Infrastructure

# Core Redis package
dotnet add package StackExchange.Redis -v 2.7.10

# Extension for easier integration
dotnet add package StackExchange.Redis.Extensions.Core -v 10.1.0
```

### Install Hangfire (Background Job Processing)
```bash
cd src/EnglishTrainingCenter.Infrastructure

# Core Hangfire
dotnet add package Hangfire.Core -v 1.7.34
dotnet add package Hangfire.SqlServer -v 1.7.34
dotnet add package Hangfire.AspNetCore -v 1.7.34
```

### Install ML.NET (Machine Learning)
```bash
cd src/EnglishTrainingCenter.Application

# Core ML.NET
dotnet add package Microsoft.ML -v 3.1.1
dotnet add package Microsoft.ML.TimeSeries -v 3.1.1

# For data processing
dotnet add package Microsoft.ML.Data -v 3.1.1
```

### Install Additional Dependencies
```bash
cd src/EnglishTrainingCenter.Application

# For workflow cron expressions
dotnet add package CronExpressionDescriptor -v 3.4.0

# For better JSON handling
dotnet add package Newtonsoft.Json -v 13.0.3

# For data validation in bulk operations
dotnet add package CsvHelper -v 30.0.1
```

### Install Testing Dependencies (for Phase 6 unit tests)
```bash
cd tests/EnglishTrainingCenter.Tests.Unit

# For mocking Redis and Hangfire
dotnet add package Hangfire.Testing -v 1.7.34
dotnet add package StackExchange.Redis.Tests -v 2.7.10
```

---

## 🔧 appsettings.json Updates

Add these sections to `src/EnglishTrainingCenter.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=EnglishTrainingCenter;Trusted_Connection=true;"
  },
  
  "Redis": {
    "ConnectionString": "localhost:6379",
    "DefaultExpirationMinutes": 60,
    "SlidingExpirationMinutes": 30,
    "AbsoluteExpirationMinutes": 1440
  },
  
  "Hangfire": {
    "ConnectionString": "Server=YOUR_SERVER;Database=EnglishTrainingCenter_Hangfire;Trusted_Connection=true;",
    "WorkerCount": 4,
    "Dashboard": {
      "Enabled": true,
      "Port": 5002,
      "Path": "/hangfire"
    }
  },
  
  "MachineLearning": {
    "ModelsPath": "Models/",
    "StudentSuccessModelPath": "Models/student-success-model.zip",
    "PerformanceThreshold": 0.75,
    "RetrainIntervalDays": 7
  },
  
  "Audit": {
    "Enabled": true,
    "RetentionDays": 365,
    "LogSensitiveData": false,
    "ExcludeProperties": ["Password", "Token", "Secret"]
  },
  
  "Compliance": {
    "GDPREnabled": true,
    "DataExportFormat": "json",
    "DeletionGracePeriodDays": 30
  },
  
  "BulkOperations": {
    "MaxBatchSize": 1000,
    "ChunkSize": 100,
    "TimeoutMinutes": 30,
    "AllowedFileExtensions": [".csv", ".xlsx"]
  }
}
```

---

## 🔧 appsettings.Development.json Updates

Add these sections to `src/EnglishTrainingCenter.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(local);Database=EnglishTrainingCenter_Dev;Trusted_Connection=true;",
    "Hangfire": "Server=(local);Database=EnglishTrainingCenter_Hangfire_Dev;Trusted_Connection=true;"
  },
  
  "Redis": {
    "ConnectionString": "localhost:6379",
    "DefaultExpirationMinutes": 10,
    "SlidingExpirationMinutes": 5,
    "AbsoluteExpirationMinutes": 60
  },
  
  "Hangfire": {
    "ConnectionString": "Server=(local);Database=EnglishTrainingCenter_Hangfire_Dev;Trusted_Connection=true;",
    "WorkerCount": 2,
    "Dashboard": {
      "Enabled": true,
      "Port": 5002,
      "Path": "/hangfire"
    }
  },
  
  "MachineLearning": {
    "ModelsPath": "Models/",
    "StudentSuccessModelPath": "Models/student-success-model.zip",
    "PerformanceThreshold": 0.7,
    "RetrainIntervalDays": 1
  },
  
  "Audit": {
    "Enabled": true,
    "RetentionDays": 30,
    "LogSensitiveData": true,
    "ExcludeProperties": ["Password", "Token"]
  },
  
  "BulkOperations": {
    "MaxBatchSize": 500,
    "ChunkSize": 50,
    "TimeoutMinutes": 60,
    "AllowedFileExtensions": [".csv", ".xlsx"]
  },
  
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Hangfire": "Debug",
      "Redis": "Information"
    }
  }
}
```

---

## 🗄️ Database Setup for Phase 6

### Step 1: Create Hangfire Database
```sql
-- Run in SQL Server Management Studio
CREATE DATABASE [EnglishTrainingCenter_Hangfire];
GO

-- Hangfire will create its own schema on first run
-- But you can manually run:
-- See: https://github.com/HangfireIO/Hangfire/blob/master/src/Hangfire.SqlServer/Install.sql
```

### Step 2: Create Phase 6 Tables

Run this SQL script:

```sql
-- 1. BULK OPERATIONS
CREATE TABLE [dbo].[BulkOperations] (
    [BulkOperationId] INT PRIMARY KEY IDENTITY(1,1),
    [OperationType] NVARCHAR(50) NOT NULL, -- 'Import', 'Export', 'Validation'
    [Status] NVARCHAR(20) NOT NULL, -- 'Pending', 'Processing', 'Completed', 'Failed'
    [TotalRecords] INT NOT NULL DEFAULT 0,
    [ProcessedRecords] INT NOT NULL DEFAULT 0,
    [FailedRecords] INT NOT NULL DEFAULT 0,
    [FilePath] NVARCHAR(500),
    [ErrorLog] NVARCHAR(MAX),
    [CreatedBy] INT,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME2,
    [IsDeleted] BIT NOT NULL DEFAULT 0
);

CREATE TABLE [dbo].[BulkOperationItems] (
    [BulkOperationItemId] INT PRIMARY KEY IDENTITY(1,1),
    [BulkOperationId] INT NOT NULL,
    [ItemData] NVARCHAR(MAX) NOT NULL, -- JSON data
    [Status] NVARCHAR(20) NOT NULL, -- 'Pending', 'Success', 'Failed'
    [ErrorMessage] NVARCHAR(MAX),
    [ProcessedDate] DATETIME2,
    FOREIGN KEY ([BulkOperationId]) REFERENCES [dbo].[BulkOperations]([BulkOperationId])
);

-- 2. STUDENT ANALYTICS
CREATE TABLE [dbo].[StudentAttendance] (
    [StudentAttendanceId] INT PRIMARY KEY IDENTITY(1,1),
    [StudentId] INT NOT NULL,
    [ClassId] INT NOT NULL,
    [AttendanceDate] DATE NOT NULL,
    [Status] NVARCHAR(20) NOT NULL, -- 'Present', 'Absent', 'Late', 'Excused'
    [Notes] NVARCHAR(500),
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME2,
    FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students]([StudentId]),
    FOREIGN KEY ([ClassId]) REFERENCES [dbo].[Classes]([ClassId])
);

CREATE TABLE [dbo].[PerformancePredictions] (
    [PerformancePredictionId] INT PRIMARY KEY IDENTITY(1,1),
    [StudentId] INT NOT NULL,
    [CourseId] INT NOT NULL,
    [PredictedScore] DECIMAL(5,2),
    [ConfidenceLevel] DECIMAL(5,2), -- 0-1
    [RiskLevel] NVARCHAR(20), -- 'Low', 'Medium', 'High'
    [Reason] NVARCHAR(MAX),
    [PredictionDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME2,
    FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students]([StudentId]),
    FOREIGN KEY ([CourseId]) REFERENCES [dbo].[Courses]([CourseId])
);

CREATE TABLE [dbo].[LearningPaths] (
    [LearningPathId] INT PRIMARY KEY IDENTITY(1,1),
    [StudentId] INT NOT NULL,
    [PathName] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(500),
    [Courses] NVARCHAR(MAX), -- JSON array
    [Status] NVARCHAR(20) NOT NULL, -- 'Active', 'Completed', 'Paused'
    [StartDate] DATETIME2,
    [EndDate] DATETIME2,
    [Progress] INT DEFAULT 0, -- 0-100
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME2,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students]([StudentId])
);

-- 3. PERFORMANCE & CACHING
CREATE TABLE [dbo].[CacheSettings] (
    [CacheSettingId] INT PRIMARY KEY IDENTITY(1,1),
    [Key] NVARCHAR(200) NOT NULL UNIQUE,
    [Value] NVARCHAR(MAX),
    [ExpirationMinutes] INT DEFAULT 60,
    [Enabled] BIT NOT NULL DEFAULT 1,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME2
);

CREATE TABLE [dbo].[QueryOptimizationLogs] (
    [QueryLogId] INT PRIMARY KEY IDENTITY(1,1),
    [QueryName] NVARCHAR(200) NOT NULL,
    [ExecutionTimeMs] INT,
    [RowsAffected] INT,
    [Status] NVARCHAR(20), -- 'Success', 'Slow', 'Failed'
    [ExecutedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- 4. AUDIT & COMPLIANCE
CREATE TABLE [dbo].[AuditLogs] (
    [AuditLogId] BIGINT PRIMARY KEY IDENTITY(1,1),
    [EntityName] NVARCHAR(100) NOT NULL,
    [EntityId] INT NOT NULL,
    [Action] NVARCHAR(50) NOT NULL, -- 'Create', 'Update', 'Delete'
    [OldValues] NVARCHAR(MAX), -- JSON
    [NewValues] NVARCHAR(MAX), -- JSON
    [UserId] INT NOT NULL,
    [UserName] NVARCHAR(100),
    [IPAddress] NVARCHAR(50),
    [Timestamp] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE [dbo].[ComplianceRecords] (
    [ComplianceRecordId] INT PRIMARY KEY IDENTITY(1,1),
    [RecordType] NVARCHAR(50) NOT NULL, -- 'GDPR', 'DataRetention', 'Policy'
    [RelatedEntityId] INT,
    [RelatedEntityType] NVARCHAR(100),
    [Action] NVARCHAR(100) NOT NULL, -- 'DataExported', 'DataDeleted', 'PolicyAccepted'
    [Details] NVARCHAR(MAX),
    [Status] NVARCHAR(20), -- 'Pending', 'Completed', 'Failed'
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CompletedDate] DATETIME2
);

CREATE TABLE [dbo].[DataRetentionPolicies] (
    [PolicyId] INT PRIMARY KEY IDENTITY(1,1),
    [EntityType] NVARCHAR(100) NOT NULL, -- 'AuditLog', 'StudentData', etc.
    [RetentionDays] INT NOT NULL,
    [AutoDeleteEnabled] BIT NOT NULL DEFAULT 1,
    [LastExecutedDate] DATETIME2,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME2
);

-- 5. WORKFLOWS
CREATE TABLE [dbo].[Workflows] (
    [WorkflowId] INT PRIMARY KEY IDENTITY(1,1),
    [WorkflowName] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(500),
    [TriggerType] NVARCHAR(50) NOT NULL, -- 'Manual', 'Scheduled', 'Event'
    [TriggerCondition] NVARCHAR(MAX), -- JSON
    [Status] NVARCHAR(20) NOT NULL, -- 'Active', 'Inactive', 'Draft'
    [Enabled] BIT NOT NULL DEFAULT 1,
    [CreatedBy] INT,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME2,
    [IsDeleted] BIT NOT NULL DEFAULT 0
);

CREATE TABLE [dbo].[WorkflowTasks] (
    [WorkflowTaskId] INT PRIMARY KEY IDENTITY(1,1),
    [WorkflowId] INT NOT NULL,
    [TaskOrder] INT NOT NULL,
    [TaskType] NVARCHAR(50) NOT NULL, -- 'SendEmail', 'UpdateStatus', 'CreateRecord'
    [TaskConfiguration] NVARCHAR(MAX) NOT NULL, -- JSON
    [Status] NVARCHAR(20) NOT NULL, -- 'Active', 'Disabled'
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME2,
    FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[Workflows]([WorkflowId])
);

CREATE TABLE [dbo].[WorkflowExecutions] (
    [ExecutionId] BIGINT PRIMARY KEY IDENTITY(1,1),
    [WorkflowId] INT NOT NULL,
    [TriggerData] NVARCHAR(MAX), -- JSON
    [Status] NVARCHAR(20) NOT NULL, -- 'Running', 'Completed', 'Failed'
    [ErrorMessage] NVARCHAR(MAX),
    [StartTime] DATETIME2 NOT NULL,
    [EndTime] DATETIME2,
    [ExecutedBy] INT,
    FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[Workflows]([WorkflowId])
);

CREATE TABLE [dbo].[AutomationRules] (
    [RuleId] INT PRIMARY KEY IDENTITY(1,1),
    [RuleName] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(500),
    [Condition] NVARCHAR(MAX) NOT NULL, -- JSON
    [Action] NVARCHAR(MAX) NOT NULL, -- JSON
    [Priority] INT DEFAULT 0,
    [Enabled] BIT NOT NULL DEFAULT 1,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME2,
    [IsDeleted] BIT NOT NULL DEFAULT 0
);

-- Create Indexes for Performance
CREATE INDEX [IX_BulkOperations_Status] ON [dbo].[BulkOperations]([Status]);
CREATE INDEX [IX_StudentAttendance_StudentId] ON [dbo].[StudentAttendance]([StudentId]);
CREATE INDEX [IX_PerformancePredictions_StudentId] ON [dbo].[PerformancePredictions]([StudentId]);
CREATE INDEX [IX_AuditLogs_EntityName_EntityId] ON [dbo].[AuditLogs]([EntityName], [EntityId]);
CREATE INDEX [IX_AuditLogs_Timestamp] ON [dbo].[AuditLogs]([Timestamp]);
CREATE INDEX [IX_Workflows_Status] ON [dbo].[Workflows]([Status]);
CREATE INDEX [IX_WorkflowExecutions_WorkflowId] ON [dbo].[WorkflowExecutions]([WorkflowId]);
```

---

## 🔌 Program.cs Updates

Add these service registrations to `src/EnglishTrainingCenter.API/Program.cs`:

```csharp
// Add Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "ETC_";
});

// Add Hangfire
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();

// Add ML.NET
builder.Services.AddSingleton<IMLModelService, MLModelService>();

// Register Phase 6 Services
builder.Services.AddScoped<IBulkOperationService, BulkOperationService>();
builder.Services.AddScoped<IPerformancePredictionService, PerformancePredictionService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<ICachingService, CachingService>();
builder.Services.AddScoped<IComplianceService, ComplianceService>();

// Add Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");
```

---

## 📁 Phase 6 Project Structure

Create these folders in the solution:

```
src/
├── EnglishTrainingCenter.API/
├── EnglishTrainingCenter.Application/
│   ├── Services/
│   │   ├── BulkOperations/
│   │   ├── Analytics/
│   │   ├── Performance/
│   │   ├── Audit/
│   │   └── Workflows/
│   ├── DTOs/
│   │   ├── BulkOperationDtos/
│   │   ├── AnalyticsDtos/
│   │   ├── WorkflowDtos/
│   │   └── ...
│   └── Models/
│       └── student-success-model.zip (ML Model)
│
├── EnglishTrainingCenter.Infrastructure/
│   ├── Data/
│   │   └── Migrations/
│   │       └── Phase6Migrations.sql
│   └── Services/
│       ├── CachingService.cs
│       ├── BackgroundJobService.cs
│       └── ...
│
└── EnglishTrainingCenter.Domain/
    └── Entities/
        ├── BulkOperation.cs
        ├── PerformancePrediction.cs
        ├── AuditLog.cs
        ├── Workflow.cs
        └── ...

Models/ (for ML.NET)
├── student-success-model.zip
└── README.md
```

---

## 🚀 Quick Setup Commands

```bash
# 1. Navigate to project
cd c:\BMAD\english-training-center

# 2. Add all NuGet packages (run from each project)
cd src/EnglishTrainingCenter.Infrastructure
dotnet add package StackExchange.Redis -v 2.7.10
dotnet add package Hangfire.Core -v 1.7.34
dotnet add package Hangfire.SqlServer -v 1.7.34
dotnet add package Hangfire.AspNetCore -v 1.7.34

cd ../EnglishTrainingCenter.Application
dotnet add package Microsoft.ML -v 3.1.1
dotnet add package Microsoft.ML.TimeSeries -v 3.1.1
dotnet add package CronExpressionDescriptor -v 3.4.0
dotnet add package CsvHelper -v 30.0.1

# 3. Restore all projects
cd c:\BMAD\english-training-center
dotnet restore

# 4. Build
dotnet build

# 5. Run API
cd src/EnglishTrainingCenter.API
dotnet run
```

---

## ✅ Verification Checklist

After setup, verify:

- [ ] All NuGet packages installed successfully
- [ ] Project builds without errors
- [ ] appsettings.json updated with Redis & Hangfire config
- [ ] SQL Server databases created (main + Hangfire)
- [ ] Phase 6 tables created in database
- [ ] Redis running (or accessible)
- [ ] `dotnet run` starts application
- [ ] Swagger UI accessible at https://localhost:5001/swagger
- [ ] Hangfire Dashboard accessible at https://localhost:5001/hangfire

---

## 🆘 Common Issues & Solutions

### Redis Connection Failed
```bash
# Make sure Redis is running
# Option 1: Install Redis locally (Windows)
# Download from: https://github.com/microsoftarchive/redis/releases

# Option 2: Use Docker
docker run -d -p 6379:6379 redis:latest

# Option 3: Use Azure Cache for Redis (production)
```

### Hangfire Dashboard Not Showing
```bash
# Verify connection string in appsettings.json
# Hangfire needs SqlServer to store jobs
# Check: https://localhost:5001/hangfire
```

### ML.NET Model Loading Failed
```bash
# Ensure model file exists at: Models/student-success-model.zip
# Or provide correct path in appsettings.json
```

### Database Permissions Error
```bash
# Make sure SQL Server allows your user to create databases
# Run: SSMS as Administrator
# Verify: Connection string uses correct authentication
```

---

## 📚 Next Steps After Setup

1. **Create Base Entity Models** - Phase 6 domain entities
2. **Create Service Interfaces** - IBulkOperationService, etc.
3. **Implement Services** - Business logic for Phase 6
4. **Create DTOs & Validators** - Data transfer objects
5. **Create API Controllers** - REST endpoints
6. **Unit Tests** - Test all services
7. **Integration Tests** - Test with Redis, Hangfire, ML.NET

---

**Version**: 1.0  
**Last Updated**: January 28, 2026  
**Related Documents**: 
- PHASE_6_ADVANCED_FEATURES_PLANNING.md
- PHASE_6_IMPLEMENTATION_ROADMAP.md
- PHASE_6_QUICK_START_GUIDE.md
