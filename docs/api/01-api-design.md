# API Design - English Training Center Management System

## 1. API Overview

### 1.1 Base URL
```
https://api.englishtrainingcenter.com/api
```

### 1.2 API Versioning
```
https://api.englishtrainingcenter.com/api/v1/...
```

### 1.3 Response Format
```json
{
  "success": true,
  "statusCode": 200,
  "data": {},
  "message": "Operation successful",
  "errors": []
}
```

## 2. Authentication & Authorization

### 2.1 Login Endpoint
```
POST /v1/auth/login
Content-Type: application/json

Request:
{
  "username": "student1",
  "password": "password123"
}

Response (200):
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "...",
    "expiresIn": 3600,
    "user": {
      "userId": 1,
      "username": "student1",
      "email": "student@example.com",
      "roles": ["Student"]
    }
  }
}
```

### 2.2 Header Authentication
```
Authorization: Bearer <token>
```

### 2.3 Roles
- `Admin` - Toàn quyền
- `Manager` - Quản lý hoạt động
- `Instructor` - Dạy học, chấm điểm
- `Student` - Học viên
- `Finance` - Quản lý tài chính

## 3. Core API Endpoints

### 3.1 Student Management

#### 3.1.1 List Students
```
GET /v1/students?page=1&pageSize=20&search=john

Query Parameters:
- page: int (default: 1)
- pageSize: int (default: 20)
- search: string (optional)
- isActive: bool (optional)

Response (200):
{
  "success": true,
  "data": {
    "items": [
      {
        "studentId": 1,
        "fullName": "John Doe",
        "email": "john@example.com",
        "phone": "0123456789",
        "dateOfBirth": "2000-01-15",
        "enrollmentDate": "2025-01-01",
        "isActive": true
      }
    ],
    "totalCount": 100,
    "pageNumber": 1,
    "pageSize": 20
  }
}
```

#### 3.1.2 Get Student by ID
```
GET /v1/students/{studentId}

Response (200):
{
  "success": true,
  "data": {
    "studentId": 1,
    "fullName": "John Doe",
    "email": "john@example.com",
    "phone": "0123456789",
    "dateOfBirth": "2000-01-15",
    "address": "123 Main Street",
    "enrollmentDate": "2025-01-01",
    "enrolledCourses": [
      {
        "classId": 1,
        "className": "Beginner English A",
        "courseId": 1,
        "courseName": "Beginner English",
        "enrollDate": "2025-01-01",
        "status": "Ongoing",
        "gpa": 8.5
      }
    ],
    "isActive": true
  }
}
```

#### 3.1.3 Create Student
```
POST /v1/students
Authorization: Bearer <token>
Content-Type: application/json

Request:
{
  "fullName": "Jane Smith",
  "email": "jane@example.com",
  "phone": "0987654321",
  "dateOfBirth": "2000-05-20",
  "address": "456 Oak Avenue",
  "enrollmentDate": "2025-01-01"
}

Response (201):
{
  "success": true,
  "data": {
    "studentId": 2,
    "fullName": "Jane Smith",
    "email": "jane@example.com",
    ...
  }
}
```

#### 3.1.4 Update Student
```
PUT /v1/students/{studentId}
Authorization: Bearer <token>

Request:
{
  "fullName": "Jane Smith",
  "email": "jane.smith@example.com",
  "phone": "0987654321",
  "address": "789 New Street"
}

Response (200): Updated student data
```

#### 3.1.5 Delete Student
```
DELETE /v1/students/{studentId}
Authorization: Bearer <token>

Response (204): No Content
```

### 3.2 Course Management

#### 3.2.1 List Courses
```
GET /v1/courses?level=Beginner&page=1

Query Parameters:
- level: string (Beginner, Intermediate, Advanced)
- search: string
- page: int

Response (200):
{
  "success": true,
  "data": {
    "items": [
      {
        "courseId": 1,
        "courseName": "Beginner English",
        "description": "Basic English course...",
        "level": "Beginner",
        "duration": 40,
        "fee": 3000000,
        "isActive": true
      }
    ],
    "totalCount": 10
  }
}
```

