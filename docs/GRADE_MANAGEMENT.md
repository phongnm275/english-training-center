# Grade Management Module Documentation

## Overview

The Grade Management module provides comprehensive academic tracking and performance analysis for the English Training Center. It enables recording student grades, calculating GPA, generating academic reports, and analyzing student performance across courses.

### Key Features

- **Grade Recording**: Record student grades with letter grades (A/B/C/D/F) and numeric scores
- **GPA Calculation**: Automatic GPA calculation on 4.0 scale (A=4.0, B=3.0, C=2.0, D=1.0, F=0.0)
- **Academic Status**: Automatic assignment of academic status based on GPA
- **Report Card Generation**: Comprehensive student report cards with GPA, status, and all course grades
- **Performance Analytics**: Grade distribution, low performers, and top students analysis
- **Audit Trail**: Complete grade history with timestamps and modification tracking

---

## Architecture

### Service Layer (`IGradeService`)

The service interface defines 12 core methods for grade management:

#### Retrieval Operations
```csharp
// Get all grades with pagination
Task<PagedResult<GradeListDto>> GetAllGradesAsync(int pageNumber, int pageSize);

// Get single grade by ID
Task<GradeDetailDto> GetGradeByIdAsync(int id);

// Get all grades for a student
Task<IEnumerable<GradeListDto>> GetGradesByStudentAsync(int studentId);

// Get all grades for a course
Task<IEnumerable<GradeListDto>> GetGradesByCourseAsync(int courseId);

// Get grades for specific student-course combination
Task<IEnumerable<GradeListDto>> GetStudentCourseGradesAsync(int studentId, int courseId);
```

#### Modification Operations
```csharp
// Create new grade
Task<GradeDetailDto> CreateGradeAsync(CreateGradeRequest request);

// Update existing grade
Task<GradeDetailDto> UpdateGradeAsync(int id, UpdateGradeRequest request);

// Delete grade
Task<bool> DeleteGradeAsync(int id);
```

#### Calculation & Analytics
```csharp
// Calculate GPA for student (4.0 scale)
Task<decimal> CalculateStudentGPAAsync(int studentId);

// Get grade distribution for course
Task<GradeDistributionDto> GetCourseGradeDistributionAsync(int courseId);

// Get complete report card
Task<AcademicReportCardDto> GetStudentReportCardAsync(int studentId);

// Get students with low grades
Task<IEnumerable<LowGradeStudentDto>> GetLowGradeStudentsAsync(decimal minimumGrade);

// Get top performing students
Task<IEnumerable<TopStudentDto>> GetTopStudentsAsync(int count);

// Get course average grade
Task<decimal> GetCourseAverageGradeAsync(int courseId);
```

### GPA Calculation Logic

GPA is calculated on a 4.0 scale using the following conversion:

| Letter Grade | GPA Points | Grade Percentage |
|-------------|-----------|------------------|
| A | 4.0 | 90-100% |
| B | 3.0 | 80-89% |
| C | 2.0 | 70-79% |
| D | 1.0 | 60-69% |
| F | 0.0 | 0-59% |

**Formula**: GPA = Sum(Grade Points) / Number of Courses

**Academic Status Mapping**:
- **Excellent**: GPA ≥ 3.5
- **Good**: GPA ≥ 3.0
- **Satisfactory**: GPA ≥ 2.5
- **Fair**: GPA ≥ 2.0
- **Needs Improvement**: GPA < 2.0

### Data Models

#### Request DTOs

**CreateGradeRequest**
```csharp
public class CreateGradeRequest
{
    /// <summary>Student ID (Required)</summary>
    public int StudentId { get; set; }

    /// <summary>Course ID (Required)</summary>
    public int CourseId { get; set; }

    /// <summary>Letter grade (A/B/C/D/F, Required)</summary>
    public string Grade { get; set; }

    /// <summary>Numeric score 0-100 (Optional)</summary>
    public int? NumericScore { get; set; }

    /// <summary>Instructor comments (Optional, max 500 chars)</summary>
    public string Comments { get; set; }
}
```

**UpdateGradeRequest**
```csharp
public class UpdateGradeRequest
{
    /// <summary>Letter grade (A/B/C/D/F, Required)</summary>
    public string Grade { get; set; }

    /// <summary>Numeric score 0-100 (Optional)</summary>
    public int? NumericScore { get; set; }

    /// <summary>Instructor comments (Optional, max 500 chars)</summary>
    public string Comments { get; set; }
}
```

