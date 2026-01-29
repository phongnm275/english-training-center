-- ============================================================================
-- Insert Sample/Test Data
-- English Training Center Management System
-- Version: 1.0
-- Created: January 28, 2026
-- ============================================================================

USE EnglishTrainingCenter;
GO

-- ============================================================================
-- 1. Insert Roles
-- ============================================================================

INSERT INTO [dbo].[Roles] ([RoleName], [Description])
VALUES 
    ('Admin', 'Administrator - Full system access'),
    ('Manager', 'Manager - Manage center operations'),
    ('Instructor', 'Instructor - Teach and grade students'),
    ('Student', 'Student - Enroll and learn'),
    ('Finance', 'Finance - Manage payments and invoices');

PRINT 'Roles inserted successfully.';
GO

-- ============================================================================
-- 2. Insert Users (with sample passwords - in production, use hashed passwords)
-- ============================================================================

-- Password hashes are placeholders - in production use bcrypt/Argon2
INSERT INTO [dbo].[Users] ([Username], [Email], [PasswordHash], [FullName], [RoleId], [IsActive])
VALUES
    -- Admin Users
    ('admin1', 'admin@englishcenter.com', '$2a$10$exampleHashedPassword1', 'Admin User', 1, 1),
    
    -- Manager Users
    ('manager1', 'manager@englishcenter.com', '$2a$10$exampleHashedPassword2', 'John Manager', 2, 1),
    
    -- Instructor Users
    ('jane.doe', 'jane.doe@englishcenter.com', '$2a$10$exampleHashedPassword3', 'Jane Doe', 3, 1),
    ('michael.smith', 'michael.smith@englishcenter.com', '$2a$10$exampleHashedPassword4', 'Michael Smith', 3, 1),
    ('sarah.johnson', 'sarah.johnson@englishcenter.com', '$2a$10$exampleHashedPassword5', 'Sarah Johnson', 3, 1),
    
    -- Student Users
    ('john.student1', 'john.student1@example.com', '$2a$10$exampleHashedPassword6', 'John Student One', 4, 1),
    ('alice.student2', 'alice.student2@example.com', '$2a$10$exampleHashedPassword7', 'Alice Student Two', 4, 1),
    ('bob.student3', 'bob.student3@example.com', '$2a$10$exampleHashedPassword8', 'Bob Student Three', 4, 1),
    ('diana.student4', 'diana.student4@example.com', '$2a$10$exampleHashedPassword9', 'Diana Student Four', 4, 1),
    ('evan.student5', 'evan.student5@example.com', '$2a$10$exampleHashedPassword10', 'Evan Student Five', 4, 1),
    
    -- Finance User
    ('finance1', 'finance@englishcenter.com', '$2a$10$exampleHashedPassword11', 'Finance Officer', 5, 1);

PRINT 'Users inserted successfully.';
GO

-- ============================================================================
-- 3. Insert Students
-- ============================================================================

INSERT INTO [dbo].[Students] ([UserId], [FullName], [Email], [Phone], [DateOfBirth], [Address], [IsActive])
VALUES
    (6, 'John Student One', 'john.student1@example.com', '0901234567', '2005-03-15', '123 Main Street, HCMC', 1),
    (7, 'Alice Student Two', 'alice.student2@example.com', '0902345678', '2004-07-22', '456 Oak Avenue, HCMC', 1),
    (8, 'Bob Student Three', 'bob.student3@example.com', '0903456789', '2005-11-10', '789 Pine Road, HCMC', 1),
    (9, 'Diana Student Four', 'diana.student4@example.com', '0904567890', '2004-05-30', '321 Elm Street, Ha Noi', 1),
    (10, 'Evan Student Five', 'evan.student5@example.com', '0905678901', '2005-09-18', '654 Maple Drive, Ha Noi', 1);

PRINT 'Students inserted successfully.';
GO

-- ============================================================================
-- 4. Insert Instructors
-- ============================================================================