#### 3.2.2 Get Course by ID
```
GET /v1/courses/{courseId}

Response (200):
{
  "success": true,
  "data": {
    "courseId": 1,
    "courseName": "Beginner English",
    "description": "...",
    "level": "Beginner",
    "duration": 40,
    "fee": 3000000,
    "classes": [
      {
        "classId": 1,
        "className": "Beginner English A",
        "instructorName": "Jane Doe",
        "maxStudents": 30,
        "enrolledCount": 28,
        "startDate": "2025-02-01",
        "endDate": "2025-04-01",
        "status": "Upcoming"
      }
    ]
  }
}
```

#### 3.2.3 Create Course
```
POST /v1/courses
Authorization: Bearer <token> (Manager/Admin)

Request:
{
  "courseName": "Intermediate English",
  "description": "Intermediate level course",
  "level": "Intermediate",
  "duration": 50,
  "fee": 4000000
}

Response (201): Created course
```

### 3.3 Class Management

#### 3.3.1 List Classes
```
GET /v1/classes?courseId=1&status=Ongoing

Response (200):
{
  "success": true,
  "data": {
    "items": [
      {
        "classId": 1,
        "className": "Beginner English A",
        "courseId": 1,
        "courseName": "Beginner English",
        "instructorId": 1,
        "instructorName": "Jane Doe",
        "maxStudents": 30,
        "enrolledCount": 28,
        "startDate": "2025-02-01",
        "endDate": "2025-04-01",
        "status": "Ongoing",
        "schedule": [
          {
            "dayOfWeek": "Monday",
            "startTime": "14:00",
            "endTime": "16:00",
            "room": "Room 101"
          }
        ]
      }
    ]
  }
}
```

#### 3.3.2 Enroll Student in Class
```
POST /v1/classes/{classId}/enroll
Authorization: Bearer <token>

Request:
{
  "studentId": 1
}

Response (200):
{
  "success": true,
  "data": {
    "studentCourseId": 101,
    "studentId": 1,
    "classId": 1,
    "enrollDate": "2025-01-28",
    "status": "Enrolled"
  }
}
```

### 3.4 Payment Management

#### 3.4.1 List Invoices
```
GET /v1/payments/invoices?studentId=1&status=Issued

Response (200):
{
  "success": true,
  "data": {
    "items": [
      {
        "invoiceId": 1,
        "studentId": 1,
        "studentName": "John Doe",
        "classId": 1,
        "className": "Beginner English A",
        "amount": 3000000,
        "issueDate": "2025-01-28",
        "dueDate": "2025-02-28",
        "status": "Issued",
        "notes": ""
      }
    ]
  }
}
```

#### 3.4.2 Create Payment
```
POST /v1/payments
Authorization: Bearer <token>

Request:
{
  "studentId": 1,
  "invoiceId": 1,
  "amount": 3000000,
  "paymentMethod": "Credit Card",
  "description": "Payment for Beginner English A"
}

Response (201):
{
  "success": true,
  "data": {
    "paymentId": 101,
    "studentId": 1,
    "amount": 3000000,
    "paymentDate": "2025-01-28",
    "paymentMethod": "Credit Card",
    "status": "Pending",
    "transactionId": "txn_1234567890"
  }
}
```

#### 3.4.3 Get Payment Status
```
GET /v1/payments/{paymentId}

Response (200):
{
  "success": true,
  "data": {
    "paymentId": 101,
    "studentId": 1,
    "amount": 3000000,
    "paymentDate": "2025-01-28",
    "paymentMethod": "Credit Card",
    "status": "Completed",
    "transactionId": "txn_1234567890"
  }
}
```

### 3.5 Evaluation & Grading

#### 3.5.1 List Assignments
```
GET /v1/assignments?classId=1

Response (200):
{
  "success": true,
  "data": [
    {
      "assignmentId": 1,
      "classId": 1,
      "title": "Unit 1 Exercise",
      "description": "Complete all exercises in Unit 1",
      "dueDate": "2025-02-05",
      "maxScore": 100,
      "createdDate": "2025-01-28"
    }
  ]
}
```

