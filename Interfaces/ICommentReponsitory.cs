using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotnet_Core_Web_API.Models;

namespace Dotnet_Core_Web_API.Interfaces
{
    public interface ICommentReponsitory
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment comment);
        Task<Comment?> UpdateAsync(int id, Comment comment);
        Task<Comment?> DeleteAsync(int id);

    }
}