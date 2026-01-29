# Course Management API Documentation

## Overview

The Course Management Module provides a comprehensive REST API for managing training courses. It handles course CRUD operations, capacity management, level-based filtering, and enrollment tracking.

**Base URL**: `http://localhost:5000/api/v1/courses`

---

## Architecture

### Service Layer (`CourseService`)
- **Purpose**: Core business logic for course operations
- **Methods**: 12 public methods covering CRUD, search, filtering, and capacity management
- **Dependencies**: Course, StudentCourse, Schedule repositories; AutoMapper; Logger
- **Features**: 
  - Pagination with metadata (total count, page info)
  - Search by course name or code
  - Filter by level (Beginner, Intermediate, Advanced, Professional)
  - Capacity management with available seats tracking
  - Enrollment count aggregation from StudentCourse table

### Controller Layer (`CoursesController`)
- **Purpose**: HTTP endpoint handling with authorization
- **Endpoints**: 10 REST endpoints covering all course operations
- **Authorization**: All endpoints require Bearer token; Delete requires Admin role
- **Response Format**: `ApiResponse<T>` wrapper with success/error status
- **Swagger**: Full OpenAPI documentation with examples

### Data Layer
- **Entity**: `Course` (16 table fields including timestamps, capacity, fee, dates)
- **Related Entities**: StudentCourse (enrollment tracking), Schedule (class scheduling)
- **Repositories**: Used via Generic Repository pattern with in-memory filtering

---

## API Endpoints

### 1. Get All Courses (Paginated)

**Request**:
```http
GET /api/v1/courses?pageNumber=1&pageSize=10
Authorization: Bearer <token>
```

**Parameters**:
- `pageNumber` (optional): Page number, default 1
- `pageSize` (optional): Records per page, default 10, max 50

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Retrieved 10 courses",
  "data": {
    "data": [
      {
        "id": 1,
        "courseCode": "ENG101",
        "courseName": "English Fundamentals",
        "level": "Beginner",
        "durationHours": 30,
        "fee": 5000000,
        "enrolledStudents": 15,
        "maxCapacity": 25,
        "isActive": true
      }
    ],
    "totalCount": 12,
    "currentPage": 1,
    "pageSize": 10,
    "totalPages": 2,
    "hasNextPage": true,
    "hasPreviousPage": false
  }
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/courses?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer <token>" \
  -H "Accept: application/json"
```

---

### 2. Get Course by ID

**Request**:
```http
GET /api/v1/courses/{id}
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Course retrieved successfully",
  "data": {
    "id": 1,
    "courseCode": "ENG101",
    "courseName": "English Fundamentals",
    "description": "Complete English fundamentals course for absolute beginners",
    "level": "Beginner",
    "durationHours": 30,
    "maxCapacity": 25,
    "fee": 5000000,
    "startDate": "2026-02-01T00:00:00Z",
    "endDate": "2026-03-31T00:00:00Z",
    "enrolledStudents": 15,
    "availableCapacity": 10,
    "scheduleCount": 12,
    "createdDate": "2026-01-28T10:00:00Z",
    "modifiedDate": "2026-01-28T10:00:00Z",
    "isActive": true
  }
}
```

**Error Response** (404 Not Found):
```json
{
  "success": false,
  "message": "Course not found",
  "data": "Course with ID 999 not found"
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/courses/1" \
  -H "Authorization: Bearer <token>"
```

---

### 3. Create Course

**Request**:
```http
POST /api/v1/courses
Authorization: Bearer <token>
Content-Type: application/json
```

**Request Body**:
```json
{
  "courseCode": "ENG102",
  "courseName": "English Conversation",
  "description": "Practice real-world English conversations",
  "level": "Intermediate",
  "durationHours": 40,
  "maxCapacity": 20,
  "fee": 7500000,
  "startDate": "2026-02-15T00:00:00Z",
  "endDate": "2026-04-30T00:00:00Z"
}
```

**Response** (201 Created):
```json
{
  "success": true,
  "message": "Course created successfully",
  "data": {
    "id": 2,
    "courseCode": "ENG102",
    "courseName": "English Conversation",
    "description": "Practice real-world English conversations",
    "level": "Intermediate",
    "durationHours": 40,
    "maxCapacity": 20,
    "fee": 7500000,
    "startDate": "2026-02-15T00:00:00Z",
    "endDate": "2026-04-30T00:00:00Z",
    "enrolledStudents": 0,
    "availableCapacity": 20,
    "scheduleCount": 0,
    "createdDate": "2026-01-28T11:30:00Z",
    "modifiedDate": "2026-01-28T11:30:00Z",
    "isActive": true
  }
}
```

**Error Response** (400 Bad Request - Duplicate Code):
```json
{
  "success": false,
  "message": "Invalid course data",
  "data": "Course code 'ENG102' already exists"
}
```

**cURL Example**:
```bash
curl -X POST "http://localhost:5000/api/v1/courses" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "courseCode": "ENG102",
    "courseName": "English Conversation",
    "description": "Practice real-world English conversations",
    "level": "Intermediate",
    "durationHours": 40,
    "maxCapacity": 20,
    "fee": 7500000,
    "startDate": "2026-02-15T00:00:00Z",
    "endDate": "2026-04-30T00:00:00Z"
  }'
