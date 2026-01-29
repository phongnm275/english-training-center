# Module Architecture - English Training Center Management System

## 1. Module Overview

Hệ thống được chia thành các module chức năng độc lập, mỗi module có một tầng Application Service riêng.

```
┌────────────────────────────────────────────────────────────┐
│                   API Controllers Layer                     │
├────────────────────────────────────────────────────────────┤
│  StudentCtrl │ CourseCtrl │ ClassCtrl │ PaymentCtrl │ EvalCtrl│
└────────────────────────────────────────────────────────────┘
                            ↓
┌────────────────────────────────────────────────────────────┐
│                  Application Services Layer                 │
├─────────────┬──────────────┬──────────────┬─────────────────┤
│  Student    │ Course/Class │ Instructor   │  Payment Module │
│  Module     │  Module      │  Module      │                 │
└─────────────┴──────────────┴──────────────┴─────────────────┘
        ↓              ↓               ↓              ↓
┌────────────────────────────────────────────────────────────┐
│                    Domain Layer                             │
│   (Entities, Domain Services, Repositories)                 │
└────────────────────────────────────────────────────────────┘
                            ↓
┌────────────────────────────────────────────────────────────┐
│                  Infrastructure Layer                       │
│   (EF Core, Repositories, External Services)               │
└────────────────────────────────────────────────────────────┘
```

## 2. Student Module

### 2.1 Chức Năng
- Quản lý hồ sơ học viên
- Đăng ký / Hủy đăng ký khóa học
- Lịch sử khoá học
- Điểm số & GPA

### 2.2 Entities
```csharp
public class Student
{
    public int StudentId { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Address { get; set; }
    public string Avatar { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; }
    public virtual ICollection<StudentCourse> StudentCourses { get; set; }
    public virtual ICollection<Payment> Payments { get; set; }
    public virtual ICollection<Grade> Grades { get; set; }
}

public class StudentCourse
{
    public int StudentCourseId { get; set; }
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public DateTime EnrollDate { get; set; }
    public string Status { get; set; } // Enrolled, Completed, Dropped
    public decimal? GPA { get; set; }
    
    // Navigation properties
    public virtual Student Student { get; set; }
    public virtual Class Class { get; set; }
}
```

### 2.3 Services Interface
```csharp
public interface IStudentService
{
    Task<StudentDto> GetStudentByIdAsync(int studentId);
    Task<PagedResult<StudentDto>> GetStudentsAsync(int page, int pageSize, string search);
    Task<StudentDto> CreateStudentAsync(CreateStudentRequest request);
    Task<StudentDto> UpdateStudentAsync(int studentId, UpdateStudentRequest request);
    Task DeleteStudentAsync(int studentId);
    Task<IEnumerable<StudentCourseDto>> GetStudentCoursesAsync(int studentId);
    Task<decimal> CalculateStudentGPAAsync(int studentId);
}
```

### 2.4 DTOs
```csharp
public class StudentDto
{
    public int StudentId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Address { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public bool IsActive { get; set; }
    public ICollection<StudentCourseDto> EnrolledCourses { get; set; }
}

public class CreateStudentRequest
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Phone]
    public string Phone { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    public string Address { get; set; }
}
```

## 3. Course & Class Module

### 3.1 Chức Năng
- Quản lý khóa học (tạo, cập nhật, xóa)
- Quản lý lớp học
- Lịch học
- Gán giáo viên

### 3.2 Entities
```csharp
public class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public string Description { get; set; }
    public string Level { get; set; } // Beginner, Intermediate, Advanced
    public int Duration { get; set; } // in hours
    public decimal Fee { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation properties
    public virtual ICollection<Class> Classes { get; set; }
}

public class Class
{
    public int ClassId { get; set; }
    public int CourseId { get; set; }
    public int InstructorId { get; set; }
    public string ClassName { get; set; }
    public int MaxStudents { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } // Upcoming, Ongoing, Completed, Cancelled
    public bool IsActive { get; set; }
    
    // Navigation properties
    public virtual Course Course { get; set; }
    public virtual Instructor Instructor { get; set; }
    public virtual ICollection<StudentCourse> StudentCourses { get; set; }
    public virtual ICollection<Schedule> Schedules { get; set; }
}

public class Schedule
{
    public int ScheduleId { get; set; }
    public int ClassId { get; set; }
    public int RoomId { get; set; }
    public int DayOfWeek { get; set; } // 0-6
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    
    // Navigation properties
    public virtual Class Class { get; set; }
    public virtual Room Room { get; set; }
}
```

