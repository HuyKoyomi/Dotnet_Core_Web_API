using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Responsitory
{
    public class StockReponsitory : IStockReponsitory
    {
        private readonly ApplicationDBContext _context; // đối tượng của DbContext, cung cấp kết nối và truy cập vào cơ sở dữ liệu.


        public StockReponsitory(ApplicationDBContext context)
        {
            _context = context;

        }

        public Task<List<Stock>> GetAllAsync()
        {
            return _context.Stocks.Include(c => c.Comments).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);// Thêm đối tượng stockModel vào tập hợp (DbSet) Stocks
            await _context.SaveChangesAsync(); // Thực hiện thao tác lưu các thay đổi (chèn bản ghi mới) vào cơ sở dữ liệu.
            return stockModel;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockModel)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingStock == null)
            {
                return null;
            }
            // set lại thông tin
            existingStock.Symbol = stockModel.Symbol;
            existingStock.CompanyName = stockModel.CompanyName;
            existingStock.Purchase = stockModel.Purchase;
            existingStock.LastDiv = stockModel.LastDiv;
            existingStock.Industry = stockModel.Industry;
            existingStock.MarketCap = stockModel.MarketCap;

            await _context.SaveChangesAsync();

            return existingStock;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return null;
            }
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }
    }
}