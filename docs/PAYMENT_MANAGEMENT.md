# Payment Management & Invoice API Documentation

## Overview

The Payment Management Module provides a comprehensive REST API for handling student payments, invoice generation, refund processing, and financial reporting. It tracks all monetary transactions, generates invoices, and provides detailed financial analytics.

**Base URL**: `http://localhost:5000/api/v1/payments`

---

## Architecture

### Service Layer (`PaymentService`)
- **Purpose**: Core business logic for payment operations and financial tracking
- **Methods**: 14 public methods covering CRUD, filtering, financial calculations, refunds
- **Dependencies**: Payment, Student, Course, StudentCourse repositories; AutoMapper; Logger
- **Features**: 
  - Automatic invoice number generation (INV-YYYYMMDD-XXXXX format)
  - Financial aggregations (total paid, outstanding balance, revenue)
  - Refund processing with partial refund support
  - Payment status tracking (Pending, Completed, Failed, Refunded, PartiallyRefunded)
  - Enrollment-based balance calculation from StudentCourse table

### Controller Layer (`PaymentsController`)
- **Purpose**: HTTP endpoint handling with authorization and validation
- **Endpoints**: 13 REST endpoints covering all payment and invoice operations
- **Authorization**: All endpoints require Bearer token; Revenue & Refund endpoints require Admin role
- **Response Format**: `ApiResponse<T>` wrapper with success/error status
- **Swagger**: Full OpenAPI documentation with examples

### Data Layer
- **Entity**: `Payment` (9 table fields: StudentId, CourseId, Amount, Date, Status, Method, Description, InvoiceNumber, Timestamps)
- **Related Entities**: Student (identifies payer), Course (identifies fee), StudentCourse (enrollment tracking)
- **Repositories**: Used via Generic Repository pattern with in-memory filtering

---

## API Endpoints

### 1. Get All Payments (Paginated)

**Request**:
```http
GET /api/v1/payments?pageNumber=1&pageSize=10
Authorization: Bearer <token>
```

**Parameters**:
- `pageNumber` (optional): Page number, default 1
- `pageSize` (optional): Records per page, default 10, max 50

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Retrieved 10 payments",
  "data": {
    "data": [
      {
        "id": 1,
        "invoiceNumber": "INV-20260128-12345",
        "studentId": 5,
        "courseId": 1,
        "amount": 5000000,
        "paymentDate": "2026-01-28T10:30:00Z",
        "status": "Completed",
        "paymentMethod": "Bank Transfer"
      }
    ],
    "totalCount": 25,
    "currentPage": 1,
    "pageSize": 10,
    "totalPages": 3,
    "hasNextPage": true,
    "hasPreviousPage": false
  }
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/payments?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer <token>"
```

---

### 2. Get Payment by ID

**Request**:
```http
GET /api/v1/payments/{id}
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Payment retrieved successfully",
  "data": {
    "id": 1,
    "invoiceNumber": "INV-20260128-12345",
    "studentId": 5,
    "studentName": "John Doe",
    "courseId": 1,
    "courseName": "English Fundamentals",
    "amount": 5000000,
    "paymentDate": "2026-01-28T10:30:00Z",
    "status": "Completed",
    "paymentMethod": "Bank Transfer",
    "description": "Course tuition payment",
    "createdDate": "2026-01-28T10:30:00Z",
    "modifiedDate": "2026-01-28T10:30:00Z"
  }
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/payments/1" \
  -H "Authorization: Bearer <token>"
```

---

### 3. Create Payment

**Request**:
```http
POST /api/v1/payments
Authorization: Bearer <token>
Content-Type: application/json
```

**Request Body**:
```json
{
  "studentId": 5,
  "courseId": 1,
  "amount": 5000000,
  "paymentMethod": "Bank Transfer",
  "description": "Course tuition payment for January"
}
```

**Response** (201 Created):
```json
{
  "success": true,
  "message": "Payment created successfully",
  "data": {
    "id": 2,
    "invoiceNumber": "INV-20260128-54321",
    "studentId": 5,
    "studentName": "John Doe",
    "courseId": 1,
    "courseName": "English Fundamentals",
    "amount": 5000000,
    "paymentDate": "2026-01-28T11:45:00Z",
    "status": "Completed",
    "paymentMethod": "Bank Transfer",
    "description": "Course tuition payment for January",
    "createdDate": "2026-01-28T11:45:00Z",
    "modifiedDate": "2026-01-28T11:45:00Z"
  }
}
```

**cURL Example**:
```bash
curl -X POST "http://localhost:5000/api/v1/payments" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "studentId": 5,
    "courseId": 1,
    "amount": 5000000,
    "paymentMethod": "Bank Transfer",
    "description": "Course tuition payment"
  }'
