using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotnet_Core_Web_API.Models;

namespace Dotnet_Core_Web_API.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}