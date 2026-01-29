# SQL Database Scripts - Hướng Dẫn Sử Dụng

## 📁 Cấu Trúc Thư Mục

```
database/
├── scripts/
│   ├── 01-create-tables.sql          # Tạo các bảng cơ sở dữ liệu
│   ├── 02-insert-sample-data.sql     # Chèn dữ liệu mẫu
│   └── 03-maintenance-scripts.sql    # Scripts bảo trì cơ sở dữ liệu
│
├── stored-procedures/
│   └── 01-stored-procedures.sql      # Các stored procedures
│
└── README.md                          # File này
```

## 🚀 Hướng Dẫn Thực Hiện

### Bước 1: Tạo Database & Tables

Mở **SQL Server Management Studio (SSMS)** hoặc **Azure Data Studio** và chạy script:

```sql
-- Kết nối đến server (ví dụ: localhost hoặc server name)
-- Chạy script 01-create-tables.sql
```

**Kết quả**: 
- Database `EnglishTrainingCenter` được tạo
- 16 bảng chính được tạo
- Indexes và constraints được thiết lập

### Bước 2: Chèn Dữ Liệu Mẫu

Sau khi tables được tạo, chạy script:

```sql
-- Chạy script 02-insert-sample-data.sql
```

**Dữ liệu mẫu bao gồm**:
- 5 Roles (Admin, Manager, Instructor, Student, Finance)
- 11 Users
- 5 Students
- 3 Instructors với qualifications
- 5 Courses
- 5 Classes với schedules
- 5 Rooms
- 9 Student Enrollments
- 7 Invoices
- 2 Payments
- 5 Assignments với submissions
- 5 Exams
- 5 Grades

### Bước 3: Tạo Stored Procedures

Chạy script để tạo các stored procedures hữu ích:

```sql
-- Chạy script stored-procedures/01-stored-procedures.sql
```

## 📊 Các Bảng Chính

| Bảng | Mô Tả | Records |
|------|-------|---------|
| **Users** | User accounts & roles | 11 |
| **Students** | Student information | 5 |
| **Instructors** | Instructor information | 3 |
| **Courses** | Course catalog | 5 |
| **Classes** | Class instances | 5 |
| **StudentCourses** | Student enrollments | 9 |
| **Invoices** | Student invoices | 7 |
| **Payments** | Payment records | 2 |
| **Assignments** | Course assignments | 5 |
| **Exams** | Course exams | 5 |
| **Grades** | Student grades | 5 |

## 🔍 Các Stored Procedures Chính

### 1. sp_GetStudentSummary
Lấy thông tin tổng hợp học viên:
```sql
EXEC sp_GetStudentSummary @StudentId = 1;
```

**Kết quả**:
- Tên & thông tin liên lạc
- Số khóa học đã đăng ký
- Điểm trung bình
- Thống kê thanh toán

### 2. sp_GetClassAttendance
Lấy thống kê lớp học:
```sql
EXEC sp_GetClassAttendance @ClassId = 1;
```

**Kết quả**:
- Tổng số học viên
- Học viên đang học
- Học viên hoàn thành
- Học viên đã rút

### 3. sp_GetInstructorSchedule
Lấy lịch dạy của giáo viên:
```sql
EXEC sp_GetInstructorSchedule @InstructorId = 1;
```

**Kết quả**:
- Các lớp dạy
- Thời gian dạy (ngày, giờ)
- Phòng học
- Số lượng học viên

### 4. sp_GetRevenueReport
Lấy báo cáo doanh thu:
```sql
EXEC sp_GetRevenueReport 
    @StartDate = '2025-01-01',
    @EndDate = '2025-12-31';
```

**Kết quả**:
- Doanh thu theo ngày
- Số giao dịch
- Phương thức thanh toán
- Tóm tắt doanh thu

### 5. sp_GetStudentGrades
Lấy điểm số học viên:
```sql
EXEC sp_GetStudentGrades @StudentId = 1;
```

**Kết quả**:
- Các kì thi
- Điểm số
- Nhận xét
- Trạng thái vượt/rớt

### 6. sp_GetEnrolledStudentsByClass
Lấy danh sách học viên trong lớp:
```sql
EXEC sp_GetEnrolledStudentsByClass @ClassId = 1;
```

**Kết quả**:
- Thông tin học viên
- Ngày đăng ký
- GPA
- Điểm trung bình

### 7. sp_GetCourseStatistics
Lấy thống kê khóa học:
```sql
EXEC sp_GetCourseStatistics @CourseId = 1;
```

**Kết quả**:
- Số lớp
- Số học viên
- Điểm trung bình
- Doanh thu

### 8. sp_GetOverdueInvoices
Lấy danh sách hóa đơn quá hạn:
```sql
EXEC sp_GetOverdueInvoices @DaysOverdue = 0;
```