INSERT INTO [dbo].[Instructors] ([UserId], [FullName], [Email], [Phone], [Specialization], [YearsExperience], [IsActive])
VALUES
    (3, 'Jane Doe', 'jane.doe@englishcenter.com', '0911111111', 'Spoken English', 8, 1),
    (4, 'Michael Smith', 'michael.smith@englishcenter.com', '0922222222', 'Grammar & Writing', 6, 1),
    (5, 'Sarah Johnson', 'sarah.johnson@englishcenter.com', '0933333333', 'Business English', 10, 1);

PRINT 'Instructors inserted successfully.';
GO

-- ============================================================================
-- 5. Insert Instructor Qualifications
-- ============================================================================

INSERT INTO [dbo].[InstructorQualifications] ([InstructorId], [Certification], [CertificationDate], [ExpiryDate], [Issuer])
VALUES
    (1, 'TOEFL', '2020-06-15', '2025-06-15', 'ETS'),
    (1, 'CELTA', '2019-08-20', NULL, 'Cambridge University'),
    (2, 'IELTS', '2021-03-10', '2026-03-10', 'British Council'),
    (2, 'TESOL', '2018-12-05', NULL, 'TESOL International'),
    (3, 'Business English Certificate', '2022-05-18', '2027-05-18', 'Cambridge Business English');

PRINT 'Instructor Qualifications inserted successfully.';
GO

-- ============================================================================
-- 6. Insert Courses
-- ============================================================================

INSERT INTO [dbo].[Courses] ([CourseName], [Description], [Level], [Duration], [Fee], [IsActive])
VALUES
    ('Beginner English', 'Basic English course for beginners', 'Beginner', 40, 3000000, 1),
    ('Intermediate English', 'Intermediate level English course', 'Intermediate', 50, 4000000, 1),
    ('Advanced English', 'Advanced level English course', 'Advanced', 60, 5000000, 1),
    ('Business English', 'English for business professionals', 'Intermediate', 45, 4500000, 1),
    ('IELTS Preparation', 'Prepare for IELTS examination', 'Advanced', 55, 5500000, 1);

PRINT 'Courses inserted successfully.';
GO

-- ============================================================================
-- 7. Insert Classes
-- ============================================================================

INSERT INTO [dbo].[Classes] ([CourseId], [InstructorId], [ClassName], [MaxStudents], [StartDate], [EndDate], [Status], [IsActive])
VALUES
    (1, 1, 'Beginner English A', 30, '2025-02-01', '2025-04-30', 'Upcoming', 1),
    (1, 1, 'Beginner English B', 30, '2025-03-01', '2025-05-31', 'Upcoming', 1),
    (2, 2, 'Intermediate English A', 25, '2025-02-15', '2025-05-15', 'Upcoming', 1),
    (3, 1, 'Advanced English A', 20, '2025-02-01', '2025-06-30', 'Upcoming', 1),
    (4, 3, 'Business English A', 25, '2025-03-01', '2025-06-01', 'Upcoming', 1);

PRINT 'Classes inserted successfully.';
GO

-- ============================================================================
-- 8. Insert Rooms
-- ============================================================================

INSERT INTO [dbo].[Rooms] ([RoomNumber], [Capacity], [Equipment], [IsActive])
VALUES
    ('Room 101', 30, 'Projector, Whiteboard, Air Conditioning', 1),
    ('Room 102', 25, 'Projector, Smart Board, Air Conditioning', 1),
    ('Room 103', 20, 'Projector, Whiteboard, Audio System', 1),
    ('Room 201', 35, 'Projector, Smart Board, Video Conference', 1),
    ('Room 202', 25, 'Projector, Whiteboard, Air Conditioning', 1);

PRINT 'Rooms inserted successfully.';
GO

-- ============================================================================
-- 9. Insert Schedules
-- ============================================================================

