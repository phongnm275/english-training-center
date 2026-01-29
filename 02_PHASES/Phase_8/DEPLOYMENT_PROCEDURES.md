# Phase 8: Deployment Procedures & Data Migration

**Project**: English Training Center LMS  
**Phase**: 8 - Deployment & Operations  
**Document**: Detailed Deployment & Migration Procedures  
**Date**: January 28, 2026  

---

## 📋 Pre-Deployment Checklist (20+ Items)

### Infrastructure Verification (5 items)
- [ ] Load balancer operational (all 3 API instances responding)
- [ ] Database replication synchronized (secondary up-to-date)
- [ ] Cache cluster operational (Redis responding)
- [ ] Monitoring dashboards live (all metrics visible)
- [ ] DNS pointing to production load balancer

### Code & Build Verification (5 items)
- [ ] All code merged to main branch
- [ ] Build succeeds without errors
- [ ] All 2,000+ tests passing (100%)
- [ ] Code coverage > 85%
- [ ] Security scan: 0 critical vulnerabilities

### Data & Configuration (5 items)
- [ ] Database backups current (within 1 hour)
- [ ] Backup to secondary location verified
- [ ] Configuration files updated for production
- [ ] Environment variables set correctly
- [ ] SSL certificates valid & installed

### Team & Process (5 items)
- [ ] Deployment team briefed & ready
- [ ] Support team on standby
- [ ] Communication channels confirmed
- [ ] Rollback procedures tested
- [ ] Emergency contacts posted

---

## 🚀 Deployment Procedure (Step-by-Step)

### Phase 1: Pre-Deployment (T-2 hours)

**Step 1.1: Final System Checks** (30 min)
```
Action: Verify all systems operational
├─ [ ] Login to deployment server
├─ [ ] Check database connectivity
│   └─ SELECT COUNT(*) FROM Students; -- Verify access
├─ [ ] Check cache connectivity
│   └─ PING redis-server
├─ [ ] Check application health
│   └─ curl https://localhost:5001/health
└─ [ ] Verify load balancer health checks
    └─ All instances showing healthy
```

**Step 1.2: Database Backup** (30 min)
```
-- Create pre-deployment backup
BACKUP DATABASE [EnglishTrainingCenter]
TO DISK = '\\backup-server\prod\PreDeployment_2026-03-01.bak'
WITH INIT, STATS = 10;

-- Verify backup file
DIR \\backup-server\prod\PreDeployment_2026-03-01.bak /s
```

**Step 1.3: Communication** (15 min)
```
Action: Send pre-deployment notification
To: All stakeholders
Subject: "LMS Production Deployment - Early March 2026"
Message: "System going live. Expected downtime: 2 hours (5:00 AM - 7:00 AM)"
```

**Step 1.4: Team Briefing** (15 min)
```
Meeting: Go-Live Team Briefing
Attendees: Deployment lead, DevOps, DBA, QA lead, Support lead
Topics:
├─ Deployment timeline review
├─ Roles & responsibilities confirmation
├─ Escalation procedures
├─ Communication protocol
└─ Emergency procedures
```

---

### Phase 2: Deployment Window (T-0 to T+2 hours)

**Step 2.1: Enable Maintenance Mode** (T+0, 15 min)
```csharp
// In appsettings.json - Set during deployment
"MaintenanceMode": {
  "Enabled": true,
  "Message": "System is undergoing maintenance. Please check back in 2 hours."
}

// Load balancer returns 503 Service Unavailable
// Users see maintenance message
```

**Step 2.2: Stop Background Services** (T+15, 10 min)
```
Action: Stop Hangfire background jobs gracefully
├─ [ ] Stop accepting new jobs
├─ [ ] Wait for in-flight jobs to complete (timeout: 5 min)
├─ [ ] Stop Hangfire service
└─ [ ] Verify all jobs completed

Action: Stop cache warming
├─ [ ] Flush Redis (non-production data only)
└─ [ ] Stop cache refresh jobs
```

**Step 2.3: Database Migration (if needed)** (T+25, 45 min)

**Option A: No Migration Needed**
```sql
-- Just verify database structure
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo'
ORDER BY TABLE_NAME;
-- Verify 50+ tables exist (Phase 1-8)
```

