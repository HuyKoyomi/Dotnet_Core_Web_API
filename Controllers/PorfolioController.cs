
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotnet_Core_Web_API.Extensions;
using Dotnet_Core_Web_API.Interfaces;
using Dotnet_Core_Web_API.Mappers;
using Dotnet_Core_Web_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/porfolio")]
    [ApiController]
    public class PorfolioController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IStockReponsitory _stockRepo;
        private readonly IPortfolioReponsitory _portfolioRepo;

        public PorfolioController(UserManager<AppUser> userManager, IStockReponsitory stockRepo, IPortfolioReponsitory porfolioRepon)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = porfolioRepon;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }

    }
}