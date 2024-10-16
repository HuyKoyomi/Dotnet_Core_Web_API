using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotnet_Core_Web_API.Data;
using Dotnet_Core_Web_API.Dtos.Stock;
using Dotnet_Core_Web_API.Interfaces;
using Dotnet_Core_Web_API.Mappers;
using Dotnet_Core_Web_API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_Core_Web_API.Controllers
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
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // IActionResult là kiểu trả về các loại phản hồi như Ok(), NotFound(), BadRequest()
            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDto = stocks.Select(s => s.ToStockDto()); // chuyển sang DTO
            return Ok(stockDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // [FromBody]: Attribute này cho biết dữ liệu sẽ được ánh xạ từ nội dung body của yêu cầu HTTP
            var stockModel = stockDto.ToStockFromCreateDto();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stockModel = await _stockRepo.UpdateAsync(id, updateDto.ToStockFromUpdateDto());
            if (stockModel == null)
            {
                return NotFound();
            }
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stockModel = await _stockRepo.DeleteAsync(id);
            if (stockModel == null)
            {
                return NotFound();
            }
            return NoContent();

        }
    }
}