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

        // Phương thức này gọi OnModelCreating của lớp cha để thực hiện các cấu hình mặc định trước khi thêm các cấu hình tùy chỉnh.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
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