**Option B: Data Migration from Legacy System**
```sql
-- Step 1: Backup production database
BACKUP DATABASE [EnglishTrainingCenter_Prod]
TO DISK = '\\backup-server\migrations\PreMigration_2026-03-01.bak'
WITH INIT;

-- Step 2: Disable foreign keys (temporarily)
EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';

-- Step 3: Migrate data from legacy system
-- Example: Migrate students
INSERT INTO Students (Email, FirstName, LastName, PhoneNumber, CreatedDate)
SELECT Email, FirstName, LastName, PhoneNumber, GETUTCDATE()
FROM [LegacyDatabase].dbo.Student_LegacyTable
WHERE Email NOT IN (SELECT Email FROM Students);

-- Step 4: Migrate courses
INSERT INTO Courses (CourseName, Description, Capacity, CreatedDate)
SELECT CourseName, Description, Capacity, GETUTCDATE()
FROM [LegacyDatabase].dbo.Course_LegacyTable
WHERE CourseName NOT IN (SELECT CourseName FROM Courses);

-- Step 5: Re-enable foreign keys
EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';

-- Step 6: Verify data integrity
SELECT 'Students' AS TableName, COUNT(*) AS RecordCount FROM Students
UNION ALL
SELECT 'Courses' AS TableName, COUNT(*) AS RecordCount FROM Courses
UNION ALL
SELECT 'Instructors' AS TableName, COUNT(*) AS RecordCount FROM Instructors
UNION ALL
SELECT 'Enrollments' AS TableName, COUNT(*) AS RecordCount FROM Enrollments;
```

**Step 2.4: Deploy API Layer** (T+70, 40 min)

**Option 1: Blue-Green Deployment (Zero Downtime)**
```
Current State (Blue):
├─ Instance 1: Running old version
├─ Instance 2: Running old version
└─ Instance 3: Running old version

Deployment Steps:
1. Deploy new version to Instance 1 (isolated)
   └─ dotnet publish -c Release
   └─ copy to Instance 1 directory
   
2. Health check Instance 1
   └─ curl https://instance1:5001/health
   └─ Verify new version responding
   
3. Move Instance 1 to load balancer (Green environment)
   └─ Load balancer adds Instance 1 to rotation
   
4. Test requests to Instance 1
   └─ Verify requests working
   └─ Verify database access working
   
5. Repeat steps 1-4 for Instance 2, then Instance 3
   
6. Old instances (Blue) removed from rotation
   └─ Keep running as backup (30 min)
   └─ Then shut down if all green

Result: Zero downtime, all users served continuously
```

**Step 2.5: Deploy Supporting Services** (T+110, 20 min)
```
Action: Start background services
├─ [ ] Start Hangfire service
│   └─ Verify dashboard accessible
│   └─ Verify pending jobs processing
├─ [ ] Start cache service (Redis)
│   └─ Verify connectivity
│   └─ Warm cache with key data
└─ [ ] Start notification services
    └─ Verify email/SMS delivery
    └─ Process pending notifications
```

**Step 2.6: Disable Maintenance Mode** (T+130, 10 min)
```csharp
// In appsettings.json
"MaintenanceMode": {
  "Enabled": false
}

// Load balancer returns 200 OK
// All requests routed normally
```

**Step 2.7: Smoke Tests** (T+140, 10 min)
```csharp
// Test critical paths
1. Student login
   └─ POST /api/auth/login
   └─ Verify JWT token received
   
2. Course retrieval
   └─ GET /api/courses
   └─ Verify 200 response with data
   
3. Enrollment
   └─ POST /api/enrollments
   └─ Verify student enrolled in course
   
4. Grade retrieval
   └─ GET /api/grades?studentId=1
   └─ Verify grades returned
   
5. Report generation
   └─ POST /api/reports/enrollment-report
   └─ Verify report generated
   
6. Database verification
   └─ SELECT COUNT(*) FROM Students;
   └─ Verify data accessible
```

---

### Phase 3: Post-Deployment Verification (T+2 hours)

**Step 3.1: Performance Validation** (15 min)
```
Metrics to verify:
├─ CPU usage: < 70%
├─ Memory usage: < 80%
├─ Database connection pool: < 20 connections
├─ Cache hit rate: > 70%
├─ API response time (p95): < 500ms
├─ Error rate: < 0.1%
└─ Active user connections: Growing normally

Action: Review monitoring dashboard
└─ Open Application Insights / DataDog
└─ Verify all metrics in normal ranges
```

**Step 3.2: Error Log Review** (15 min)
```
Action: Check error logs for issues
├─ [ ] Application logs (past 2 hours)
│   └─ grep "ERROR" logs.txt | head -20
│   └─ Verify errors are expected/known
├─ [ ] Database logs
│   └─ Check for connection errors
│   └─ Verify no data integrity issues
└─ [ ] Integration service logs
    └─ Verify external API calls working
    └─ Confirm payment processing OK
```

**Step 3.3: Sample User Testing** (15 min)
```
Action: Test as different user types
├─ Student user
│   ├─ Login
│   ├─ View courses
│   ├─ Enroll in course
│   └─ View grades
├─ Instructor user
│   ├─ Login
│   ├─ View student list
│   ├─ Assign grades
│   └─ View reports
└─ Admin user
    ├─ Login
    ├─ Create new student
    ├─ Create new course
    └─ View system reports
```

