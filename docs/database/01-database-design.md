# Database Design - English Training Center Management System

## 1. Entity Relationship Diagram (ERD)

```
┌──────────────────┐         ┌──────────────────┐
│      Users       │         │      Roles       │
├──────────────────┤         ├──────────────────┤
│ PK: UserId       │────M:1──│ PK: RoleId       │
│ Username         │         │ RoleName         │
│ Email            │         │ Description      │
│ PasswordHash     │         └──────────────────┘
│ IsActive         │
│ CreatedDate      │
└──────────────────┘

┌──────────────────────┐      ┌──────────────────┐
│    Students          │      │   StudentCourse  │
├──────────────────────┤      ├──────────────────┤
│ PK: StudentId        │─M:M──│ PK: ScId         │
│ FullName             │      │ FK: StudentId    │
│ Email                │      │ FK: ClassId      │
│ Phone                │      │ EnrollDate       │
│ DateOfBirth          │      │ Status           │
│ Address              │      │ GPA              │
│ Avatar               │      └──────────────────┘
│ EnrollmentDate       │
│ IsActive             │
└──────────────────────┘

┌──────────────────────┐      ┌──────────────────┐      ┌──────────────────┐
│     Courses          │      │      Classes     │      │   Instructors    │
├──────────────────────┤      ├──────────────────┤      ├──────────────────┤
│ PK: CourseId         │─1:M──│ PK: ClassId      │      │ PK: InstructorId │
│ CourseName           │      │ FK: CourseId     │─M:1──│ FullName         │
│ Description          │      │ FK: InstructorId │      │ Email            │
│ Level                │      │ ClassName        │      │ Phone            │
│ Duration(hours)      │      │ MaxStudents      │      │ Qualifications   │
│ Fee                  │      │ StartDate        │      │ Specialization   │
│ CreatedDate          │      │ EndDate          │      │ YearsExperience  │
└──────────────────────┘      │ Status           │      │ IsActive         │
                               └──────────────────┘      └──────────────────┘
                                      ↓
                               ┌──────────────────┐
                               │    Schedules     │
                               ├──────────────────┤
                               │ PK: ScheduleId   │
                               │ FK: ClassId      │
                               │ FK: RoomId       │
                               │ DayOfWeek        │
                               │ StartTime        │
                               │ EndTime          │
                               │ IsActive         │
                               └──────────────────┘
                                      ↓
                               ┌──────────────────┐
                               │     Rooms        │
                               ├──────────────────┤
                               │ PK: RoomId       │
                               │ RoomNumber       │
                               │ Capacity         │
                               │ Equipment        │
                               │ IsActive         │
                               └──────────────────┘

┌──────────────────────┐      ┌──────────────────┐
│    Payments          │      │    Invoices      │
├──────────────────────┤      ├──────────────────┤
│ PK: PaymentId        │      │ PK: InvoiceId    │
│ FK: StudentId        │──1:M─│ FK: StudentId    │
│ Amount               │      │ FK: ClassId      │
│ PaymentDate          │      │ Amount           │
│ PaymentMethod        │      │ DueDate          │
│ Status               │      │ IssueDate        │
│ TransactionId        │      │ Status           │
│ Description          │      │ Notes            │
└──────────────────────┘      └──────────────────┘

┌──────────────────────────┐  ┌──────────────────┐
│    Assignments           │  │    Exams         │
├──────────────────────────┤  ├──────────────────┤
│ PK: AssignmentId         │  │ PK: ExamId       │
│ FK: ClassId              │  │ FK: ClassId      │
│ Title                    │  │ ExamName         │
│ Description              │  │ ExamDate         │
│ DueDate                  │  │ Duration(min)    │
│ MaxScore                 │  │ MaxScore         │
│ CreatedDate              │  │ PassingScore     │
└──────────────────────────┘  └──────────────────┘
         ↓                              ↓
┌──────────────────────────┐  ┌──────────────────┐
│  AssignmentSubmissions   │  │     Grades       │
├──────────────────────────┤  ├──────────────────┤
│ PK: SubmissionId         │  │ PK: GradeId      │
│ FK: AssignmentId         │  │ FK: StudentId    │
│ FK: StudentId            │  │ FK: ExamId       │
│ SubmissionDate           │  │ Score            │
│ Content                  │  │ GradeValue       │
│ Score                    │  │ Feedback         │
│ Feedback                 │  │ GradedDate       │
└──────────────────────────┘  └──────────────────┘
```

