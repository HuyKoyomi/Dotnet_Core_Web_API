using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dotnet_Core_Web_API.Models
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; } // primary key

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public int? StockId { get; set; } // foreign key - dấu ? cho phép giá trị có thể null => không bắt buộc phải liên kết

        public Stock? Stock { get; set; } // quan hệ nhiều - một

    }
}