using api.Data;
using api.Interfaces;
using api.Responsitory;
using Microsoft.EntityFrameworkCore;

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

app.Run(); // khởi động ứng dụng 
