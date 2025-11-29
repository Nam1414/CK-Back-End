using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OrderManagementAPI.Entity; // <-- Để Program biết AppDbContext ở đâu
using System.Text.Json.Serialization;
using OrderManagementAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình kết nối Database với Entity Framework Core sử dụng SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Cấu hình xác thực JWT (Json Web Token) cho API
var key = Encoding.ASCII.GetBytes("Chuoi_Bi_Mat_Dai_It_Nhat_32_Ky_Tu_ABCXYZ_123456"); // Thay bằng chuỗi bảo mật riêng
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Xác định chuẩn authentication mặc định sử dụng JWT
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // Cho phép HTTP hoặc HTTPS (thường để false cho dev)
    x.SaveToken = true; // Lưu token trong context
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Xác thực khóa bảo mật
        IssuerSigningKey = new SymmetricSecurityKey(key),
        
        // Không xác thực nhà phát hành và người dùng token
        ValidateIssuer = false, 
        ValidateAudience = false
    };
});

// 3. Cấu hình CORS để cho phép frontend hoặc client khác gọi API mà không bị chặn
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); // Cho phép mọi nguồn, phương thức, header
});

// 4. Thêm services kiểm soát Controllers và cấu hình JSON Serializer
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// 5. Thêm hỗ trợ Swagger để dễ dàng test và xem tài liệu API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 6. Đăng ký dịch vụ gửi email theo giao thức DI (Dependency Injection)
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build(); // Tạo ứng dụng web từ builder

// Tự động tạo database nếu chưa có
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>(); // Lấy instance AppDbContext
    db.Database.EnsureCreated(); // Kiểm tra và tạo DB nếu chưa tồn tại
}

// Cấu hình trung gian (middleware) trong pipeline xử lý request
app.UseSwagger();
app.UseSwaggerUI();
app.UseDefaultFiles(); 
app.UseStaticFiles();   
app.UseCors("AllowAll"); 
app.UseAuthentication(); 
app.UseAuthorization();  
app.MapControllers();    
app.Run();               
