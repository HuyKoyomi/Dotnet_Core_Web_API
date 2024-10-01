using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")] // Định nghĩa route - URL trong Controller này bắt đầu với ("api/stock")
    [ApiController] // Gắn nhãn controller này như một API controller, kích hoạt chức năng tự động như "validate đầu vào", "Binding dữ liệu", "(Bad Request) cho dữ liệu không hợp lệ"
    public class StockController : ControllerBase
    {
        // gõ phím tắt ctor
        private readonly IStockReponsitory _stockRepo;
        public StockController(IStockReponsitory stockRepo)
        {
            _stockRepo = stockRepo;
        }

        [HttpGet] // một attribute trong ASP.NET Core => phương thức GetAll() sẽ phản hồi với các yêu cầu HTTP GET

        public async Task<IActionResult> GetAll()
        {
            // IActionResult là kiểu trả về các loại phản hồi như Ok(), NotFound(), BadRequest()
            var stocks = await _stockRepo.GetAllAsync();
            var stockDto = stocks.Select(s => s.ToStockDto()); // chuyển sang DTO
            return Ok(stockDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepo.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            // [FromBody]: Attribute này cho biết dữ liệu sẽ được ánh xạ từ nội dung body của yêu cầu HTTP
            var stockModel = stockDto.ToStockFromCreateDto();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await _stockRepo.UpdateAsync(id, updateDto);
            if (stockModel == null)
            {
                return NotFound();
            }
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _stockRepo.DeleteAsync(id);
            if (stockModel == null)
            {
                return NotFound();
            }
            return NoContent();

        }
    }
}