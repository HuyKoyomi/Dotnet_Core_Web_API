using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotnet_Core_Web_API.Data;
using Dotnet_Core_Web_API.Interfaces;
using Dotnet_Core_Web_API.Models;
using Dotnet_Core_Web_API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_Core_Web_API.Responsitory
{
    public class StockReponsitory : IStockReponsitory
    {
        private readonly ApplicationDBContext _context; // đối tượng của DbContext, cung cấp kết nối và truy cập vào cơ sở dữ liệu.

        public StockReponsitory(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            // return _context.Stocks.Include(c => c.Comments).ToListAsync();
            var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.Companyname))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.Companyname));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDecsending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();

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

        public async Task<Stock?> UpdateAsync(int id, Stock stockModel)
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

        public Task<bool> StockExists(int id)
        {
            return _context.Stocks.AnyAsync(s => s.Id == id);
        }
    }
}