```

---

### 4. Update Course

**Request**:
```http
PUT /api/v1/courses/{id}
Authorization: Bearer <token>
Content-Type: application/json
```

**Request Body**:
```json
{
  "courseCode": "ENG102",
  "courseName": "Advanced English Conversation",
  "description": "Master advanced English conversations",
  "level": "Advanced",
  "durationHours": 50,
  "maxCapacity": 18,
  "fee": 9000000,
  "startDate": "2026-02-15T00:00:00Z",
  "endDate": "2026-04-30T00:00:00Z",
  "isActive": true
}
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Course updated successfully",
  "data": { /* updated course details */ }
}
```

**cURL Example**:
```bash
curl -X PUT "http://localhost:5000/api/v1/courses/2" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{ /* request body */ }'
```

---

### 5. Delete Course

**Request**:
```http
DELETE /api/v1/courses/{id}
Authorization: Bearer <token>
```

**Requirements**: 
- User must have Admin role
- Course must have 0 enrolled students

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Course deleted successfully",
  "data": true
}
```

**Error Response** (400 Bad Request - Has Students):
```json
{
  "success": false,
  "message": "Cannot delete course with 5 enrolled students. Remove students first.",
  "data": "Cannot delete course with 5 enrolled students. Remove students first."
}
```

**cURL Example**:
```bash
curl -X DELETE "http://localhost:5000/api/v1/courses/2" \
  -H "Authorization: Bearer <token>"
```

---

### 6. Search Courses

**Request**:
```http
GET /api/v1/courses/search/{searchTerm}
Authorization: Bearer <token>
```

**Search Scope**: Searches course name and code (case-insensitive)

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Found 3 courses",
  "data": [
    {
      "id": 1,
      "courseCode": "ENG101",
      "courseName": "English Fundamentals",
      "level": "Beginner",
      "durationHours": 30,
      "fee": 5000000,
      "enrolledStudents": 15,
      "maxCapacity": 25,
      "isActive": true
    }
  ]
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/courses/search/ENG" \
  -H "Authorization: Bearer <token>"
```

---

### 7. Get Courses by Level

**Request**:
```http
GET /api/v1/courses/level/{level}
Authorization: Bearer <token>
```

**Path Parameters**:
- `level`: One of "Beginner", "Intermediate", "Advanced", "Professional"

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Found 5 courses at Beginner level",
  "data": [ /* course list */ ]
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/courses/level/Beginner" \
  -H "Authorization: Bearer <token>"
```

---

### 8. Get Courses by Status

**Request**:
```http
GET /api/v1/courses/status/{isActive}
Authorization: Bearer <token>
```

**Path Parameters**:
- `isActive`: true (active courses) or false (inactive courses)

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Found 8 active courses",
  "data": [ /* course list */ ]
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/courses/status/true" \
  -H "Authorization: Bearer <token>"
```

---

### 9. Get Courses with Available Capacity

**Request**:
```http
GET /api/v1/courses/capacity/available
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Found 7 courses with available capacity",
  "data": [ /* courses with open seats */ ]
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/courses/capacity/available" \
  -H "Authorization: Bearer <token>"
```

---

### 10. Get Total Course Count

**Request**:
```http
GET /api/v1/courses/count/total
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Total course count retrieved",
  "data": 12
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/courses/count/total" \
  -H "Authorization: Bearer <token>"
```

---

### 11. Get Enrolled Student Count

**Request**:
```http
GET /api/v1/courses/{id}/enrolled-count
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Enrolled student count retrieved",
  "data": 15
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/courses/1/enrolled-count" \
  -H "Authorization: Bearer <token>"