#### Response DTOs

**GradeListDto** - Compact grade information
```json
{
  "id": 1,
  "studentId": 1,
  "courseId": 1,
  "grade": "A",
  "numericScore": 95,
  "gradeDate": "2024-01-15T10:30:00"
}
```

**GradeDetailDto** - Complete grade with aggregations
```json
{
  "id": 1,
  "studentId": 1,
  "courseId": 1,
  "grade": "A",
  "numericScore": 95,
  "gradeDate": "2024-01-15T10:30:00",
  "studentName": "Nguyen Van A",
  "courseName": "English Business Communication",
  "courseCode": "ENG301",
  "comments": "Excellent performance",
  "createdAt": "2024-01-15T10:30:00",
  "updatedAt": "2024-01-15T10:30:00"
}
```

**GradeDistributionDto** - Grade statistics for a course
```json
{
  "totalGrades": 45,
  "aCount": 12,
  "bCount": 18,
  "cCount": 10,
  "dCount": 4,
  "fCount": 1,
  "averageGrade": 82.5
}
```

**AcademicReportCardDto** - Student's complete academic record
```json
{
  "studentId": 1,
  "name": "Nguyen Van A",
  "email": "student@example.com",
  "gpa": 3.75,
  "academicStatus": "Excellent",
  "courseGrades": [
    {
      "courseId": 1,
      "courseName": "English Business Communication",
      "courseCode": "ENG301",
      "grade": "A",
      "gradeDate": "2024-01-15T10:30:00"
    }
  ],
  "reportCardDate": "2024-01-15T10:30:00"
}
```

**LowGradeStudentDto** - Students with grades below threshold
```json
{
  "studentId": 1,
  "name": "Nguyen Van A",
  "lowGrades": [
    {
      "courseId": 1,
      "courseName": "English Business Communication",
      "grade": "D",
      "gradePercentage": 65,
      "gradeDate": "2024-01-15T10:30:00"
    }
  ]
}
```

**TopStudentDto** - High-performing students
```json
{
  "studentId": 1,
  "name": "Nguyen Van A",
  "gpa": 3.95
}
```

---

## API Endpoints

### Base URL
```
/api/v1/grades
```

### 1. Get All Grades (Paginated)

**Endpoint**: `GET /api/v1/grades`

**Authorization**: Required

**Parameters**:
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| pageNumber | int | 1 | Page number for pagination |
| pageSize | int | 10 | Records per page (max 50) |

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Retrieved 10 grades",
  "data": {
    "pageNumber": 1,
    "pageSize": 10,
    "totalRecords": 100,
    "totalPages": 10,
    "data": [
      {
        "id": 1,
        "studentId": 1,
        "courseId": 1,
        "grade": "A",
        "numericScore": 95,
        "gradeDate": "2024-01-15T10:30:00"
      }
    ]
  }
}
```

---

### 2. Get Grade by ID

**Endpoint**: `GET /api/v1/grades/{id}`

**Authorization**: Required

**Path Parameters**:
| Parameter | Type | Description |
|-----------|------|-------------|
| id | int | Grade ID |

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Grade retrieved successfully",
  "data": {
    "id": 1,
    "studentId": 1,
    "courseId": 1,
    "grade": "A",
    "numericScore": 95,
    "gradeDate": "2024-01-15T10:30:00",
    "studentName": "Nguyen Van A",
    "courseName": "English Business Communication",
    "courseCode": "ENG301",
    "comments": "Excellent performance",
    "createdAt": "2024-01-15T10:30:00",
    "updatedAt": "2024-01-15T10:30:00"
  }
}
```

**Error** (404 Not Found):
```json
{
  "success": false,
  "message": "Grade not found",
  "data": "Grade with ID 999 not found"
}
```

---

### 3. Create Grade

**Endpoint**: `POST /api/v1/grades`

**Authorization**: Required

**Request Body**:
```json
{
  "studentId": 1,
  "courseId": 1,
  "grade": "A",
  "numericScore": 95,
  "comments": "Excellent performance in the course"
}
```

