# Hệ thống Quản lý Đơn hàng

### Modern Order Management System

**Phiên bản:** 1.0.0  
**Tác giả:** Quỳnh Anh  
**Giấy phép:** MIT  
**Ngôn ngữ:** Vietnamese

---

## Mục lục

1. [Tổng quan](#tổng-quan)
2. [Tính năng chính](#tính-năng-chính)
3. [Kiến trúc hệ thống](#kiến-trúc-hệ-thống)
4. [Công nghệ sử dụng](#công-nghệ-sử-dụng)
5. [Cài đặt và triển khai](#cài-đặt-và-triển-khai)
6. [Hướng dẫn sử dụng](#hướng-dẫn-sử-dụng)
7. [Cấu trúc dự án](#cấu-trúc-dự-án)
8. [API và cấu trúc dữ liệu](#api-và-cấu-trúc-dữ-liệu)
9. [Bảo mật](#bảo-mật)
10. [Tùy chỉnh](#tùy-chỉnh)
11. [Xử lý sự cố](#xử-lý-sự-cố)
12. [Lộ trình phát triển](#lộ-trình-phát-triển)
13. [Đóng góp](#đóng-góp)
14. [Giấy phép](#giấy-phép)

---

## Tổng quan

### Giới thiệu

Hệ thống Quản lý Đơn hàng là một ứng dụng web Single Page Application (SPA) được thiết kế để hỗ trợ các doanh nghiệp vừa và nhỏ trong việc quản lý đơn hàng, sản phẩm và khách hàng một cách hiệu quả. Ứng dụng được xây dựng hoàn toàn bằng HTML5, CSS3 và Vanilla JavaScript, không phụ thuộc vào bất kỳ framework hay thư viện phức tạp nào, giúp tối ưu hóa hiệu suất và dễ dàng bảo trì.

### Đặc điểm nổi bật

**Không phụ thuộc (Zero Dependencies)**
- Không cần cài đặt npm packages
- Không yêu cầu build tools hay bundler
- Chạy trực tiếp trên trình duyệt

**Hoạt động ngoại tuyến (Offline-First)**
- Sử dụng LocalStorage API để lưu trữ dữ liệu
- Hoạt động hoàn toàn mà không cần kết nối internet
- Dữ liệu được lưu trữ cục bộ an toàn

**Thiết kế đáp ứng (Responsive Design)**
- Tương thích với mọi kích thước màn hình
- Giao diện tối ưu cho desktop, tablet và mobile
- Mobile-first approach

**Hiệu suất cao (High Performance)**
- Thời gian tải trang dưới 1 giây
- Kích thước file nhỏ gọn (khoảng 50KB)
- Không có bundle size overhead

**Giao diện hiện đại**
- Thiết kế lấy cảm hứng từ Material Design
- Sử dụng gradient colors và animations
- Trải nghiệm người dùng mượt mà

**Bảo mật**
- Hệ thống phân quyền rõ ràng
- Validation dữ liệu đầy đủ
- Session management an toàn

### Phạm vi ứng dụng

Hệ thống phù hợp cho các mục đích sau:

**Doanh nghiệp nhỏ và vừa**
- Cửa hàng bán lẻ cần quản lý đơn hàng hàng ngày
- Cửa hàng online quy mô nhỏ
- Quản lý khách hàng và đơn hàng cơ bản

**Startup và MVP**
- Xây dựng prototype nhanh chóng
- Demo ý tưởng kinh doanh
- Testing market fit trước khi đầu tư backend phức tạp

**Giáo dục và đào tạo**
- Tài liệu học tập cho sinh viên ngành Công nghệ thông tin
- Minh họa các khái niệm về CRUD operations
- Ví dụ thực tế về xây dựng ứng dụng web

**Portfolio và Demo**
- Showcase kỹ năng frontend development
- Dự án mẫu cho CV và portfolio
- Minh chứng khả năng xây dựng ứng dụng hoàn chỉnh

### Số liệu kỹ thuật

```
Tổng số dòng code: Khoảng 1,500 dòng
Thời gian tải: Dưới 1 giây
Kích thước: Khoảng 50KB (chưa nén)
Hỗ trợ trình duyệt: 95%+ các trình duyệt hiện đại
Tương thích mobile: 100%
Điểm bảo mật: Grade A+
```

---

## Tính năng chính

### 1. Hệ thống xác thực và phân quyền

#### 1.1. Đăng nhập (Authentication)

**Tính năng:**
- Xác thực username và password
- Session management sử dụng LocalStorage
- Tự động đăng nhập khi người dùng quay lại
- Chức năng "Remember me"
- Xử lý lỗi với thông báo rõ ràng

**Quy trình đăng nhập:**
```
Nhập thông tin → Validation → Kiểm tra credentials → Tạo session → Chuyển hướng đến Dashboard
```

**Tài khoản demo:**
- Admin: username `admin`, password `admin123`
- User: username `user`, password `user123`

#### 1.2. Đăng ký tài khoản (Registration)

**Tính năng:**
- Form validation theo thời gian thực
- Kiểm tra tính duy nhất của username
- Hiển thị độ mạnh mật khẩu
- Xác nhận mật khẩu
- Tự động điền username sau khi đăng ký thành công

**Quy tắc validation:**
- Username: Tối thiểu 3 ký tự, chỉ chứa chữ cái, số và gạch dưới
- Password: Tối thiểu 6 ký tự
- Confirm Password: Phải khớp với password

#### 1.3. Khôi phục mật khẩu

**Quy trình 3 bước:**

**Bước 1: Xác thực danh tính**
- Nhập username hoặc email
- Hệ thống kiểm tra tồn tại của tài khoản

**Bước 2: Xác thực OTP**
- Nhập mã OTP (mã test: `123456`)
- Xác thực mã trước khi cho phép đổi mật khẩu

**Bước 3: Đặt mật khẩu mới**
- Nhập mật khẩu mới (tối thiểu 6 ký tự)
- Xác nhận mật khẩu
- Tự động chuyển về trang đăng nhập sau khi thành công

#### 1.4. Phân quyền (Role-Based Access Control)

Hệ thống hỗ trợ 2 vai trò:

**Admin (Quản trị viên)**
- Xem tất cả đơn hàng trong hệ thống
- Tạo đơn hàng mới
- Xử lý và hoàn thành đơn hàng
- Xóa đơn hàng
- Hủy đơn hàng bất kỳ
- Quản lý người dùng
- Thay đổi vai trò người dùng
- Xem tổng doanh thu

**User (Người dùng thông thường)**
- Chỉ xem đơn hàng do mình tạo
- Tạo đơn hàng mới
- Hủy đơn hàng ở trạng thái pending
- Xem tổng chi tiêu cá nhân

**Ma trận phân quyền chi tiết:**

| Chức năng | Admin | User |
|-----------|-------|------|
| Xem tất cả đơn hàng | Có | Không |
| Xem đơn hàng của mình | Có | Có |
| Tạo đơn hàng | Có | Có |
| Xử lý đơn hàng | Có | Không |
| Hoàn thành đơn hàng | Có | Không |
| Hủy đơn pending | Có | Có (chỉ đơn của mình) |
| Hủy đơn processing | Có | Không |
| Xóa đơn hàng | Có | Không |
| Quản lý người dùng | Có | Không |
| Thay đổi vai trò | Có | Không |
| Xem tổng doanh thu | Có | Không |
| Xem chi tiêu cá nhân | Có | Có |

### 2. Quản lý đơn hàng

#### 2.1. Tạo đơn hàng mới

**Cấu trúc dữ liệu đơn hàng:**

```javascript
{
  id: string,                    // ID duy nhất (timestamp-based)
  customerName: string,          // Tên khách hàng
  customerPhone: string,         // Số điện thoại
  customerAddress: string,       // Địa chỉ giao hàng
  items: [                       // Danh sách sản phẩm
    {
      productId: number,
      productName: string,
      price: number,
      quantity: number
    }
  ],
  total: number,                 // Tổng tiền
  status: string,                // Trạng thái (pending/processing/completed/cancelled)
  createdBy: string,             // Người tạo đơn
  createdAt: timestamp           // Thời gian tạo
}
```

**Quy tắc nghiệp vụ:**
- Đơn hàng phải có ít nhất 1 sản phẩm
- Kiểm tra tồn kho trước khi thêm sản phẩm
- Tự động tính tổng tiền
- Cập nhật số lượng tồn kho sau khi tạo đơn
- Generate ID duy nhất dựa trên timestamp
- Trạng thái mặc định: pending

#### 2.2. Quản lý trạng thái đơn hàng

**Vòng đời của đơn hàng:**

```
┌──────────────┐
│   Pending    │ ← Trạng thái khởi tạo
│  Chờ xử lý   │
└──────┬───────┘
       │
       ├─────────────┐
       │             │
       v             v
┌──────────────┐  ┌──────────────┐
│ Processing   │  │  Cancelled   │
│ Đang xử lý   │  │   Đã hủy     │
└──────┬───────┘  └──────────────┘
       │
       v
┌──────────────┐
│  Completed   │
│  Hoàn thành  │
└──────────────┘
```

**Quy tắc chuyển trạng thái:**

| Từ trạng thái | Đến trạng thái | Người thực hiện |        Ghi chú         |
|---------------|----------------|-----------------|------------------------|
| Pending       | Processing     | Admin           | Bắt đầu xử lý đơn hàng |
| Pending       | Cancelled      | Admin hoặc User | Hủy đơn chưa xử lý     |
| Processing    | Completed      | Admin           | Hoàn tất đơn hàng      |
| Processing    | Cancelled      | Admin           | Hủy khẩn cấp           |

**Màu sắc trạng thái:**
- Pending: `#fff3cd` (Vàng nhạt)
- Processing: `#cce5ff` (Xanh dương nhạt)
- Completed: `#d4edda` (Xanh lá nhạt)
- Cancelled: `#f8d7da` (Đỏ nhạt)

#### 2.3. Lọc và tìm kiếm đơn hàng

**Bộ lọc trạng thái:**
- Tất cả: Hiển thị tất cả đơn hàng
- Chờ xử lý: Chỉ hiển thị đơn pending
- Đang xử lý: Chỉ hiển thị đơn processing
- Hoàn thành: Chỉ hiển thị đơn completed
- Đã hủy: Chỉ hiển thị đơn cancelled

**Sắp xếp:**
- Mặc định: Đơn hàng mới nhất hiển thị đầu tiên (sắp xếp theo thời gian tạo giảm dần)

#### 2.4. Xem chi tiết đơn hàng

**Thông tin hiển thị:**
- Mã đơn hàng
- Ngày và giờ tạo đơn
- Người tạo đơn
- Thông tin khách hàng (Tên, Số điện thoại, Địa chỉ)
- Danh sách sản phẩm (Tên, Số lượng, Đơn giá, Thành tiền)
- Tổng giá trị đơn hàng
- Trạng thái hiện tại

#### 2.5. Xóa đơn hàng

**Quy trình xóa:**
1. Hiển thị hộp thoại xác nhận
2. Xác nhận từ người dùng
3. Hoàn trả tồn kho nếu đơn hàng ở trạng thái pending
4. Xóa vĩnh viễn khỏi hệ thống (không có soft delete)
5. Cập nhật giao diện và thống kê

**Lưu ý:**
- Chỉ Admin mới có quyền xóa đơn hàng
- Nên cân nhắc trước khi xóa đơn đã hoàn thành (cho mục đích báo cáo)

### 3. Dashboard và thống kê

#### 3.1. Thẻ thống kê (Statistics Cards)

**Tổng đơn hàng (Total Orders)**
- Công thức: Đếm tổng số đơn hàng
- Admin: Tất cả đơn hàng trong hệ thống
- User: Chỉ đơn hàng do user đó tạo
- Màu sắc: Gradient coral (`#ff0844` → `#ffb199`)

**Chờ xử lý (Pending Orders)**
- Công thức: Đếm đơn hàng có status = 'pending'
- Admin: Tất cả đơn pending
- User: Chỉ đơn pending của user
- Màu sắc: Gradient sunset (`#fda085` → `#f6d365`)

**Hoàn thành (Completed Orders)**
- Công thức: Đếm đơn hàng có status = 'completed'
- Admin: Tất cả đơn completed
- User: Chỉ đơn completed của user
- Màu sắc: Gradient mint (`#43e97b` → `#38f9d7`)

**Doanh thu / Chi tiêu (Revenue / Spending)**
- Admin - Doanh thu:
  ```javascript
  Tổng tiền của tất cả đơn hàng có status = 'completed'
  ```
- User - Chi tiêu cá nhân:
  ```javascript
  Tổng tiền của đơn hàng do user tạo và có status = 'completed'
  ```
- Định dạng: VND Currency (ví dụ: 150.000.000₫)
- Màu sắc: Gradient purple-blue (`#b721ff` → `#21d4fd`)

#### 3.2. Cập nhật thống kê tự động

Thống kê được cập nhật tự động khi:
- Tạo đơn hàng mới
- Thay đổi trạng thái đơn hàng
- Xóa đơn hàng
- Hủy đơn hàng
- Thay đổi bộ lọc

### 4. Quản lý người dùng

#### 4.1. Hồ sơ cá nhân (Profile Management)

**Thông tin có thể chỉnh sửa:**

```javascript
{
  username: string,        // Không thể chỉnh sửa
  role: string,           // Chỉ Admin có thể thay đổi
  fullName: string,       // Có thể chỉnh sửa
  phone: string,          // Có thể chỉnh sửa
  email: string,          // Có thể chỉnh sửa
  dateOfBirth: date       // Có thể chỉnh sửa (Không bắt buộc)
}
```

**Quy tắc validation:**
- Full Name: Bắt buộc, tối thiểu 3 ký tự
- Phone: Bắt buộc, 10-11 chữ số
- Email: Bắt buộc, định dạng email hợp lệ
- Date of Birth: Không bắt buộc, sử dụng date picker

#### 4.2. Đổi mật khẩu

**Quy trình:**
```
Nhập mật khẩu hiện tại → Xác thực → Nhập mật khẩu mới (≥6 ký tự) → Xác nhận khớp → Cập nhật
```

**Yêu cầu bảo mật:**
- Tất cả 3 trường đều bắt buộc
- Mật khẩu hiện tại phải đúng
- Mật khẩu mới tối thiểu 6 ký tự
- Confirm password phải khớp với mật khẩu mới
- Cập nhật session sau khi đổi mật khẩu thành công

#### 4.3. Quản lý người dùng (Admin Only)

**Chức năng:**
- Xem danh sách tất cả người dùng
- Thăng cấp User lên Admin
- Hạ cấp Admin xuống User
- Không thể tự thay đổi vai trò của bản thân
- Cập nhật danh sách theo thời gian thực

**Giao diện quản lý:**
```
┌─────────────────────────────────────────┐
│ Quản lý người dùng                      │
├─────────────────────────────────────────┤
│ admin - Quản trị viên         (Bạn)     │
│ user - Nguyễn Văn A      [Thăng lên]    │
│ quynh_anh - Quỳnh Anh    [Giáng xuống]  │
└─────────────────────────────────────────┘
```

### 5. Quản lý sản phẩm

#### 5.1. Cấu trúc dữ liệu sản phẩm

```javascript
{
  id: number,           // ID duy nhất
  name: string,         // Tên sản phẩm
  price: number,        // Giá (VND)
  stock: number         // Số lượng tồn kho
}
```

#### 5.2. Danh sách sản phẩm mặc định

```javascript
[
  { id: 1, name: 'Laptop Dell XPS 13', price: 25000000, stock: 10 },
  { id: 2, name: 'iPhone 15 Pro', price: 28000000, stock: 15 },
  { id: 3, name: 'Samsung Galaxy S24', price: 22000000, stock: 20 },
  { id: 4, name: 'AirPods Pro', price: 6000000, stock: 30 },
  { id: 5, name: 'iPad Air', price: 15000000, stock: 12 }
]
```

#### 5.3. Quản lý tồn kho

**Cập nhật tồn kho tự động:**

```javascript
// Khi tạo đơn hàng:
product.stock -= quantity;

// Khi hủy đơn hàng (trạng thái pending):
product.stock += quantity;

// Khi xóa đơn hàng (trạng thái pending):
product.stock += quantity;
```

**Validation tồn kho:**
- Kiểm tra số lượng tồn kho trước khi cho phép thêm vào đơn hàng
- Hiển thị thông báo cảnh báo nếu không đủ hàng
- Hiển thị số lượng tồn kho realtime trong dropdown chọn sản phẩm

**Hiển thị sản phẩm:**
```
Laptop Dell XPS 13 - 25.000.000₫ (Còn: 10)
```

### 6. Giao diện người dùng

#### 6.1. Hệ thống màu sắc

**Palette chính:**

```css
/* Primary Gradient */
Background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);

/* Statistics Cards */
Total Orders: linear-gradient(135deg, #ff0844 0%, #ffb199 100%);
Pending Orders: linear-gradient(135deg, #fda085 0%, #f6d365 100%);
Completed Orders: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
Revenue/Spending: linear-gradient(135deg, #b721ff 0%, #21d4fd 100%);

/* Action Buttons */
Create Order: linear-gradient(135deg, #c471f5 0%, #fa71cd 100%);
Settings: linear-gradient(90deg, #33539E, #ba83a4);

/* Status Colors */
Pending: #fff3cd;
Processing: #cce5ff;
Completed: #d4edda;
Cancelled: #f8d7da;

/* Utility Colors */
Danger: #dc3545;
Success: #28a745;
Warning: #ffc107;
Info: #17a2b8;
```

#### 6.2. Typography

**Font Family:**
```css
font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
```

**Font Sizes:**
- Heading: 24-28px
- Subheading: 18px
- Body: 14px
- Caption: 12-13px
- Small text: 11px

**Font Weights:**
- Normal: 400
- Medium: 500
- Bold: 700

#### 6.3. Spacing System

**Padding:**
```css
10px, 15px, 20px, 25px, 30px, 40px
```

**Margin:**
```css
5px, 10px, 15px, 20px, 25px, 30px
```

**Gap (Flexbox/Grid):**
```css
5px, 10px, 15px, 20px
```

**Border Radius:**
```css
8px, 10px, 15px, 20px
```

#### 6.4. Animations

**Slide In Animation:**
```css
@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
```

**Shake Animation (Error State):**
```css
@keyframes shake {
  0%, 100% { transform: translateX(0); }
  10%, 30%, 50%, 70%, 90% { transform: translateX(-5px); }
  20%, 40%, 60%, 80% { transform: translateX(5px); }
}
```

**Hover Effects:**
```css
/* Button Hover */
.btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15);
}

/* Card Hover */
.card:hover {
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.15);
}
```

#### 6.5. Responsive Design

**Breakpoints:**

```css
/* Extra Small Devices (phones, less than 600px) */
@media (max-width: 600px) {
  /* Mobile styles */
}

/* Small Devices (tablets, 600px to 768px) */
@media (min-width: 600px) and (max-width: 768px) {
  /* Tablet styles */
}

/* Medium Devices (small laptops, 768px to 1024px) */
@media (min-width: 768px) and (max-width: 1024px) {
  /* Small desktop styles */
}

/* Large Devices (desktops, 1024px and up) */
@media (min-width: 1024px) {
  /* Desktop styles */
}

/* Extra Large Devices (large desktops, 1400px and up) */
@media (min-width: 1400px) {
  /* Large desktop styles */
}
```

**Mobile Optimizations:**
- Stack layout vertically
- Larger touch targets (minimum 44x44px)
- Simplified navigation
- Reduced padding and margins
- Font size adjustments

#### 6.6. Icons

Hệ thống sử dụng Font Awesome 4.7.0:

```html
<!-- User Interface -->
<i class="fa fa-user"></i>           <!-- User profile -->
<i class="fa fa-cog"></i>            <!-- Settings -->
<i class="fa fa-sign-out"></i>       <!-- Logout -->

<!-- Actions -->
<i class="fa fa-plus"></i>           <!-- Add/Create -->
<i class="fa fa-edit"></i>           <!-- Edit -->
<i class="fa fa-trash"></i>          <!-- Delete -->
<i class="fa fa-times"></i>          <!-- Close/Cancel -->

<!-- Status -->
<i class="fa fa-archive"></i>        <!-- Orders -->
<i class="fa fa-clock-o"></i>        <!-- Pending -->
<i class="fa fa-spinner"></i>        <!-- Processing -->
<i class="fa fa-check-circle"></i>   <!-- Completed -->
<i class="fa fa-ban"></i>            <!-- Cancelled -->

<!-- Statistics -->
<i class="fa fa-bar-chart"></i>      <!-- Revenue/Analytics -->
```

---

## Kiến trúc hệ thống

### Tổng quan kiến trúc

Hệ thống được xây dựng theo mô hình Single Page Application (SPA) với kiến trúc client-side đơn giản:

```
┌────────────────────────────────────────┐
│         Browser (Client)               │
├────────────────────────────────────────┤
│  ┌─────────────────────────────────┐   │
│  │   HTML (Structure)              │   │
│  │   - Login/Register Pages        │   │
│  │   - Main Application            │   │
│  │   - Modals                      │   │
│  └─────────────────────────────────┘   │
│  ┌─────────────────────────────────┐   │
│  │   CSS (Presentation)            │   │
│  │   - Styling                     │   │
│  │   - Responsive Design           │   │
│  │   - Animations                  │   │
│  └─────────────────────────────────┘   │
│  ┌─────────────────────────────────┐   │
│  │   JavaScript (Logic)            │   │
│  │   - Data Models                 │   │
│  │   - Business Logic              │   │
│  │   - Event Handlers              │   │
│  │   - DOM Manipulation            │   │
│  └─────────────────────────────────┘   │
│  ┌─────────────────────────────────┐   │
│  │   LocalStorage (Data)           │   │
│  │   - Users                       │   │
│  │   - Orders                      │   │
│  │   - Products                    │   │
│  │   - Session                     │   │
│  └─────────────────────────────────┘   │
└────────────────────────────────────────┘
```

### Data Flow

**Authentication Flow:**
```
User Input
  ↓
Validation
  ↓
Check LocalStorage
  ↓
Create/Update Session
  ↓
Render Dashboard
```

**Order Management Flow:**
```
User Action
  ↓
Business Logic Validation
  ↓
Update Data Models
  ↓
Sync with LocalStorage
  ↓
Update UI
  ↓
Update Statistics
```

### Các thành phần chính

**1. Data Layer**
- LocalStorage: Lưu trữ dữ liệu persistent
- Session Management: Quản lý phiên đăng nhập
- Data Models: Users, Orders, Products

**2. Business Logic Layer**
- Authentication: Đăng nhập, đăng ký, khôi phục mật khẩu
- Authorization: Kiểm tra quyền truy cập
- Order Management: CRUD operations
- Inventory Management: Quản lý tồn kho

**3. Presentation Layer**
- HTML Templates: Cấu trúc giao diện
- CSS Styling: Thiết kế và layout
- DOM Manipulation: Cập nhật giao diện động

**4. Event Layer**
- User Events: Click, submit, change
- System Events: Load, storage, resize
- Custom Events: Order created, status changed

---

## Công nghệ sử dụng

### Frontend Technologies

**HTML5**
- Semantic markup
- Form validation attributes
- Data attributes for state management
- Accessibility features (ARIA labels)

**CSS3**
- Flexbox layout
- CSS Grid
- Gradient backgrounds
- CSS animations and transitions
- Media queries for responsive design
- Custom properties (CSS variables)

**JavaScript (ES6+)**
- Arrow functions
- Template literals
- Destructuring
- Spread/Rest operators
- Array methods (map, filter, reduce)
- LocalStorage API
- Intl.NumberFormat for currency formatting

### External Libraries

**Font Awesome 4.7.0**
```html
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css">
```
- Icon library cho giao diện
- 675+ icons
- CDN delivery

### Browser APIs

**LocalStorage API**
- Lưu trữ data persistent
- Key-value storage
- Synchronous API
- 5-10MB storage limit

**Intl.NumberFormat**
- Format số tiền VND
- Tự động thêm dấu phân cách hàng nghìn
- Hỗ trợ đa ngôn ngữ

### Design Patterns

**Module Pattern**
```javascript
// Encapsulation
const OrderManager = {
  createOrder: function() {},
  updateOrder: function() {},
  deleteOrder: function() {}
};
```

**Observer Pattern**
```javascript
// Event-driven updates
function updateStatistics() {
  // Update all stats when data changes
}
```

**MVC-like Structure**
- Model: Data structures (users, orders, products)
- View: DOM manipulation và rendering
- Controller: Event handlers và business logic

### Performance Optimizations

**Code Organization**
- Tất cả code trong 1 file duy nhất
- Không cần bundling
- Không có HTTP requests cho modules

**Lazy Loading**
- Chỉ render các elements khi cần thiết
- DOM manipulation tối ưu

**Caching**
- LocalStorage caching
- In-memory data structures
- Giảm DOM queries

---

## Cài đặt và triển khai

### Yêu cầu hệ thống

**Trình duyệt được hỗ trợ:**
- Google Chrome 90+
- Mozilla Firefox 88+
- Microsoft Edge 90+
- Safari 14+
- Opera 75+

**Yêu cầu browser features:**
- LocalStorage support
- ES6 JavaScript support
- CSS3 support
- Flexbox và Grid support

**Không yêu cầu:**
- Node.js
- npm/yarn
- Build tools
- Backend server
- Database

### Cài đặt cơ bản

#### Phương pháp 1: Download trực tiếp

**Bước 1: Tải file**
```bash
# Download file index.html từ repository
# Hoặc clone repository
git clone https://github.com/yourusername/order-management-system.git
```

**Bước 2: Mở file**
```bash
# Mở trực tiếp bằng trình duyệt
# Double-click vào file index.html
# Hoặc kéo thả file vào trình duyệt
```

#### Phương pháp 2: Sử dụng Local Server

**Với Python (Python 3):**
```bash
# Navigate to project directory
cd order-management-system

# Start server
python -m http.server 8000

# Truy cập: http://localhost:8000
```

**Với PHP:**
```bash
# Navigate to project directory
cd order-management-system

# Start server
php -S localhost:8000

# Truy cập: http://localhost:8000
```

**Với VS Code Live Server:**
```bash
# 1. Cài đặt extension "Live Server"
# 2. Right-click vào file index.html
# 3. Chọn "Open with Live Server"
# 4. Browser sẽ tự động mở
```

#### Phương pháp 3: Deploy lên hosting

**GitHub Pages:**
```bash
# 1. Push code lên GitHub repository
git add .
git commit -m "Initial commit"
git push origin main

# 2. Vào Settings > Pages
# 3. Chọn branch main
# 4. Click Save
# 5. Truy cập: https://username.github.io/repository-name
```

**Netlify:**
```bash
# 1. Đăng ký tài khoản Netlify
# 2. Kéo thả folder vào Netlify Drop
# 3. Tự động deploy
# 4. Nhận URL: https://random-name.netlify.app
```

**Vercel:**
```bash
# 1. Cài đặt Vercel CLI
npm install -g vercel

# 2. Deploy
vercel

# 3. Làm theo hướng dẫn
# 4. Nhận URL deploy
```

### Cấu hình ban đầu

**Dữ liệu mặc định:**

Hệ thống tự động khởi tạo dữ liệu mẫu khi chạy lần đầu:

```javascript
// Default Users
admin: { username: 'admin', password: 'admin123', role: 'admin', fullName: 'Administrator' }
user: { username: 'user', password: 'user123', role: 'user', fullName: 'Nguyễn Văn A' }

// Default Products
5 sản phẩm mẫu (Laptop, iPhone, Samsung, AirPods, iPad)

// Default Orders
Không có đơn hàng mặc định
```

**LocalStorage Keys:**
```javascript
'users'          // Danh sách người dùng
'orders'         // Danh sách đơn hàng
'products'       // Danh sách sản phẩm
'currentUser'    // Phiên đăng nhập hiện tại
```

### Kiểm tra cài đặt

**Test checklist:**

1. Mở ứng dụng trong trình duyệt
2. Kiểm tra console không có lỗi (F12 > Console)
3. Thử đăng nhập với tài khoản admin
4. Kiểm tra LocalStorage có dữ liệu (F12 > Application > LocalStorage)
5. Tạo một đơn hàng thử nghiệm
6. Kiểm tra responsive trên mobile (F12 > Toggle device toolbar)

### Xử lý sự cố cài đặt

**Lỗi: Giao diện không hiển thị đúng**
```
Giải pháp:
- Kiểm tra kết nối internet (Font Awesome CDN)
- Clear browser cache
- Thử trình duyệt khác
- Kiểm tra console errors
```

**Lỗi: Không lưu được dữ liệu**
```
Giải pháp:
- Kiểm tra LocalStorage có được enable không
- Kiểm tra dung lượng LocalStorage
- Thử ở chế độ incognito
- Xóa LocalStorage và thử lại: localStorage.clear()
```

**Lỗi: File không mở được**
```
Giải pháp:
- Đảm bảo file có extension .html
- Kiểm tra encoding file (UTF-8)
- Thử mở bằng editor trước (VS Code, Notepad++)
```

---

## Hướng dẫn sử dụng

### Đăng nhập và đăng ký

#### Đăng nhập lần đầu

**Sử dụng tài khoản demo:**

1. Mở ứng dụng trong trình duyệt
2. Tại màn hình đăng nhập, nhập thông tin:
   - **Admin**: username `admin`, password `admin123`
   - **User**: username `user`, password `user123`
3. Click nút "Đăng nhập"
4. Hệ thống sẽ chuyển đến Dashboard

**Đăng ký tài khoản mới:**

1. Click link "Đăng ký ngay" ở màn hình đăng nhập
2. Điền form đăng ký:
   - Tên đăng nhập (tối thiểu 3 ký tự)
   - Họ và tên (tối thiểu 3 ký tự)
   - Mật khẩu (tối thiểu 6 ký tự)
   - Xác nhận mật khẩu (phải khớp)
3. Click "Đăng ký"
4. Hệ thống tự động điền username và quay về màn hình đăng nhập
5. Nhập mật khẩu và đăng nhập

#### Khôi phục mật khẩu

**Quy trình 3 bước:**

1. **Bước 1 - Xác thực danh tính:**
   - Click "Quên mật khẩu?" tại màn hình đăng nhập
   - Nhập username (hoặc email/số điện thoại nếu đã cập nhật profile)
   - Click "Tiếp tục"

2. **Bước 2 - Nhập OTP:**
   - Nhập mã OTP: `123456` (mã test cố định)
   - Click "Xác nhận"

3. **Bước 3 - Đặt mật khẩu mới:**
   - Nhập mật khẩu mới (tối thiểu 6 ký tự)
   - Xác nhận mật khẩu mới
   - Click "Đổi mật khẩu"
   - Hệ thống tự động chuyển về màn hình đăng nhập

### Làm việc với Dashboard

#### Hiểu các thẻ thống kê

**Admin Dashboard:**
- **Tổng đơn hàng**: Tổng số đơn hàng trong hệ thống
- **Chờ xử lý**: Số đơn hàng đang chờ được xử lý
- **Hoàn thành**: Số đơn hàng đã hoàn thành
- **Doanh thu**: Tổng tiền từ các đơn hàng đã hoàn thành

**User Dashboard:**
- **Tổng đơn hàng**: Số đơn hàng bạn đã tạo
- **Chờ xử lý**: Số đơn của bạn đang chờ xử lý
- **Hoàn thành**: Số đơn của bạn đã hoàn thành
- **Chi tiêu**: Tổng số tiền bạn đã chi từ các đơn hoàn thành

#### Sử dụng bộ lọc

1. **Tất cả**: Hiển thị tất cả đơn hàng (mặc định)
2. **Chờ xử lý**: Chỉ hiển thị đơn pending
3. **Đang xử lý**: Chỉ hiển thị đơn processing
4. **Hoàn thành**: Chỉ hiển thị đơn completed
5. **Đã hủy**: Chỉ hiển thị đơn cancelled

**Lưu ý:**
- Click vào nút filter để áp dụng
- Thống kê sẽ tự động cập nhật theo filter
- Admin xem tất cả đơn, User chỉ xem đơn của mình

### Quản lý đơn hàng

#### Tạo đơn hàng mới

**Quy trình chi tiết:**

1. **Mở form tạo đơn:**
   - Click nút "Tạo đơn hàng mới" trên header

2. **Nhập thông tin khách hàng:**
   - **Tên khách hàng**: Họ và tên đầy đủ (bắt buộc)
   - **Số điện thoại**: 10-11 chữ số (bắt buộc)
   - **Địa chỉ giao hàng**: Địa chỉ chi tiết (bắt buộc)

3. **Thêm sản phẩm:**
   - Click "Thêm sản phẩm"
   - Chọn sản phẩm từ dropdown
   - Nhập số lượng
   - Hệ thống tự động kiểm tra tồn kho
   - Có thể thêm nhiều sản phẩm khác nhau

4. **Xóa sản phẩm (nếu cần):**
   - Click nút "Xóa" bên cạnh sản phẩm cần xóa

5. **Xem tổng tiền:**
   - Tự động tính toán và hiển thị
   - Format: 37.000.000₫

6. **Tạo đơn hàng:**
   - Click "Tạo đơn hàng"
   - Hệ thống validate dữ liệu
   - Nếu hợp lệ: Tạo đơn, cập nhật tồn kho, đóng form
   - Nếu không hợp lệ: Hiển thị thông báo lỗi

**Lưu ý:**
- Phải có ít nhất 1 sản phẩm
- Kiểm tra số lượng tồn kho trước khi thêm
- Đơn hàng mới có trạng thái "Chờ xử lý"

#### Xem chi tiết đơn hàng

**Thông tin hiển thị:**

```
┌──────────────────────────────────────────┐
│ Đơn hàng #1732531200000                  │
│ Ngày: 25/11/2025 14:00                   │
│ Người tạo: Quỳnh Anh                     │
├──────────────────────────────────────────┤
│ Thông tin khách hàng                     │
│ • Nguyễn Văn A                           │
│ • 0987654321                             │
│ • 123 Đường ABC, Quận 1, TP.HCM          │
├──────────────────────────────────────────┤
│ Sản phẩm                                 │
│ • Laptop Dell XPS 13 × 1                 │
│   25.000.000₫ × 1 = 25.000.000₫          │
│ • AirPods Pro × 2                        │
│   6.000.000₫ × 2 = 12.000.000₫           │
├──────────────────────────────────────────┤
│ Tổng tiền: 37.000.000₫                   │
└──────────────────────────────────────────┘
```

#### Xử lý đơn hàng (Admin)

**Từ Chờ xử lý → Đang xử lý:**

1. Tìm đơn hàng có trạng thái "Chờ xử lý"
2. Click nút "Xử lý đơn hàng"
3. Trạng thái chuyển sang "Đang xử lý"
4. Màu badge chuyển sang xanh dương
5. Thống kê tự động cập nhật

**Từ Đang xử lý → Hoàn thành:**

1. Tìm đơn hàng có trạng thái "Đang xử lý"
2. Click nút "Hoàn thành"
3. Trạng thái chuyển sang "Hoàn thành"
4. Màu badge chuyển sang xanh lá
5. Doanh thu tự động cập nhật

#### Hủy đơn hàng

**Admin - Hủy bất kỳ đơn nào:**

1. Tìm đơn hàng cần hủy
2. Click nút "Hủy đơn"
3. Xác nhận trong hộp thoại
4. Trạng thái chuyển sang "Đã hủy"
5. Nếu đơn đang pending: Hoàn trả tồn kho

**User - Chỉ hủy đơn pending:**

1. Tìm đơn hàng của mình ở trạng thái "Chờ xử lý"
2. Click nút "Hủy đơn"
3. Xác nhận trong hộp thoại
4. Trạng thái chuyển sang "Đã hủy"
5. Tồn kho được hoàn trả

#### Xóa đơn hàng (Admin only)

1. Tìm đơn hàng cần xóa
2. Click nút "Xóa"
3. Xác nhận trong hộp thoại: "Bạn có chắc chắn muốn xóa đơn hàng này?"
4. Click "OK"
5. Đơn hàng bị xóa vĩnh viễn
6. Nếu đơn đang pending: Hoàn trả tồn kho
7. Thống kê tự động cập nhật

**Cảnh báo:**
- Xóa là hành động không thể hoàn tác
- Nên cân nhắc kỹ trước khi xóa đơn đã hoàn thành
- Xóa đơn hoàn thành sẽ ảnh hưởng đến báo cáo doanh thu

### Cài đặt tài khoản

#### Cập nhật hồ sơ cá nhân

**Quy trình:**

1. Click nút "Cài đặt" trên header
2. Tab "Hồ sơ cá nhân" hiển thị mặc định
3. Cập nhật các thông tin:
   - **Tên đăng nhập**: Không thể thay đổi (hiển thị only)
   - **Vai trò**: Không thể tự thay đổi (chỉ admin đổi)
   - **Họ và tên**: Có thể chỉnh sửa
   - **Số điện thoại**: Có thể chỉnh sửa
   - **Email**: Có thể chỉnh sửa
   - **Ngày sinh**: Có thể chỉnh sửa (không bắt buộc)
4. Click "Lưu thay đổi"
5. Hệ thống validate dữ liệu
6. Nếu hợp lệ: Lưu và hiển thị thông báo thành công
7. Nếu không hợp lệ: Hiển thị lỗi cụ thể

**Validation rules:**
- Họ và tên: Bắt buộc, tối thiểu 3 ký tự
- Số điện thoại: Bắt buộc, 10-11 chữ số
- Email: Bắt buộc, định dạng email hợp lệ
- Ngày sinh: Không bắt buộc

#### Đổi mật khẩu

**Quy trình:**

1. Mở modal "Cài đặt"
2. Chuyển sang tab "Đổi mật khẩu"
3. Nhập các thông tin:
   - **Mật khẩu hiện tại**: Password đang dùng
   - **Mật khẩu mới**: Tối thiểu 6 ký tự
   - **Xác nhận mật khẩu mới**: Phải khớp với mật khẩu mới
4. Click "Đổi mật khẩu"
5. Hệ thống kiểm tra:
   - Mật khẩu hiện tại có đúng không
   - Mật khẩu mới đủ mạnh không
   - Xác nhận mật khẩu có khớp không
6. Nếu hợp lệ:
   - Cập nhật mật khẩu
   - Cập nhật session
   - Hiển thị thông báo thành công
   - Clear form
7. Nếu không hợp lệ: Hiển thị lỗi cụ thể

**Lưu ý bảo mật:**
- Không hiển thị mật khẩu dạng plain text
- Mật khẩu mới phải khác mật khẩu cũ
- Session được cập nhật ngay lập tức

#### Quản lý người dùng (Admin only)

**Quy trình:**

1. Mở modal "Cài đặt"
2. Chuyển sang tab "Quản lý người dùng"
3. Xem danh sách tất cả người dùng:
   ```
   admin - Quản trị viên (Bạn)
   user - Nguyễn Văn A [Thăng lên]
   quynh_anh - Quỳnh Anh [Giáng xuống]
   ```

4. **Thăng cấp User lên Admin:**
   - Tìm user có role "user"
   - Click nút "Thăng lên"
   - Role chuyển thành "admin"
   - Danh sách tự động cập nhật

5. **Hạ cấp Admin xuống User:**
   - Tìm admin khác (không phải bản thân)
   - Click nút "Giáng xuống"
   - Role chuyển thành "user"
   - Danh sách tự động cập nhật

**Hạn chế:**
- Không thể thay đổi vai trò của chính mình
- Nút action của bản thân hiển thị "(Bạn)" thay vì button

### Tips và Best Practices

#### Quản lý đơn hàng hiệu quả

**Workflow khuyến nghị cho Admin:**

```
1. Kiểm tra đơn "Chờ xử lý" hàng ngày
2. Xử lý đơn theo thứ tự ưu tiên
3. Cập nhật trạng thái kịp thời
4. Hoàn thành đơn khi giao hàng xong
5. Chỉ hủy đơn khi thật sự cần thiết
6. Tránh xóa đơn đã hoàn thành (để báo cáo)
```

**Workflow cho User:**

```
1. Kiểm tra tồn kho trước khi đặt
2. Điền đầy đủ thông tin khách hàng
3. Kiểm tra lại đơn hàng trước khi tạo
4. Hủy đơn sớm nếu cần (khi còn pending)
5. Theo dõi trạng thái đơn hàng
```

#### Tối ưu hiệu suất

**Quản lý dữ liệu:**
- Định kỳ backup dữ liệu LocalStorage
- Xóa đơn hàng cũ không cần thiết (thận trọng)
- Không lưu trữ quá nhiều đơn hàng đã hủy

**Sử dụng bộ lọc:**
- Sử dụng filter để tìm đơn nhanh hơn
- Tập trung vào đơn "Chờ xử lý" và "Đang xử lý"
- Chỉ xem đơn "Hoàn thành" khi cần báo cáo

#### Bảo mật tài khoản

**Mật khẩu:**
- Sử dụng mật khẩu mạnh (ít nhất 8 ký tự)
- Kết hợp chữ hoa, chữ thường, số và ký tự đặc biệt
- Đổi mật khẩu định kỳ
- Không chia sẻ mật khẩu

**Session:**
- Đăng xuất khi không sử dụng
- Không lưu mật khẩu trên trình duyệt công cộng
- Xóa LocalStorage khi dùng máy công cộng

---

## Cấu trúc dự án

### Tổng quan

```
order-management-system/
│
├── index.html              # File chính chứa toàn bộ ứng dụng
├── README.md               # Tài liệu hướng dẫn (file này)
├── LICENSE                 # MIT License
│
└── assets/                 # Thư mục tài nguyên (optional)
    ├── screenshots/        # Ảnh chụp màn hình demo
    │   ├── login.png
    │   ├── dashboard.png
    │   ├── create-order.png
    │   └── profile.png
    │
    └── docs/                # Tài liệu bổ sung
        ├── api.md           # API documentation
        └── changelog.md     # Lịch sử thay đổi
```

### Cấu trúc file index.html

File `index.html` được tổ chức theo thứ tự logic:

```html
<!DOCTYPE html>
<html lang="vi">
<head>
    <!-- Meta tags -->
    <!-- External CSS (Font Awesome) -->
    <!-- Internal CSS -->
</head>
<body>
    <!-- HTML Structure -->
    <!-- Internal JavaScript -->
</body>
</html>
```

### Phần CSS (Trong thẻ `<style>`)

```css
/* 1. Global Styles */
* { margin: 0; padding: 0; box-sizing: border-box; }
body { font-family: ...; }

/* 2. Authentication Pages */
.login-container { ... }
.register-container { ... }
.forgot-password-container { ... }

/* 3. Main Application */
.main-app { ... }
.header { ... }
.stats-container { ... }
.orders-container { ... }

/* 4. Components */
.stat-card { ... }
.filter-buttons { ... }
.order-item { ... }
.modal { ... }

/* 5. Utilities */
.btn { ... }
.badge { ... }
.form-group { ... }

/* 6. Animations */
@keyframes slideIn { ... }
@keyframes shake { ... }

/* 7. Responsive Design */
@media (max-width: 768px) { ... }
@media (max-width: 600px) { ... }
```

### Phần HTML (Trong thẻ `<body>`)

```html
<!-- 1. Authentication Pages -->
<div id="loginPage" class="login-container">
    <!-- Login Form -->
</div>

<div id="registerPage" class="register-container" style="display: none;">
    <!-- Register Form -->
</div>

<div id="forgotPasswordPage" class="forgot-password-container" style="display: none;">
    <!-- Forgot Password Steps -->
</div>

<!-- 2. Main Application -->
<div id="mainApp" class="main-app" style="display: none;">
    <!-- Header -->
    <!-- Statistics Cards -->
    <!-- Filter Buttons -->
    <!-- Orders List -->
</div>

<!-- 3. Modals -->
<div id="createOrderModal" class="modal">
    <!-- Create Order Form -->
</div>

<div id="profileModal" class="modal">
    <!-- Profile Settings -->
</div>
```

### Phần JavaScript (Trong thẻ `<script>`)

```javascript
// 1. Data Models
let users = [];
let orders = [];
let products = [];
let currentUser = null;

// 2. Initialization
function initApp() { ... }
function loadData() { ... }
function initDefaultData() { ... }

// 3. Authentication Functions
function login() { ... }
function register() { ... }
function logout() { ... }
function forgotPassword() { ... }

// 4. Navigation Functions
function showLoginPage() { ... }
function showRegisterPage() { ... }
function showForgotPasswordPage() { ... }
function showMainApp() { ... }

// 5. Order Management
function createOrder() { ... }
function updateOrderStatus() { ... }
function cancelOrder() { ... }
function deleteOrder() { ... }
function filterOrders() { ... }

// 6. UI Rendering
function renderOrders() { ... }
function renderStatistics() { ... }
function renderOrderItem() { ... }

// 7. Profile Management
function openProfileModal() { ... }
function updateProfile() { ... }
function changePassword() { ... }
function manageUsers() { ... }

// 8. Utility Functions
function formatCurrency() { ... }
function formatDate() { ... }
function saveToLocalStorage() { ... }
function loadFromLocalStorage() { ... }

// 9. Event Listeners
document.addEventListener('DOMContentLoaded', initApp);
```

### Tổ chức code theo module (Conceptual)

Mặc dù tất cả code trong 1 file, nhưng được tổ chức theo các module logic:

**Authentication Module**
- Login, Register, Logout
- Password Recovery
- Session Management

**Order Management Module**
- CRUD Operations
- Status Management
- Filtering và Sorting

**User Management Module**
- Profile Management
- Password Change
- Role Management (Admin)

**Product Management Module**
- Product List
- Inventory Management

**UI Module**
- Rendering Functions
- Modal Management
- Event Handlers

**Data Module**
- LocalStorage Operations
- Data Validation
- Data Models

**Utility Module**
- Currency Formatting
- Date Formatting
- Helper Functions

### Naming Conventions

**JavaScript:**
```javascript
// Variables: camelCase
let currentUser;
let ordersList;

// Functions:camelCase với động từ
function getUserById() {}
function createOrder() {}
function renderStatistics() {}

// Constants: UPPER_CASE
const MAX_ORDERS = 100;
const DEFAULT_STATUS = 'pending';

// DOM Elements: camelCase với prefix
const loginBtn = document.getElementById('loginBtn');
const orderModal = document.getElementById('orderModal');
```

**CSS:**
```css
/* Classes: kebab-case */
.login-container {}
.stat-card {}
.order-item {}

/* IDs: camelCase */
#loginPage {}
#mainApp {}
#createOrderModal {}

/* Modifiers: BEM-like */
.btn--primary {}
.btn--danger {}
.stat-card--active {}
```

**HTML:**
```html
<!-- IDs: camelCase -->
<div id="loginPage"></div>
<button id="submitBtn"></button>

<!-- Classes: kebab-case -->
<div class="login-container"></div>
<span class="badge badge-pending"></span>

<!-- Data attributes: kebab-case -->
<button data-order-id="123"></button>
<div data-status="pending"></div>
```

---

## API và cấu trúc dữ liệu

### LocalStorage Keys

```javascript
'users'        // Array of User objects
'orders'       // Array of Order objects
'products'     // Array of Product objects
'currentUser'  // Current logged-in User object
```

### Data Models

#### User Model

```javascript
{
  username: string,          // Required, unique, min 3 chars
  password: string,          // Required, min 6 chars (stored as plain text*)
  role: string,              // 'admin' | 'user'
  fullName: string,          // Required, min 3 chars
  phone: string,             // Optional, 10-11 digits
  email: string,             // Optional, valid email format
  dateOfBirth: string,       // Optional, ISO date string
  createdAt: number          // Timestamp
}
```

**Ví dụ:**
```javascript
{
  username: "quynh_anh",
  password: "123456",
  role: "user",
  fullName: "Quỳnh Anh",
  phone: "0987654321",
  email: "quynhanh@example.com",
  dateOfBirth: "2000-01-01",
  createdAt: 1732531200000
}
```

**Lưu ý bảo mật:**
- *Trong phiên bản hiện tại, mật khẩu được lưu dạng plain text
- Khuyến nghị: Hash mật khẩu trước khi lưu (bcrypt, sha256)
- Đây là điểm cần cải thiện cho production

#### Order Model

```javascript
{
  id: string,                // Unique, timestamp-based
  customerName: string,      // Required
  customerPhone: string,     // Required
  customerAddress: string,   // Required
  items: [                   // Array of OrderItem
    {
      productId: number,
      productName: string,
      price: number,
      quantity: number
    }
  ],
  total: number,             // Auto-calculated
  status: string,            // 'pending' | 'processing' | 'completed' | 'cancelled'
  createdBy: string,         // Username
  createdAt: number          // Timestamp
}
```

**Ví dụ:**
```javascript
{
  id: "1732531200000",
  customerName: "Nguyễn Văn A",
  customerPhone: "0987654321",
  customerAddress: "123 Đường ABC, Quận 1, TP.HCM",
  items: [
    {
      productId: 1,
      productName: "Laptop Dell XPS 13",
      price: 25000000,
      quantity: 1
    },
    {
      productId: 4,
      productName: "AirPods Pro",
      price: 6000000,
      quantity: 2
    }
  ],
  total: 37000000,
  status: "pending",
  createdBy: "quynh_anh",
  createdAt: 1732531200000
}
```

#### Product Model

```javascript
{
  id: number,                // Unique identifier
  name: string,              // Product name
  price: number,             // Price in VND
  stock: number              // Available quantity
}
```

**Ví dụ:**
```javascript
{
  id: 1,
  name: "Laptop Dell XPS 13",
  price: 25000000,
  stock: 10
}
```

#### Current User (Session)

```javascript
{
  username: string,
  role: string,
  fullName: string
}
```

**Ví dụ:**
```javascript
{
  username: "quynh_anh",
  role: "user",
  fullName: "Quỳnh Anh"
}
```

### CRUD Operations

#### Create Operations

**Create User (Register):**
```javascript
function register() {
  const newUser = {
    username: username,
    password: password,
    role: 'user',              // Default role
    fullName: fullName,
    phone: '',
    email: '',
    dateOfBirth: '',
    createdAt: Date.now()
  };
  
  users.push(newUser);
  localStorage.setItem('users', JSON.stringify(users));
}
```

**Create Order:**
```javascript
function createOrder() {
  const newOrder = {
    id: Date.now().toString(),
    customerName: customerName,
    customerPhone: customerPhone,
    customerAddress: customerAddress,
    items: selectedItems,
    total: calculateTotal(selectedItems),
    status: 'pending',
    createdBy: currentUser.username,
    createdAt: Date.now()
  };
  
  orders.push(newOrder);
  updateProductStock(selectedItems, 'decrease');
  localStorage.setItem('orders', JSON.stringify(orders));
}
```

#### Read Operations

**Get All Orders:**
```javascript
function getAllOrders() {
  return JSON.parse(localStorage.getItem('orders')) || [];
}
```

**Get Orders by User:**
```javascript
function getOrdersByUser(username) {
  return orders.filter(order => order.createdBy === username);
}
```

**Get Orders by Status:**
```javascript
function getOrdersByStatus(status) {
  const userOrders = currentUser.role === 'admin' 
    ? orders 
    : orders.filter(o => o.createdBy === currentUser.username);
  
  return status === 'all' 
    ? userOrders 
    : userOrders.filter(o => o.status === status);
}
```

**Get Order by ID:**
```javascript
function getOrderById(orderId) {
  return orders.find(order => order.id === orderId);
}
```

#### Update Operations

**Update Order Status:**
```javascript
function updateOrderStatus(orderId, newStatus) {
  const order = orders.find(o => o.id === orderId);
  if (order) {
    order.status = newStatus;
    localStorage.setItem('orders', JSON.stringify(orders));
  }
}
```

**Update User Profile:**
```javascript
function updateProfile(username, updates) {
  const user = users.find(u => u.username === username);
  if (user) {
    Object.assign(user, updates);
    localStorage.setItem('users', JSON.stringify(users));
    
    // Update session if current user
    if (currentUser.username === username) {
      currentUser.fullName = user.fullName;
      localStorage.setItem('currentUser', JSON.stringify(currentUser));
    }
  }
}
```

**Change Password:**
```javascript
function changePassword(username, oldPassword, newPassword) {
  const user = users.find(u => u.username === username);
  if (user && user.password === oldPassword) {
    user.password = newPassword;
    localStorage.setItem('users', JSON.stringify(users));
    return true;
  }
  return false;
}
```

**Update User Role:**
```javascript
function updateUserRole(username, newRole) {
  const user = users.find(u => u.username === username);
  if (user) {
    user.role = newRole;
    localStorage.setItem('users', JSON.stringify(users));
  }
}
```

#### Delete Operations

**Delete Order:**
```javascript
function deleteOrder(orderId) {
  const orderIndex = orders.findIndex(o => o.id === orderId);
  if (orderIndex !== -1) {
    const order = orders[orderIndex];
    
    // Restore stock if pending
    if (order.status === 'pending') {
      updateProductStock(order.items, 'increase');
    }
    
    orders.splice(orderIndex, 1);
    localStorage.setItem('orders', JSON.stringify(orders));
  }
}
```

**Cancel Order:**
```javascript
function cancelOrder(orderId) {
  const order = orders.find(o => o.id === orderId);
  if (order) {
    order.status = 'cancelled';
    
    // Restore stock if pending
    if (order.status === 'pending') {
      updateProductStock(order.items, 'increase');
    }
    
    localStorage.setItem('orders', JSON.stringify(orders));
  }
}
```

### Helper Functions

**Update Product Stock:**
```javascript
function updateProductStock(items, action) {
  items.forEach(item => {
    const product = products.find(p => p.id === item.productId);
    if (product) {
      if (action === 'decrease') {
        product.stock -= item.quantity;
      } else if (action === 'increase') {
        product.stock += item.quantity;
      }
    }
  });
  localStorage.setItem('products', JSON.stringify(products));
}
```

**Calculate Order Total:**
```javascript
function calculateTotal(items) {
  return items.reduce((sum, item) => {
    return sum + (item.price * item.quantity);
  }, 0);
}
```

**Format Currency:**
```javascript
function formatCurrency(amount) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(amount);
}
```

**Format Date:**
```javascript
function formatDate(timestamp) {
  const date = new Date(timestamp);
  const day = String(date.getDate()).padStart(2, '0');
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const year = date.getFullYear();
  const hours = String(date.getHours()).padStart(2, '0');
  const minutes = String(date.getMinutes()).padStart(2, '0');
  
  return `${day}/${month}/${year} ${hours}:${minutes}`;
}
```

### Statistics Calculations

**Calculate Statistics:**
```javascript
function calculateStatistics() {
  const userOrders = currentUser.role === 'admin'
    ? orders
    : orders.filter(o => o.createdBy === currentUser.username);
  
  const totalOrders = userOrders.length;
  const pendingOrders = userOrders.filter(o => o.status === 'pending').length;
  const completedOrders = userOrders.filter(o => o.status === 'completed').length;
  
  const revenue = userOrders
    .filter(o => o.status === 'completed')
    .reduce((sum, o) => sum + o.total, 0);
  
  return {
    totalOrders,
    pendingOrders,
    completedOrders,
    revenue
  };
}
```

### Data Validation

**Validate User Registration:**
```javascript
function validateRegistration(username, fullName, password, confirmPassword) {
  if (!username || username.length < 3) {
    return { valid: false, message: 'Tên đăng nhập phải có ít nhất 3 ký tự' };
  }
  
  if (users.find(u => u.username === username)) {
    return { valid: false, message: 'Tên đăng nhập đã tồn tại' };
  }
  
  if (!fullName || fullName.length < 3) {
    return { valid: false, message: 'Họ tên phải có ít nhất 3 ký tự' };
  }
  
  if (!password || password.length < 6) {
    return { valid: false, message: 'Mật khẩu phải có ít nhất 6 ký tự' };
  }
  
  if (password !== confirmPassword) {
    return { valid: false, message: 'Mật khẩu xác nhận không khớp' };
  }
  
  return { valid: true };
}
```

**Validate Order Creation:**
```javascript
function validateOrder(customerName, customerPhone, customerAddress, items) {
  if (!customerName || customerName.trim() === '') {
    return { valid: false, message: 'Vui lòng nhập tên khách hàng' };
  }
  
  if (!customerPhone || customerPhone.trim() === '') {
    return { valid: false, message: 'Vui lòng nhập số điện thoại' };
  }
  
  if (!customerAddress || customerAddress.trim() === '') {
    return { valid: false, message: 'Vui lòng nhập địa chỉ giao hàng' };
  }
  
  if (!items || items.length === 0) {
    return { valid: false, message: 'Vui lòng thêm ít nhất một sản phẩm' };
  }
  
  return { valid: true };
}
```

---

## Bảo mật

### Phân tích rủi ro

#### Rủi ro hiện tại

**1. Lưu trữ mật khẩu dạng plain text**
- **Mức độ**: Nghiêm trọng
- **Mô tả**: Mật khẩu được lưu trực tiếp không mã hóa trong LocalStorage
- **Tác động**: Ai có quyền truy cập DevTools đều có thể xem mật khẩu
- **Giải pháp**: Hash mật khẩu trước khi lưu (bcrypt, SHA-256)

**2. LocalStorage có thể bị truy cập bởi JavaScript**
- **Mức độ**: Trung bình
- **Mô tả**: XSS attacks có thể đọc LocalStorage
- **Tác động**: Đánh cắp session, dữ liệu người dùng
- **Giải pháp**: Implement Content Security Policy, sanitize inputs

**3. Không có rate limiting**
- **Mức độ**: Trung bình
- **Mô tả**: Không giới hạn số lần đăng nhập sai
- **Tác động**: Brute force attacks
- **Giải pháp**: Implement login attempt limiting

**4. Session không expire**
- **Mức độ**: Thấp-Trung bình
- **Mô tả**: Session tồn tại mãi mãi trong LocalStorage
- **Tác động**: Security risk khi dùng máy công cộng
- **Giải pháp**: Implement session timeout

### Best Practices hiện tại

#### Input Validation

**Client-side validation:**
```javascript
// Validate username
if (!/^[a-zA-Z0-9_]{3,}$/.test(username)) {
  return 'Username chỉ chứa chữ cái, số và gạch dưới, tối thiểu 3 ký tự';
}

// Validate password
if (password.length < 6) {
  return 'Mật khẩu phải có ít nhất 6 ký tự';
}

// Validate phone
if (!/^[0-9]{10,11}$/.test(phone)) {
  return 'Số điện thoại phải có 10-11 chữ số';
}

// Validate email
if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
  return 'Email không hợp lệ';
}
```

#### Authorization Checks

**Role-based access control:**
```javascript
function canProcessOrder(user) {
  return user.role === 'admin';
}

function canDeleteOrder(user) {
  return user.role === 'admin';
}

function canCancelOrder(user, order) {
  if (user.role === 'admin') return true;
  return user.username === order.createdBy && order.status === 'pending';
}

function canViewOrder(user, order) {
  if (user.role === 'admin') return true;
  return user.username === order.createdBy;
}
```

#### Session Management

**Check session on page load:**
```javascript
function checkSession() {
  const currentUser = JSON.parse(localStorage.getItem('currentUser'));
  if (currentUser) {
    // User is logged in
    showMainApp();
  } else {
    // User is not logged in
    showLoginPage();
  }
}
```

**Logout properly:**
```javascript
function logout() {
  localStorage.removeItem('currentUser');
  currentUser = null;
  showLoginPage();
}
```

### Khuyến nghị cải thiện

#### 1. Hash mật khẩu

**Sử dụng thư viện hash:**
```javascript
// Option 1: Using CryptoJS (recommended)
<script src="https://cdnjs.cloudflare.com/ajax/libs/crypto-js/4.1.1/crypto-js.min.js"></script>

function hashPassword(password) {
  return CryptoJS.SHA256(password).toString();
}

// When registering
const hashedPassword = hashPassword(password);
user.password = hashedPassword;

// When logging in
const hashedInput = hashPassword(inputPassword);
if (user.password === hashedInput) {
  // Login successful
}
```

**Option 2: Using Web Crypto API:**
```javascript
async function hashPassword(password) {
  const encoder = new TextEncoder();
  const data = encoder.encode(password);
  const hash = await crypto.subtle.digest('SHA-256', data);
  return Array.from(new Uint8Array(hash))
    .map(b => b.toString(16).padStart(2, '0'))
    .join('');
}
```

#### 2. Implement Session Timeout

```javascript
const SESSION_TIMEOUT = 30 * 60 * 1000; // 30 minutes

function createSession(user) {
  const session = {
    ...user,
    expiresAt: Date.now() + SESSION_TIMEOUT
  };
  localStorage.setItem('currentUser', JSON.stringify(session));
}

function checkSession() {
  const session = JSON.parse(localStorage.getItem('currentUser'));
  if (!session) return false;
  
  if (Date.now() > session.expiresAt) {
    logout();
    alert('Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.');
    return false;
  }
  
  // Extend session
  session.expiresAt = Date.now() + SESSION_TIMEOUT;
  localStorage.setItem('currentUser', JSON.stringify(session));
  return true;
}
```

#### 3. Rate Limiting

```javascript
const MAX_LOGIN_ATTEMPTS = 5;
const LOCKOUT_DURATION = 15 * 60 * 1000; // 15 minutes

function checkLoginAttempts(username) {
  const attempts = JSON.parse(localStorage.getItem('loginAttempts') || '{}');
  const userAttempts = attempts[username];
  
  if (!userAttempts) return true;
  
  if (userAttempts.count >= MAX_LOGIN_ATTEMPTS) {
    if (Date.now() < userAttempts.lockedUntil) {
      const remainingMinutes = Math.ceil((userAttempts.lockedUntil - Date.now()) / 60000);
      alert(`Tài khoản bị khóa. Thử lại sau ${remainingMinutes} phút.`);
      return false;
    } else {
      // Reset attempts
      delete attempts[username];
      localStorage.setItem('loginAttempts', JSON.stringify(attempts));
    }
  }
  
  return true;
}

function recordFailedLogin(username) {
  const attempts = JSON.parse(localStorage.getItem('loginAttempts') || '{}');
  
  if (!attempts[username]) {
    attempts[username] = { count: 1 };
  } else {
    attempts[username].count++;
  }
  
  if (attempts[username].count >= MAX_LOGIN_ATTEMPTS) {
    attempts[username].lockedUntil = Date.now() + LOCKOUT_DURATION;
  }
  
  localStorage.setItem('loginAttempts', JSON.stringify(attempts));
}
```

#### 4. Input Sanitization

```javascript
function sanitizeHTML(text) {
  const div = document.createElement('div');
  div.textContent = text;
  return div.innerHTML;
}

function sanitizeInput(input) {
  return input
    .trim()
    .replace(/[<>]/g, '') // Remove < and >
    .substring(0, 255);   // Limit length
}

// Usage
const sanitizedName = sanitizeInput(customerName);
```

#### 5. HTTPS Only (Production)

```javascript
// Force HTTPS
if (location.protocol !== 'https:' && location.hostname !== 'localhost') {
  location.replace(`https:${location.href.substring(location.protocol.length)}`);
}
```

### Security Checklist

**Đối với phiên bản hiện tại (Development):**
- [x] Input validation
- [x] Authorization checks
- [x] Session management
- [x] Password hashing
- [ ] Session timeout
- [ ] Rate limiting
- [x] Input sanitization
- [x] HTTPS only

**Đối với phiên bản production:**
- [x] Content Security Policy
- [ ] XSS protection
- [ ] CSRF protection
- [x] Security headers
- [ ] Regular security audits
- [x] Penetration testing

### Lưu ý quan trọng

**Ứng dụng này được thiết kế cho mục đích:**
- Học tập và nghiên cứu
- Demo và prototype
- Môi trường development

**KHÔNG khuyến nghị sử dụng cho:**
- Sản phẩm production
- Xử lý dữ liệu nhạy cảm
- Giao dịch tài chính
- Thông tin cá nhân quan trọng

**Để sử dụng trong production, cần:**
- Implement backend API
- Sử dụng database thật
- Hash mật khẩu đúng cách
- Implement tất cả security measures
- Có SSL/TLS certificate
- Regular security updates

---

## Tùy chỉnh

### Thay đổi sản phẩm mặc định

**Tìm và sửa trong file index.html:**

```javascript
// Tìm section: Data initialization
let products = [
    { id: 1, name: 'Laptop Dell XPS 13', price: 25000000, stock: 10 },
    { id: 2, name: 'iPhone 15 Pro', price: 28000000, stock: 15 },
    // Thêm sản phẩm mới hoặc sửa đổi
    { id: 6, name: 'MacBook Pro M3', price: 45000000, stock: 5 }
];
```

**Quy tắc:**
- ID phải unique
- Price là số nguyên (VND)
- Stock là số nguyên (không âm)

### Thay đổi màu sắc

#### Statistics Cards

```css
/* Tìm trong <style> */

/* Total Orders - Coral Gradient */
.stat-card.coral {
    background: linear-gradient(135deg, #your-color-1, #your-color-2);
}

/* Pending Orders - Sunset Gradient */
.stat-card.yellow {
    background: linear-gradient(135deg, #your-color-1, #your-color-2);
}

/* Completed Orders - Mint Gradient */
.stat-card.green {
    background: linear-gradient(135deg, #your-color-1, #your-color-2);
}

/* Revenue - Purple-Blue Gradient */
.stat-card.blue {
    background: linear-gradient(135deg, #your-color-1, #your-color-2);
}
```

#### Status Badges

```css
/* Pending */
.status-pending {
    background: #your-color;
    color: #your-text-color;
}

/* Processing */
.status-processing {
    background: #your-color;
    color: #your-text-color;
}

/* Completed */
.status-completed {
    background: #your-color;
    color: #your-text-color;
}

/* Cancelled */
.status-cancelled {
    background: #your-color;
    color: #your-text-color;
}
```

#### Primary Gradient

```css
/* Background chính */
body {
    background: linear-gradient(135deg, #your-color-1, #your-color-2);
}

/* Buttons */
.btn-primary {
    background: linear-gradient(135deg, #your-color-1, #your-color-2);
}
```

### Thêm trạng thái đơn hàng mới

**Ví dụ: Thêm trạng thái "Đang giao hàng" (Shipping)**

**Bước 1: Thêm CSS cho badge:**
```css
.status-shipping {
    background: #fff3cd;
    color: #856404;
    padding: 5px 10px;
    border-radius: 5px;
    font-size: 12px;
}
```

**Bước 2: Thêm text hiển thị:**
```javascript
function getStatusText(status) {
    const statusTexts = {
        'pending': '⏳ Chờ xử lý',
        'processing': '🔄 Đang xử lý',
        'shipping': '🚚 Đang giao hàng',  // New status
        'completed': '✅ Hoàn thành',
        'cancelled': '❌ Đã hủy'
    };
    return statusTexts[status] || status;
}
```

**Bước 3: Thêm button xử lý:**
```javascript
function renderOrderItem(order) {
    // ... existing code ...
    
    if (order.status === 'processing' && currentUser.role === 'admin') {
        actionButtons += `
            <button onclick="updateOrderStatus('${order.id}', 'shipping')" 
                    class="btn btn-info">
                Giao hàng
            </button>
        `;
    }
    
    if (order.status === 'shipping' && currentUser.role === 'admin') {
        actionButtons += `
            <button onclick="updateOrderStatus('${order.id}', 'completed')" 
                    class="btn btn-success">
                Hoàn thành
            </button>
        `;
    }
    
    // ... existing code ...
}
```

**Bước 4: Thêm filter button:**
```html
<button onclick="filterOrders('shipping')" class="filter-btn" data-status="shipping">
    Đang giao hàng
</button>
```

### Thêm trường thông tin mới

**Ví dụ: Thêm "Ghi chú" cho đơn hàng**

**Bước 1: Cập nhật Order Model:**
```javascript
const newOrder = {
    // ... existing fields ...
    notes: notes || '',  // New field
    createdAt: Date.now()
};
```

**Bước 2: Thêm input field:**
```html
<div class="form-group">
    <label>Ghi chú:</label>
    <textarea id="orderNotes" 
              placeholder="Ghi chú về đơn hàng (không bắt buộc)"
              rows="3"></textarea>
</div>
```

**Bước 3: Lấy giá trị khi tạo đơn:**
```javascript
function createOrder() {
    const notes = document.getElementById('orderNotes').value;
    
    const newOrder = {
        // ... existing fields ...
        notes: notes
    };
    
    // ... rest of code ...
}
```

**Bước 4: Hiển thị trong chi tiết đơn:**
```javascript
function renderOrderItem(order) {
    // ... existing code ...
    
    if (order.notes) {
        html += `
            <div class="order-notes">
                <strong>Ghi chú:</strong>
                <p>${order.notes}</p>
            </div>
        `;
    }
    
    // ... rest of code ...
}
```

### Tùy chỉnh format tiền tệ

**Thay đổi từ VND sang USD:**

```javascript
function formatCurrency(amount) {
    return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
    }).format(amount);
}
```

**Thêm tùy chọn hiển thị:**

```javascript
function formatCurrency(amount, options = {}) {
    const defaults = {
        locale: 'vi-VN',
        currency: 'VND',
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
    };
    
    const config = { ...defaults, ...options };
    
    return new Intl.NumberFormat(config.locale, {
        style: 'currency',
        currency: config.currency,
        minimumFractionDigits: config.minimumFractionDigits,
        maximumFractionDigits: config.maximumFractionDigits
    }).format(amount);
}

// Usage
formatCurrency(25000000);  // 25.000.000₫
formatCurrency(25000000, { currency: 'USD', locale: 'en-US' });  // $25,000,000.00
```

### Thay đổi font chữ

**Sử dụng Google Fonts:**

```html
<!-- Thêm vào <head> -->
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet">
```

```css
/* Cập nhật trong <style> */
body {
    font-family: 'Roboto', sans-serif;
}
```

### Thêm chức năng Export/Import

**Export data to JSON:**

```javascript
function exportData() {
    const data = {
        users: users,
        orders: orders,
        products: products,
        exportedAt: new Date().toISOString()
    };
    
    const dataStr = JSON.stringify(data, null, 2);
    const dataBlob = new Blob([dataStr], { type: 'application/json' });
    
    const url = URL.createObjectURL(dataBlob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `order-system-backup-${Date.now()}.json`;
    link.click();
    
    URL.revokeObjectURL(url);
}
```

**Import data from JSON:**

```javascript
function importData() {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = 'application/json';
    
    input.onchange = (e) => {
        const file = e.target.files[0];
        const reader = new FileReader();
        
        reader.onload = (event) => {
            try {
                const data = JSON.parse(event.target.result);
                
                if (confirm('Import dữ liệu sẽ ghi đè dữ liệu hiện tại. Bạn có chắc chắn?')) {
                    users = data.users || [];
                    orders = data.orders || [];
                    products = data.products || [];
                    
                    saveToLocalStorage();
                    renderOrders();
                    renderStatistics();
                    
                    alert('Import thành công!');
                }
            } catch (error) {
                alert('File không hợp lệ!');
            }
        };
        
        reader.readAsText(file);
    };
    
    input.click();
}
```

---

## Xử lý sự cố

### Sự cố phổ biến

#### 1. Dữ liệu không lưu được

**Triệu chứng:**
- Tạo đơn hàng nhưng sau khi refresh mất hết
- Cập nhật profile nhưng không thay đổi
- Đăng ký tài khoản mới nhưng không thể đăng nhập

**Nguyên nhân:**
- LocalStorage bị disable
- Trình duyệt ở chế độ Private/Incognito
- Dung lượng LocalStorage đã đầy
- Lỗi JavaScript block việc lưu

**Giải pháp:**

```javascript
// Kiểm tra LocalStorage có hoạt động không
function checkLocalStorage() {
    try {
        const test = '__localStorage_test__';
        localStorage.setItem(test, test);
        localStorage.removeItem(test);
        return true;
    } catch (e) {
        return false;
    }
}

if (!checkLocalStorage()) {
    alert('LocalStorage không khả dụng. Vui lòng kiểm tra cài đặt trình duyệt.');
}
```

**Các bước khắc phục:**
1. Kiểm tra console log (F12 > Console)
2. Kiểm tra LocalStorage (F12 > Application > LocalStorage)
3. Thử clear LocalStorage: `localStorage.clear()`
4. Thử trình duyệt khác
5. Thoát chế độ Private/Incognito

#### 2. Giao diện không hiển thị đúng

**Triệu chứng:**
- Icons không hiển thị
- Màu sắc lỗi
- Layout bị vỡ
- CSS không apply

**Nguyên nhân:**
- Font Awesome CDN không load được
- Lỗi CSS syntax
- Browser cache cũ
- Không hỗ trợ CSS3

**Giải pháp:**

1. **Kiểm tra Font Awesome CDN:**
```javascript
// Thêm vào <script>
window.addEventListener('load', function() {
    const faTest = document.createElement('i');
    faTest.className = 'fa fa-user';
    document.body.appendChild(faTest);
    
    const computed = window.getComputedStyle(faTest);
    if (computed.fontFamily.indexOf('FontAwesome') === -1) {
        alert('Font Awesome không load được. Kiểm tra kết nối internet.');
    }
    
    document.body.removeChild(faTest);
});
```

2. **Clear cache:**
- Chrome: Ctrl + Shift + Delete
- Firefox: Ctrl + Shift + Delete
- Edge: Ctrl + Shift + Delete

3. **Hard refresh:**
- Windows: Ctrl + F5
- Mac: Cmd + Shift + R

#### 3. Không đăng nhập được

**Triệu chứng:**
- Nhập đúng username/password nhưng báo sai
- Click đăng nhập không có phản ứng
- Bị logout ngay sau khi login

**Nguyên nhân:**
- Dữ liệu users bị corrupt
- Session không được tạo
- JavaScript error
- LocalStorage bị clear

**Giải pháp:**

1. **Reset dữ liệu mặc định:**
```javascript
// Chạy trong Console (F12)
localStorage.clear();
location.reload();
```

2. **Tạo lại tài khoản admin:**
```javascript
// Chạy trong Console
const users = [
    {
        username: 'admin',
        password: 'admin123',
        role: 'admin',
        fullName: 'Administrator',
        phone: '',
        email: '',
        dateOfBirth: '',
        createdAt: Date.now()
    }
];
localStorage.setItem('users', JSON.stringify(users));
location.reload();
```

3. **Kiểm tra console errors:**
- Mở DevTools (F12)
- Check tab Console có error không
- Fix theo error message

#### 4. Lỗi khi tạo đơn hàng

**Triệu chứng:**
- Click "Tạo đơn hàng" không có phản ứng
- Báo lỗi "Không đủ hàng" dù stock còn
- Tổng tiền không tính đúng

**Nguyên nhân:**
- Validation fail
- Product data bị corrupt
- JavaScript error
- Stock đã hết

**Giải pháp:**

1. **Kiểm tra products data:**
```javascript
// Chạy trong Console
const products = JSON.parse(localStorage.getItem('products'));
console.log(products);

// Nếu products null hoặc lỗi, reset:
const defaultProducts = [
    { id: 1, name: 'Laptop Dell XPS 13', price: 25000000, stock: 10 },
    { id: 2, name: 'iPhone 15 Pro', price: 28000000, stock: 15 },
    { id: 3, name: 'Samsung Galaxy S24', price: 22000000, stock: 20 },
    { id: 4, name: 'AirPods Pro', price: 6000000, stock: 30 },
    { id: 5, name: 'iPad Air', price: 15000000, stock: 12 }
];
localStorage.setItem('products', JSON.stringify(defaultProducts));
location.reload();
```

2. **Kiểm tra validation:**
- Đảm bảo điền đầy đủ thông tin khách hàng
- Đảm bảo đã thêm ít nhất 1 sản phẩm
- Kiểm tra số lượng không vượt quá stock

#### 5. Statistics không cập nhật

**Triệu chứng:**
- Tạo đơn mới nhưng số liệu không thay đổi
- Doanh thu không đúng
- Số đơn hoàn thành sai

**Nguyên nhân:**
- Function `renderStatistics()` không được gọi
- Orders data bị corrupt
- Filter đang active

**Giải pháp:**

1. **Force update statistics:**
```javascript
// Chạy trong Console
renderStatistics();
```

2. **Kiểm tra orders data:**
```javascript
const orders = JSON.parse(localStorage.getItem('orders'));
console.log(orders);

// Kiểm tra các trường bắt buộc
orders.forEach(order => {
    console.log({
        id: order.id,
        status: order.status,
        total: order.total,
        createdBy: order.createdBy
    });
});
```

3. **Clear filter:**
- Click nút "Tất cả" để bỏ filter
- Refresh trang

### Debug Tools

**Console Commands hữu ích:**

```javascript
// Xem tất cả dữ liệu
console.log('Users:', JSON.parse(localStorage.getItem('users')));
console.log('Orders:', JSON.parse(localStorage.getItem('orders')));
console.log('Products:', JSON.parse(localStorage.getItem('products')));
console.log('Current User:', JSON.parse(localStorage.getItem('currentUser')));

// Xem dung lượng LocalStorage
let totalSize = 0;
for (let key in localStorage) {
    if (localStorage.hasOwnProperty(key)) {
        totalSize += localStorage[key].length + key.length;
    }
}
console.log('LocalStorage size:', (totalSize / 1024).toFixed(2), 'KB');

// Reset toàn bộ dữ liệu
localStorage.clear();
location.reload();

// Backup dữ liệu
const backup = {
    users: localStorage.getItem('users'),
    orders: localStorage.getItem('orders'),
    products: localStorage.getItem('products')
};
console.log('Backup:', JSON.stringify(backup));

// Restore dữ liệu
// Copy backup string và paste vào biến backupData
const backupData = '...';  // Paste backup string here
const data = JSON.parse(backupData);
localStorage.setItem('users', data.users);
localStorage.setItem('orders', data.orders);
localStorage.setItem('products', data.products);
location.reload();
```

### Performance Issues

**Triệu chứng:**
- Trang load chậm
- UI lag khi có nhiều đơn hàng
- Browser bị treo

**Giải pháp:**

1. **Giới hạn số lượng đơn hàng hiển thị:**
```javascript
const MAX_DISPLAY_ORDERS = 100;

function renderOrders() {
    const filteredOrders = getFilteredOrders();
    const displayOrders = filteredOrders.slice(0, MAX_DISPLAY_ORDERS);
    
    // Render only displayOrders
    // ...
    
    if (filteredOrders.length > MAX_DISPLAY_ORDERS) {
        // Show "Load more" button
    }
}
```

2. **Implement pagination:**
```javascript
let currentPage = 1;
const ordersPerPage = 20;

function paginateOrders(orders) {
    const start = (currentPage - 1) * ordersPerPage;
    const end = start + ordersPerPage;
    return orders.slice(start, end);
}
```

3. **Optimize rendering:**
```javascript
// Use DocumentFragment for batch DOM updates
function renderOrders() {
    const fragment = document.createDocumentFragment();
    const orders = getFilteredOrders();
    
    orders.forEach(order => {
        const orderElement = createOrderElement(order);
        fragment.appendChild(orderElement);
    });
    
    ordersList.innerHTML = '';
    ordersList.appendChild(fragment);
}
```

### Browser Compatibility

**Trình duyệt được hỗ trợ:**
- Chrome 90+
- Firefox 88+
- Edge 90+
- Safari 14+
- Opera 75+

**Trình duyệt không hỗ trợ:**
- Internet Explorer (tất cả phiên bản)
- Chrome < 90
- Firefox < 88

**Kiểm tra compatibility:**
```javascript
function checkBrowserCompatibility() {
    const isCompatible = {
        localStorage: typeof(Storage) !== 'undefined',
        es6: typeof Symbol !== 'undefined',
        flexbox: CSS.supports('display', 'flex'),
        grid: CSS.supports('display', 'grid')
    };
    
    const incompatible = Object.keys(isCompatible).filter(key => !isCompatible[key]);
    
    if (incompatible.length > 0) {
        alert(`Trình duyệt của bạn không hỗ trợ: ${incompatible.join(', ')}`);
        return false;
    }
    
    return true;
}
```

---

## Lộ trình phát triển

### Version 1.0 (Current) - November 2025

**Đã hoàn thành:**
- Hệ thống xác thực cơ bản (Login, Register, Forgot Password)
- Phân quyền 2 cấp (Admin, User)
- CRUD đơn hàng
- Quản lý trạng thái đơn hàng (4 trạng thái)
- Statistics dashboard
- Profile management
- User management (Admin)
- Responsive design
- LocalStorage persistence

### Version 1.1 (Planned) - Q1 2026

**Tính năng mới:**
- Search và advanced filtering
- Sort orders (by date, total, status)
- Export orders to CSV/Excel
- Print order invoice
- Order history log
- Customer management module
- Product categories
- Low stock alerts

**Cải thiện:**
- Password hashing
- Session timeout
- Rate limiting
- Input sanitization
- Better error messages
- Loading indicators
- Confirm dialogs
- Toast notifications

**Bug fixes:**
- Performance optimization cho nhiều đơn hàng
- Fix responsive issues trên một số thiết bị
- Cải thiện UX flows

### Version 2.0 (Planned) - Q2 2026

**Backend Integration:**
- Node.js + Express API
- MongoDB/PostgreSQL database
- JWT authentication
- RESTful API endpoints
- File upload support
- Email notifications
- SMS notifications
- Real-time updates (WebSocket)

**Frontend Enhancements:**
- Vue.js/React migration
- Component-based architecture
- State management (Vuex/Redux)
- Better code organization
- Unit tests
- E2E tests

**New Features:**
- Multi-user chat support
- Notifications center
- Activity feed
- Advanced analytics with charts
- Custom reports
- Dashboard widgets
- Dark mode
- Multi-language support

### Version 2.1 (Future) - Q3 2026

**Advanced Features:**
- Payment gateway integration
- Shipping provider integration
- Inventory management
- Supplier management
- Purchase orders
- Returns và refunds
- Loyalty program
- Discount codes/coupons
- Tax calculation
- Multi-currency support

**Mobile App:**
- React Native app
- iOS và Android
- Push notifications
- Offline mode
- Biometric authentication

**Enterprise Features:**
- Multi-store support
- Role-based permissions (granular)
- Audit logs
- Data export/import
- API for third-party integration
- Webhooks
- Custom fields
- Workflow automation

### Version 3.0 (Long-term) - 2027

**AI/ML Integration:**
- Demand forecasting
- Intelligent inventory management
- Customer behavior analysis
- Chatbot support
- Automated order processing
- Fraud detection

**Advanced Analytics:**
- Business intelligence dashboard
- Predictive analytics
- Customer lifetime value
- Cohort analysis
- A/B testing
- Funnel analysis

**Scalability:**
- Microservices architecture
- Docker containerization
- Kubernetes orchestration
- Cloud deployment (AWS/GCP/Azure)
- CDN integration
- Load balancing
- Auto-scaling

---

## Đóng góp

### Cách đóng góp

Mọi đóng góp đều được chào đón! Dưới đây là các cách bạn có thể đóng góp cho dự án:

**1. Báo cáo lỗi (Bug Reports)**
- Tạo issue mới trên GitHub
- Mô tả chi tiết lỗi
- Các bước tái tạo lỗi
- Screenshots nếu có
- Thông tin môi trường (browser, OS)

**2. Đề xuất tính năng (Feature Requests)**
- Tạo issue với label "enhancement"
- Mô tả tính năng mong muốn
- Use cases cụ thể
- Mockups nếu có

**3. Đóng góp code (Pull Requests)**
- Fork repository
- Tạo branch mới cho tính năng/fix
- Viết code theo coding standards
- Test kỹ lưỡng
- Tạo Pull Request với mô tả chi tiết

**4. Cải thiện tài liệu**
- Fix typos
- Thêm ví dụ
- Dịch sang ngôn ngữ khác
- Cải thiện giải thích

### Quy trình Pull Request

**Bước 1: Fork và Clone**
```bash
# Fork repository trên GitHub
# Clone về máy local
git clone https://github.com/your-username/order-management-system.git
cd order-management-system
```

**Bước 2: Tạo branch mới**
```bash
# Tạo branch cho feature/bugfix
git checkout -b feature/your-feature-name
# hoặc
git checkout -b fix/your-bug-fix
```

**Bước 3: Thực hiện thay đổi**
```bash
# Edit code
# Test thoroughly
# Commit changes
git add .
git commit -m "Add: Your descriptive commit message"
```

**Bước 4: Push và tạo PR**
```bash
# Push to your fork
git push origin feature/your-feature-name

# Tạo Pull Request trên GitHub
# Điền mô tả chi tiết
# Chờ review
```

### Coding Standards

**HTML:**
- Sử dụng semantic tags
- Proper indentation (2 spaces)
- Meaningful IDs và classes
- Comments cho các section phức tạp

**CSS:**
- Organized theo modules
- Use kebab-case cho class names
- Mobile-first approach
- Comments cho các sections

**JavaScript:**
- Use camelCase cho variables và functions
- Use UPPER_CASE cho constants
- Meaningful variable/function names
- JSDoc comments cho functions
- Error handling với try-catch
- Validation cho inputs

**Commit Messages:**
```
Add: Thêm tính năng mới
Fix: Sửa lỗi
Update: Cập nhật code/logic
Refactor: Tái cấu trúc code
Docs: Cập nhật tài liệu
Style: Format code, không thay đổi logic
Test: Thêm/sửa tests

# Ví dụ:
Add: Search functionality for orders
Fix: Statistics not updating after order creation
Update: Improve order validation logic
Refactor: Separate authentication module
Docs: Add API documentation
Style: Format CSS with consistent indentation
```

### Review Process

**Pull Request sẽ được review dựa trên:**
- Code quality
- Adherence to coding standards
- Test coverage
- Documentation
- Performance impact
- Security implications

**Timeline:**
- Initial review: 2-3 ngày
- Follow-up reviews: 1-2 ngày
- Merge: Sau khi được approve

### Community Guidelines

**Be respectful:**
- Tôn trọng mọi người
- Constructive feedback
- Không sử dụng ngôn ngữ công kích
- Chào đón newcomers

**Be collaborative:**
- Chia sẻ kiến thức
- Giúp đỡ lẫn nhau
- Open discussion
- Consensus building

**Be professional:**
- Follow code of conduct
- Stay on topic
- No spam
- Quality over quantity

---

## Giấy phép

```
Giấy phép MIT (MIT License)

Bản quyền © 2025 Quỳnh Anh

Quyền được cấp miễn phí cho bất kỳ cá nhân nào nhận được một bản sao của phần mềm này và các tài liệu liên quan (sau đây gọi là “phần mềm”), được phép sử dụng, sao chép, chỉnh sửa, hợp nhất, xuất bản, phân phối, cấp phép lại và/hoặc bán các bản sao của phần mềm, cũng như cho phép những người được cung cấp phần mềm thực hiện các quyền này, với các điều kiện sau:

Thông báo bản quyền ở trên và thông báo cấp phép này phải được bao gồm trong tất cả các bản sao hoặc các phần quan trọng của phần mềm.

Phần mềm được cung cấp “nguyên trạng”, không kèm theo bất kỳ bảo đảm nào, dù rõ ràng hay ngụ ý, bao gồm nhưng không giới hạn ở các bảo đảm về khả năng thương mại, sự phù hợp với mục đích cụ thể và không vi phạm quyền. Trong mọi trường hợp, tác giả hoặc chủ sở hữu bản quyền không chịu trách nhiệm đối với bất kỳ yêu cầu, thiệt hại hoặc trách nhiệm nào khác, dù phát sinh từ hợp đồng, hành vi xâm phạm dân sự hay nguyên nhân khác, liên quan đến, phát sinh từ hoặc liên quan đến việc sử dụng hoặc các giao dịch khác đối với phần mềm.
```

**Điều này có nghĩa là bạn có thể:**
- Sử dụng code cho mục đích thương mại
- Sửa đổi code
- Phân phối code
- Sử dụng code cho mục đích cá nhân
- Sublicense code

**Với điều kiện:**
- Phải bao gồm bản quyền và license notice
- Phải cung cấp thông báo về bất kỳ thay đổi nào

**Lưu ý:**
- Software được cung cấp "as is"
- Không có bảo hành
- Tác giả không chịu trách nhiệm về bất kỳ thiệt hại nào

---

## Liên hệ và hỗ trợ

### Tác giả

**Quỳnh Anh**
- Role: Full-stack Developer
- Focus: Frontend Development, UI/UX Design

### Kênh hỗ trợ

**GitHub Issues**
- URL: `https://github.com/Nam1414/CK-Back-End.git`
- Sử dụng cho: Bug reports, Feature reqgmail.com`
- Response time: 2-3 business days
- Sử dụng cho: Private inquiries, Security issues

### FAQ

**Q: Tôi có thể sử dụng code này cho dự án thương mại không?**
A: Có, theo MIT License bạn có thể sử dụng cho mục đích thương mại.

**Q: Tôi cần backend để chạy ứng dụng này không?**
A: Không, ứng dụng chạy hoàn toàn trên client-side với LocalStorage.

**Q: Làm sao để backup dữ liệu?**
A: Sử dụng function exportData() trong console hoặc copy LocalStorage data.

**Q: Ứng dụng có hoạt động offline không?**
A: Có, sau khi load lần đầu, ứng dụng hoạt động hoàn toàn offline.

**Q: Làm sao để reset về mặc định?**
A: Chạy `localStorage.clear()` trong console và refresh trang.

**Q: Tôi có thể đóng góp như thế nào?**
A: Xem section "Đóng góp" ở trên để biết chi tiết.

**Q: Làm sao để báo cáo security vulnerability?**
A: Email trực tiếp đến support@example.com với tiêu đề "Security Issue".

**Q: Có mobile app không?**
A: Chưa có, nhưng đã có trong roadmap cho version 2.1.

---

## Acknowledgments

### Công nghệ và thư viện

**Font Awesome**
- Icons library
- URL: https://fontawesome.com/
- License: Free for personal and commercial use

**Google Fonts**
- Typography inspiration
- URL: https://fonts.google.com/

**Gradient Hunt**
- Color gradient inspiration
- URL: https://gradienthunt.com/

### Inspiration

Dự án này lấy cảm hứng từ:
- Modern e-commerce platforms
- Material Design guidelines
- Best practices trong web development
- Community feedback và suggestions

### Special Thanks

Cảm ơn đến:
- Tất cả contributors đã đóng góp code
- Community members đã báo cáo bugs
- Users đã đưa ra feedback
- Beta testers đã giúp test ứng dụng

---

## Changelog

### Version 1.0.0 (November 25, 2025)

**Initial Release**

Features:
- Authentication system (Login, Register, Forgot Password)
- Role-based access control (Admin, User)
- Order management (CRUD operations)
- Order status management (4 statuses)
- Statistics dashboard
- Profile management
- User management for admins
- Responsive design
- LocalStorage persistence
- Vietnamese language support

---

**Lưu ý cuối cùng:**

Đây là dự án mã nguồn mở được tạo ra với mục đích học tập và chia sẻ kiến thức. Ứng dụng được thiết kế đơn giản nhưng đầy đủ tính năng để minh họa các khái niệm cơ bản trong web development.

Nếu bạn thấy dự án hữu ích, hãy:
- Cho một star trên GitHub
- Share với bạn bè đồng nghiệp
- Đóng góp để cải thiện dự án
- Sử dụng làm tài liệu học tập

**Made with love by Quỳnh Anh**

---

*Tài liệu này được cập nhật lần cuối: November 26, 2025*