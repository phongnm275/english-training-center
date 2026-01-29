# Project Overview - Quản lý Trung tâm Đào tạo Tiếng Anh

## 1. Mô Tả Dự Án

### 1.1 Tên Dự Án
**English Training Center Management System (ETCMS)**

### 1.2 Mục Tiêu
Xây dựng một hệ thống quản lý tích hợp cho trung tâm đào tạo tiếng Anh, tự động hóa các quy trình quản lý học viên, giáo viên, khóa học, lịch học, thanh toán và đánh giá.

### 1.3 Phạm Vi (Scope)

#### Chức Năng Chính:
1. **Quản lý Học viên (Student Management)**
   - Đăng ký / Hủy đăng ký
   - Hồ sơ cá nhân
   - Lịch sử khóa học
   - Điểm số / Kết quả

2. **Quản lý Khóa Học (Course Management)**
   - Tạo / Cập nhật khóa học
   - Mô tả, mục tiêu, tài liệu
   - Cấp độ (Beginner, Intermediate, Advanced)
   - Khoá lịch học

3. **Quản lý Lớp Học (Class Management)**
   - Tạo lớp học cho từng khóa
   - Gán giáo viên
   - Giới hạn số lượng học viên
   - Trạng thái lớp (Upcoming, Ongoing, Completed)

4. **Lịch Học (Schedule Management)**
   - Xếp lịch học (Ngày, giờ, phòng học)
   - Quản lý phòng học
   - Kiểm soát điểm danh
   - Nghỉ học / Bù học

5. **Quản lý Thanh Toán (Payment Management)**
   - Hóa đơn học phí
   - Theo dõi thanh toán
   - Hoàn tiền / Điều chỉnh
   - Báo cáo doanh thu

6. **Đánh Giá & Bảng Điểm (Evaluation & Grading)**
   - Bài kiểm tra / Bài tập
   - Chấm điểm
   - Báo cáo tiến độ
   - Chứng chỉ hoàn thành

7. **Quản lý Giáo Viên (Instructor Management)**
   - Hồ sơ giáo viên
   - Chuyên môn / Bằng cấp
   - Lịch dạy
   - Đánh giá hiệu suất

8. **Báo Cáo & Thống Kê (Reports & Analytics)**
   - Báo cáo doanh thu
   - Thống kê học viên
   - Hiệu suất giáo viên
   - Phân tích tỷ lệ hoàn thành

### 1.4 Loại Người Dùng (User Roles)

| Role | Mô Tả |
|------|-------|
| **Admin** | Quản lý hệ thống, cấu hình, quản lý người dùng |
| **Manager** | Quản lý toàn bộ hoạt động trung tâm |
| **Instructor** | Dạy học, chấm điểm, ghi chú học viên |
| **Student** | Đăng ký khóa, xem lịch, nộp bài tập |
| **Finance** | Quản lý thanh toán, hóa đơn, báo cáo |

## 2. Thông Số Kỹ Thuật

### 2.1 Công Nghệ
- **Backend**: .NET Core 8.0
- **Database**: SQL Server 2019+
- **API**: RESTful API (JSON)
- **Architecture**: Clean Architecture / Layered Architecture
- **ORM**: Entity Framework Core

### 2.2 Yêu Cầu Phi Chức Năng
- **Performance**: Response time < 500ms
- **Availability**: 99.5%
- **Scalability**: Hỗ trợ 1000+ học viên đồng thời
- **Security**: Authentication (JWT), Authorization, Data Encryption
- **Data Backup**: Sao lưu hàng ngày

## 3. Các Bên Liên Quan (Stakeholders)

| Stakeholder | Quyền Lợi | Ảnh Hưởng |
|-------------|-----------|----------|
| Quản lý trung tâm | Tối ưu hóa quản lý, tăng doanh thu | Cao |
| Giáo viên | Đơn giản hóa công việc dạy học | Cao |
| Học viên | Dễ dàng theo dõi tiến độ | Cao |
| Bộ phận tài chính | Quản lý thanh toán hiệu quả | Trung bình |
| IT | Duy trì, cập nhật hệ thống | Trung bình |

## 4. Ràng Buộc & Rủi Ro

### 4.1 Ràng Buộc
- Budget giới hạn
- Thời gian phát triển: 4-5 tháng
- Cần tuân thủ các quy định về bảo vệ dữ liệu cá nhân

### 4.2 Rủi Ro Chính
- **Mất dữ liệu**: Mitigated by automated backups
- **Downtime hệ thống**: Mitigated by monitoring & alerting
- **Thay đổi yêu cầu**: Mitigated by agile methodology
- **Hiệu suất**: Mitigated by load testing & optimization

## 5. Giả Định

- Có sẵn infrastructure để deploy
- Người dùng sẵn sàng đào tạo
- Dữ liệu cũ có thể được migrate hoặc input thủ công

## 6. Định Nghĩa Thành Công

✅ Hệ thống hoạt động ổn định 99.5% thời gian  
✅ Tất cả chức năng chính được triển khai  
✅ Tài liệu đầy đủ và người dùng được đào tạo  
✅ Response time < 500ms  
✅ 95% người dùng hài lòng (survey)

---

**Version**: 1.0  
**Last Updated**: January 28, 2026
