# Student Management Module Documentation

## Overview
The Student Management Module provides complete CRUD operations for student records in the English Training Center system. It includes enrollment tracking, payment history, and academic performance metrics.

## Architecture

### Components

1. **IStudentService / StudentService** 
   - Core business logic for student operations
   - Data validation and consistency checks
   - Integration with related entities (courses, payments, grades)

2. **StudentController**
   - 9 REST API endpoints for student management
   - Authorization and role-based access control
   - Comprehensive logging and error handling

3. **StudentDTOs**
   - CreateStudentRequest - for new student creation
   - UpdateStudentRequest - for student updates
   - StudentListDto - summary information for lists
   - StudentDetailDto - complete student information

4. **StudentValidators**
   - CreateStudentRequestValidator - validates new student data
   - UpdateStudentRequestValidator - validates student updates
   - FluentValidation with comprehensive rules

5. **StudentMappingProfile**
   - AutoMapper configuration for entity/DTO mapping
   - Automatic date/status handling

## API Endpoints

### 1. Get All Students (Paginated)
```http
GET /api/v1/students?pageNumber=1&pageSize=10
Authorization: Bearer {accessToken}
```

**Response (200)**
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": 1,
        "fullName": "John Doe",
        "email": "john@example.com",
        "phone": "0912345678",
        "enrollmentDate": "2024-01-15",
        "isActive": true,
        "enrolledCourses": 3
      }
    ],
    "totalCount": 25,
    "pageNumber": 1,
    "pageSize": 10,
    "hasPreviousPage": false,
    "hasNextPage": true
  }
}
```

### 2. Get Student by ID
```http
GET /api/v1/students/{id}
Authorization: Bearer {accessToken}
```

**Response (200)**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "userId": 5,
    "fullName": "John Doe",
    "email": "john@example.com",
    "phone": "0912345678",
    "dateOfBirth": "1995-05-15",
    "address": "123 Main St, City",
    "avatar": "https://example.com/avatar.jpg",
    "enrollmentDate": "2024-01-15",
    "isActive": true,
    "createdDate": "2024-01-15",
    "modifiedDate": "2024-01-20",
    "enrolledCourses": 3,
    "totalPayments": 1500.00,
    "averageGPA": 3.8
  }
}
```

### 3. Create New Student
```http
POST /api/v1/students
Authorization: Bearer {accessToken}
Content-Type: application/json

{
  "userId": 5,
  "fullName": "Jane Smith",
  "email": "jane@example.com",
  "phone": "0923456789",
  "dateOfBirth": "1996-03-20",
  "address": "456 Oak Ave, City",
  "avatar": "https://example.com/jane.jpg"
}
```

**Response (201)**
```json
{
  "success": true,
  "data": {
    "id": 26,
    "userId": 5,
    "fullName": "Jane Smith",
    ...
  }
}
```

### 4. Update Student
```http
PUT /api/v1/students/{id}
Authorization: Bearer {accessToken}
Content-Type: application/json

{
  "fullName": "Jane Smith Updated",
  "email": "jane.updated@example.com",
  "phone": "0934567890",
  "dateOfBirth": "1996-03-20",
  "address": "789 Pine Rd, City",
  "avatar": "https://example.com/jane-new.jpg",
  "isActive": true
}
```

### 5. Delete Student
```http
DELETE /api/v1/students/{id}
Authorization: Bearer {accessToken}
```

**Note**: Only Admin role can delete students

**Response (200)**
```json
{
  "success": true,
  "data": {
    "message": "Student deleted successfully"
  }
}
```

### 6. Search Students
```http
GET /api/v1/students/search/{searchTerm}
Authorization: Bearer {accessToken}
```

**Searches by**: Full name, email, or phone number

