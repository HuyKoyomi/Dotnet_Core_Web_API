using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotnet_Core_Web_API.Dtos.Stock;
using Dotnet_Core_Web_API.Models;

namespace Dotnet_Core_Web_API.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }

        public static Stock ToStockFromCreateDto(this CreateStockRequestDto stockDto)
        {
            return new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap,
            };
        }

        public static Stock ToStockFromUpdateDto(this UpdateStockRequestDto stockUpdateDto)
        {
            return new Stock
            {
                Symbol = stockUpdateDto.Symbol,
                CompanyName = stockUpdateDto.CompanyName,
                Purchase = stockUpdateDto.Purchase,
                LastDiv = stockUpdateDto.LastDiv,
                Industry = stockUpdateDto.Industry,
                MarketCap = stockUpdateDto.MarketCap,
            };
        }
    }
}

/*
Sự khác biệt khi thêm từ khóa static:
    1. Không thể tạo instance: // một class static chỉ tồn tại một bản duy nhất trong bộ nhớ
    2. Chỉ chứa các thành viên static // Một class static chỉ có thể chứa các thành viên static
    3. Không thể kế thừa hoặc được kế thừa:
    4. Hiệu suất: Class static thường có hiệu suất tốt hơn một chút trong các trường hợp chỉ cần dùng các phương thức tiện ích (utility methods) hoặc các phương thức dùng chung vì nó không yêu cầu tạo đối tượng và cũng không có trạng thái nội bộ để quản lý.

    => 
    + static class không thể tạo đối tượng và chỉ chứa các thành viên static.
    + static class phù hợp cho các phương thức tiện ích chung mà không cần lưu trạng thái.
    + non-static class có thể tạo nhiều đối tượng và có thể lưu trạng thái, nhưng không bắt buộc các thành viên phải là static.   

*/