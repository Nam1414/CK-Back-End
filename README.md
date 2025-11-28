# Modern Order Management System

**Phiên bản:** 2.0.0
**Tác giả:** Quỳnh Anh
**Giấy phép:** MIT
**Ngôn ngữ:** Vietnamese
**Repository:** https://github.com/Nam1414/CK-Back-End.git

---

## MỤC LỤC

1.  GIỚI THIỆU CHUNG VÀ KIẾN TRÚC HỆ THỐNG
2.  PHẦN 1: FRONTEND (CLIENT-SIDE)
    *   Triết lý thiết kế Zero Dependencies
    *   Phân tích kỹ thuật chi tiết
    *   Cấu trúc và Luồng dữ liệu Frontend
    *   Trải nghiệm người dùng (UX) và Giao diện (UI)
3.  PHẦN 2: BACKEND (SERVER-SIDE)
    *   Công nghệ cốt lõi và Nền tảng
    *   Phân tích Cấu trúc thư mục OrderManagementAPI
    *   Chi tiết triển khai các lớp (Layers)
    *   Cơ chế Bảo mật và Xác thực (Security & Auth)
    *   Cơ sở dữ liệu và Entity Framework Core
4.  HƯỚNG DẪN CÀI ĐẶT VÀ TRIỂN KHAI
    *   Yêu cầu môi trường
    *   Thiết lập Backend
    *   Thiết lập Frontend
    *   Kết nối hệ thống
5.  API DOCUMENTATION (TÀI LIỆU KỸ THUẬT API)
6.  LỘ TRÌNH PHÁT TRIỂN VÀ BẢO TRÌ

---

## 1. GIỚI THIỆU CHUNG VÀ KIẾN TRÚC HỆ THỐNG

Dự án Hệ thống Quản lý Đơn hàng là một giải pháp phần mềm toàn diện (Fullstack Solution) được phát triển nhằm giải quyết bài toán vận hành cốt lõi của các doanh nghiệp bán lẻ vừa và nhỏ. Hệ thống không chỉ đơn thuần là một công cụ ghi nhận đơn hàng, mà còn là một minh chứng kỹ thuật (Technical Showcase) về việc áp dụng các chuẩn mực thiết kế phần mềm hiện đại.

Hệ thống được xây dựng dựa trên kiến trúc phân tán (Decoupled Architecture), nơi mà Frontend và Backend được phát triển, triển khai và vận hành như hai thực thể độc lập, giao tiếp với nhau thông qua giao thức HTTP RESTful API.

**Lợi ích của kiến trúc này:**
*   **Khả năng mở rộng độc lập (Independent Scalability):** Có thể nâng cấp server Backend để chịu tải cao mà không ảnh hưởng đến trải nghiệm Frontend, hoặc cập nhật giao diện Frontend mà không cần dừng Server.
*   **Đa nền tảng (Multi-platform):** Backend API hiện tại không chỉ phục vụ cho Web App mà có thể dễ dàng mở rộng để phục vụ cho Mobile App (iOS/Android) hoặc các hệ thống bên thứ 3 trong tương lai.
*   **Chuyên môn hóa:** Cho phép tách biệt đội ngũ phát triển Frontend và Backend, tối ưu hóa quy trình làm việc.

---

## 2. PHẦN 1: FRONTEND (CLIENT-SIDE)

Phân hệ Frontend đóng vai trò là điểm tiếp xúc trực tiếp với người dùng cuối, nơi diễn ra mọi tương tác, hiển thị dữ liệu và thu thập thông tin đầu vào.

### 2.1. Triết lý thiết kế Zero Dependencies

Trong kỷ nguyên mà các Framework như React, Vue, hay Angular đang thống trị, dự án này lựa chọn một hướng đi khác biệt nhưng vững chắc: **Sử dụng Vanilla JavaScript (JavaScript thuần)**.

**Tại sao lại là Zero Dependencies?**
1.  **Hiệu năng tuyệt đối:** Bằng cách loại bỏ hoàn toàn các thư viện bên ngoài, chúng ta loại bỏ được thời gian tải (loading time), thời gian phân tích (parsing time) và bộ nhớ tiêu thụ cho các đoạn mã không cần thiết. Kích thước toàn bộ ứng dụng (HTML/CSS/JS) được tối ưu hóa ở mức cực thấp (dưới 100KB gzipped).
2.  **Sự bền vững (Long-term Stability):** Các Framework thay đổi liên tục, code viết bằng React version cũ có thể không chạy được trên version mới. Tuy nhiên, code viết bằng chuẩn Web Standard (ES6+) sẽ luôn chạy tốt trên trình duyệt trong hàng chục năm tới.
3.  **Kiểm soát sâu (Deep Control):** Lập trình viên buộc phải hiểu rõ cơ chế hoạt động của DOM (Document Object Model), Event Loop và Browser API thay vì phụ thuộc vào các lớp trừu tượng (Abstraction Layers) của Framework.

