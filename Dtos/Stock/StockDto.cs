using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotnet_Core_Web_API.Dtos.Comment;

namespace Dotnet_Core_Web_API.Dtos.Stock
{
    public class StockDto
    {
        public int Id { get; set; } // gõ prop

        public string Symbol { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public decimal Purchase { get; set; }

        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = string.Empty;

        public long MarketCap { get; set; }

        public List<CommentDto> Comments { get; set; }
    }
}