**Validation Rules**:
- StudentId: Required, must be > 0
- CourseId: Required, must be > 0
- Grade: Required, must be A/B/C/D/F (case-insensitive)
- NumericScore: Optional, if provided must be 0-100
- Comments: Optional, max 500 characters
- Student must be enrolled in the course

**Response** (201 Created):
```json
{
  "success": true,
  "message": "Grade created successfully",
  "data": {
    "id": 1,
    "studentId": 1,
    "courseId": 1,
    "grade": "A",
    "numericScore": 95,
    "gradeDate": "2024-01-15T10:30:00",
    "studentName": "Nguyen Van A",
    "courseName": "English Business Communication",
    "courseCode": "ENG301",
    "comments": "Excellent performance",
    "createdAt": "2024-01-15T10:30:00",
    "updatedAt": "2024-01-15T10:30:00"
  }
}
```

**Error** (400 Bad Request - Not Enrolled):
```json
{
  "success": false,
  "message": "Invalid grade data",
  "data": "Student is not enrolled in this course"
}
```

---

### 4. Update Grade

**Endpoint**: `PUT /api/v1/grades/{id}`

**Authorization**: Required

**Path Parameters**:
| Parameter | Type | Description |
|-----------|------|-------------|
| id | int | Grade ID |

**Request Body**:
```json
{
  "grade": "B",
  "numericScore": 85,
  "comments": "Improved performance"
}
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Grade updated successfully",
  "data": {
    "id": 1,
    "studentId": 1,
    "courseId": 1,
    "grade": "B",
    "numericScore": 85,
    "gradeDate": "2024-01-15T10:30:00",
    "studentName": "Nguyen Van A",
    "courseName": "English Business Communication",
    "courseCode": "ENG301",
    "comments": "Improved performance",
    "createdAt": "2024-01-15T10:30:00",
    "updatedAt": "2024-01-15T11:00:00"
  }
}
```

---

### 5. Delete Grade (Admin Only)

**Endpoint**: `DELETE /api/v1/grades/{id}`

**Authorization**: Required + Admin Role

**Path Parameters**:
| Parameter | Type | Description |
|-----------|------|-------------|
| id | int | Grade ID |

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Grade deleted successfully",
  "data": true
}
```

**Error** (403 Forbidden - Non-Admin):
```json
{
  "success": false,
  "message": "Access denied. Admin role required."
}
```

---

### 6. Get Student's Grades

**Endpoint**: `GET /api/v1/grades/student/{studentId}`

**Authorization**: Required

**Path Parameters**:
| Parameter | Type | Description |
|-----------|------|-------------|
| studentId | int | Student ID |

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Found 5 grades for student 1",
  "data": [
    {
      "id": 1,
      "studentId": 1,
      "courseId": 1,
      "grade": "A",
      "numericScore": 95,
      "gradeDate": "2024-01-15T10:30:00"
    },
    {
      "id": 2,
      "studentId": 1,
      "courseId": 2,
      "grade": "B",
      "numericScore": 85,
      "gradeDate": "2024-01-20T14:15:00"
    }
  ]
}
```

---

### 7. Get Course Grades

**Endpoint**: `GET /api/v1/grades/course/{courseId}`

**Authorization**: Required

**Path Parameters**:
| Parameter | Type | Description |
|-----------|------|-------------|
| courseId | int | Course ID |

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Found 35 grades for course 1",
  "data": [
    {
      "id": 1,
      "studentId": 1,
      "courseId": 1,
      "grade": "A",
      "numericScore": 95,
      "gradeDate": "2024-01-15T10:30:00"
    }
  ]
}
```

---

### 8. Calculate Student GPA

**Endpoint**: `GET /api/v1/grades/student/{studentId}/gpa`

**Authorization**: Required

**Path Parameters**:
| Parameter | Type | Description |
|-----------|------|-------------|
| studentId | int | Student ID |

**Response** (200 OK):
```json
{
  "success": true,
  "message": "GPA calculated successfully",
  "data": 3.75
}
```

---

### 9. Get Grade Distribution

**Endpoint**: `GET /api/v1/grades/course/{courseId}/distribution`

**Authorization**: Required

**Path Parameters**:
| Parameter | Type | Description |
|-----------|------|-------------|
| courseId | int | Course ID |

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Grade distribution retrieved successfully",
  "data": {
    "totalGrades": 45,
    "aCount": 12,
    "bCount": 18,
    "cCount": 10,
    "dCount": 4,
    "fCount": 1,
    "averageGrade": 82.5
  }
}
```

