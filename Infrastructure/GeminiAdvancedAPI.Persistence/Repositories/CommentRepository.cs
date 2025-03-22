using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using GeminiAdvancedAPI.Domain.Entities.Blog;
using GeminiAdvancedAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Persistence.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync(); // DbContext üzerinden kaydet
            return comment;
        }

        public async Task DeleteCommentAsync(Guid id)
        {
            // Yorumu ve alt yorumlarını (varsa) silmek için daha kapsamlı bir yöntem:
            var comment = await _context.Comments
                .Include(c => c.ChildComments) // Alt yorumları da yükle
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment != null)
            {
                // Önce alt yorumları sil (recursive - dikkatli kullan!)
                if (comment.ChildComments != null && comment.ChildComments.Any())
                {
                    foreach (var childComment in comment.ChildComments.ToList()) // ToList() önemli, collection değişirken hata almamak için
                    {
                        await DeleteCommentAsync(childComment.Id); // Recursive çağrı
                    }
                }

                // Sonra ana yorumu sil
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync(); // DEĞİŞİKLİKLERİ KAYDET
            }

            // Alternatif: Sadece tek bir yorumu silmek (alt yorumları "yetim" bırakmak)
            // var comment = await _context.Comments.FindAsync(id);
            // if (comment != null)
            // {
            //     _context.Comments.Remove(comment);
            // }
        }



        public async Task<Comment> GetCommentByIdAsync(Guid id)
        {
            return await _context.Comments
                .Include(c => c.Author) // Yorum yazarını dahil et
                .Include(c => c.ParentComment) // Varsa üst yorumu dahil et
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByBlogIdAsync(Guid blogId)
        {
            // Bir blog'a ait TÜM yorumları (ana yorumlar ve alt yorumlar DAHİL) getir.
            // Hiyerarşik yapıyı KORUMADAN, DÜZ bir liste olarak.
            return await _context.Comments
                .Include(c => c.Author) // Yorum yazarını dahil et
                .Include(c => c.ParentComment) // Varsa üst yorumu dahil et
                .Where(c => c.BlogId == blogId)
                .OrderByDescending(c => c.CreatedDate)  // Tarihe göre sırala (en yeni en üstte)
                .ToListAsync();


            // ALTERNATİF: Sadece ANA yorumları getir, alt yorumları AYRI bir sorgu ile getir.
            // Bu, daha performanslı olabilir, ancak istemci tarafında (client-side) hiyerarşiyi oluşturmanız gerekir.
            //return await _context.Comments
            //    .Include(c => c.Author)
            //    .Where(c => c.BlogId == blogId && c.ParentCommentId == null) // Sadece ana yorumlar
            //    .OrderByDescending(c => c.CreatedDate)
            //    .ToListAsync();
        }


        public async Task<IEnumerable<Comment>> GetChildCommentsAsync(Guid parentCommentId)
        {
            // Belirli bir yorumun ALT yorumlarını getir.
            return await _context.Comments
                   .Include(c => c.Author)
                   .Where(c => c.ParentCommentId == parentCommentId)
                   .OrderBy(c => c.CreatedDate)  // Veya başka bir sıralama kriteri
                   .ToListAsync();
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            // Yorumu güncelle.  Sadece Content'i güncellemek yeterli olabilir.
            // Diğer property'leri (AuthorId, BlogId, ParentCommentId) değiştirmemelisiniz.
            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync(); // DEĞİŞİKLİKLERİ KAYDET


            // Alternatif (daha güvenli): Sadece belirli alanları güncelle
            // var existingComment = await _context.Comments.FindAsync(comment.Id);
            // if (existingComment != null)
            // {
            //     existingComment.Content = comment.Content;
            //     // Başka güncellenecek alanlar...
            // }
        }
    }
}
