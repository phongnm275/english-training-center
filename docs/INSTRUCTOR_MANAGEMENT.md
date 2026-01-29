# Instructor Management Module Documentation

## Overview

The Instructor Management module provides comprehensive instructor lifecycle management, including recruitment, course assignment, salary administration, and performance evaluation. It enables the center to efficiently manage instructor resources and track professional development.

### Key Features

- **Instructor Records**: Complete profile management with qualifications and experience
- **Course Assignments**: Assign and manage instructor-course relationships
- **Salary Management**: Base salary configuration and calculated salary with bonuses
- **Performance Evaluation**: Rating system with performance tracking
- **Search & Filtering**: Find instructors by name, email, qualification, or status
- **Analytics**: Instructor statistics with course load and compensation data
- **Status Management**: Track active/inactive instructor status

---

## Architecture

### Service Layer (`IInstructorService`)

The service interface defines 17 core methods for instructor management:

#### Retrieval Operations
```csharp
// Get all instructors with pagination
Task<PagedResult<InstructorListDto>> GetAllInstructorsAsync(int pageNumber, int pageSize);

// Get single instructor by ID with aggregations
Task<InstructorDetailDto> GetInstructorByIdAsync(int id);

// Search instructors by name or email
Task<IEnumerable<InstructorListDto>> SearchInstructorsAsync(string searchTerm);

// Get instructors by qualification level
Task<IEnumerable<InstructorListDto>> GetInstructorsByQualificationAsync(string qualification);

// Get instructors by status (Active/Inactive)
Task<IEnumerable<InstructorListDto>> GetInstructorsByStatusAsync(bool isActive);
```

#### Modification Operations
```csharp
// Create new instructor
Task<InstructorDetailDto> CreateInstructorAsync(CreateInstructorRequest request);

// Update existing instructor
Task<InstructorDetailDto> UpdateInstructorAsync(int id, UpdateInstructorRequest request);

// Delete instructor
Task<bool> DeleteInstructorAsync(int id);

// Update instructor status
Task<InstructorDetailDto> UpdateInstructorStatusAsync(int instructorId, bool isActive);

// Update instructor salary
Task<InstructorDetailDto> UpdateSalaryAsync(int instructorId, decimal baseSalary, string salaryFrequency);
```

#### Course Assignment Operations
```csharp
// Assign course to instructor
Task<InstructorAssignmentDto> AssignCourseAsync(int instructorId, int courseId);

// Remove course assignment
Task<bool> RemoveCourseAssignmentAsync(int instructorId, int courseId);

// Get courses assigned to instructor
Task<IEnumerable<InstructorAssignmentDto>> GetInstructorCoursesAsync(int instructorId);

// Get instructors for a course
Task<IEnumerable<InstructorListDto>> GetCourseInstructorsAsync(int courseId);
```

#### Salary & Performance Operations
```csharp
// Calculate salary based on experience and courses
Task<decimal> CalculateSalaryAsync(int instructorId);

// Add performance rating
Task<InstructorPerformanceDto> AddPerformanceRatingAsync(int instructorId, int rating, string comments);

// Get average performance rating
Task<decimal> GetAveragePerformanceRatingAsync(int instructorId);

// Get instructor statistics
Task<InstructorStatisticsDto> GetInstructorStatisticsAsync(int instructorId);

// Get top-rated instructors
Task<IEnumerable<InstructorPerformanceDto>> GetTopRatedInstructorsAsync(int count);
```

### Salary Calculation Logic

Instructor salary is calculated dynamically based on experience and course assignments:

**Formula**:
```
Calculated Salary = Base Salary × (1 + Course Bonus + Experience Bonus)

Where:
- Course Bonus = Min(Number of Courses × 5%, 50%)    [5% per course, capped at 50%]
- Experience Bonus = Years of Experience × 2%         [2% per year]
```

**Example**:
- Base Salary: $2,000
- 3 Assigned Courses: 3 × 5% = 15% bonus
- 5 Years Experience: 5 × 2% = 10% bonus
- **Calculated Salary**: $2,000 × (1 + 0.15 + 0.10) = $2,500

### Performance Rating System

| Rating | Level | Academic Status |
|--------|-------|-----------------|
| 5 | Excellent | Outstanding instructor, exemplary performance |
| 4 | Good | Above average, strong performance |
| 3 | Satisfactory | Meets expectations, adequate performance |
| 2 | Fair | Below average, needs improvement |
| 1 | Poor | Unsatisfactory, requires corrective action |

### Data Models

#### Request DTOs