### 2.2. Phân tích kỹ thuật chi tiết

**Công nghệ sử dụng:**
*   **HTML5:** Sử dụng Semantic HTML (các thẻ có ý nghĩa ngữ nghĩa như header, nav, main, section, article) giúp tối ưu hóa cho SEO và Accessibility (khả năng truy cập).
*   **CSS3:**
    *   Sử dụng biến CSS (CSS Variables / Custom Properties) để quản lý màu sắc, spacing, typography toàn cục, cho phép dễ dàng thay đổi Theme.
    *   Layout: Sử dụng kết hợp Flexbox cho các thành phần một chiều (thanh điều hướng, form) và CSS Grid cho các bố cục hai chiều phức tạp (Dashboard, danh sách sản phẩm).
    *   Animation: Sử dụng CSS Keyframes và Transitions để tạo hiệu ứng mượt mà (60fps) mà không cần dùng JavaScript animation nặng nề.
*   **JavaScript (ES6+):**
    *   Module Pattern: Mặc dù viết chung trong một file index.tsx (giả lập môi trường module), tư duy lập trình được chia nhỏ thành các hàm chức năng riêng biệt.
    *   Async/Await: Xử lý các tác vụ bất đồng bộ (như gọi API, giả lập loading) một cách tuần tự, giúp code dễ đọc và dễ debug.
    *   DOM Manipulation: Sử dụng các API hiện đại như `querySelector`, `createElement`, `template literals` để render giao diện động.

### 2.3. Cấu trúc và Luồng dữ liệu Frontend

Mô hình dữ liệu tại Frontend được thiết kế để phản ánh cấu trúc dữ liệu tại Backend, giúp việc đồng bộ hóa trở nên dễ dàng.

**Cơ chế quản lý trạng thái (State Management):**
Thay vì dùng Redux hay Vuex, ứng dụng tự quản lý State thông qua các biến toàn cục và LocalStorage:
*   `currentUser`: Lưu thông tin phiên làm việc của người dùng.
*   `orders`: Mảng chứa danh sách đơn hàng.
*   `products`: Mảng chứa danh mục sản phẩm.

**Cơ chế Persistence (Lưu trữ bền vững):**
Dữ liệu được tự động đồng bộ xuống `LocalStorage` của trình duyệt sau mỗi thao tác (Thêm, Sửa, Xóa). Điều này đảm bảo tính năng **Offline-first**: Người dùng có thể tiếp tục xem dữ liệu ngay cả khi mất kết nối mạng (trong phiên bản demo này) hoặc giữ lại dữ liệu khi tải lại trang.

### 2.4. Trải nghiệm người dùng (UX) và Giao diện (UI)

*   **Hệ thống Xác thực (Authentication UI):**
    *   Form đăng nhập/đăng ký có khả năng **Real-time Validation**. Hệ thống lắng nghe sự kiện `input` hoặc `blur` để kiểm tra tính hợp lệ của dữ liệu ngay khi người dùng gõ phím (ví dụ: định dạng email, độ mạnh mật khẩu).
    *   Hiệu ứng phản hồi trực quan: Input đổi màu viền đỏ/xanh, hiển thị thông báo lỗi ngay bên dưới trường nhập liệu.

*   **Dashboard & Visualization:**
    *   Các thẻ thống kê (Statistic Cards) được tính toán tự động dựa trên dữ liệu thô. Ví dụ: Tổng doanh thu = Tổng giá trị các đơn hàng có trạng thái 'Completed'.
    *   Giao diện tự động điều chỉnh (Responsive) trên các thiết bị di động, tablet và desktop.

*   **Quy trình Đặt hàng (Order Flow):**
    *   Thiết kế dạng Wizard hoặc Modal, giúp người dùng tập trung vào từng bước nhập liệu.
    *   Tự động tính toán tổng tiền (Subtotal, Tax, Total) mỗi khi thay đổi số lượng sản phẩm.

---

## 3. PHẦN 2: BACKEND (SERVER-SIDE)

Đây là trái tim của hệ thống, nơi xử lý mọi logic nghiệp vụ phức tạp, đảm bảo tính toàn vẹn dữ liệu và bảo mật.

### 3.1. Công nghệ cốt lõi và Nền tảng