**Step 3.4: Communication** (10 min)
```
Action: Send post-deployment notification
To: All stakeholders
Subject: "LMS Production Deployment Successful ✅"
Message: """
Deployment completed successfully at 7:00 AM.
System is operational and performing normally.

Key Metrics:
- Uptime: 100%
- Response Time: 250ms (p95)
- Active Users: 500+
- Errors: 0 critical issues

Support team is actively monitoring.
Contact: support@example.com or 1-800-XXX-XXXX
"""
```

---

## 💾 Data Migration Detailed Procedure

### Pre-Migration Steps

**Step 1: Verify Legacy Data Quality**
```sql
-- Check for data issues in legacy system
SELECT COUNT(*) AS InvalidEmails
FROM [LegacyDatabase].dbo.Student_LegacyTable
WHERE Email IS NULL OR Email LIKE '% %' OR Email NOT LIKE '%@%';

-- Verify no duplicate emails
SELECT Email, COUNT(*) 
FROM [LegacyDatabase].dbo.Student_LegacyTable
GROUP BY Email
HAVING COUNT(*) > 1;

-- Check for orphaned records
SELECT *
FROM [LegacyDatabase].dbo.Enrollment_LegacyTable
WHERE StudentID NOT IN (
  SELECT StudentID FROM [LegacyDatabase].dbo.Student_LegacyTable
);
```

**Step 2: Create Migration Mapping**
```
Mapping Reference:
├─ Legacy Table → New Table
│   ├─ Student_LegacyTable → Students
│   ├─ Course_LegacyTable → Courses
│   ├─ Instructor_LegacyTable → Instructors
│   ├─ Enrollment_LegacyTable → Enrollments
│   ├─ Grade_LegacyTable → Grades
│   └─ Payment_LegacyTable → Payments
│
└─ Legacy Column → New Column Mapping
    ├─ StudentID → StudentId
    ├─ StudEmail → Email
    ├─ CourseName → CourseName
    └─ ...
```

### Migration Execution

**Step 3: Migrate Students** (15 min)
```sql
INSERT INTO Students (
    Email, 
    FirstName, 
    LastName, 
    PhoneNumber, 
    StatusId,
    CreatedDate,
    ModifiedDate
)
SELECT 
    LOWER(TRIM(StudEmail)) AS Email,
    TRIM(FirstName) AS FirstName,
    TRIM(LastName) AS LastName,
    TRIM(PhoneNumber) AS PhoneNumber,
    1 AS StatusId, -- Active
    GETUTCDATE() AS CreatedDate,
    GETUTCDATE() AS ModifiedDate
FROM [LegacyDatabase].dbo.Student_LegacyTable
WHERE StudEmail NOT IN (SELECT Email FROM Students)
  AND StudEmail IS NOT NULL;

-- Verify migration
SELECT 'Legacy Students' AS Source, COUNT(*) AS Count 
FROM [LegacyDatabase].dbo.Student_LegacyTable
UNION ALL
SELECT 'New Students' AS Source, COUNT(*) AS Count 
FROM Students;

-- Expected: Same counts
```

**Step 4: Migrate Courses** (10 min)
```sql
INSERT INTO Courses (
    CourseName,
    Description,
    Capacity,
    StartDate,
    EndDate,
    CreatedDate,
    ModifiedDate
)
SELECT 
    TRIM(CourseName) AS CourseName,
    TRIM(CourseDescription) AS Description,
    Capacity,
    StartDate,
    EndDate,
    GETUTCDATE() AS CreatedDate,
    GETUTCDATE() AS ModifiedDate
FROM [LegacyDatabase].dbo.Course_LegacyTable
WHERE CourseName NOT IN (SELECT CourseName FROM Courses)
  AND CourseName IS NOT NULL;
```

**Step 5: Migrate Instructors** (10 min)
```sql
INSERT INTO Instructors (
    Email,
    FirstName,
    LastName,
    PhoneNumber,
    Department,
    CreatedDate,
    ModifiedDate
)
SELECT 
    LOWER(TRIM(InstructorEmail)) AS Email,
    TRIM(FirstName) AS FirstName,
    TRIM(LastName) AS LastName,
    TRIM(PhoneNumber) AS PhoneNumber,
    TRIM(Department) AS Department,
    GETUTCDATE() AS CreatedDate,
    GETUTCDATE() AS ModifiedDate
FROM [LegacyDatabase].dbo.Instructor_LegacyTable
WHERE InstructorEmail NOT IN (SELECT Email FROM Instructors)
  AND InstructorEmail IS NOT NULL;
```

