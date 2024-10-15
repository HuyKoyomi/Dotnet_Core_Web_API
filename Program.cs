using api.Data;
using api.Interfaces;
using api.Responsitory;
using Dotnet_Core_Web_API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(); // đăng ký dịch vụ cho controllers trong ứng dụng

// cấu hình Entity Framework Core => thiết lập kết nối cơ sở dữ liệu SQL Server
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 0)))); // Nếu không muốn chỉ định phiên bản, bạn có thể bỏ dòng này
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true; // Yêu cầu mật khẩu có ít nhất một chữ số.
        options.Password.RequireUppercase = true; // Yêu cầu mật khẩu có ít nhất một chữ cái viết hoa.
        options.Password.RequireLowercase = true; // Yêu cầu mật khẩu có ít nhất một chữ cái viết thường.
        options.Password.RequireNonAlphanumeric = true; // Yêu cầu mật khẩu có ít nhất một ký tự không phải chữ số hoặc chữ cái (ví dụ: @, #, ...).
        options.Password.RequiredLength = 12; // Đặt độ dài tối thiểu của mật khẩu là 12 ký tự.

    }
).AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Kiểm tra thông tin người phát hành (issuer) của token.
        ValidIssuer = builder.Configuration["JWT:Issuer"], // Thiết lập giá trị hợp lệ của issuer từ cấu hình (appsettings.json).
        ValidateAudience = true, // Kiểm tra audience của token.
        ValidAudience = builder.Configuration["JWT:Audience"], // Thiết lập giá trị hợp lệ của audience từ cấu hình.
        ValidateIssuerSigningKey = true, // Kiểm tra khóa ký token.
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]) // Thiết lập khóa ký để kiểm tra tính hợp lệ của token.
        )
    };
});
builder.Services.AddScoped<IStockReponsitory, StockReponsitory>(); // cấu hình Reponsitory
builder.Services.AddScoped<ICommentReponsitory, CommentRepository>(); // cấu hình Reponsitory
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    // sử dụng Newtonsoft.Json để tuần tự hóa JSON thay vì mặc định
    // Tránh các lỗi vòng lặp tham chiếu bằng cách bỏ qua chúng trong quá trình tuần tự hóa.
});

var app = builder.Build(); // tạo và xây dựng ứng dụng web từ builder

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Middleware này tự động chuyển hướng các yêu cầu HTTP sang HTTPS để đảm bảo rằng kết nối an toàn.

app.MapControllers(); // định tuyến tất cả các yêu cầu HTTP đến các controller đã được định nghĩa trong ứng dụng
app.UseAuthentication();
app.UseAuthorization();
app.Run(); // khởi động ứng dụng 
