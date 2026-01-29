-- ============================================================================
-- English Training Center Management System - Database Creation Script
-- SQL Server 2019+
-- Version: 1.0
-- Created: January 28, 2026
-- ============================================================================

-- Drop existing database (for development only)
-- IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'EnglishTrainingCenter')
-- BEGIN
--     DROP DATABASE EnglishTrainingCenter;
-- END

-- Create Database
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'EnglishTrainingCenter')
BEGIN
    CREATE DATABASE EnglishTrainingCenter;
END
GO

USE EnglishTrainingCenter;
GO

-- ============================================================================
-- Create Tables
-- ============================================================================

-- 1. ROLES TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]'))
BEGIN
    CREATE TABLE [dbo].[Roles] (
        [RoleId] INT PRIMARY KEY IDENTITY(1,1),
        [RoleName] NVARCHAR(50) NOT NULL UNIQUE,
        [Description] NVARCHAR(MAX),
        [CreatedDate] DATETIME DEFAULT GETDATE()
    );
    
    CREATE INDEX [IX_Roles_RoleName] ON [dbo].[Roles]([RoleName]);
    
    PRINT 'Table [Roles] created successfully.';
END
GO

-- 2. USERS TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]'))
BEGIN
    CREATE TABLE [dbo].[Users] (
        [UserId] INT PRIMARY KEY IDENTITY(1,1),
        [Username] NVARCHAR(50) NOT NULL UNIQUE,
        [Email] NVARCHAR(100) NOT NULL UNIQUE,
        [PasswordHash] NVARCHAR(MAX) NOT NULL,
        [FullName] NVARCHAR(100) NOT NULL,
        [RoleId] INT NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [LastLoginDate] DATETIME,
        [CreatedDate] DATETIME DEFAULT GETDATE(),
        [ModifiedDate] DATETIME,
        CONSTRAINT [FK_Users_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([RoleId])
    );
    
    CREATE INDEX [IX_Users_Email] ON [dbo].[Users]([Email]);
    CREATE INDEX [IX_Users_RoleId] ON [dbo].[Users]([RoleId]);
    CREATE INDEX [IX_Users_Username] ON [dbo].[Users]([Username]);
    
    PRINT 'Table [Users] created successfully.';
END
GO

-- 3. STUDENTS TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Students]'))
BEGIN
    CREATE TABLE [dbo].[Students] (
        [StudentId] INT PRIMARY KEY IDENTITY(1,1),
        [UserId] INT NOT NULL,
        [FullName] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(100) NOT NULL,
        [Phone] NVARCHAR(20),
        [DateOfBirth] DATE,
        [Address] NVARCHAR(MAX),
        [Avatar] NVARCHAR(MAX),
        [EnrollmentDate] DATETIME DEFAULT GETDATE(),
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME DEFAULT GETDATE(),
        [ModifiedDate] DATETIME,
        CONSTRAINT [FK_Students_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([UserId])
    );
    
    CREATE INDEX [IX_Students_Email] ON [dbo].[Students]([Email]);
    CREATE INDEX [IX_Students_UserId] ON [dbo].[Students]([UserId]);
    
    PRINT 'Table [Students] created successfully.';
END
GO

-- 4. COURSES TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Courses]'))
BEGIN
    CREATE TABLE [dbo].[Courses] (
        [CourseId] INT PRIMARY KEY IDENTITY(1,1),
        [CourseName] NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(MAX),
        [Level] NVARCHAR(20), -- 'Beginner', 'Intermediate', 'Advanced'
        [Duration] INT, -- in hours
        [Fee] DECIMAL(12,2),
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME DEFAULT GETDATE(),
        [ModifiedDate] DATETIME
    );
    
    CREATE INDEX [IX_Courses_Level] ON [dbo].[Courses]([Level]);
    CREATE INDEX [IX_Courses_IsActive] ON [dbo].[Courses]([IsActive]);
    
    PRINT 'Table [Courses] created successfully.';
END
GO