## 2. Chi Tiết Các Bảng (Tables)

### 2.1 Users & Roles
```sql
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(MAX)
);

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    RoleId INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    LastLoginDate DATETIME,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME,
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);

CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_RoleId ON Users(RoleId);
```

### 2.2 Students & Student Courses
```sql
CREATE TABLE Students (
    StudentId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    DateOfBirth DATE,
    Address NVARCHAR(MAX),
    Avatar NVARCHAR(MAX),
    EnrollmentDate DATETIME DEFAULT GETDATE(),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE StudentCourses (
    StudentCourseId INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    ClassId INT NOT NULL,
    EnrollDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20), -- 'Enrolled', 'Completed', 'Dropped'
    GPA DECIMAL(3,2),
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId),
    UNIQUE(StudentId, ClassId)
);

CREATE INDEX IX_StudentCourses_StudentId ON StudentCourses(StudentId);
CREATE INDEX IX_StudentCourses_ClassId ON StudentCourses(ClassId);
```

### 2.3 Courses & Classes
```sql
CREATE TABLE Courses (
    CourseId INT PRIMARY KEY IDENTITY(1,1),
    CourseName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    Level NVARCHAR(20), -- 'Beginner', 'Intermediate', 'Advanced'
    Duration INT, -- in hours
    Fee DECIMAL(12,2),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME
);

CREATE TABLE Classes (
    ClassId INT PRIMARY KEY IDENTITY(1,1),
    CourseId INT NOT NULL,
    InstructorId INT NOT NULL,
    ClassName NVARCHAR(100) NOT NULL,
    MaxStudents INT DEFAULT 30,
    StartDate DATETIME,
    EndDate DATETIME,
    Status NVARCHAR(20), -- 'Upcoming', 'Ongoing', 'Completed', 'Cancelled'
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME,
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId),
    FOREIGN KEY (InstructorId) REFERENCES Instructors(InstructorId)
);

CREATE INDEX IX_Classes_CourseId ON Classes(CourseId);
CREATE INDEX IX_Classes_InstructorId ON Classes(InstructorId);
```

### 2.4 Instructors & Qualifications
```sql
CREATE TABLE Instructors (
    InstructorId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    Specialization NVARCHAR(100),
    YearsExperience INT,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE InstructorQualifications (
    QualificationId INT PRIMARY KEY IDENTITY(1,1),
    InstructorId INT NOT NULL,
    Certification NVARCHAR(100),
    CertificationDate DATETIME,
    ExpiryDate DATETIME,
    Issuer NVARCHAR(100),
    FOREIGN KEY (InstructorId) REFERENCES Instructors(InstructorId)
);
```

### 2.5 Schedules & Rooms
```sql
CREATE TABLE Rooms (
    RoomId INT PRIMARY KEY IDENTITY(1,1),
    RoomNumber NVARCHAR(20) NOT NULL UNIQUE,
    Capacity INT NOT NULL,
    Equipment NVARCHAR(MAX), -- Projector, Whiteboard, etc.
    IsActive BIT NOT NULL DEFAULT 1
);

CREATE TABLE Schedules (
    ScheduleId INT PRIMARY KEY IDENTITY(1,1),
    ClassId INT NOT NULL,
    RoomId INT NOT NULL,
    DayOfWeek INT, -- 0=Sunday, 1=Monday, ..., 6=Saturday
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId),
    FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId)
);

CREATE INDEX IX_Schedules_ClassId ON Schedules(ClassId);
CREATE INDEX IX_Schedules_RoomId ON Schedules(RoomId);
```

### 2.6 Payments & Invoices
```sql
CREATE TABLE Payments (
    PaymentId INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    Amount DECIMAL(12,2) NOT NULL,
    PaymentDate DATETIME DEFAULT GETDATE(),
    PaymentMethod NVARCHAR(50), -- 'Credit Card', 'Bank Transfer', 'Cash'
    Status NVARCHAR(20), -- 'Pending', 'Completed', 'Failed', 'Refunded'
    TransactionId NVARCHAR(100),
    Description NVARCHAR(MAX),
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId)
);

CREATE TABLE Invoices (
    InvoiceId INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    ClassId INT NOT NULL,
    Amount DECIMAL(12,2) NOT NULL,
    DueDate DATETIME,
    IssueDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20), -- 'Issued', 'Paid', 'Overdue', 'Cancelled'
    Notes NVARCHAR(MAX),
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId)
);

CREATE INDEX IX_Invoices_StudentId ON Invoices(StudentId);
CREATE INDEX IX_Invoices_ClassId ON Invoices(ClassId);
```