### 3.3 Services Interface
```csharp
public interface ICourseService
{
    Task<CourseDto> GetCourseByIdAsync(int courseId);
    Task<PagedResult<CourseDto>> GetCoursesAsync(string level, int page, int pageSize);
    Task<CourseDto> CreateCourseAsync(CreateCourseRequest request);
    Task<CourseDto> UpdateCourseAsync(int courseId, UpdateCourseRequest request);
    Task DeleteCourseAsync(int courseId);
}

public interface IClassService
{
    Task<ClassDto> GetClassByIdAsync(int classId);
    Task<PagedResult<ClassDto>> GetClassesAsync(int courseId, string status, int page, int pageSize);
    Task<ClassDto> CreateClassAsync(CreateClassRequest request);
    Task<StudentCourseDto> EnrollStudentAsync(int classId, int studentId);
    Task<decimal> GetAverageClassGPAAsync(int classId);
}

public interface IScheduleService
{
    Task<IEnumerable<ScheduleDto>> GetSchedulesByClassAsync(int classId);
    Task<ScheduleDto> CreateScheduleAsync(CreateScheduleRequest request);
    Task<bool> CheckRoomConflictAsync(int roomId, DateTime startTime, DateTime endTime);
}
```

## 4. Instructor Module

### 4.1 Chức Năng
- Quản lý hồ sơ giáo viên
- Chứng chỉ / Bằng cấp
- Lịch dạy
- Đánh giá hiệu suất

### 4.2 Entities
```csharp
public class Instructor
{
    public int InstructorId { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Specialization { get; set; }
    public int YearsExperience { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; }
    public virtual ICollection<Class> Classes { get; set; }
    public virtual ICollection<InstructorQualification> Qualifications { get; set; }
}

public class InstructorQualification
{
    public int QualificationId { get; set; }
    public int InstructorId { get; set; }
    public string Certification { get; set; }
    public DateTime CertificationDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Issuer { get; set; }
    
    // Navigation properties
    public virtual Instructor Instructor { get; set; }
}
```

### 4.3 Services Interface
```csharp
public interface IInstructorService
{
    Task<InstructorDto> GetInstructorByIdAsync(int instructorId);
    Task<PagedResult<InstructorDto>> GetInstructorsAsync(int page, int pageSize);
    Task<InstructorDto> CreateInstructorAsync(CreateInstructorRequest request);
    Task<InstructorDto> UpdateInstructorAsync(int instructorId, UpdateInstructorRequest request);
    Task<IEnumerable<InstructorScheduleDto>> GetInstructorScheduleAsync(int instructorId);
}
```

## 5. Payment Module

### 5.1 Chức Năng
- Tạo hóa đơn
- Theo dõi thanh toán
- Hoàn tiền / Điều chỉnh
- Báo cáo doanh thu

### 5.2 Entities
```csharp
public class Invoice
{
    public int InvoiceId { get; set; }
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public decimal Amount { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } // Issued, Paid, Overdue, Cancelled
    public string Notes { get; set; }
    
    // Navigation properties
    public virtual Student Student { get; set; }
    public virtual Class Class { get; set; }
    public virtual ICollection<Payment> Payments { get; set; }
}

public class Payment
{
    public int PaymentId { get; set; }
    public int StudentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } // Credit Card, Bank Transfer, Cash
    public string Status { get; set; } // Pending, Completed, Failed, Refunded
    public string TransactionId { get; set; }
    public string Description { get; set; }
    
    // Navigation properties
    public virtual Student Student { get; set; }
}
```

### 5.3 Services Interface
```csharp
public interface IPaymentService
{
    Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceRequest request);
    Task<PagedResult<InvoiceDto>> GetInvoicesAsync(int studentId, string status, int page, int pageSize);
    Task<PaymentDto> ProcessPaymentAsync(ProcessPaymentRequest request);
    Task<decimal> GetRevenueReportAsync(DateTime startDate, DateTime endDate);
    Task<bool> RefundPaymentAsync(int paymentId, string reason);
}
```

## 6. Evaluation & Grading Module

### 6.1 Chức Năng
- Tạo bài kiểm tra & bài tập
- Ghi nhận điểm số
- Báo cáo tiến độ
- Chứng chỉ hoàn thành