**CreateInstructorRequest**
```csharp
public class CreateInstructorRequest
{
    public string FirstName { get; set; }              // Required, 2-50 chars
    public string LastName { get; set; }               // Required, 2-50 chars
    public string Email { get; set; }                  // Required, unique, valid format
    public string PhoneNumber { get; set; }            // Optional, valid format
    public string Specialization { get; set; }         // Required, 3-100 chars
    public string Qualification { get; set; }          // Required: High School, Bachelor, Master, PhD
    public int YearsOfExperience { get; set; }         // Required, 0-70
    public decimal BaseSalary { get; set; }            // Required, > 0
    public string SalaryFrequency { get; set; }        // Required: Monthly, Quarterly, Yearly
}
```

**UpdateInstructorRequest**
```csharp
public class UpdateInstructorRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Specialization { get; set; }
    public string Qualification { get; set; }
    public int YearsOfExperience { get; set; }
    public decimal BaseSalary { get; set; }
    public string SalaryFrequency { get; set; }
}
```

#### Response DTOs

**InstructorListDto** - Compact instructor information
```json
{
  "id": 1,
  "fullName": "Nguyen Thi Hoa",
  "email": "hoa@example.com",
  "phoneNumber": "+84912345678",
  "specialization": "English Communication",
  "qualification": "Master",
  "yearsOfExperience": 5,
  "isActive": true
}
```

**InstructorDetailDto** - Complete instructor profile
```json
{
  "id": 1,
  "firstName": "Nguyen",
  "lastName": "Hoa",
  "fullName": "Nguyen Thi Hoa",
  "email": "hoa@example.com",
  "phoneNumber": "+84912345678",
  "specialization": "English Communication",
  "qualification": "Master",
  "yearsOfExperience": 5,
  "baseSalary": 2000,
  "salaryFrequency": "Monthly",
  "assignedCourseCount": 3,
  "averagePerformanceRating": 4.5,
  "joinDate": "2020-01-15T00:00:00Z",
  "isActive": true,
  "createdAt": "2020-01-15T10:30:00Z",
  "updatedAt": "2024-01-15T14:20:00Z"
}
```

**InstructorAssignmentDto** - Course assignment details
```json
{
  "instructorId": 1,
  "instructorName": "Nguyen Thi Hoa",
  "courseId": 1,
  "courseName": "English Business Communication",
  "courseCode": "ENG301",
  "assignmentDate": "2024-01-10T10:30:00Z"
}
```

**InstructorPerformanceDto** - Performance evaluation
```json
{
  "instructorId": 1,
  "instructorName": "Nguyen Thi Hoa",
  "rating": 5,
  "comments": "Excellent classroom management and student engagement",
  "ratingDate": "2024-01-15T10:30:00Z"
}
```

**InstructorStatisticsDto** - Analytics and metrics
```json
{
  "instructorId": 1,
  "instructorName": "Nguyen Thi Hoa",
  "totalCoursesAssigned": 3,
  "yearsOfExperience": 5,
  "baseSalary": 2000,
  "calculatedSalary": 2500,
  "averagePerformanceRating": 4.5,
  "joinDate": "2020-01-15T00:00:00Z",
  "isActive": true
}
```

---

## API Endpoints

### Base URL
```
/api/v1/instructors
```

### 1. Get All Instructors (Paginated)

**Endpoint**: `GET /api/v1/instructors`

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
  "message": "Retrieved 10 instructors",
  "data": {
    "pageNumber": 1,
    "pageSize": 10,
    "totalRecords": 25,
    "totalPages": 3,
    "data": [
      {
        "id": 1,
        "fullName": "Nguyen Thi Hoa",
        "email": "hoa@example.com",
        "specialization": "English Communication",
        "qualification": "Master",
        "yearsOfExperience": 5,
        "isActive": true
      }
    ]
  }
}
```

---

### 2. Get Instructor by ID

**Endpoint**: `GET /api/v1/instructors/{id}`

**Authorization**: Required

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Instructor retrieved successfully",
  "data": {
    "id": 1,
    "firstName": "Nguyen",
    "lastName": "Hoa",
    "fullName": "Nguyen Thi Hoa",
    "email": "hoa@example.com",
    "specialization": "English Communication",
    "qualification": "Master",
    "yearsOfExperience": 5,
    "baseSalary": 2000,
    "assignedCourseCount": 3,
    "averagePerformanceRating": 4.5,
    "isActive": true,
    "joinDate": "2020-01-15T00:00:00Z"
  }
}
```

---

### 3. Create Instructor

**Endpoint**: `POST /api/v1/instructors`

**Authorization**: Required

