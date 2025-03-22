using GeminiAdvancedAPI.Domain.Entities.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetCommentsByBlogIdAsync(Guid blogId);
        Task<Comment> GetCommentByIdAsync(Guid id); // Gerekirse
        Task<Comment> AddCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment); // Gerekirse
        Task DeleteCommentAsync(Guid id);
        Task<IEnumerable<Comment>> GetChildCommentsAsync(Guid parentCommentId); //Alt yorumları getirme

    }
}
