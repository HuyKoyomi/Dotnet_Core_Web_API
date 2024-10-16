using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dotnet_Core_Web_API.Models
{
     [Table("Stocks")]
    public class Stock
    {
        public int Id { get; set; } // gõ prop

        public string Symbol { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")] // kiểu decimal với độ chính xác 18 chữ số, 2 chữ số sau dấu thập phân
        public decimal Purchase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = string.Empty;

        public long MarketCap { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>(); // quan hệ một - nhiều với một khóa ngoại trong bảng Comment trỏ đến bảng Stock. 1 Stock có thể chứa nhiều Comment

        public List<Portfolio> Porfolios { get; set; } = new List<Portfolio>();
    }
}