INSERT INTO [dbo].[Schedules] ([ClassId], [RoomId], [DayOfWeek], [StartTime], [EndTime], [IsActive])
VALUES
    -- Beginner English A (Class 1) - 3 sessions per week
    (1, 1, 1, '14:00', '16:00', 1), -- Monday
    (1, 1, 3, '14:00', '16:00', 1), -- Wednesday
    (1, 1, 5, '14:00', '16:00', 1), -- Friday
    
    -- Beginner English B (Class 2)
    (2, 2, 2, '10:00', '12:00', 1), -- Tuesday
    (2, 2, 4, '10:00', '12:00', 1), -- Thursday
    (2, 2, 6, '10:00', '12:00', 1), -- Saturday
    
    -- Intermediate English A (Class 3)
    (3, 3, 1, '09:00', '11:00', 1), -- Monday
    (3, 3, 3, '09:00', '11:00', 1), -- Wednesday
    (3, 3, 5, '09:00', '11:00', 1), -- Friday
    
    -- Advanced English A (Class 4)
    (4, 4, 2, '14:00', '17:00', 1), -- Tuesday (3 hours)
    (4, 4, 4, '14:00', '17:00', 1), -- Thursday (3 hours)
    (4, 4, 6, '14:00', '17:00', 1), -- Saturday (3 hours)
    
    -- Business English A (Class 5)
    (5, 5, 1, '18:00', '20:00', 1), -- Monday
    (5, 5, 3, '18:00', '20:00', 1), -- Wednesday
    (5, 5, 5, '18:00', '20:00', 1); -- Friday

PRINT 'Schedules inserted successfully.';
GO

-- ============================================================================
-- 10. Insert Student Enrollments (StudentCourses)
-- ============================================================================

INSERT INTO [dbo].[StudentCourses] ([StudentId], [ClassId], [Status], [GPA])
VALUES
    -- Class 1: Beginner English A
    (1, 1, 'Enrolled', NULL),
    (2, 1, 'Enrolled', NULL),
    (3, 1, 'Enrolled', NULL),
    
    -- Class 2: Beginner English B
    (4, 2, 'Enrolled', NULL),
    (5, 2, 'Enrolled', NULL),
    
    -- Class 3: Intermediate English A
    (1, 3, 'Enrolled', NULL),
    
    -- Class 4: Advanced English A
    (2, 4, 'Enrolled', NULL),
    
    -- Class 5: Business English A
    (3, 5, 'Enrolled', NULL),
    (4, 5, 'Enrolled', NULL);

PRINT 'Student Enrollments inserted successfully.';
GO

-- ============================================================================
-- 11. Insert Invoices
-- ============================================================================

INSERT INTO [dbo].[Invoices] ([StudentId], [ClassId], [Amount], [IssueDate], [DueDate], [Status])
VALUES
    (1, 1, 3000000, '2025-01-15', '2025-02-01', 'Issued'),
    (2, 1, 3000000, '2025-01-15', '2025-02-01', 'Issued'),
    (3, 1, 3000000, '2025-01-15', '2025-02-01', 'Paid'),
    (4, 2, 3000000, '2025-02-01', '2025-03-01', 'Issued'),
    (5, 2, 3000000, '2025-02-01', '2025-03-01', 'Issued'),
    (1, 3, 4000000, '2025-01-20', '2025-02-15', 'Paid'),
    (2, 4, 5000000, '2025-01-15', '2025-02-01', 'Issued');

PRINT 'Invoices inserted successfully.';
GO

-- ============================================================================
-- 12. Insert Payments
-- ============================================================================

INSERT INTO [dbo].[Payments] ([StudentId], [Amount], [PaymentDate], [PaymentMethod], [Status], [TransactionId], [Description])
VALUES
    (3, 3000000, '2025-01-28', 'Bank Transfer', 'Completed', 'TXN20250128001', 'Payment for Beginner English A'),
    (1, 4000000, '2025-01-25', 'Credit Card', 'Completed', 'TXN20250125001', 'Payment for Intermediate English A');

