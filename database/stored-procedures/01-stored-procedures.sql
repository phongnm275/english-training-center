-- ============================================================================
-- Stored Procedures
-- English Training Center Management System
-- Version: 1.0
-- Created: January 28, 2026
-- ============================================================================

USE EnglishTrainingCenter;
GO

-- ============================================================================
-- 1. sp_GetStudentSummary
-- ============================================================================
-- Description: Get comprehensive student information including enrolled courses,
--              average scores, and financial status
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_GetStudentSummary]
    @StudentId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        s.[StudentId],
        s.[FullName],
        s.[Email],
        s.[Phone],
        COUNT(DISTINCT sc.[ClassId]) AS [EnrolledClasses],
        AVG(CAST(sc.[GPA] AS FLOAT)) AS [AverageGPA],
        AVG(CAST(g.[Score] AS FLOAT)) AS [AverageExamScore],
        SUM(i.[Amount]) AS [TotalInvoices],
        SUM(CASE WHEN p.[Status] = 'Completed' THEN p.[Amount] ELSE 0 END) AS [TotalPaid],
        SUM(CASE WHEN i.[Status] = 'Paid' THEN i.[Amount] ELSE 0 END) AS [PaidInvoices],
        SUM(CASE WHEN i.[Status] IN ('Issued', 'Overdue') THEN i.[Amount] ELSE 0 END) AS [OutstandingAmount]
    FROM [dbo].[Students] s
    LEFT JOIN [dbo].[StudentCourses] sc ON s.[StudentId] = sc.[StudentId]
    LEFT JOIN [dbo].[Grades] g ON s.[StudentId] = g.[StudentId]
    LEFT JOIN [dbo].[Invoices] i ON s.[StudentId] = i.[StudentId]
    LEFT JOIN [dbo].[Payments] p ON s.[StudentId] = p.[StudentId]
    WHERE s.[StudentId] = @StudentId
    GROUP BY s.[StudentId], s.[FullName], s.[Email], s.[Phone];
END
GO

-- ============================================================================
-- 2. sp_GetClassAttendance
-- ============================================================================
-- Description: Get attendance statistics for a class
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_GetClassAttendance]
    @ClassId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        c.[ClassId],
        c.[ClassName],
        co.[CourseName],
        COUNT(sc.[StudentId]) AS [TotalEnrolled],
        COUNT(CASE WHEN sc.[Status] = 'Enrolled' THEN 1 END) AS [ActiveStudents],
        COUNT(CASE WHEN sc.[Status] = 'Completed' THEN 1 END) AS [CompletedStudents],
        COUNT(CASE WHEN sc.[Status] = 'Dropped' THEN 1 END) AS [DroppedStudents]
    FROM [dbo].[Classes] c
    JOIN [dbo].[Courses] co ON c.[CourseId] = co.[CourseId]
    LEFT JOIN [dbo].[StudentCourses] sc ON c.[ClassId] = sc.[ClassId]
    WHERE c.[ClassId] = @ClassId
    GROUP BY c.[ClassId], c.[ClassName], co.[CourseName];
END
GO

-- ============================================================================
-- 3. sp_GetInstructorSchedule
-- ============================================================================
-- Description: Get teaching schedule for an instructor
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_GetInstructorSchedule]
    @InstructorId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        i.[InstructorId],
        i.[FullName],
        c.[ClassId],
        c.[ClassName],
        co.[CourseName],
        co.[Level],
        c.[StartDate],
        c.[EndDate],
        c.[Status],
        COUNT(sc.[StudentId]) AS [StudentCount],
        r.[RoomNumber],
        s.[DayOfWeek],
        s.[StartTime],
        s.[EndTime]
    FROM [dbo].[Instructors] i
    JOIN [dbo].[Classes] c ON i.[InstructorId] = c.[InstructorId]
    JOIN [dbo].[Courses] co ON c.[CourseId] = co.[CourseId]
    LEFT JOIN [dbo].[StudentCourses] sc ON c.[ClassId] = sc.[ClassId]
    LEFT JOIN [dbo].[Schedules] s ON c.[ClassId] = s.[ClassId]
    LEFT JOIN [dbo].[Rooms] r ON s.[RoomId] = r.[RoomId]
    WHERE i.[InstructorId] = @InstructorId
    GROUP BY i.[InstructorId], i.[FullName], c.[ClassId], c.[ClassName], 
             co.[CourseName], co.[Level], c.[StartDate], c.[EndDate], c.[Status],
             r.[RoomNumber], s.[DayOfWeek], s.[StartTime], s.[EndTime]
    ORDER BY s.[DayOfWeek], s.[StartTime];
END
GO