### 9. sp_CalculateStudentGPA
Tính GPA học viên:
```sql
EXEC sp_CalculateStudentGPA 
    @StudentId = 1,
    @ClassId = 1;
```

### 10. sp_GetOutstandingPayments
Lấy danh sách thanh toán chưa hoàn:
```sql
EXEC sp_GetOutstandingPayments;
```

## 🔧 Maintenance Scripts

### Backup Database
```sql
EXEC sp_BackupDatabase
    @BackupPath = 'C:\Backups\',
    @DatabaseName = 'EnglishTrainingCenter';
```

### Rebuild Indexes
```sql
EXEC sp_RebuildIndexes
    @DatabaseName = 'EnglishTrainingCenter',
    @FragmentationThreshold = 10;
```

### Update Statistics
```sql
EXEC sp_UpdateStatistics
    @DatabaseName = 'EnglishTrainingCenter';
```

### Check Database Integrity
```sql
EXEC sp_CheckDatabaseIntegrity
    @DatabaseName = 'EnglishTrainingCenter';
```

## 📋 Queries Hữu Ích

### Xem tất cả học viên
```sql
SELECT * FROM Students WHERE IsActive = 1
ORDER BY FullName;
```

### Xem danh sách khóa học
```sql
SELECT 
    CourseId,
    CourseName,
    Level,
    Duration,
    Fee
FROM Courses
WHERE IsActive = 1
ORDER BY Level;
```

### Xem lịch học hôm nay
```sql
SELECT 
    c.ClassName,
    sc.DayOfWeek,
    sc.StartTime,
    sc.EndTime,
    r.RoomNumber,
    i.FullName AS InstructorName
FROM Schedules sc
JOIN Classes c ON sc.ClassId = c.ClassId
JOIN Rooms r ON sc.RoomId = r.RoomId
JOIN Instructors i ON c.InstructorId = i.InstructorId
WHERE sc.DayOfWeek = DATEPART(WEEKDAY, GETDATE()) - 1
ORDER BY sc.StartTime;
```

### Xem doanh thu hôm nay
```sql
SELECT 
    COUNT(*) AS TransactionCount,
    SUM(Amount) AS TotalRevenue,
    PaymentMethod
FROM Payments
WHERE CAST(PaymentDate AS DATE) = CAST(GETDATE() AS DATE)
AND Status = 'Completed'
GROUP BY PaymentMethod;
```

### Xem học viên quá hạn thanh toán
```sql
SELECT 
    s.StudentId,
    s.FullName,
    s.Email,
    i.InvoiceId,
    i.Amount,
    i.DueDate,
    DATEDIFF(DAY, i.DueDate, GETDATE()) AS DaysOverdue
FROM Students s
JOIN Invoices i ON s.StudentId = i.StudentId
WHERE i.Status IN ('Issued', 'Overdue')
AND i.DueDate < GETDATE()
ORDER BY i.DueDate;
```

## ⚙️ Connection String

### SQL Server (Local)
```
Server=localhost;Database=EnglishTrainingCenter;Trusted_Connection=true;
```

### SQL Server (Express)
```
Server=localhost\SQLEXPRESS;Database=EnglishTrainingCenter;Trusted_Connection=true;
```

### SQL Server (with Username/Password)
```
Server=YOUR_SERVER;Database=EnglishTrainingCenter;User Id=YOUR_USER;Password=YOUR_PASSWORD;
```

### Azure SQL Database
```
Server=tcp:servername.database.windows.net,1433;Initial Catalog=EnglishTrainingCenter;Persist Security Info=False;User ID=username@servername;Password=password;Encrypt=True;Connection Timeout=30;
```

## 📝 Notes

- **Sample Data**: Dữ liệu mẫu được tạo để test, có thể xóa và thêm dữ liệu thực tế
- **Passwords**: Mật khẩu trong dữ liệu mẫu là hash placeholders, cần replace bằng bcrypt/Argon2 hashes thực tế
- **Indexes**: Các index được tạo tự động trên foreign keys và frequently queried columns
- **Constraints**: Check constraints được tạo để đảm bảo data integrity

## 🔐 Security

- Không để connection string trong code (sử dụng environment variables)
- Sử dụng SQL parameterized queries để tránh SQL injection
- Encrypt password trước khi lưu
- Limit user permissions (không dùng sa account)
- Enable SQL Server authentication

## 📞 Tiếp Theo

Sau khi database được thiết lập, bạn có thể:
1. **Tạo .NET Core project** với EF Core DbContext
2. **Scaffold entities** từ database
3. **Tạo repositories** & services
4. **Build API endpoints**

---

**Version**: 1.0  
**Last Updated**: January 28, 2026