```

---

### 4. Update Payment

**Request**:
```http
PUT /api/v1/payments/{id}
Authorization: Bearer <token>
Content-Type: application/json
```

**Request Body**:
```json
{
  "status": "Completed",
  "paymentMethod": "Credit Card",
  "description": "Updated payment notes"
}
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Payment updated successfully",
  "data": { /* updated payment details */ }
}
```

**cURL Example**:
```bash
curl -X PUT "http://localhost:5000/api/v1/payments/2" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{ /* request body */ }'
```

---

### 5. Delete Payment

**Request**:
```http
DELETE /api/v1/payments/{id}
Authorization: Bearer <token>
```

**Requirements**: 
- User must have Admin role
- Payment must NOT be "Completed" (soft delete only for pending/cancelled)

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Payment deleted successfully",
  "data": true
}
```

**Error Response** (400 Bad Request):
```json
{
  "success": false,
  "message": "Cannot delete completed payment. Mark as cancelled instead.",
  "data": "Cannot delete completed payment. Mark as cancelled instead."
}
```

**cURL Example**:
```bash
curl -X DELETE "http://localhost:5000/api/v1/payments/2" \
  -H "Authorization: Bearer <token>"
```

---

### 6. Get Payments by Status

**Request**:
```http
GET /api/v1/payments/status/{status}
Authorization: Bearer <token>
```

**Status Values**: Pending, Completed, Failed, Cancelled, Refunded, PartiallyRefunded

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Found 8 payments with status 'Completed'",
  "data": [ /* payment list */ ]
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/payments/status/Completed" \
  -H "Authorization: Bearer <token>"
```

---

### 7. Get Payments by Student

**Request**:
```http
GET /api/v1/payments/student/{studentId}
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Found 5 payments for student 5",
  "data": [ /* student's payments */ ]
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/payments/student/5" \
  -H "Authorization: Bearer <token>"
```

---

### 8. Get Payments by Course

**Request**:
```http
GET /api/v1/payments/course/{courseId}
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Found 12 payments for course 1",
  "data": [ /* course payments */ ]
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/payments/course/1" \
  -H "Authorization: Bearer <token>"
```

---

### 9. Get Student Total Paid

**Request**:
```http
GET /api/v1/payments/student/{studentId}/total-paid
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Total paid amount retrieved",
  "data": 15000000
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/payments/student/5/total-paid" \
  -H "Authorization: Bearer <token>"
```

---

### 10. Get Student Outstanding Balance

**Request**:
```http
GET /api/v1/payments/student/{studentId}/outstanding-balance
Authorization: Bearer <token>
```

**Calculation**: Total course fees enrolled - Total paid amount

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Outstanding balance retrieved",
  "data": 5000000
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/payments/student/5/outstanding-balance" \
  -H "Authorization: Bearer <token>"
```

---

### 11. Process Refund (Admin Only)

**Request**:
```http
POST /api/v1/payments/{paymentId}/refund?refundAmount=2500000&reason=StudentRequest
Authorization: Bearer <token>
```

**Query Parameters**:
- `refundAmount` (required): Amount to refund
- `reason` (required): Reason for refund

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Refund of 2500000 processed successfully",
  "data": {
    "id": 3,
    "invoiceNumber": "REF-INV-20260128-12345",
    "studentId": 5,
    "amount": -2500000,
    "paymentDate": "2026-01-28T12:00:00Z",
    "status": "Refunded",
    "description": "Refund for StudentRequest"
  }
}
```

**cURL Example**:
```bash
curl -X POST "http://localhost:5000/api/v1/payments/1/refund?refundAmount=2500000&reason=StudentRequest" \
  -H "Authorization: Bearer <token>"
```

---

### 12. Get Invoice

**Request**:
```http
GET /api/v1/payments/{paymentId}/invoice
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Invoice retrieved successfully",
  "data": {
    "invoiceNumber": "INV-20260128-12345",
    "paymentId": 1,
    "studentName": "John Doe",
    "studentEmail": "john@example.com",
    "courseName": "English Fundamentals",
    "amount": 5000000,
    "paymentDate": "2026-01-28T10:30:00Z",
    "status": "Completed",
    "paymentMethod": "Bank Transfer",
    "description": "Course tuition payment",
    "createdDate": "2026-01-28T10:30:00Z"
  }
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/payments/1/invoice" \
  -H "Authorization: Bearer <token>"