### 6.2 Entities
```csharp
public class Assignment
{
    public int AssignmentId { get; set; }
    public int ClassId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public decimal MaxScore { get; set; }
    
    // Navigation properties
    public virtual Class Class { get; set; }
    public virtual ICollection<AssignmentSubmission> Submissions { get; set; }
}

public class AssignmentSubmission
{
    public int SubmissionId { get; set; }
    public int AssignmentId { get; set; }
    public int StudentId { get; set; }
    public DateTime SubmissionDate { get; set; }
    public string Content { get; set; }
    public string FileUrl { get; set; }
    public decimal? Score { get; set; }
    public string Feedback { get; set; }
    public bool IsLate { get; set; }
    
    // Navigation properties
    public virtual Assignment Assignment { get; set; }
    public virtual Student Student { get; set; }
}

public class Exam
{
    public int ExamId { get; set; }
    public int ClassId { get; set; }
    public string ExamName { get; set; }
    public DateTime ExamDate { get; set; }
    public int Duration { get; set; } // in minutes
    public decimal MaxScore { get; set; }
    public decimal PassingScore { get; set; }
    
    // Navigation properties
    public virtual Class Class { get; set; }
    public virtual ICollection<Grade> Grades { get; set; }
}

public class Grade
{
    public int GradeId { get; set; }
    public int StudentId { get; set; }
    public int ExamId { get; set; }
    public decimal Score { get; set; }
    public string GradeValue { get; set; } // A, B, C, D, F
    public string Feedback { get; set; }
    public DateTime GradedDate { get; set; }
    
    // Navigation properties
    public virtual Student Student { get; set; }
    public virtual Exam Exam { get; set; }
}
```

### 6.3 Services Interface
```csharp
public interface IEvaluationService
{
    Task<AssignmentDto> CreateAssignmentAsync(CreateAssignmentRequest request);
    Task<AssignmentSubmissionDto> SubmitAssignmentAsync(SubmitAssignmentRequest request);
    Task<AssignmentSubmissionDto> GradeSubmissionAsync(int submissionId, GradeSubmissionRequest request);
    Task<IEnumerable<AssignmentDto>> GetAssignmentsByClassAsync(int classId);
}

public interface IGradeService
{
    Task<ExamDto> CreateExamAsync(CreateExamRequest request);
    Task<GradeDto> SubmitGradeAsync(SubmitGradeRequest request);
    Task<IEnumerable<GradeDto>> GetStudentGradesAsync(int studentId, int classId);
    Task<decimal> CalculateGPAAsync(int studentId, int classId);
    Task<string> CalculateLetterGradeAsync(decimal score);
}
```

## 7. Integration Between Modules

### 7.1 Dependencies
```
Student Module
    ↓
    ├→ Student + Payment (Invoices)
    ├→ Student + Course/Class (Enrollment)
    ├→ Student + Evaluation (Grades)
    
Course/Class Module
    ↓
    ├→ Class + Instructor
    ├→ Class + Schedule + Room
    ├→ Class + Student (Enrollment)
    
Evaluation Module
    ↓
    ├→ Assignment + Student
    ├→ Grade + Student + Exam
```

### 7.2 Cross-Module Services
```csharp
// Event Bus Pattern
public interface IEventBus
{
    Task PublishAsync<T>(T @event) where T : IEvent;
    Task SubscribeAsync<T>(IEventHandler<T> handler) where T : IEvent;
}

// Events
public class StudentEnrolledEvent : IEvent
{
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public DateTime EnrollDate { get; set; }
}

public class PaymentCompletedEvent : IEvent
{
    public int PaymentId { get; set; }
    public int StudentId { get; set; }
    public decimal Amount { get; set; }
}
```

## 8. Validation Rules

### 8.1 Student Module
- Email phải unique
- Tuổi tối thiểu: 15 (nếu áp dụng)
- Không được đăng ký khóa nếu đã đăng ký

### 8.2 Course/Class Module
- Tên khóa phải unique
- Fee > 0
- StartDate < EndDate
- MaxStudents >= 5

### 8.3 Payment Module
- Amount > 0
- DueDate phải >= IssueDate
- Không được refund nếu đã refund trước

### 8.4 Evaluation Module
- MaxScore > 0
- Score <= MaxScore
- DueDate phải trong khoảng ngày học

---

**Version**: 1.0  
**Last Updated**: January 28, 2026
