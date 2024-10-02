using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockReponsitory
    {
        Task<List<Stock>> GetAllAsync();
        Task<Stock?> GetByIdAsync(int id); // FirstOrDefault can be NULL
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockModel);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> StockExists(int stockId);

    }
}