```

---

### 13. Get Revenue (Admin Only)

**Request**:
```http
GET /api/v1/payments/revenue?startDate=2026-01-01&endDate=2026-01-31
Authorization: Bearer <token>
```

**Query Parameters**:
- `startDate` (required): Start date in yyyy-MM-dd format
- `endDate` (required): End date in yyyy-MM-dd format

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Revenue from 2026-01-01 to 2026-01-31",
  "data": 125000000
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/payments/revenue?startDate=2026-01-01&endDate=2026-01-31" \
  -H "Authorization: Bearer <token>"
```

---

### 14. Get Payment Statistics (Admin Only)

**Request**:
```http
GET /api/v1/payments/statistics
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Payment statistics retrieved",
  "data": {
    "totalPayments": 35,
    "completedPayments": 28,
    "pendingPayments": 5,
    "refundedPayments": 2,
    "totalRevenue": 125000000,
    "totalRefunded": 5000000,
    "averagePaymentAmount": 4464286,
    "highestPaymentAmount": 10000000,
    "lowestPaymentAmount": 1000000
  }
}
```

**cURL Example**:
```bash
curl -X GET "http://localhost:5000/api/v1/payments/statistics" \
  -H "Authorization: Bearer <token>"
```

---

## Validation Rules

### Create Payment Request

| Field | Rule | Message |
|-------|------|---------|
| StudentId | Required, > 0 | "Student ID must be greater than 0" |
| CourseId | Optional, > 0 if provided | "Course ID must be greater than 0" |
| Amount | Required, > 0, ≤ 1,000,000,000 | "Payment amount must be greater than 0" |
| PaymentMethod | Required, 3-100 chars, one of list | "Payment method must be one of: Cash, Bank Transfer, Credit Card, Check, E-wallet, Other" |
| Description | Optional, max 500 chars | "Description cannot exceed 500 characters" |

### Payment Methods
- Cash
- Bank Transfer
- Credit Card
- Check
- E-wallet
- Other

### Payment Status Values
- Pending (awaiting confirmation)
- Completed (payment processed)
- Failed (payment failed)
- Cancelled (cancelled by admin)
- Refunded (fully refunded)
- PartiallyRefunded (partially refunded)

---

## Data Models

### PaymentListDto
Used in paginated list responses.

```json
{
  "id": 1,
  "invoiceNumber": "INV-20260128-12345",
  "studentId": 5,
  "courseId": 1,
  "amount": 5000000,
  "paymentDate": "2026-01-28T10:30:00Z",
  "status": "Completed",
  "paymentMethod": "Bank Transfer"
}
```

### PaymentDetailDto
Used in single payment retrieval and create/update responses.

```json
{
  "id": 1,
  "invoiceNumber": "INV-20260128-12345",
  "studentId": 5,
  "studentName": "John Doe",
  "courseId": 1,
  "courseName": "English Fundamentals",
  "amount": 5000000,
  "paymentDate": "2026-01-28T10:30:00Z",
  "status": "Completed",
  "paymentMethod": "Bank Transfer",
  "description": "Course tuition payment",
  "createdDate": "2026-01-28T10:30:00Z",
  "modifiedDate": "2026-01-28T10:30:00Z"
}
```

### PaymentInvoiceDto
Used for invoice generation and reporting.

```json
{
  "invoiceNumber": "INV-20260128-12345",
  "paymentId": 1,
  "studentName": "John Doe",
  "studentEmail": "john@example.com",
  "courseName": "English Fundamentals",
  "amount": 5000000,
  "paymentDate": "2026-01-28T10:30:00Z",
  "status": "Completed",
  "paymentMethod": "Bank Transfer",
  "description": "Course tuition payment",
  "createdDate": "2026-01-28T10:30:00Z"
}
```

### PaymentStatisticsDto
Used in admin analytics endpoints.

```json
{
  "totalPayments": 35,
  "completedPayments": 28,
  "pendingPayments": 5,
  "refundedPayments": 2,
  "totalRevenue": 125000000,
  "totalRefunded": 5000000,
  "averagePaymentAmount": 4464286,
  "highestPaymentAmount": 10000000,
  "lowestPaymentAmount": 1000000
}
```