-- 5. INSTRUCTORS TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Instructors]'))
BEGIN
    CREATE TABLE [dbo].[Instructors] (
        [InstructorId] INT PRIMARY KEY IDENTITY(1,1),
        [UserId] INT NOT NULL,
        [FullName] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(100) NOT NULL,
        [Phone] NVARCHAR(20),
        [Specialization] NVARCHAR(100),
        [YearsExperience] INT,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME DEFAULT GETDATE(),
        [ModifiedDate] DATETIME,
        CONSTRAINT [FK_Instructors_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([UserId])
    );
    
    CREATE INDEX [IX_Instructors_Email] ON [dbo].[Instructors]([Email]);
    CREATE INDEX [IX_Instructors_UserId] ON [dbo].[Instructors]([UserId]);
    
    PRINT 'Table [Instructors] created successfully.';
END
GO

-- 6. INSTRUCTOR QUALIFICATIONS TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InstructorQualifications]'))
BEGIN
    CREATE TABLE [dbo].[InstructorQualifications] (
        [QualificationId] INT PRIMARY KEY IDENTITY(1,1),
        [InstructorId] INT NOT NULL,
        [Certification] NVARCHAR(100),
        [CertificationDate] DATETIME,
        [ExpiryDate] DATETIME,
        [Issuer] NVARCHAR(100),
        [CreatedDate] DATETIME DEFAULT GETDATE(),
        CONSTRAINT [FK_InstructorQualifications_Instructors] FOREIGN KEY ([InstructorId]) REFERENCES [dbo].[Instructors]([InstructorId]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_InstructorQualifications_InstructorId] ON [dbo].[InstructorQualifications]([InstructorId]);
    
    PRINT 'Table [InstructorQualifications] created successfully.';
END
GO

-- 7. CLASSES TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Classes]'))
BEGIN
    CREATE TABLE [dbo].[Classes] (
        [ClassId] INT PRIMARY KEY IDENTITY(1,1),
        [CourseId] INT NOT NULL,
        [InstructorId] INT NOT NULL,
        [ClassName] NVARCHAR(100) NOT NULL,
        [MaxStudents] INT DEFAULT 30,
        [StartDate] DATETIME,
        [EndDate] DATETIME,
        [Status] NVARCHAR(20), -- 'Upcoming', 'Ongoing', 'Completed', 'Cancelled'
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME DEFAULT GETDATE(),
        [ModifiedDate] DATETIME,
        CONSTRAINT [FK_Classes_Courses] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[Courses]([CourseId]),
        CONSTRAINT [FK_Classes_Instructors] FOREIGN KEY ([InstructorId]) REFERENCES [dbo].[Instructors]([InstructorId]),
        CONSTRAINT [CK_Classes_DateRange] CHECK ([EndDate] > [StartDate])
    );
    
    CREATE INDEX [IX_Classes_CourseId] ON [dbo].[Classes]([CourseId]);
    CREATE INDEX [IX_Classes_InstructorId] ON [dbo].[Classes]([InstructorId]);
    CREATE INDEX [IX_Classes_Status] ON [dbo].[Classes]([Status]);
    
    PRINT 'Table [Classes] created successfully.';
END
GO

-- 8. STUDENT COURSES (ENROLLMENT) TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StudentCourses]'))
BEGIN
    CREATE TABLE [dbo].[StudentCourses] (
        [StudentCourseId] INT PRIMARY KEY IDENTITY(1,1),
        [StudentId] INT NOT NULL,
        [ClassId] INT NOT NULL,
        [EnrollDate] DATETIME DEFAULT GETDATE(),
        [Status] NVARCHAR(20), -- 'Enrolled', 'Completed', 'Dropped'
        [GPA] DECIMAL(3,2),
        [CreatedDate] DATETIME DEFAULT GETDATE(),
        CONSTRAINT [FK_StudentCourses_Students] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students]([StudentId]) ON DELETE CASCADE,
        CONSTRAINT [FK_StudentCourses_Classes] FOREIGN KEY ([ClassId]) REFERENCES [dbo].[Classes]([ClassId]),
        CONSTRAINT [UQ_StudentCourses_StudentClass] UNIQUE ([StudentId], [ClassId])
    );
    
    CREATE INDEX [IX_StudentCourses_StudentId] ON [dbo].[StudentCourses]([StudentId]);
    CREATE INDEX [IX_StudentCourses_ClassId] ON [dbo].[StudentCourses]([ClassId]);
    
    PRINT 'Table [StudentCourses] created successfully.';
END
GO

-- 9. ROOMS TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rooms]'))
BEGIN
    CREATE TABLE [dbo].[Rooms] (
        [RoomId] INT PRIMARY KEY IDENTITY(1,1),
        [RoomNumber] NVARCHAR(20) NOT NULL UNIQUE,
        [Capacity] INT NOT NULL,
        [Equipment] NVARCHAR(MAX), -- Projector, Whiteboard, etc.
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME DEFAULT GETDATE()
    );
    
    CREATE INDEX [IX_Rooms_IsActive] ON [dbo].[Rooms]([IsActive]);
    
    PRINT 'Table [Rooms] created successfully.';
END
GO

