# System Architecture Design - Quản lý Trung tâm Đào tạo Tiếng Anh

## 1. Tổng Quan Kiến Trúc

### 1.1 Kiến Trúc Lớp (Layered Architecture)

```
┌─────────────────────────────────────────────┐
│         Presentation Layer                  │
│   (ASP.NET Core Web API / MVC)             │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│         Application Layer                   │
│   (Business Logic, DTOs, Validators)       │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│         Domain Layer                        │
│   (Domain Models, Entities, Interfaces)    │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│         Data Access Layer                   │
│   (Repository, Entity Framework Core)      │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│         SQL Server Database                 │
└─────────────────────────────────────────────┘
```

## 2. Chi Tiết Các Lớp

### 2.1 Presentation Layer (API Layer)
**Công Nghệ**: ASP.NET Core Web API  
**Trách Nhiệm**:
- HTTP endpoints / routes
- Authentication & Authorization
- Input validation
- Response formatting

**Thành Phần**:
- Controllers (StudentController, CourseController, etc.)
- Middleware (Authentication, Error Handling, Logging)
- Startup configuration

### 2.2 Application Layer
**Công Nghệ**: .NET Core Libraries  
**Trách Nhiệm**:
- Business logic orchestration
- DTO (Data Transfer Object) mapping
- Validation rules
- Cross-cutting concerns (logging, caching)

**Thành Phần**:
- Services (StudentService, CourseService, etc.)
- DTOs & ViewModels
- Validators
- AutoMapper configurations

### 2.3 Domain Layer
**Công Nghệ**: .NET Core Classes  
**Trách Nhiệm**:
- Core business entities
- Domain rules
- Domain interfaces

**Thành Phần**:
- Entities (Student, Course, Class, Payment, etc.)
- Domain Value Objects
- Repository interfaces
- Domain exceptions

### 2.4 Data Access Layer (Persistence Layer)
**Công Nghệ**: Entity Framework Core  
**Trách Nhiệm**:
- Database operations
- ORM mapping
- Query construction
- Transaction management

**Thành Phần**:
- DbContext (ETCContext)
- Repository implementations
- Database migrations
- Connection management

### 2.5 Infrastructure Layer
**Công Nghệ**: Various libraries  
**Trách Nhiệm**:
- External services (Email, SMS, Payment Gateway)
- File storage
- Caching
- Background jobs

## 3. Core Components

### 3.1 Database Context
```csharp
public class ETCContext : DbContext
{
    // Students
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentCourse> StudentCourses { get; set; }
    
    // Courses & Classes
    public DbSet<Course> Courses { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Room> Rooms { get; set; }
    
    // Instructors
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<InstructorQualification> InstructorQualifications { get; set; }
    
    // Payments
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    
    // Evaluation
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Grade> Grades { get; set; }
    
    // Admin
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
}
```

### 3.2 Generic Repository Pattern
```csharp
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task SaveChangesAsync();
}

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ETCContext _context;
    private readonly DbSet<T> _dbSet;
    
    // Implementation
}
```

### 3.3 Dependency Injection
```csharp
// Startup configuration
services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
services.AddScoped<IStudentService, StudentService>();
services.AddScoped<ICourseService, CourseService>();
services.AddScoped<IPaymentService, PaymentService>();
services.AddScoped<IEvaluationService, EvaluationService>();
```

## 4. Data Flow

### 4.1 Request Flow
```
Client Request
    ↓
API Gateway / Middleware (Auth, Logging)
    ↓
Controller (Receives & Validates)
    ↓
Service (Business Logic)
    ↓
Repository (Data Access)
    ↓
Database (SQL Server)
    ↓
Response ← Response Mapping ← Entity
```

### 4.2 Example: Get Student by ID
```
GET /api/students/1
    ↓
StudentController.GetById(1)
    ↓
StudentService.GetStudentByIdAsync(1)
    ↓
StudentRepository.GetByIdAsync(1)
    ↓
SELECT * FROM Students WHERE Id = 1
    ↓
Entity → DTO → JSON Response
```

## 5. Security Architecture

### 5.1 Authentication
- **JWT (JSON Web Tokens)**
- Token issued on login
- Stored in secure storage (HttpOnly Cookie or LocalStorage)

### 5.2 Authorization
- **Role-Based Access Control (RBAC)**
- Roles: Admin, Manager, Instructor, Student, Finance
- Permission checks in middleware & service layer

### 5.3 Data Protection
- HTTPS for all communications
- Password hashing (bcrypt / Argon2)
- SQL Injection prevention (EF Core parameterized queries)
- CORS policy configuration

### 5.4 Sensitive Data
- Encrypt PII (Personal Identifiable Information)
- Audit logging for sensitive operations
- Rate limiting on endpoints

## 6. Scalability & Performance

### 6.1 Caching Strategy
- In-memory cache (IMemoryCache)
- Distributed cache (Redis) for multi-server setup
- Cache invalidation patterns

### 6.2 Database Optimization
- Indexing on frequently queried columns
- Connection pooling
- Query optimization
- Async operations

### 6.3 Background Jobs
- Scheduled tasks (Hangfire / Quartz)
  - Generate monthly reports
  - Send reminder emails
  - Process batch payments

## 7. Integration Points

### 7.1 External Services
- **Email Service**: SendGrid / SMTP
- **Payment Gateway**: VNPay / Stripe
- **SMS Service**: Twilio / Local provider
- **Cloud Storage**: Azure Blob / AWS S3

### 7.2 API Integrations
- Payment verification
- Email notifications
- SMS alerts

## 8. Error Handling & Logging

### 8.1 Centralized Error Handling
- Global exception handler middleware
- Custom exceptions for business logic
- Appropriate HTTP status codes

### 8.2 Logging
- Structured logging (Serilog)
- Log levels: Debug, Info, Warning, Error, Fatal
- Centralized log storage

## 9. Testing Strategy

### 9.1 Unit Tests
- Service layer tests
- Repository pattern for isolation
- Mock external dependencies

### 9.2 Integration Tests
- Database integration tests
- API endpoint tests
- Transaction rollback

### 9.3 Performance Tests
- Load testing
- Database query performance
- API response time

---

**Version**: 1.0  
**Last Updated**: January 28, 2026
