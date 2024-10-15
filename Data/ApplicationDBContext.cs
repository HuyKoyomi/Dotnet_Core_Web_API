using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Dotnet_Core_Web_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
  public class ApplicationDBContext : IdentityDbContext<AppUser>
  {
    // gõ ctor
    public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {

    }

    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Porfolio> Porfolios { get; set; }


    // Phương thức này gọi OnModelCreating của lớp cha để thực hiện các cấu hình mặc định trước khi thêm các cấu hình tùy chỉnh.
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<Porfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId })); // Thiết lập khóa chính (primary key) cho thực thể Portfolio bao gồm hai trường: AppUserId và StockId.

      builder.Entity<Porfolio>().HasOne(u => u.AppUser).WithMany(u => u.Porfolios).HasForeignKey(p => p.AppUserId); // Thiết lập một mối quan hệ "một-nhiều" giữa thực thể AppUser và Portfolio

      builder.Entity<Porfolio>().HasOne(u => u.Stock).WithMany(u => u.Porfolios).HasForeignKey(p => p.StockId);
      // => một người dùng (AppUser) sở hữu nhiều danh mục đầu tư (Portfolio), và một cổ phiếu (Stock) có thể thuộc về nhiều danh mục đầu tư (Portfolio).

      List<IdentityRole> roles = new List<IdentityRole>{
                // tạo 2 đối tượng IdentityRole 
              new IdentityRole  {
                Name = "Admin",
                NormalizedName = "ADMIN"
              },
              new IdentityRole
              {
                Name = "User",
                NormalizedName = "USER"
              },

            };
      builder.Entity<IdentityRole>().HasData(roles); // thêm các vai trò này vào cơ sở dữ liệu khi chạy quá trình di chuyển (migration)
    }
  }
}