-- ============================================================================
-- 4. sp_GetRevenueReport
-- ============================================================================
-- Description: Get revenue report for a date range
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_GetRevenueReport]
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        CONVERT(DATE, p.[PaymentDate]) AS [PaymentDate],
        COUNT(DISTINCT p.[PaymentId]) AS [TransactionCount],
        SUM(p.[Amount]) AS [TotalRevenue],
        COUNT(DISTINCT p.[StudentId]) AS [UniqueStudents],
        p.[PaymentMethod],
        p.[Status]
    FROM [dbo].[Payments] p
    WHERE CONVERT(DATE, p.[PaymentDate]) BETWEEN @StartDate AND @EndDate
    GROUP BY CONVERT(DATE, p.[PaymentDate]), p.[PaymentMethod], p.[Status]
    ORDER BY [PaymentDate] DESC;
    
    -- Summary
    SELECT 
        SUM(p.[Amount]) AS [TotalRevenue],
        COUNT(DISTINCT p.[PaymentId]) AS [TotalTransactions],
        COUNT(DISTINCT p.[StudentId]) AS [TotalUniqueStudents],
        COUNT(CASE WHEN p.[Status] = 'Completed' THEN 1 END) AS [CompletedPayments],
        COUNT(CASE WHEN p.[Status] = 'Pending' THEN 1 END) AS [PendingPayments],
        COUNT(CASE WHEN p.[Status] = 'Failed' THEN 1 END) AS [FailedPayments],
        COUNT(CASE WHEN p.[Status] = 'Refunded' THEN 1 END) AS [RefundedPayments]
    FROM [dbo].[Payments] p
    WHERE CONVERT(DATE, p.[PaymentDate]) BETWEEN @StartDate AND @EndDate;
END
GO

-- ============================================================================
-- 5. sp_GetStudentGrades
-- ============================================================================
-- Description: Get all grades for a student
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_GetStudentGrades]
    @StudentId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        s.[StudentId],
        s.[FullName],
        c.[ClassId],
        c.[ClassName],
        co.[CourseName],
        co.[Level],
        e.[ExamId],
        e.[ExamName],
        e.[ExamDate],
        g.[Score],
        g.[GradeValue],
        e.[MaxScore],
        e.[PassingScore],
        CASE 
            WHEN g.[Score] >= e.[PassingScore] THEN 'Passed'
            ELSE 'Failed'
        END AS [ExamStatus],
        g.[Feedback],
        g.[GradedDate]
    FROM [dbo].[Students] s
    JOIN [dbo].[StudentCourses] sc ON s.[StudentId] = sc.[StudentId]
    JOIN [dbo].[Classes] c ON sc.[ClassId] = c.[ClassId]
    JOIN [dbo].[Courses] co ON c.[CourseId] = co.[CourseId]
    LEFT JOIN [dbo].[Exams] e ON c.[ClassId] = e.[ClassId]
    LEFT JOIN [dbo].[Grades] g ON s.[StudentId] = g.[StudentId] AND e.[ExamId] = g.[ExamId]
    WHERE s.[StudentId] = @StudentId
    ORDER BY c.[StartDate] DESC, e.[ExamDate] DESC;
END
GO

-- ============================================================================
-- 6. sp_GetOverdueInvoices
-- ============================================================================
-- Description: Get all overdue invoices
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_GetOverdueInvoices]
    @DaysOverdue INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        i.[InvoiceId],
        i.[StudentId],
        s.[FullName],
        s.[Email],
        s.[Phone],
        i.[ClassId],
        c.[ClassName],
        i.[Amount],
        i.[IssueDate],
        i.[DueDate],
        DATEDIFF(DAY, i.[DueDate], GETDATE()) AS [DaysOverdue],
        i.[Status],
        i.[Notes]
    FROM [dbo].[Invoices] i
    JOIN [dbo].[Students] s ON i.[StudentId] = s.[StudentId]
    JOIN [dbo].[Classes] c ON i.[ClassId] = c.[ClassId]
    WHERE i.[Status] IN ('Issued', 'Overdue')
    AND DATEDIFF(DAY, i.[DueDate], GETDATE()) >= @DaysOverdue
    ORDER BY [DaysOverdue] DESC, i.[DueDate] ASC;
END
GO