**Request Body**:
```json
{
  "firstName": "Nguyen",
  "lastName": "Hoa",
  "email": "hoa@example.com",
  "phoneNumber": "+84912345678",
  "specialization": "English Communication",
  "qualification": "Master",
  "yearsOfExperience": 5,
  "baseSalary": 2000,
  "salaryFrequency": "Monthly"
}
```

**Validation Rules**:
- FirstName/LastName: Required, 2-50 chars
- Email: Required, unique, valid format
- PhoneNumber: Optional, valid format (+XX format recommended)
- Specialization: Required, 3-100 chars
- Qualification: Required, one of (High School, Bachelor, Master, PhD)
- YearsOfExperience: Required, 0-70 range
- BaseSalary: Required, > 0, max 10,000,000
- SalaryFrequency: Required, one of (Monthly, Quarterly, Yearly)

**Response** (201 Created):
```json
{
  "success": true,
  "message": "Instructor created successfully",
  "data": { /* InstructorDetailDto */ }
}
```

**Error** (400 Bad Request - Email Exists):
```json
{
  "success": false,
  "message": "Invalid instructor data",
  "data": "Instructor with email hoa@example.com already exists"
}
```

---

### 4. Update Instructor

**Endpoint**: `PUT /api/v1/instructors/{id}`

**Authorization**: Required

**Request Body**: Same as Create

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Instructor updated successfully",
  "data": { /* InstructorDetailDto */ }
}
```

---

### 5. Delete Instructor (Admin Only)

**Endpoint**: `DELETE /api/v1/instructors/{id}`

**Authorization**: Required + Admin Role

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Instructor deleted successfully",
  "data": true
}
```

---

### 6. Search Instructors

**Endpoint**: `GET /api/v1/instructors/search?searchTerm=nguyen`

**Authorization**: Required

**Query Parameters**:
| Parameter | Type | Description |
|-----------|------|-------------|
| searchTerm | string | Search by name or email |

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Found 2 instructors matching 'nguyen'",
  "data": [ /* InstructorListDto array */ ]
}
```

---

### 7. Get by Qualification

**Endpoint**: `GET /api/v1/instructors/by-qualification?qualification=Master`

**Authorization**: Required

**Valid Qualifications**: High School, Bachelor, Master, PhD

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Found 8 instructors with Master qualification",
  "data": [ /* InstructorListDto array */ ]
}
```

---

### 8. Get by Status

**Endpoint**: `GET /api/v1/instructors/by-status?isActive=true`

**Authorization**: Required

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Found 20 active instructors",
  "data": [ /* InstructorListDto array */ ]
}
```

---

### 9. Update Instructor Status

**Endpoint**: `PATCH /api/v1/instructors/{id}/status?isActive=false`

**Authorization**: Required

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Instructor status updated to Inactive",
  "data": { /* InstructorDetailDto */ }
}
```

---

### 10. Assign Course to Instructor

**Endpoint**: `POST /api/v1/instructors/{instructorId}/courses/{courseId}`

**Authorization**: Required

**Response** (201 Created):
```json
{
  "success": true,
  "message": "Course assigned successfully",
  "data": {
    "instructorId": 1,
    "instructorName": "Nguyen Thi Hoa",
    "courseId": 1,
    "courseName": "English Business Communication",
    "courseCode": "ENG301",
    "assignmentDate": "2024-01-15T10:30:00Z"
  }
}
```

**Error** (400 Bad Request - Already Assigned):
```json
{
  "success": false,
  "message": "Assignment failed",
  "data": "Instructor is already assigned to this course"
}
```

---

### 11. Remove Course Assignment

**Endpoint**: `DELETE /api/v1/instructors/{instructorId}/courses/{courseId}`

**Authorization**: Required

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Course assignment removed successfully",
  "data": true
}
```

---

### 12. Get Instructor Courses

**Endpoint**: `GET /api/v1/instructors/{instructorId}/courses`

**Authorization**: Required

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Retrieved 3 assigned courses",
  "data": [
    {
      "instructorId": 1,
      "instructorName": "Nguyen Thi Hoa",
      "courseId": 1,
      "courseName": "English Business Communication",
      "courseCode": "ENG301",
      "assignmentDate": "2024-01-10T10:30:00Z"
    }
  ]
}
```

---

### 13. Get Course Instructors

**Endpoint**: `GET /api/v1/instructors/course/{courseId}`

**Authorization**: Required

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Retrieved 2 instructors for course",
  "data": [ /* InstructorListDto array */ ]
}
```

---

### 14. Update Salary

**Endpoint**: `PATCH /api/v1/instructors/{id}/salary?baseSalary=2500&salaryFrequency=Monthly`

**Authorization**: Required

**Query Parameters**:
| Parameter | Type | Validation |
|-----------|------|-----------|
| baseSalary | decimal | > 0, max 10,000,000 |
| salaryFrequency | string | Monthly/Quarterly/Yearly |

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Salary updated successfully",
  "data": { /* InstructorDetailDto */ }
}
```