Backend được xây dựng trên nền tảng **Microsoft .NET 8 (ASP.NET Core Web API)**. Đây là phiên bản Long Term Support (LTS) mới nhất, mang lại hiệu năng vượt trội so với các phiên bản trước và các nền tảng khác như Node.js hay Java Spring Boot trong nhiều bài benchmark.

*   **Ngôn ngữ:** C# 12.
*   **Framework:** ASP.NET Core 8.0.
*   **ORM (Object-Relational Mapping):** Entity Framework Core 8.0.
*   **Database:** SQL Server (hoặc tương thích với PostgreSQL/MySQL nhờ EF Core).
*   **Authentication:** JWT (JSON Web Tokens) Bearer Authentication.
*   **API Documentation:** Swagger / OpenAPI.

### 3.2. Phân tích Cấu trúc thư mục OrderManagementAPI

Dự án tuân thủ cấu trúc phân tầng (Layered Architecture) nhưng được tổ chức gọn gàng để phù hợp với quy mô Microservices hoặc Monolithic hiện đại.

```text
OrderManagementAPI
│
├── Controllers
│   ├── AuthController.cs
│   ├── OrdersController.cs
│   ├── ProductsController.cs
│   └── UsersController.cs
│
├── DTOs
│   └── AppDtos.cs
│
├── Entity
│   ├── AppDbContext.cs
│   └── Migrations
│
├── Models
│   ├── User.cs
│   ├── Product.cs
│   ├── Order.cs
│   └── OrderDetail.cs
│
├── Services
│   ├── EmailService.cs
│   └── IEmailService.cs
│
├── Properties
│   └── launchSettings.json
│
├── wwwroot
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
├── OrderManagementAPI.csproj
├── OrderManagementAPI.http
└── OrderManagementAPI.sln
```

### 3.3. Chi tiết triển khai các lớp (Layers)

#### A. Controllers (Lớp điều khiển)
Nằm trong thư mục `Controllers`, đây là điểm tiếp nhận các HTTP Request từ Frontend.
*   **AuthController.cs:** Chịu trách nhiệm về bảo mật. Các API: `POST /api/auth/login`, `POST /api/auth/register`. Controller này không trả về dữ liệu nghiệp vụ mà trả về Token truy cập.
*   **OrdersController.cs:** Trái tim của nghiệp vụ bán hàng.
    *   Sử dụng các phương thức `GET` để lấy danh sách đơn hàng (hỗ trợ phân trang, lọc).
    *   `POST` để tạo đơn hàng mới. Đặc biệt, API này xử lý **Transaction** (Giao dịch): Đảm bảo rằng việc tạo Header đơn hàng và tạo các dòng chi tiết (OrderDetails) phải cùng thành công hoặc cùng thất bại, tránh dữ liệu rác.
*   **ProductsController.cs:** Cung cấp dữ liệu cho danh mục sản phẩm.
*   **UsersController.cs:** Dành cho Admin quản lý người dùng. Được bảo vệ bởi Attribute `[Authorize(Roles = "Admin")]`.

#### B. DTOs (Data Transfer Objects)
Nằm trong `DTOs/AppDtos.cs`.
Chúng ta không bao giờ trả về trực tiếp Entity của Database cho Client vì lý do bảo mật (lộ cấu trúc bảng) và hiệu năng (dữ liệu dư thừa, vòng lặp tham chiếu).
Thay vào đó, hệ thống sử dụng DTO. File `AppDtos.cs` có thể chứa các record hoặc class như:
*   `LoginDto`: Chỉ chứa Username, Password.
*   `OrderResponseDto`: Chứa thông tin đơn hàng đã được "làm phẳng" hoặc định dạng lại cho dễ hiển thị.

#### C. Entity & Models (Lớp dữ liệu)
*   **Models:** Định nghĩa các thực thể (User, Product, Order). Các class này ánh xạ trực tiếp 1-1 với các bảng trong SQL Server. Sử dụng Data Annotations (như `[Key]`, `[MaxLength]`) để định nghĩa ràng buộc.
*   **Entity/AppDbContext.cs:** Kế thừa từ `DbContext`. Đây là nơi cấu hình kết nối Database và Fluent API.
    *   Quản lý các `DbSet<T>`.
    *   Hàm `OnModelCreating`: Dùng để Seed Data (tạo dữ liệu mẫu admin/product ban đầu) và định nghĩa các mối quan hệ phức tạp (ví dụ: Composite Key cho bảng trung gian).

#### D. Services (Lớp dịch vụ)
Để tuân thủ nguyên lý Single Respo29, 2025*