---

### 10. Get Academic Report Card

**Endpoint**: `GET /api/v1/grades/student/{studentId}/report-card`

**Authorization**: Required

**Path Parameters**:
| Parameter | Type | Description |
|-----------|------|-------------|
| studentId | int | Student ID |

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Report card generated successfully",
  "data": {
    "studentId": 1,
    "name": "Nguyen Van A",
    "email": "student@example.com",
    "gpa": 3.75,
    "academicStatus": "Excellent",
    "courseGrades": [
      {
        "courseId": 1,
        "courseName": "English Business Communication",
        "courseCode": "ENG301",
        "grade": "A",
        "gradeDate": "2024-01-15T10:30:00"
      },
      {
        "courseId": 2,
        "courseName": "Advanced English Grammar",
        "courseCode": "ENG302",
        "grade": "A",
        "gradeDate": "2024-01-20T14:15:00"
      }
    ],
    "reportCardDate": "2024-01-25T10:30:00"
  }
}
```

---

### 11. Get Low Grade Students (Admin Only)

**Endpoint**: `GET /api/v1/grades/low-grades?minimumGrade=60`

**Authorization**: Required + Admin Role

**Query Parameters**:
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| minimumGrade | decimal | 60 | Grade threshold (0-100) |

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Found 3 students with grades below 60",
  "data": [
    {
      "studentId": 5,
      "name": "Tran Van B",
      "lowGrades": [
        {
          "courseId": 1,
          "courseName": "English Business Communication",
          "grade": "D",
          "gradePercentage": 55,
          "gradeDate": "2024-01-15T10:30:00"
        }
      ]
    }
  ]
}
```

---

### 12. Get Top Students (Admin Only)

**Endpoint**: `GET /api/v1/grades/top-students?count=10`

**Authorization**: Required + Admin Role

**Query Parameters**:
| Parameter | Type | Default | Range | Description |
|-----------|------|---------|-------|-------------|
| count | int | 10 | 1-100 | Number of top students |

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Retrieved top 10 students",
  "data": [
    {
      "studentId": 1,
      "name": "Nguyen Van A",
      "gpa": 3.95
    },
    {
      "studentId": 3,
      "name": "Le Thi C",
      "gpa": 3.85
    }
  ]
}
```

---

### 13. Get Course Average Grade

**Endpoint**: `GET /api/v1/grades/course/{courseId}/average`

**Authorization**: Required

**Path Parameters**:
| Parameter | Type | Description |
|-----------|------|-------------|
| courseId | int | Course ID |

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Course average grade retrieved",
  "data": 82.5
}
```

---

## Validation Rules

### Grade Field Validation
- **Length**: Required
- **Pattern**: Must match A/B/C/D/F (case-insensitive)
- **Example**: Grade "A" or "a" are both valid

### Numeric Score Validation
- **Type**: Integer
- **Range**: 0-100 (if provided)
- **Optional**: Can be null
- **Example**: 95, 80, 0 are valid

### Comments Field Validation
- **Type**: String
- **MaxLength**: 500 characters
- **Optional**: Can be null
- **Example**: "Excellent performance in assignments"

### StudentId & CourseId Validation
- **Required**: Must be provided
- **Range**: Must be > 0
- **Existence**: Student and course must exist in database
- **Enrollment**: Student must be enrolled in course

---

## Error Handling

### Common Error Responses

**Validation Error** (400 Bad Request):
```json
{
  "success": false,
  "message": "Validation failed",
  "data": [
    {
      "field": "Grade",
      "message": "Grade must be A, B, C, D, or F"
    }
  ]
}
```

**Not Found Error** (404 Not Found):
```json
{
  "success": false,
  "message": "Grade not found",
  "data": "Grade with ID 999 does not exist"
}
```

**Unauthorized Error** (401 Unauthorized):
```json
{
  "success": false,
  "message": "Unauthorized",
  "data": "Authentication token is missing or invalid"
}
```

**Forbidden Error** (403 Forbidden):
```json
{
  "success": false,
  "message": "Access denied",
  "data": "Admin role is required for this operation"
}
```

**Enrollment Error** (400 Bad Request):
```json
{
  "success": false,
  "message": "Invalid grade data",
  "data": "Student is not enrolled in this course"
}
```