**Step 6: Migrate Enrollments** (15 min)
```sql
INSERT INTO Enrollments (
    StudentId,
    CourseId,
    EnrollmentDate,
    CreatedDate,
    ModifiedDate
)
SELECT 
    s.StudentId,
    c.CourseId,
    le.EnrollmentDate,
    GETUTCDATE() AS CreatedDate,
    GETUTCDATE() AS ModifiedDate
FROM [LegacyDatabase].dbo.Enrollment_LegacyTable le
INNER JOIN Students s ON s.Email = 
    (SELECT StudEmail FROM [LegacyDatabase].dbo.Student_LegacyTable 
     WHERE StudentID = le.StudentID)
INNER JOIN Courses c ON c.CourseName = 
    (SELECT CourseName FROM [LegacyDatabase].dbo.Course_LegacyTable 
     WHERE CourseID = le.CourseID)
WHERE NOT EXISTS (
    SELECT 1 FROM Enrollments e 
    WHERE e.StudentId = s.StudentId 
    AND e.CourseId = c.CourseId
);
```

**Step 7: Verify Data Integrity**
```sql
-- Verify all foreign keys intact
SELECT COUNT(*) AS InvalidEnrollments
FROM Enrollments e
WHERE NOT EXISTS (
    SELECT 1 FROM Students s WHERE s.StudentId = e.StudentId
)
OR NOT EXISTS (
    SELECT 1 FROM Courses c WHERE c.CourseId = e.CourseId
);

-- Should return 0

-- Verify all students have unique emails
SELECT Email, COUNT(*)
FROM Students
GROUP BY Email
HAVING COUNT(*) > 1;

-- Should return no rows

-- Verify record counts match (approximately)
SELECT 
    'Students' AS TableName, 
    COUNT(*) AS Count
FROM Students
UNION ALL
SELECT 'Courses', COUNT(*) FROM Courses
UNION ALL
SELECT 'Instructors', COUNT(*) FROM Instructors
UNION ALL
SELECT 'Enrollments', COUNT(*) FROM Enrollments;
```

### Post-Migration Steps

**Step 8: Run Analytics & Reports**
```sql
-- Verify key metrics
SELECT 
    'Total Students' AS Metric, 
    COUNT(*) AS Value
FROM Students
UNION ALL
SELECT 'Total Courses', COUNT(*) FROM Courses
UNION ALL
SELECT 'Total Instructors', COUNT(*) FROM Instructors
UNION ALL
SELECT 'Total Enrollments', COUNT(*) FROM Enrollments
UNION ALL
SELECT 'Avg Students per Course', 
    (SELECT AVG(StudentCount) FROM 
        (SELECT COUNT(*) AS StudentCount FROM Enrollments GROUP BY CourseId) t)
UNION ALL
SELECT 'Active Students', 
    COUNT(*) FROM Students WHERE StatusId = 1;
```

**Step 9: Backup Post-Migration Database**
```sql
-- Create backup of migrated data
BACKUP DATABASE [EnglishTrainingCenter]
TO DISK = '\\backup-server\prod\PostMigration_2026-03-01.bak'
WITH INIT, STATS = 10;

-- Backup completion notification
-- Email: PostMigration backup completed at HH:MM
```

---

## ⚡ Rollback Procedure (If Needed)

**When to Rollback**:
- Critical system failure (unrecoverable)
- Data integrity issues discovered
- Security vulnerability exploited
- Performance unacceptable

**Rollback Steps** (< 30 minutes):

```
Step 1: Declare Rollback (5 min)
├─ [ ] Notify all stakeholders
├─ [ ] Activate rollback team
└─ [ ] Stop all new transactions

Step 2: Stop Current Services (5 min)
├─ [ ] Stop API instances gracefully
├─ [ ] Stop background services
└─ [ ] Verify all services stopped

Step 3: Restore from Backup (10 min)
├─ [ ] Stop database
├─ [ ] Restore database from pre-deployment backup
│   └─ RESTORE DATABASE [EnglishTrainingCenter]
│       FROM DISK = '\\backup-server\prod\PreDeployment_2026-03-01.bak'
│       WITH REPLACE
└─ [ ] Verify restored database

Step 4: Restart Services (5 min)
├─ [ ] Start API instances (previous version)
├─ [ ] Start background services
└─ [ ] Verify health checks passing

Step 5: Verify System (5 min)
├─ [ ] Test critical paths
├─ [ ] Verify data integrity
└─ [ ] Confirm users can access system

Result: System restored to previous state
```

---

**Version**: 1.0  
**Last Updated**: January 28, 2026  
**Status**: Ready for Execution