#### 3.5.2 Submit Assignment
```
POST /v1/assignments/{assignmentId}/submit
Authorization: Bearer <token>

Request:
{
  "studentId": 1,
  "content": "Assignment answer text",
  "fileUrl": "https://storage.com/assignment_file.pdf"
}

Response (201):
{
  "success": true,
  "data": {
    "submissionId": 1,
    "assignmentId": 1,
    "studentId": 1,
    "submissionDate": "2025-02-03",
    "isLate": false,
    "status": "Submitted"
  }
}
```

#### 3.5.3 Get Grades
```
GET /v1/students/{studentId}/grades?classId=1

Response (200):
{
  "success": true,
  "data": {
    "items": [
      {
        "gradeId": 1,
        "examId": 1,
        "examName": "Unit 1 Test",
        "score": 85,
        "gradeValue": "A",
        "feedback": "Good work!",
        "gradedDate": "2025-02-10"
      }
    ],
    "averageScore": 85,
    "classId": 1
  }
}
```

#### 3.5.4 Submit Grade
```
POST /v1/grades
Authorization: Bearer <token> (Instructor/Admin)

Request:
{
  "studentId": 1,
  "examId": 1,
  "score": 85,
  "feedback": "Good work!"
}

Response (201):
{
  "success": true,
  "data": {
    "gradeId": 1,
    "studentId": 1,
    "examId": 1,
    "score": 85,
    "gradeValue": "A",
    "feedback": "Good work!",
    "gradedDate": "2025-01-28"
  }
}
```

### 3.6 Instructor Management

#### 3.6.1 List Instructors
```
GET /v1/instructors?page=1

Response (200):
{
  "success": true,
  "data": {
    "items": [
      {
        "instructorId": 1,
        "fullName": "Jane Doe",
        "email": "jane@example.com",
        "specialization": "Communication",
        "yearsExperience": 5,
        "qualifications": [
          {
            "certification": "TOEFL",
            "issuer": "ETS",
            "certificationDate": "2020-06-15"
          }
        ],
        "isActive": true
      }
    ]
  }
}
```

### 3.7 Reports & Analytics

#### 3.7.1 Revenue Report
```
GET /v1/reports/revenue?startDate=2025-01-01&endDate=2025-12-31

Response (200):
{
  "success": true,
  "data": {
    "totalRevenue": 150000000,
    "completedPayments": 50,
    "pendingPayments": 5,
    "monthlyRevenue": [
      {
        "month": "January",
        "amount": 5000000
      }
    ]
  }
}
```

#### 3.7.2 Student Statistics
```
GET /v1/reports/students?courseId=1

Response (200):
{
  "success": true,
  "data": {
    "totalStudents": 100,
    "activeStudents": 85,
    "completedStudents": 15,
    "averageGPA": 7.8,
    "enrollmentTrend": [...]
  }
}
```

## 4. Error Responses

### 4.1 Common Error Codes

| Status | Code | Message |
|--------|------|---------|
| 400 | BAD_REQUEST | Invalid input data |
| 401 | UNAUTHORIZED | Authentication required |
| 403 | FORBIDDEN | Access denied |
| 404 | NOT_FOUND | Resource not found |
| 409 | CONFLICT | Duplicate entry |
| 500 | INTERNAL_ERROR | Server error |

### 4.2 Error Response Format
```json
{
  "success": false,
  "statusCode": 400,
  "message": "Invalid input",
  "errors": [
    {
      "field": "email",
      "message": "Email is required"
    }
  ]
}
```

## 5. Rate Limiting

```
X-RateLimit-Limit: 1000
X-RateLimit-Remaining: 999
X-RateLimit-Reset: 1635789600
```

## 6. Pagination

```json
{
  "pageNumber": 1,
  "pageSize": 20,
  "totalCount": 100,
  "totalPages": 5
}
```

## 7. Sorting

```
GET /v1/students?sortBy=fullName&sortOrder=asc
```

---

**Version**: 1.0  
**Last Updated**: January 28, 2026