**Response (200)**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "fullName": "John Doe",
      ...
    }
  ]
}
```

### 7. Get Students by Status
```http
GET /api/v1/students/status/{isActive}
Authorization: Bearer {accessToken}
```

**Parameters**: isActive = true or false

### 8. Get Total Student Count
```http
GET /api/v1/students/count/total
Authorization: Bearer {accessToken}
```

**Response (200)**
```json
{
  "success": true,
  "data": 45
}
```

### 9. Get Students in Course
```http
GET /api/v1/students/course/{courseId}
Authorization: Bearer {accessToken}
```

## Validation Rules

### Student Email
- Required
- Must be valid email format
- Must be unique (no duplicates)
- Maximum 256 characters

### Full Name
- Required
- Minimum 2 characters
- Maximum 256 characters

### Phone Number
- Optional
- Maximum 20 characters
- Valid characters: digits, spaces, hyphen, plus, parentheses

### Date of Birth
- Optional
- Must be in the past
- Must be realistic (not more than 120 years ago)

### Avatar URL
- Optional
- Maximum 500 characters
- Must be valid URL format

## Request/Response Format

### Success Response
```json
{
  "success": true,
  "message": "Operation successful",
  "statusCode": 200,
  "data": { /* response data */ }
}
```

### Error Response
```json
{
  "success": false,
  "message": "Error description",
  "statusCode": 400,
  "errors": ["Error 1", "Error 2"]
}
```

## Authorization

All endpoints require authentication (Bearer token) except login/register.

**Role-based Access**:
- All authenticated users can: GET (retrieve)
- All authenticated users can: POST (create), PUT (update)
- Only Admin role can: DELETE

## HTTP Status Codes

| Code | Meaning |
|------|---------|
| 200 | OK - Request successful |
| 201 | Created - Resource created successfully |
| 400 | Bad Request - Invalid input |
| 401 | Unauthorized - Missing/invalid token |
| 404 | Not Found - Resource not found |
| 500 | Internal Server Error |

## Error Messages

### Email Already Exists
```json
{
  "success": false,
  "message": "Email already exists",
  "statusCode": 400
}
```

### Student Not Found
```json
{
  "success": false,
  "message": "Student with ID 999 not found",
  "statusCode": 404
}
```

### Invalid Input
```json
{
  "success": false,
  "message": "Validation failed",
  "statusCode": 400,
  "errors": [
    "Full name must be between 2 and 256 characters",
    "Invalid email address"
  ]
}
```

## Testing with Postman

### 1. Get All Students
```
GET http://localhost:5000/api/v1/students?pageNumber=1&pageSize=10
Headers:
  Authorization: Bearer {{accessToken}}
```

### 2. Create Student
```
POST http://localhost:5000/api/v1/students
Headers:
  Authorization: Bearer {{accessToken}}
  Content-Type: application/json
Body:
{
  "userId": 1,
  "fullName": "Test Student",
  "email": "test@example.com",
  "phone": "0912345678",
  "dateOfBirth": "2000-01-15",
  "address": "Test Address",
  "avatar": "https://example.com/avatar.jpg"
}
```

### 3. Update Student
```
PUT http://localhost:5000/api/v1/students/1
Headers:
  Authorization: Bearer {{accessToken}}
  Content-Type: application/json
Body:
{
  "fullName": "Updated Name",
  "email": "updated@example.com",
  "phone": "0987654321",
  "isActive": true
}
```

### 4. Delete Student (Admin only)
```
DELETE http://localhost:5000/api/v1/students/1
Headers:
  Authorization: Bearer {{accessToken}}
```

## Database Integration

### Related Entities
- **User**: Associated user account (UserId)
- **StudentCourse**: Course enrollments
- **Payment**: Payment records
- **Grade**: Academic grades
- **Assignment**: Course assignments
- **Exam**: Exam records

### Database Fields Added to Student Entity
- LastLogin: DateTime? (from User)
- LastLogout: DateTime? (from User)
- Phone: string? (to User)

## Performance Considerations

### Pagination
All list endpoints support pagination with:
- **pageNumber**: Starting from 1 (default: 1)
- **pageSize**: Records per page (default: 10, max recommended: 100)

### Search
Search functionality is case-insensitive and searches across:
- Full name
- Email
- Phone number

### Filtering
Students can be filtered by:
- Status (active/inactive)
- Course enrollment
- ID (direct lookup)

## Future Enhancements

1. **Advanced Filtering**
   - Filter by enrollment date range
   - Filter by payment status
   - Filter by GPA range

2. **Bulk Operations**
   - Bulk update student status
   - Bulk assign to courses
   - Bulk export (CSV/Excel)

3. **Analytics**
   - Student performance reports
   - Enrollment trends
   - Payment analytics

4. **Integration**
   - Automated email notifications
   - Document generation
   - Third-party system sync

## Troubleshooting

### Issue: "401 Unauthorized"
- Ensure Bearer token is included in Authorization header
- Verify token has not expired
- Check token format: `Bearer <token>`

### Issue: "Email already exists"
- The email address is already registered
- Use a different email or verify if student exists

### Issue: "Validation failed"
- Review error messages for specific field issues
- Check all required fields are provided
- Verify field formats match requirements

### Issue: "404 Not Found"
- Student with provided ID doesn't exist
- Verify ID is correct
- Check if student was deleted

## Best Practices

1. **Always paginate** large result sets
2. **Use search** instead of retrieving all records
3. **Validate input** before sending requests
4. **Handle errors** gracefully in client code
5. **Cache responses** where appropriate
6. **Use appropriate HTTP methods** (GET, POST, PUT, DELETE)

## Related Files

- Service: `Application/Services/Student/StudentService.cs`
- Interface: `Application/Services/Student/IStudentService.cs`
- Controller: `API/Controllers/StudentController.cs`
- DTOs: `Application/DTOs/Student/StudentDTOs.cs`
- Validators: `Application/Validators/Student/StudentValidators.cs`
- Mapper: `Application/Mappers/StudentMappingProfile.cs`
- Entity: `Domain/Entities/DomainEntities.cs` (Student class)

## Support

For issues or questions:
1. Check this documentation
2. Review error messages and logs
3. Check database for data consistency
4. Review Swagger UI at `/swagger`