### 2.7 Assignments & Submissions
```sql
CREATE TABLE Assignments (
    AssignmentId INT PRIMARY KEY IDENTITY(1,1),
    ClassId INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    DueDate DATETIME,
    MaxScore DECIMAL(5,2) DEFAULT 100,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME,
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId)
);

CREATE TABLE AssignmentSubmissions (
    SubmissionId INT PRIMARY KEY IDENTITY(1,1),
    AssignmentId INT NOT NULL,
    StudentId INT NOT NULL,
    SubmissionDate DATETIME DEFAULT GETDATE(),
    Content NVARCHAR(MAX),
    FileUrl NVARCHAR(MAX),
    Score DECIMAL(5,2),
    Feedback NVARCHAR(MAX),
    IsLate BIT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (AssignmentId) REFERENCES Assignments(AssignmentId),
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    UNIQUE(AssignmentId, StudentId)
);

CREATE INDEX IX_AssignmentSubmissions_AssignmentId ON AssignmentSubmissions(AssignmentId);
CREATE INDEX IX_AssignmentSubmissions_StudentId ON AssignmentSubmissions(StudentId);
```

### 2.8 Exams & Grades
```sql
CREATE TABLE Exams (
    ExamId INT PRIMARY KEY IDENTITY(1,1),
    ClassId INT NOT NULL,
    ExamName NVARCHAR(100) NOT NULL,
    ExamDate DATETIME,
    Duration INT, -- in minutes
    MaxScore DECIMAL(5,2) DEFAULT 100,
    PassingScore DECIMAL(5,2) DEFAULT 50,
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId)
);

CREATE TABLE Grades (
    GradeId INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    ExamId INT NOT NULL,
    Score DECIMAL(5,2),
    GradeValue NVARCHAR(5), -- 'A', 'B', 'C', 'D', 'F'
    Feedback NVARCHAR(MAX),
    GradedDate DATETIME,
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    FOREIGN KEY (ExamId) REFERENCES Exams(ExamId),
    UNIQUE(StudentId, ExamId)
);

CREATE INDEX IX_Grades_StudentId ON Grades(StudentId);
CREATE INDEX IX_Grades_ExamId ON Grades(ExamId);
```

## 3. Database Constraints & Indexes

### 3.1 Primary Keys
- Tất cả bảng có Primary Key (ID)
- Auto-increment cho INSERT tự động

### 3.2 Foreign Keys
- Enforce referential integrity
- ON DELETE CASCADE / NO ACTION tùy từng quan hệ

### 3.3 Unique Constraints
- Email, Username trong Users
- RoleName trong Roles
- StudentId + ClassId trong StudentCourses
- ExamId + StudentId trong Grades

### 3.4 Check Constraints
```sql
ALTER TABLE Classes ADD CONSTRAINT CK_StartEndDate
    CHECK (EndDate > StartDate);

ALTER TABLE Grades ADD CONSTRAINT CK_GradeScore
    CHECK (Score >= 0 AND Score <= 100);

ALTER TABLE Payments ADD CONSTRAINT CK_PaymentAmount
    CHECK (Amount > 0);
```

## 4. Stored Procedures (Optional)

### 4.1 Get Student Summary
```sql
CREATE PROCEDURE sp_GetStudentSummary
    @StudentId INT
AS
BEGIN
    SELECT 
        s.StudentId,
        s.FullName,
        COUNT(DISTINCT sc.ClassId) AS EnrolledClasses,
        AVG(g.Score) AS AverageScore,
        SUM(i.Amount) AS TotalFees
    FROM Students s
    LEFT JOIN StudentCourses sc ON s.StudentId = sc.StudentId
    LEFT JOIN Grades g ON s.StudentId = g.StudentId
    LEFT JOIN Invoices i ON s.StudentId = i.StudentId
    WHERE s.StudentId = @StudentId
    GROUP BY s.StudentId, s.FullName
END;
```

## 5. Backup & Maintenance

- **Backup Strategy**: Daily full backup, hourly incremental
- **Maintenance**: Index rebuild, stats update (weekly)
- **Retention**: 30 days minimum

---

**Version**: 1.0  
**Last Updated**: January 28, 2026