PRINT 'Payments inserted successfully.';
GO

-- ============================================================================
-- 13. Insert Assignments
-- ============================================================================

INSERT INTO [dbo].[Assignments] ([ClassId], [Title], [Description], [DueDate], [MaxScore])
VALUES
    (1, 'Unit 1 Exercise', 'Complete all exercises in Unit 1', '2025-02-08', 100),
    (1, 'Unit 2 Essay', 'Write a short essay about yourself (200 words)', '2025-02-22', 100),
    (2, 'Unit 1 Quiz', 'Online quiz covering Unit 1 vocabulary', '2025-03-10', 100),
    (3, 'Unit 1 Listening', 'Listen and answer comprehension questions', '2025-02-20', 100),
    (4, 'Unit 1 Speaking', 'Record a 2-minute presentation', '2025-02-15', 100);

PRINT 'Assignments inserted successfully.';
GO

-- ============================================================================
-- 14. Insert Assignment Submissions
-- ============================================================================

INSERT INTO [dbo].[AssignmentSubmissions] ([AssignmentId], [StudentId], [SubmissionDate], [Content], [Score], [Feedback], [IsLate])
VALUES
    (1, 1, '2025-02-07', 'Exercise answers...', 95, 'Excellent work!', 0),
    (1, 2, '2025-02-08', 'Exercise answers...', 88, 'Good, but review question 5', 0),
    (1, 3, '2025-02-10', 'Exercise answers...', 78, 'Late submission', 1),
    (3, 4, '2025-03-09', 'Quiz answers...', 92, 'Great vocabulary!', 0),
    (4, 1, '2025-02-19', 'Listening answers...', 85, 'Good comprehension', 0);

PRINT 'Assignment Submissions inserted successfully.';
GO

-- ============================================================================
-- 15. Insert Exams
-- ============================================================================

INSERT INTO [dbo].[Exams] ([ClassId], [ExamName], [ExamDate], [Duration], [MaxScore], [PassingScore])
VALUES
    (1, 'Midterm Exam', '2025-03-15', 90, 100, 50),
    (1, 'Final Exam', '2025-04-26', 120, 100, 50),
    (2, 'Placement Test', '2025-03-01', 60, 100, 60),
    (3, 'Midterm Exam', '2025-03-29', 90, 100, 50),
    (4, 'Final Exam', '2025-06-28', 120, 100, 50);

PRINT 'Exams inserted successfully.';
GO

-- ============================================================================
-- 16. Insert Grades
-- ============================================================================

INSERT INTO [dbo].[Grades] ([StudentId], [ExamId], [Score], [GradeValue], [Feedback], [GradedDate])
VALUES
    (1, 1, 85, 'A', 'Excellent performance', '2025-03-16'),
    (2, 1, 78, 'B', 'Good work, needs improvement in speaking', '2025-03-16'),
    (3, 1, 65, 'C', 'Satisfactory, focus on grammar', '2025-03-16'),
    (4, 3, 88, 'A', 'Strong vocabulary and comprehension', '2025-03-02'),
    (5, 3, 72, 'B', 'Good progress', '2025-03-02');

PRINT 'Grades inserted successfully.';
GO

-- ============================================================================
-- Summary
-- ============================================================================

PRINT '========================================';
PRINT 'Sample Data Insertion Completed!';
PRINT '========================================';
PRINT 'Total Records Inserted:';
PRINT '- Roles: 5';
PRINT '- Users: 11';
PRINT '- Students: 5';
PRINT '- Instructors: 3';
PRINT '- Courses: 5';
PRINT '- Classes: 5';
PRINT '- Rooms: 5';
PRINT '- Student Enrollments: 9';
PRINT '- Invoices: 7';
PRINT '- Payments: 2';
PRINT '- Assignments: 5';
PRINT '- Assignment Submissions: 5';
PRINT '- Exams: 5';
PRINT '- Grades: 5';
PRINT '========================================';