-- 10. SCHEDULES TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Schedules]'))
BEGIN
    CREATE TABLE [dbo].[Schedules] (
        [ScheduleId] INT PRIMARY KEY IDENTITY(1,1),
        [ClassId] INT NOT NULL,
        [RoomId] INT NOT NULL,
        [DayOfWeek] INT, -- 0=Sunday, 1=Monday, ..., 6=Saturday
        [StartTime] TIME NOT NULL,
        [EndTime] TIME NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME DEFAULT GETDATE(),
        CONSTRAINT [FK_Schedules_Classes] FOREIGN KEY ([ClassId]) REFERENCES [dbo].[Classes]([ClassId]) ON DELETE CASCADE,
        CONSTRAINT [FK_Schedules_Rooms] FOREIGN KEY ([RoomId]) REFERENCES [dbo].[Rooms]([RoomId]),
        CONSTRAINT [CK_Schedules_TimeRange] CHECK ([EndTime] > [StartTime]),
        CONSTRAINT [CK_Schedules_DayOfWeek] CHECK ([DayOfWeek] >= 0 AND [DayOfWeek] <= 6)
    );
    
    CREATE INDEX [IX_Schedules_ClassId] ON [dbo].[Schedules]([ClassId]);
    CREATE INDEX [IX_Schedules_RoomId] ON [dbo].[Schedules]([RoomId]);
    
    PRINT 'Table [Schedules] created successfully.';
END
GO

-- 11. INVOICES TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Invoices]'))
BEGIN
    CREATE TABLE [dbo].[Invoices] (
        [InvoiceId] INT PRIMARY KEY IDENTITY(1,1),
        [StudentId] INT NOT NULL,
        [ClassId] INT NOT NULL,
        [Amount] DECIMAL(12,2) NOT NULL,
        [DueDate] DATETIME,
        [IssueDate] DATETIME DEFAULT GETDATE(),
        [Status] NVARCHAR(20), -- 'Issued', 'Paid', 'Overdue', 'Cancelled'
        [Notes] NVARCHAR(MAX),
        [CreatedDate] DATETIME DEFAULT GETDATE(),
        CONSTRAINT [FK_Invoices_Students] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students]([StudentId]),
        CONSTRAINT [FK_Invoices_Classes] FOREIGN KEY ([ClassId]) REFERENCES [dbo].[Classes]([ClassId]),
        CONSTRAINT [CK_Invoices_Amount] CHECK ([Amount] > 0)
    );
    
    CREATE INDEX [IX_Invoices_StudentId] ON [dbo].[Invoices]([StudentId]);
    CREATE INDEX [IX_Invoices_ClassId] ON [dbo].[Invoices]([ClassId]);
    CREATE INDEX [IX_Invoices_Status] ON [dbo].[Invoices]([Status]);
    
    PRINT 'Table [Invoices] created successfully.';
END
GO

-- 12. PAYMENTS TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Payments]'))
BEGIN
    CREATE TABLE [dbo].[Payments] (
        [PaymentId] INT PRIMARY KEY IDENTITY(1,1),
        [StudentId] INT NOT NULL,
        [Amount] DECIMAL(12,2) NOT NULL,
        [PaymentDate] DATETIME DEFAULT GETDATE(),
        [PaymentMethod] NVARCHAR(50), -- 'Credit Card', 'Bank Transfer', 'Cash'
        [Status] NVARCHAR(20), -- 'Pending', 'Completed', 'Failed', 'Refunded'
        [TransactionId] NVARCHAR(100),
        [Description] NVARCHAR(MAX),
        [CreatedDate] DATETIME DEFAULT GETDATE(),
        CONSTRAINT [FK_Payments_Students] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students]([StudentId]),
        CONSTRAINT [CK_Payments_Amount] CHECK ([Amount] > 0)
    );
    
    CREATE INDEX [IX_Payments_StudentId] ON [dbo].[Payments]([StudentId]);
    CREATE INDEX [IX_Payments_Status] ON [dbo].[Payments]([Status]);
    
    PRINT 'Table [Payments] created successfully.';
END
GO

-- 13. ASSIGNMENTS TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Assignments]'))
BEGIN
    CREATE TABLE [dbo].[Assignments] (
        [AssignmentId] INT PRIMARY KEY IDENTITY(1,1),
        [ClassId] INT NOT NULL,
        [Title] NVARCHAR(200) NOT NULL,
        [Description] NVARCHAR(MAX),
        [DueDate] DATETIME,
        [MaxScore] DECIMAL(5,2) DEFAULT 100,
        [CreatedDate] DATETIME DEFAULT GETDATE(),
        [ModifiedDate] DATETIME,
        CONSTRAINT [FK_Assignments_Classes] FOREIGN KEY ([ClassId]) REFERENCES [dbo].[Classes]([ClassId]) ON DELETE CASCADE,
        CONSTRAINT [CK_Assignments_MaxScore] CHECK ([MaxScore] > 0)
    );
    
    CREATE INDEX [IX_Assignments_ClassId] ON [dbo].[Assignments]([ClassId]);
    
    PRINT 'Table [Assignments] created successfully.';
