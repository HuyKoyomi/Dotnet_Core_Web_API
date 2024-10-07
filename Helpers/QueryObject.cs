using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dotnet_Core_Web_API.Helpers
{
    public class QueryObject
    {
        public string? Symbol { get; set; } = null;
        public string? Companyname { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsDecsending { get; set; } = false;


    }
}