-- ============================================================================
-- 7. sp_GetEnrolledStudentsByClass
-- ============================================================================
-- Description: Get all enrolled students for a class with their details
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_GetEnrolledStudentsByClass]
    @ClassId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        sc.[StudentCourseId],
        sc.[StudentId],
        s.[FullName],
        s.[Email],
        s.[Phone],
        sc.[EnrollDate],
        sc.[Status],
        sc.[GPA],
        c.[ClassName],
        co.[CourseName],
        COUNT(DISTINCT a.[AssignmentId]) AS [AssignmentCount],
        COUNT(DISTINCT asb.[SubmissionId]) AS [SubmittedAssignments],
        AVG(CAST(g.[Score] AS FLOAT)) AS [AverageGrade]
    FROM [dbo].[StudentCourses] sc
    JOIN [dbo].[Students] s ON sc.[StudentId] = s.[StudentId]
    JOIN [dbo].[Classes] c ON sc.[ClassId] = c.[ClassId]
    JOIN [dbo].[Courses] co ON c.[CourseId] = co.[CourseId]
    LEFT JOIN [dbo].[Assignments] a ON c.[ClassId] = a.[ClassId]
    LEFT JOIN [dbo].[AssignmentSubmissions] asb ON a.[AssignmentId] = asb.[AssignmentId] AND s.[StudentId] = asb.[StudentId]
    LEFT JOIN [dbo].[Grades] g ON s.[StudentId] = g.[StudentId]
    WHERE sc.[ClassId] = @ClassId
    GROUP BY sc.[StudentCourseId], sc.[StudentId], s.[FullName], s.[Email], s.[Phone],
             sc.[EnrollDate], sc.[Status], sc.[GPA], c.[ClassName], co.[CourseName]
    ORDER BY s.[FullName];
END
GO

-- ============================================================================
-- 8. sp_GetCourseStatistics
-- ============================================================================
-- Description: Get statistics for a course
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_GetCourseStatistics]
    @CourseId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        co.[CourseId],
        co.[CourseName],
        co.[Level],
        co.[Duration],
        co.[Fee],
        COUNT(DISTINCT c.[ClassId]) AS [TotalClasses],
        COUNT(DISTINCT sc.[StudentId]) AS [TotalEnrolledStudents],
        COUNT(CASE WHEN sc.[Status] = 'Completed' THEN 1 END) AS [CompletedStudents],
        COUNT(CASE WHEN sc.[Status] = 'Dropped' THEN 1 END) AS [DroppedStudents],
        COUNT(CASE WHEN c.[Status] = 'Ongoing' THEN 1 END) AS [OngoingClasses],
        COUNT(CASE WHEN c.[Status] = 'Completed' THEN 1 END) AS [CompletedClasses],
        AVG(CAST(sc.[GPA] AS FLOAT)) AS [AverageGPA],
        SUM(i.[Amount]) AS [TotalRevenue]
    FROM [dbo].[Courses] co
    LEFT JOIN [dbo].[Classes] c ON co.[CourseId] = c.[CourseId]
    LEFT JOIN [dbo].[StudentCourses] sc ON c.[ClassId] = sc.[ClassId]
    LEFT JOIN [dbo].[Invoices] i ON c.[ClassId] = i.[ClassId]
    WHERE co.[CourseId] = @CourseId
    GROUP BY co.[CourseId], co.[CourseName], co.[Level], co.[Duration], co.[Fee];
END
GO

-- ============================================================================
-- 9. sp_CalculateStudentGPA
-- ============================================================================
-- Description: Calculate GPA for a student in a class
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_CalculateStudentGPA]
    @StudentId INT,
    @ClassId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @AverageGrade DECIMAL(3,2);
    
    SELECT @AverageGrade = AVG(CAST(g.[Score] AS DECIMAL(5,2)))
    FROM [dbo].[Grades] g
    JOIN [dbo].[Exams] e ON g.[ExamId] = e.[ExamId]
    WHERE g.[StudentId] = @StudentId
    AND e.[ClassId] = @ClassId;
    
    -- Update StudentCourse GPA
    UPDATE [dbo].[StudentCourses]
    SET [GPA] = @AverageGrade
    WHERE [StudentId] = @StudentId
    AND [ClassId] = @ClassId;
    
    SELECT @AverageGrade AS [CalculatedGPA];
END
GO

-- ============================================================================
-- 10. sp_GetOutstandingPayments
-- ============================================================================
-- Description: Get outstanding payments by student
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_GetOutstandingPayments]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        s.[StudentId],
        s.[FullName],
        s.[Email],
        s.[Phone],
        SUM(i.[Amount]) AS [TotalOutstanding],
        COUNT(DISTINCT i.[InvoiceId]) AS [OverdueInvoiceCount],
        MAX(i.[DueDate]) AS [OldestDueDate],
        MIN(DATEDIFF(DAY, i.[DueDate], GETDATE())) AS [DaysOverdue]
    FROM [dbo].[Students] s
    JOIN [dbo].[Invoices] i ON s.[StudentId] = i.[StudentId]
    WHERE i.[Status] IN ('Issued', 'Overdue')
    GROUP BY s.[StudentId], s.[FullName], s.[Email], s.[Phone]
    ORDER BY [TotalOutstanding] DESC;
END
GO

PRINT '========================================';
PRINT 'All Stored Procedures Created!';
PRINT '========================================';