---

## Invoice Generation

### Invoice Number Format
```
INV-YYYYMMDD-XXXXX
```

- **INV**: Invoice prefix
- **YYYYMMDD**: Date of invoice (e.g., 20260128 for January 28, 2026)
- **XXXXX**: Random 5-digit number (10000-99999)

### Example
```
INV-20260128-54321
```

### Refund Invoice Format
```
REF-INV-YYYYMMDD-XXXXX
```

---

## Financial Calculations

### Outstanding Balance Calculation
```
Outstanding Balance = Total Course Fees Enrolled - Total Paid Amount
```

**Example**:
- Student enrolled in 3 courses: 5M + 7.5M + 10M = 22.5M total
- Student has paid: 15M
- Outstanding Balance: 22.5M - 15M = 7.5M

### Revenue Calculation
```
Revenue = Sum of all "Completed" payments within date range
```

### Refund Logic
- **Full Refund**: Refund amount = Original payment amount → Status = "Refunded"
- **Partial Refund**: Refund amount < Original payment amount → Status = "PartiallyRefunded"
- Refund cannot exceed original payment amount

---

## Error Handling

### Common Error Responses

**400 Bad Request** - Invalid input:
```json
{
  "success": false,
  "message": "Error creating payment",
  "data": "Specific error message"
}
```

**404 Not Found** - Payment doesn't exist:
```json
{
  "success": false,
  "message": "Payment not found",
  "data": "Payment with ID 999 not found"
}
```

**403 Forbidden** - Insufficient permissions:
```json
{
  "success": false,
  "message": "Forbidden",
  "data": "Admin role required for this operation"
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
2. Set base URL variable: `{{baseUrl}}/api/v1/payments`
3. Add `Authorization` header: `Bearer {{token}}`

### Test Workflow

1. **Login** (get token)
   - POST `/api/v1/auth/login`
   - Extract token from response

2. **Create Payment**
   - POST `/payments`
   - Student must exist in system
   - Course must exist (if courseId provided)

3. **Get All Payments**
   - GET `/payments?pageNumber=1&pageSize=10`

4. **Get Payment Details**
   - GET `/payments/1`

5. **Get Student Payments**
   - GET `/payments/student/5`

6. **Get Outstanding Balance**
   - GET `/payments/student/5/outstanding-balance`

7. **Get Invoice**
   - GET `/payments/1/invoice`

8. **Process Refund** (Admin)
   - POST `/payments/1/refund?refundAmount=2500000&reason=StudentRequest`

9. **Get Statistics** (Admin)
   - GET `/payments/statistics`

10. **Update Payment**
    - PUT `/payments/1` with updated status

---

## Integration with Other Modules

### Student Module
- When payment created: Student must exist
- Payment table stores StudentId foreign key
- Student module can query outstanding balance for billing

### Course Module
- When payment created: Course fee stored in Payment entity
- Course module can calculate revenue per course
- Course details include course fee for enrollment

### StudentCourse Module
- Outstanding balance calculation queries StudentCourse enrollments
- Determines total course fees owed based on enrollments
- Refund processing affects payment status only, not enrollments

---

## Best Practices

1. **Invoice Management**: Each payment generates unique invoice number
2. **Status Tracking**: Always update status through dedicated endpoint
3. **Refund Process**: Only admin can process refunds
4. **Balance Verification**: Check outstanding balance before enrollment
5. **Payment Methods**: Use consistent payment method naming
6. **Description Details**: Always include meaningful description for audit
7. **Date Range**: Use specific date ranges for revenue reports
8. **Financial Audits**: Regular statistics checks for reconciliation

---

## Troubleshooting

### Issue: Student Not Found Error
**Solution**: Ensure student exists in system before creating payment. Use endpoint GET `/api/v1/students/{id}` to verify.

### Issue: Cannot Process Refund
**Solution**: Ensure refund amount ≤ original payment amount. Provide refund reason. Must have Admin role.

### Issue: Invalid Payment Method
**Solution**: Use one of: Cash, Bank Transfer, Credit Card, Check, E-wallet, Other

### Issue: Cannot Delete Completed Payment
**Solution**: Update status to "Cancelled" instead of deleting. Completed payments are permanent records.

### Issue: Outstanding Balance Not Calculated
**Solution**: Ensure student is enrolled in courses via StudentCourse table. Balance = Course fees - Payments made.

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