---

### 15. Calculate Salary

**Endpoint**: `GET /api/v1/instructors/{id}/calculated-salary`

**Authorization**: Required

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Salary calculated successfully",
  "data": 2500
}
```

---

### 16. Add Performance Rating (Admin Only)

**Endpoint**: `POST /api/v1/instructors/{id}/performance-rating?rating=5&comments=Excellent+performance`

**Authorization**: Required + Admin Role

**Query Parameters**:
| Parameter | Type | Validation |
|-----------|------|-----------|
| rating | int | 1-5 |
| comments | string | Performance feedback |

**Response** (201 Created):
```json
{
  "success": true,
  "message": "Performance rating added successfully",
  "data": {
    "instructorId": 1,
    "instructorName": "Nguyen Thi Hoa",
    "rating": 5,
    "comments": "Excellent performance",
    "ratingDate": "2024-01-15T10:30:00Z"
  }
}
```

---

### 17. Get Average Performance Rating

**Endpoint**: `GET /api/v1/instructors/{id}/average-rating`

**Authorization**: Required

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Average rating retrieved",
  "data": 4.5
}
```

---

### 18. Get Instructor Statistics

**Endpoint**: `GET /api/v1/instructors/{id}/statistics`

**Authorization**: Required

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Statistics retrieved",
  "data": {
    "instructorId": 1,
    "instructorName": "Nguyen Thi Hoa",
    "totalCoursesAssigned": 3,
    "yearsOfExperience": 5,
    "baseSalary": 2000,
    "calculatedSalary": 2500,
    "averagePerformanceRating": 4.5,
    "joinDate": "2020-01-15T00:00:00Z",
    "isActive": true
  }
}
```

---

### 19. Get Top-Rated Instructors (Admin Only)

**Endpoint**: `GET /api/v1/instructors/top-rated?count=10`

**Authorization**: Required + Admin Role

**Query Parameters**:
| Parameter | Type | Default | Range |
|-----------|------|---------|-------|
| count | int | 10 | 1-100 |

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Retrieved top 10 rated instructors",
  "data": [
    {
      "instructorId": 1,
      "instructorName": "Nguyen Thi Hoa",
      "rating": 5,
      "comments": "Average rating: 4.8",
      "ratingDate": "2024-01-15T10:30:00Z"
    }
  ]
}
```

---

## Validation Rules

### Name Fields
- Required, 2-50 characters
- Can contain letters, spaces, hyphens, apostrophes
- Examples: "Nguyen Thi Hoa", "Mary-Jane Smith", "O'Connor"

### Email Validation
- Required, valid format (RFC 5322 compliant)
- Unique (no duplicate emails)
- Max 100 characters
- Examples: "instructor@example.com", "hoa.nguyen@center.edu.vn"

### Phone Number Validation
- Optional
- Valid format with 10+ digits
- Supports international format: +84, +1, etc.
- Supports formats: +84 (0) 912 345 678, 0912345678
- Examples: "+84912345678", "0912 345 678", "(123) 456-7890"

### Specialization
- Required, 3-100 characters
- Examples: "English Communication", "Business English", "IELTS Preparation"

### Qualification Levels
- Required, one of 4 levels:
  - High School (10-12 years education)
  - Bachelor (4-year degree)
  - Master (graduate degree)
  - PhD (doctorate degree)

### Experience
- Required, non-negative integer
- Range: 0-70 years
- Calculated from hiring date if not provided

### Salary
- Required, decimal amount
- Min: 0.01, Max: 10,000,000
- Base salary before bonuses
- Examples: 2000, 2500.50, 3000

### Salary Frequency
- Required, one of 3 options:
  - Monthly (12 payments per year)
  - Quarterly (4 payments per year)
  - Yearly (1 payment per year)

### Performance Rating
- Integer 1-5
- 5 = Excellent
- 4 = Good
- 3 = Satisfactory
- 2 = Fair
- 1 = Poor

---

## Integration with Other Modules

### Student Module
- Instructors teach courses to students
- Student progress tracked by assigned instructors
- Performance ratings help improve teaching quality

### Course Module
- Many-to-many relationship via InstructorCourse table
- Each course can have multiple instructors
- Each instructor teaches multiple courses
- Course assignments trigger salary recalculation

### Payment Module
- Instructor salary used for payroll processing
- Performance ratings can affect bonuses
- Salary calculations integrate with payroll system