END
GO

-- 14. ASSIGNMENT SUBMISSIONS TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AssignmentSubmissions]'))
BEGIN
    CREATE TABLE [dbo].[AssignmentSubmissions] (
        [SubmissionId] INT PRIMARY KEY IDENTITY(1,1),
        [AssignmentId] INT NOT NULL,
        [StudentId] INT NOT NULL,
        [SubmissionDate] DATETIME DEFAULT GETDATE(),
        [Content] NVARCHAR(MAX),
        [FileUrl] NVARCHAR(MAX),
        [Score] DECIMAL(5,2),
        [Feedback] NVARCHAR(MAX),
        [IsLate] BIT,
        [CreatedDate] DATETIME DEFAULT GETDATE(),
        CONSTRAINT [FK_AssignmentSubmissions_Assignments] FOREIGN KEY ([AssignmentId]) REFERENCES [dbo].[Assignments]([AssignmentId]) ON DELETE CASCADE,
        CONSTRAINT [FK_AssignmentSubmissions_Students] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students]([StudentId]),
        CONSTRAINT [UQ_AssignmentSubmissions_AssignmentStudent] UNIQUE ([AssignmentId], [StudentId])
    );
    
    CREATE INDEX [IX_AssignmentSubmissions_AssignmentId] ON [dbo].[AssignmentSubmissions]([AssignmentId]);
    CREATE INDEX [IX_AssignmentSubmissions_StudentId] ON [dbo].[AssignmentSubmissions]([StudentId]);
    
    PRINT 'Table [AssignmentSubmissions] created successfully.';
END
GO

-- 15. EXAMS TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Exams]'))
BEGIN
    CREATE TABLE [dbo].[Exams] (
        [ExamId] INT PRIMARY KEY IDENTITY(1,1),
        [ClassId] INT NOT NULL,
        [ExamName] NVARCHAR(100) NOT NULL,
        [ExamDate] DATETIME,
        [Duration] INT, -- in minutes
        [MaxScore] DECIMAL(5,2) DEFAULT 100,
        [PassingScore] DECIMAL(5,2) DEFAULT 50,
        [CreatedDate] DATETIME DEFAULT GETDATE(),
        CONSTRAINT [FK_Exams_Classes] FOREIGN KEY ([ClassId]) REFERENCES [dbo].[Classes]([ClassId]) ON DELETE CASCADE,
        CONSTRAINT [CK_Exams_Scores] CHECK ([PassingScore] <= [MaxScore] AND [MaxScore] > 0)
    );
    
    CREATE INDEX [IX_Exams_ClassId] ON [dbo].[Exams]([ClassId]);
    
    PRINT 'Table [Exams] created successfully.';
END
GO

-- 16. GRADES TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Grades]'))
BEGIN
    CREATE TABLE [dbo].[Grades] (
        [GradeId] INT PRIMARY KEY IDENTITY(1,1),
        [StudentId] INT NOT NULL,
        [ExamId] INT NOT NULL,
        [Score] DECIMAL(5,2),
        [GradeValue] NVARCHAR(5), -- 'A', 'B', 'C', 'D', 'F'
        [Feedback] NVARCHAR(MAX),
        [GradedDate] DATETIME,
        [CreatedDate] DATETIME DEFAULT GETDATE(),
        CONSTRAINT [FK_Grades_Students] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students]([StudentId]),
        CONSTRAINT [FK_Grades_Exams] FOREIGN KEY ([ExamId]) REFERENCES [dbo].[Exams]([ExamId]) ON DELETE CASCADE,
        CONSTRAINT [UQ_Grades_StudentExam] UNIQUE ([StudentId], [ExamId]),
        CONSTRAINT [CK_Grades_Score] CHECK ([Score] >= 0 AND [Score] <= 100)
    );
    
    CREATE INDEX [IX_Grades_StudentId] ON [dbo].[Grades]([StudentId]);
    CREATE INDEX [IX_Grades_ExamId] ON [dbo].[Grades]([ExamId]);
    
    PRINT 'Table [Grades] created successfully.';
END
GO

PRINT '========================================';
PRINT 'Database Schema Creation Completed!';
PRINT '========================================';