---

## Integration with Other Modules

### Student Module Integration
- Grade service queries Student repository for student names and details
- Student report cards include personal information (name, email)
- Low performer alerts can integrate with student support systems

### Course Module Integration
- Grade service queries Course repository for course names and codes
- Course grade distribution provides analytics on course difficulty
- Course average grades help identify challenging courses

### StudentCourse Module Integration
- Grade creation verifies student enrollment through StudentCourse table
- Prevents orphaned grades for students not enrolled in courses
- Required for maintaining referential integrity

---

## Testing Guide

### Prerequisites
- Valid JWT authentication token with User role
- Admin token for admin-only operations (delete, low-grades, top-students)
- Existing students and courses in database

### Test Scenarios

#### 1. Create Grade
```bash
POST /api/v1/grades
Headers:
  Authorization: Bearer {token}
  Content-Type: application/json

Body:
{
  "studentId": 1,
  "courseId": 1,
  "grade": "A",
  "numericScore": 95,
  "comments": "Excellent performance"
}

Expected: 201 Created with grade details
```

#### 2. Calculate GPA
```bash
GET /api/v1/grades/student/1/gpa
Headers:
  Authorization: Bearer {token}

Expected: 200 OK with GPA value (e.g., 3.75)
```

#### 3. Get Report Card
```bash
GET /api/v1/grades/student/1/report-card
Headers:
  Authorization: Bearer {token}

Expected: 200 OK with student report card including GPA, status, and all grades
```

#### 4. View Low Performers
```bash
GET /api/v1/grades/low-grades?minimumGrade=70
Headers:
  Authorization: Bearer {token}
  X-Admin-Role: admin

Expected: 200 OK with students having grades below 70%
```

#### 5. Delete Grade (Admin)
```bash
DELETE /api/v1/grades/1
Headers:
  Authorization: Bearer {admin-token}

Expected: 200 OK with success = true
```

---

## Performance Considerations

### Database Indexing
- Index on GradeId (primary key)
- Index on StudentId for quick student grade lookups
- Index on CourseId for course grade analytics
- Composite index on (StudentId, CourseId) for enrollment verification

### Pagination
- Default page size: 10 records
- Maximum page size: 50 records
- Prevents large data transfers

### Caching Opportunities
- GPA calculations can be cached per student
- Grade distributions can be cached per course
- Cache invalidation on grade create/update/delete

### Query Optimization
- Use pagination for listing operations
- Use specific queries for single-record retrievals
- Group operations for analytics (e.g., grade distribution)

---

## Best Practices

1. **Grade Entry**
   - Always verify student enrollment before creating grades
   - Include comments for non-standard grades
   - Use numeric scores alongside letter grades for consistency

2. **GPA Management**
   - Review GPAs periodically for accuracy
   - Use GPA for academic standing decisions
   - Monitor students with declining GPAs

3. **Report Generation**
   - Generate report cards at semester end
   - Archive report cards for historical records
   - Use report cards for academic counseling

4. **Performance Monitoring**
   - Review low performers regularly
   - Identify at-risk students early
   - Provide targeted academic support

5. **Data Integrity**
   - Verify all grades before publishing
   - Maintain audit trail for grade changes
   - Use admin-only delete for corrections

---

## Troubleshooting

### Issue: "Student is not enrolled in this course"
**Cause**: StudentCourse record doesn't exist
**Solution**: Enroll student in course using StudentCourse module first

### Issue: "Grade not found"
**Cause**: Invalid grade ID provided
**Solution**: Verify grade exists and ID is correct

### Issue: GPA shows 0
**Cause**: Student has no grades or all grades are F
**Solution**: Add grades for student or check if grades were calculated

### Issue: Pagination returns empty
**Cause**: Page number exceeds total pages
**Solution**: Use page number 1 or adjust pageSize

### Issue: Authorization error on delete
**Cause**: User doesn't have Admin role
**Solution**: Use admin token or request admin assistance

---

## Future Enhancements

- Grade curve adjustments for equitable grading
- Attendance impact on grades
- Extra credit opportunities tracking
- Grade appeal workflow
- Transcript generation
- Grade history and change audit log
- Letter grade comment templates
- Automated low-performer notifications
- Parent/Guardian grade access
- Mobile app grade tracking

