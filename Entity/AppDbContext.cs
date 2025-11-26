using Microsoft.EntityFrameworkCore;


namespace OrderManagementAPI.Entity
{  // <--- BẠN ĐANG THIẾU DẤU MỞ NGOẶC NHỌN Ở ĐÂY
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed dữ liệu mẫu Admin
            modelBuilder.Entity<User>().HasData(new User 
            { 
                Id = 1, 
                Username = "admin", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), // Cần cài package BCrypt.Net-Next
                FullName = "Administrator", 
                Role = "Admin" 
            });
            modelBuilder.Entity<Product>().HasData(
        new Product 
        { 
            Id = 1, 
            Name = "Laptop Dell XPS 13", 
            Price = 25000000, 
            Description = "Laptop mỏng nhẹ cao cấp", 
            Stock = 10 
        },
        new Product 
        { 
            Id = 2, 
            Name = "iPhone 15 Pro", 
            Price = 28000000, 
            Description = "Titan tự nhiên, 256GB", 
            Stock = 15 
        },
        new Product 
        { 
            Id = 3, 
            Name = "Samsung Galaxy S24", 
            Price = 22000000, 
            Description = "AI Phone mới nhất", 
            Stock = 20 
        },
        new Product 
        { 
            Id = 4, 
            Name = "AirPods Pro 2", 
            Price = 6000000, 
            Description = "Tai nghe chống ồn chủ động", 
            Stock = 50 
        },
        new Product 
        { 
            Id = 5, 
            Name = "iPad Air M1", 
            Price = 15000000, 
            Description = "Máy tính bảng hiệu năng cao", 
            Stock = 12 
        }
    );
        }
    }
}  // <--- VÀ THIẾU DẤU ĐÓNG NGOẶC NHỌN Ở CUỐI CÙNG NÀY