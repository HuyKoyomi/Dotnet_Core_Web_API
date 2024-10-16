using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotnet_Core_Web_API.Models;
using Dotnet_Core_Web_API.Helpers;

namespace Dotnet_Core_Web_API.Interfaces
{
    public interface IStockReponsitory
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(int id); // FirstOrDefault can be NULL
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, Stock stockModel);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> StockExists(int stockId);

    }
}