---

## Error Handling

### Common Error Responses

**Validation Error** (400 Bad Request):
```json
{
  "success": false,
  "message": "Validation failed",
  "data": [
    {"field": "Email", "message": "Email must be in valid format"},
    {"field": "BaseSalary", "message": "Base salary must be greater than zero"}
  ]
}
```

**Duplicate Email** (400 Bad Request):
```json
{
  "success": false,
  "message": "Invalid instructor data",
  "data": "Instructor with email hoa@example.com already exists"
}
```

**Not Found** (404 Not Found):
```json
{
  "success": false,
  "message": "Instructor not found",
  "data": "Instructor with ID 999 does not exist"
}
```

**Unauthorized** (401 Unauthorized):
```json
{
  "success": false,
  "message": "Unauthorized",
  "data": "Authentication token is missing or invalid"
}
```

**Forbidden** (403 Forbidden):
```json
{
  "success": false,
  "message": "Access denied",
  "data": "Admin role is required for this operation"
}
```

---

## Testing Guide

### Test Scenarios

#### 1. Create Instructor
```bash
POST /api/v1/instructors
Headers:
  Authorization: Bearer {token}
  Content-Type: application/json

Body:
{
  "firstName": "Nguyen",
  "lastName": "Hoa",
  "email": "hoa@example.com",
  "phoneNumber": "+84912345678",
  "specialization": "English Communication",
  "qualification": "Master",
  "yearsOfExperience": 5,
  "baseSalary": 2000,
  "salaryFrequency": "Monthly"
}

Expected: 201 Created
```

#### 2. Assign Course
```bash
POST /api/v1/instructors/1/courses/1
Headers:
  Authorization: Bearer {token}

Expected: 201 Created with assignment details
```

#### 3. Calculate Salary
```bash
GET /api/v1/instructors/1/calculated-salary
Headers:
  Authorization: Bearer {token}

Expected: 200 OK with calculated amount (e.g., 2500)
```

#### 4. Add Performance Rating
```bash
POST /api/v1/instructors/1/performance-rating?rating=5&comments=Excellent
Headers:
  Authorization: Bearer {admin-token}

Expected: 201 Created with rating details
```

#### 5. Search Instructors
```bash
GET /api/v1/instructors/search?searchTerm=nguyen
Headers:
  Authorization: Bearer {token}

Expected: 200 OK with matching instructors
```

---

## Performance Considerations

### Database Indexes
- Index on Email (unique constraint)
- Index on Qualification
- Index on IsActive status
- Composite index on (InstructorId, CourseId)

### Caching Opportunities
- Top-rated instructors (cache for 1 hour)
- Average performance ratings (cache for 1 hour)
- Instructor statistics (cache for 30 minutes)

### Query Optimization
- Use pagination for list operations
- Lazy-load course assignments
- Group performance ratings for averages
- Index frequently searched fields

---

## Best Practices

1. **Profile Management**
   - Keep qualifications current
   - Update experience annually
   - Review join dates for tenure calculations

2. **Course Assignments**
   - Match qualifications to courses
   - Distribute course load evenly
   - Consider specialization alignment

3. **Performance Reviews**
   - Conduct reviews quarterly
   - Base on student feedback
   - Document specific observations

4. **Salary Administration**
   - Review salary annually
   - Apply merit increases consistently
   - Recalculate after course changes

5. **Status Management**
   - Deactivate rather than delete
   - Maintain historical records
   - Review inactive status quarterly

---

## Troubleshooting

### Issue: "Email already exists"
**Cause**: Email already assigned to another instructor
**Solution**: Use unique email address or update existing record

### Issue: "Instructor not found"
**Cause**: Invalid instructor ID provided
**Solution**: Verify ID is correct and instructor exists

### Issue: "Salary frequency is invalid"
**Cause**: Spelling error or invalid value
**Solution**: Use one of: Monthly, Quarterly, Yearly

### Issue: "Course assignment failed"
**Cause**: Instructor already assigned to course
**Solution**: Remove existing assignment or update existing record

### Issue: "Qualification not found"
**Cause**: Invalid qualification level
**Solution**: Use one of: High School, Bachelor, Master, PhD

---

## Future Enhancements

- [ ] Instructor availability scheduling
- [ ] Student feedback integration
- [ ] Professional development tracking
- [ ] Certification management
- [ ] Leave management (vacation, sick leave)
- [ ] Continuing education credits
- [ ] Research and publication tracking
- [ ] Peer mentoring assignments
- [ ] Automatic performance alerts
- [ ] Salary negotiation workflows