```

---

## Validation Rules

### Create Course Request

| Field | Rule | Message |
|-------|------|---------|
| CourseCode | Required, 3-20 chars, pattern: `^[A-Z]{3,}\d{2,}$` | "Course code must be uppercase letters followed by numbers (e.g., ENG101)" |
| CourseName | Required, 3-256 chars | "Course name must be between 3 and 256 characters" |
| Description | Required, 10-2000 chars | "Description must be between 10 and 2000 characters" |
| Level | Required, one of: Beginner, Intermediate, Advanced, Professional | "Level must be one of: Beginner, Intermediate, Advanced, Professional" |
| DurationHours | > 0, ≤ 1000 | "Duration cannot exceed 1000 hours" |
| MaxCapacity | > 0, ≤ 100 | "Maximum capacity cannot exceed 100 students" |
| Fee | ≥ 0, ≤ 10,000,000 | "Fee cannot exceed 10,000,000" |
| StartDate | Required, today or future | "Start date must be today or in the future" |
| EndDate | Required, after StartDate | "End date must be after start date" |

### Update Course Request

Same validation rules as Create Course Request, plus:
- `IsActive`: Boolean flag for course active/inactive status

---

## Data Models

### CourseListDto
Used in paginated list responses.

```json
{
  "id": 1,
  "courseCode": "ENG101",
  "courseName": "English Fundamentals",
  "level": "Beginner",
  "durationHours": 30,
  "fee": 5000000,
  "enrolledStudents": 15,
  "maxCapacity": 25,
  "isActive": true
}
```

### CourseDetailDto
Used in single course retrieval and create/update responses.

```json
{
  "id": 1,
  "courseCode": "ENG101",
  "courseName": "English Fundamentals",
  "description": "Complete English fundamentals course",
  "level": "Beginner",
  "durationHours": 30,
  "maxCapacity": 25,
  "fee": 5000000,
  "startDate": "2026-02-01T00:00:00Z",
  "endDate": "2026-03-31T00:00:00Z",
  "enrolledStudents": 15,
  "availableCapacity": 10,
  "scheduleCount": 12,
  "createdDate": "2026-01-28T10:00:00Z",
  "modifiedDate": "2026-01-28T10:00:00Z",
  "isActive": true
}
```

---

## Error Handling

### Common Error Responses

**400 Bad Request** - Invalid input:
```json
{
  "success": false,
  "message": "Error creating course",
  "data": "Specific error message"
}
```

**404 Not Found** - Course doesn't exist:
```json
{
  "success": false,
  "message": "Course not found",
  "data": "Course with ID 999 not found"
}
```

**403 Forbidden** - Insufficient permissions:
```json
{
  "success": false,
  "message": "Forbidden",
  "data": "User does not have permission"
}
```

**401 Unauthorized** - Missing or invalid token:
```json
{
  "success": false,
  "message": "Unauthorized"
}
```

---

## Testing with Postman

### Setup

1. Import collection or create new requests
2. Set base URL variable: `{{baseUrl}}/api/v1/courses`
3. Add `Authorization` header: `Bearer {{token}}`

### Test Workflow

1. **Login** (get token)
   - POST `/api/v1/auth/login`
   - Extract token from response

2. **Create Course**
   - POST `/courses`
   - Use valid course code (e.g., ENG102)

3. **Get All Courses**
   - GET `/courses?pageNumber=1&pageSize=10`

4. **Search Courses**
   - GET `/courses/search/ENG`

5. **Filter by Level**
   - GET `/courses/level/Beginner`

6. **Get Courses with Capacity**
   - GET `/courses/capacity/available`

7. **Get Course Details**
   - GET `/courses/1`

8. **Update Course**
   - PUT `/courses/1` with updated data

9. **Delete Course** (Admin only, no enrollments)
   - DELETE `/courses/1`

---

## Integration with Other Modules

### Student Module
- When student enrolls in course: `StudentCourse` table updated
- Course endpoint `GetEnrolledStudentCountAsync` reads from `StudentCourse` table
- Available capacity calculated as: `MaxCapacity - EnrolledStudents`

### Schedule Module
- Schedules associated with courses via `CourseId` foreign key
- Course detail includes `ScheduleCount` showing number of associated schedules

### Payment Module (Future)
- Course fee stored in `Course` table
- Payment processing will reference course fee

---

## Performance Considerations

### Pagination
- Default page size: 10 records
- Maximum page size: 50 records
- Reduces memory usage and improves response time for large datasets

### Searching
- Case-insensitive search on course name and code
- In-memory filtering (not database query optimization)
- Recommended for up to 1000 courses

### Capacity Queries
- Only active courses included in "available capacity" results
- Reduces administrative confusion with inactive courses

### Caching
- Consider caching course list (changes infrequently)
- Implement invalidation on create/update/delete operations

---

## Best Practices

1. **Course Code Format**: Always use uppercase letters + numbers (e.g., ENG101, TOEFL501)
2. **Descriptions**: Write clear, detailed course descriptions for students
3. **Date Ranges**: Ensure sufficient gap between start and end dates for scheduling
4. **Capacity Planning**: Set realistic maximum capacity based on classroom size
5. **Fee Management**: Review and update fees periodically
6. **Status Management**: Deactivate completed courses instead of deleting
7. **Enrollment Limits**: Monitor course capacity and open new sessions when full
8. **Level Progression**: Establish clear level progression for student pathway

---

## Troubleshooting

### Issue: Duplicate Course Code Error
**Solution**: Ensure course code is unique. Use format `[SUBJECT][LEVEL_NUMBER]` (e.g., ENG101 for English Level 1)

### Issue: Cannot Delete Course
**Solution**: Course has enrolled students. Either remove enrollments first or deactivate the course instead.

### Issue: Invalid Level Error
**Solution**: Use one of: "Beginner", "Intermediate", "Advanced", "Professional"

### Issue: Start Date in Past Error
**Solution**: Ensure start date is today or later. Cannot schedule courses in the past.

### Issue: Unauthorized Access to Delete
**Solution**: Only Admin users can delete courses. Contact system administrator to upgrade role.

---

## API Response Format

All responses follow this format:

```json
{
  "success": boolean,
  "message": string,
  "data": T (generic type)
}
```

- `success`: Boolean indicating operation result
- `message`: Human-readable status message
- `data`: Response payload (varies by endpoint)

---

## Version Information

- **API Version**: 1.0
- **Last Updated**: January 28, 2026
- **Module Status**: Production Ready

---
