# 🧪 TEST IMPLEMENTATION SAMPLES

**English Training Center LMS**  
**Unit & Integration Test Examples**

---

## 📚 TABLE OF CONTENTS

1. [Unit Test Structure](#unit-test-structure)
2. [Service Layer Tests](#service-layer-tests)
3. [Validator Tests](#validator-tests)
4. [Repository Tests](#repository-tests)
5. [Controller/API Tests](#controllerapi-tests)
6. [Integration Tests](#integration-tests)
7. [Test Fixtures & Helpers](#test-fixtures--helpers)

---

## UNIT TEST STRUCTURE

### Test Project Setup

```xml
<!-- tests/EnglishTrainingCenter.Tests.Unit/EnglishTrainingCenter.Tests.Unit.csproj -->

<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <IsTestProject>true</IsTestProject>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <!-- Test Framework -->
    <PackageReference Include="xunit" Version="2.6.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.3" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.2" />
    
    <!-- Mocking & Assertions -->
    <PackageReference Include="Moq" Version="4.20.69" />
    <PackageReference Include="FluentAssertions" Version="6.12.0" />
    
    <!-- Database Testing -->
    <PackageReference Include="Respawn" Version="5.0.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.1" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\src\EnglishTrainingCenter.API\EnglishTrainingCenter.API.csproj" />
    <ProjectReference Include="..\..\src\EnglishTrainingCenter.Application\EnglishTrainingCenter.Application.csproj" />
    <ProjectReference Include="..\..\src\EnglishTrainingCenter.Domain\EnglishTrainingCenter.Domain.csproj" />
  </ItemGroup>

</Project>
```

### Basic Test Class Template

```csharp
using Xunit;
using FluentAssertions;
using Moq;

namespace EnglishTrainingCenter.Tests.Unit.Services
{
    public class StudentServiceTests
    {
        private readonly Mock<IStudentRepository> _mockRepository;
        private readonly StudentService _service;

        public StudentServiceTests()
        {
            // Arrange - Setup
            _mockRepository = new Mock<IStudentRepository>();
            _service = new StudentService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetStudentById_WithValidId_ReturnsStudent()
        {
            // Arrange
            var studentId = 1;
            var student = new Student { Id = studentId, FullName = "John Doe" };
            _mockRepository.Setup(r => r.GetByIdAsync(studentId))
                .ReturnsAsync(student);

            // Act
            var result = await _service.GetStudentByIdAsync(studentId);

            // Assert
            result.Should().NotBeNull();
            result.FullName.Should().Be("John Doe");
            _mockRepository.Verify(r => r.GetByIdAsync(studentId), Times.Once);
        }

        [Fact]
        public async Task GetStudentById_WithInvalidId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Student)null);

            // Act
            var result = await _service.GetStudentByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetStudentById_WithInvalidId_ThrowsException(int invalidId)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _service.GetStudentByIdAsync(invalidId));
        }
    }
}
```

---

## SERVICE LAYER TESTS

### Student Service Tests

```csharp
namespace EnglishTrainingCenter.Tests.Unit.Services
{
    public class StudentServiceTests
    {
        private readonly Mock<IRepository<Student>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly StudentService _service;

        public StudentServiceTests()
        {
            _mockRepository = new Mock<IRepository<Student>>();
            _mockMapper = new Mock<IMapper>();
            _service = new StudentService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        [Trait("Category", "Services")]
        public async Task CreateStudent_WithValidData_ReturnsCreatedStudent()
        {
            // Arrange
            var createStudentDto = new CreateStudentDto
            {
                FullName = "John Doe",
                Email = "john@example.com",
                Phone = "555-0001"
            };

            var student = new Student
            {
                Id = 1,
                FullName = createStudentDto.FullName,
                Email = createStudentDto.Email,
                Phone = createStudentDto.Phone
            };

            var studentDto = new StudentDto { Id = 1, FullName = "John Doe" };

            _mockMapper.Setup(m => m.Map<Student>(createStudentDto))
                .Returns(student);
            _mockMapper.Setup(m => m.Map<StudentDto>(student))
                .Returns(studentDto);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Student>()))
                .ReturnsAsync(student);

            // Act
            var result = await _service.CreateStudentAsync(createStudentDto);

            // Assert
            result.Should().NotBeNull();
            result.FullName.Should().Be("John Doe");
            result.Id.Should().Be(1);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Student>()), Times.Once);
        }

        [Fact]
        [Trait("Category", "Services")]
        public async Task GetAllStudents_ReturnsPagedList()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, FullName = "Student 1" },
                new Student { Id = 2, FullName = "Student 2" }
            };

            _mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(students);

            // Act
            var result = await _service.GetAllStudentsAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(s => s.FullName == "Student 1");
        }

        [Fact]
        [Trait("Category", "Services")]
        public async Task UpdateStudent_WithValidData_UpdatesSuccessfully()
        {
            // Arrange
            var studentId = 1;
            var updateDto = new UpdateStudentDto { FullName = "Updated Name" };
            var existingStudent = new Student { Id = studentId, FullName = "Old Name" };

            _mockRepository.Setup(r => r.GetByIdAsync(studentId))
                .ReturnsAsync(existingStudent);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Student>()))
                .ReturnsAsync(existingStudent);

            // Act
            var result = await _service.UpdateStudentAsync(studentId, updateDto);

            // Assert
            result.Should().NotBeNull();
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Student>()), Times.Once);
        }

        [Fact]
        [Trait("Category", "Services")]
        public async Task DeleteStudent_WithValidId_DeletesSuccessfully()
        {
            // Arrange
            var studentId = 1;
            _mockRepository.Setup(r => r.DeleteAsync(studentId))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteStudentAsync(studentId);

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(r => r.DeleteAsync(studentId), Times.Once);
        }
    }
}
```

---

## VALIDATOR TESTS

### FluentValidation Tests

```csharp
namespace EnglishTrainingCenter.Tests.Unit.Validators
{
    public class CreateStudentValidatorTests
    {
        private readonly CreateStudentValidator _validator;

        public CreateStudentValidatorTests()
        {
            _validator = new CreateStudentValidator();
        }

        [Fact]
        [Trait("Category", "Validators")]
        public void Validate_WithValidData_PassesValidation()
        {
            // Arrange
            var dto = new CreateStudentDto
            {
                FullName = "John Doe",
                Email = "john@example.com",
                Phone = "555-0001"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        [Trait("Category", "Validators")]
        public void Validate_WithEmptyName_FailsValidation()
        {
            // Arrange
            var dto = new CreateStudentDto
            {
                FullName = "",
                Email = "john@example.com",
                Phone = "555-0001"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "FullName");
        }

        [Theory]
        [InlineData("invalid-email")]
        [InlineData("test@")]
        [InlineData("@example.com")]
        [Trait("Category", "Validators")]
        public void Validate_WithInvalidEmail_FailsValidation(string invalidEmail)
        {
            // Arrange
            var dto = new CreateStudentDto
            {
                FullName = "John Doe",
                Email = invalidEmail,
                Phone = "555-0001"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Theory]
        [InlineData("123")]
        [InlineData("invalid-phone")]
        [InlineData("")]
        [Trait("Category", "Validators")]
        public void Validate_WithInvalidPhone_FailsValidation(string invalidPhone)
        {
            // Arrange
            var dto = new CreateStudentDto
            {
                FullName = "John Doe",
                Email = "john@example.com",
                Phone = invalidPhone
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
```

---

## REPOSITORY TESTS

### Repository Tests with In-Memory Database

```csharp
namespace EnglishTrainingCenter.Tests.Unit.Repositories
{
    public class StudentRepositoryTests : IDisposable
    {
        private readonly ETCContext _context;
        private readonly StudentRepository _repository;

        public StudentRepositoryTests()
        {
            // Arrange - Setup in-memory database
            var options = new DbContextOptionsBuilder<ETCContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ETCContext(options);
            _repository = new StudentRepository(_context);
        }

        [Fact]
        [Trait("Category", "Repository")]
        public async Task AddAsync_WithValidStudent_AddsToDatabase()
        {
            // Arrange
            var student = new Student
            {
                FullName = "John Doe",
                Email = "john@example.com",
                Phone = "555-0001"
            };

            // Act
            await _repository.AddAsync(student);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _context.Students.FirstOrDefaultAsync(s => s.Email == "john@example.com");
            result.Should().NotBeNull();
            result.FullName.Should().Be("John Doe");
        }

        [Fact]
        [Trait("Category", "Repository")]
        public async Task GetByIdAsync_WithExistingId_ReturnsStudent()
        {
            // Arrange
            var student = new Student { FullName = "John Doe", Email = "john@example.com" };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(student.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(student.Id);
        }

        [Fact]
        [Trait("Category", "Repository")]
        public async Task GetAllAsync_ReturnsAllStudents()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { FullName = "Student 1", Email = "student1@example.com" },
                new Student { FullName = "Student 2", Email = "student2@example.com" }
            };
            _context.Students.AddRange(students);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        [Trait("Category", "Repository")]
        public async Task UpdateAsync_WithValidStudent_UpdatesDatabase()
        {
            // Arrange
            var student = new Student { FullName = "Original Name", Email = "test@example.com" };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            student.FullName = "Updated Name";

            // Act
            await _repository.UpdateAsync(student);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _context.Students.FindAsync(student.Id);
            result.FullName.Should().Be("Updated Name");
        }

        [Fact]
        [Trait("Category", "Repository")]
        public async Task DeleteAsync_WithExistingId_RemovesFromDatabase()
        {
            // Arrange
            var student = new Student { FullName = "John Doe", Email = "test@example.com" };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(student.Id);

            // Assert
            var result = await _context.Students.FindAsync(student.Id);
            result.Should().BeNull();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
```

---

## CONTROLLER/API TESTS

### API Controller Tests

```csharp
namespace EnglishTrainingCenter.Tests.Unit.Controllers
{
    public class StudentControllerTests
    {
        private readonly Mock<IStudentService> _mockService;
        private readonly StudentController _controller;

        public StudentControllerTests()
        {
            _mockService = new Mock<IStudentService>();
            _controller = new StudentController(_mockService.Object);
        }

        [Fact]
        [Trait("Category", "Controller")]
        public async Task GetStudents_ReturnsOkWithData()
        {
            // Arrange
            var students = new List<StudentDto>
            {
                new StudentDto { Id = 1, FullName = "Student 1" },
                new StudentDto { Id = 2, FullName = "Student 2" }
            };

            _mockService.Setup(s => s.GetAllStudentsAsync())
                .ReturnsAsync(students);

            // Act
            var result = await _controller.GetStudents();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as ApiResponse<List<StudentDto>>;
            response.Data.Should().HaveCount(2);
        }

        [Fact]
        [Trait("Category", "Controller")]
        public async Task CreateStudent_WithValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var createDto = new CreateStudentDto
            {
                FullName = "New Student",
                Email = "new@example.com",
                Phone = "555-0001"
            };

            var createdDto = new StudentDto { Id = 1, FullName = "New Student" };

            _mockService.Setup(s => s.CreateStudentAsync(createDto))
                .ReturnsAsync(createdDto);

            // Act
            var result = await _controller.CreateStudent(createDto);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult.StatusCode.Should().Be(201);
        }

        [Fact]
        [Trait("Category", "Controller")]
        public async Task GetStudentById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetStudentByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((StudentDto)null);

            // Act
            var result = await _controller.GetStudentById(999);

            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }
    }
}
```

---

## INTEGRATION TESTS

### Full Integration Test Example

```csharp
namespace EnglishTrainingCenter.Tests.Integration
{
    public class StudentIntegrationTests : IAsyncLifetime
    {
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private ETCContext _context;

        public StudentIntegrationTests()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Replace real DB with in-memory
                        var descriptor = services.SingleOrDefault(d => 
                            d.ServiceType == typeof(DbContextOptions<ETCContext>));
                        
                        if (descriptor != null)
                            services.Remove(descriptor);

                        services.AddDbContext<ETCContext>(options =>
                        {
                            options.UseInMemoryDatabase("IntegrationTestDb");
                        });
                    });
                });

            _client = _factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            // Setup test data
            var scope = _factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<ETCContext>();
            
            await _context.Database.EnsureCreatedAsync();
            
            // Add seed data
            _context.Students.Add(new Student
            {
                FullName = "Test Student",
                Email = "test@example.com",
                Phone = "555-0001"
            });
            await _context.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            _context.Dispose();
            _factory.Dispose();
            _client.Dispose();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task CreateStudent_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var createRequest = new CreateStudentDto
            {
                FullName = "New Student",
                Email = "new@example.com",
                Phone = "555-0002"
            };

            var json = JsonSerializer.Serialize(createRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/students", content);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetAllStudents_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/students");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var json = await response.Content.ReadAsStringAsync();
            json.Should().Contain("Test Student");
        }
    }
}
```

---

## TEST FIXTURES & HELPERS

### Base Test Class

```csharp
namespace EnglishTrainingCenter.Tests.Unit.Fixtures
{
    public abstract class BaseTestFixture
    {
        protected readonly Mock<ILogger<object>> MockLogger;

        protected BaseTestFixture()
        {
            MockLogger = new Mock<ILogger<object>>();
        }

        protected static T CreateInstance<T>() where T : class, new()
        {
            return (T)Activator.CreateInstance(typeof(T))!;
        }

        protected static List<T> CreateRandomList<T>(int count) where T : class, new()
        {
            return Enumerable.Range(0, count)
                .Select(_ => CreateInstance<T>())
                .ToList();
        }
    }
}
```

### Test Data Builders

```csharp
namespace EnglishTrainingCenter.Tests.Unit.Fixtures
{
    public class StudentBuilder
    {
        private int _id = 1;
        private string _fullName = "Test Student";
        private string _email = "test@example.com";
        private string _phone = "555-0001";

        public StudentBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public StudentBuilder WithFullName(string fullName)
        {
            _fullName = fullName;
            return this;
        }

        public StudentBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public Student Build()
        {
            return new Student
            {
                Id = _id,
                FullName = _fullName,
                Email = _email,
                Phone = _phone
            };
        }

        public List<Student> BuildList(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => WithId(i).Build())
                .ToList();
        }
    }

    // Usage in tests:
    // var student = new StudentBuilder()
    //     .WithFullName("John Doe")
    //     .WithEmail("john@example.com")
    //     .Build();
}
```

---

## RUNNING TEST SAMPLES

```powershell
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "ClassName=StudentServiceTests"

# Run specific test method
dotnet test --filter "FullyQualifiedName~StudentServiceTests.CreateStudent_WithValidData_ReturnsCreatedStudent"

# Run tests by category
dotnet test --filter "Category=Services"
dotnet test --filter "Category=Validators"
dotnet test --filter "Category=Repository"
dotnet test --filter "Category=Controller"
dotnet test --filter "Category=Integration"
```

---

**Status**: ✅ Ready to implement  
**Framework**: xUnit + Moq + FluentAssertions  
**Coverage Target**: > 80%

