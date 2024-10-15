using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Interfaces;
using Dotnet_Core_Web_API.Models;
using Microsoft.IdentityModel.Tokens;

namespace Dotnet_Core_Web_API.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
        }
        public string CreateToken(AppUser user)
        {
            // Claim là danh sách thông tin mà token chứa
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
            };

            // xác định cách thức token sẽ được ký, đảm bảo rằng token không bị giả mạo
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // SecurityTokenDescriptor chứa các thông tin cần thiết để tạo ra token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // chứa các thông tin xác thực 
                Expires = DateTime.Now.AddDays(7), // thời gian hết hạn của token (ở đây là 7 ngày).
                SigningCredentials = creds, // thông tin dùng để ký token
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
                // được lấy từ cấu hình để chỉ định nơi phát hành và đối tượng mà token hướng đến.
            };

            var tokenHanlder = new JwtSecurityTokenHandler(); // được sử dụng để tạo ra token dựa trên tokenDescriptor.
            var token = tokenHanlder.CreateToken(tokenDescriptor);
            return tokenHanlder.WriteToken(token);
        }
    }
}