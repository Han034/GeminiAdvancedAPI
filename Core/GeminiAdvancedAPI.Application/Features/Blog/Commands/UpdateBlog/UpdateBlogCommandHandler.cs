using GeminiAdvancedAPI.Application.Exceptions;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using GeminiAdvancedAPI.Domain.Entities.Blog;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Blog.Commands.UpdateBlog
{
    public class UpdateBlogCommandHandler : IRequestHandler<UpdateBlogCommand>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateBlogCommandHandler(IBlogRepository blogRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository, IHttpContextAccessor httpContextAccessor)
        {
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _httpContextAccessor = httpContextAccessor; // Atama yapıldı
        }
        public async Task Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
        {
            // 1. Mevcut blog'u veritabanından getir (ilişkili verilerle birlikte)
            var blog = await _blogRepository.GetByIdAsync(request.Id); // GetByIdAsync, ilişkileri (Tags, BlogCategories) yüklemeli

            if (blog == null)
            {
                throw new NotFoundException(nameof(Blog), request.Id);
            }

            // TODO: Yetkilendirme kontrolü eklenebilir (örneğin, sadece blog'un yazarı veya admin güncelleyebilir)

            // 2. Kategori ID'lerini kontrol et
            var existingCategoryIds = await _categoryRepository.GetAll()
                                                .Where(c => request.CategoryIds.Contains(c.Id))
                                                .Select(c => c.Id)
                                                .ToListAsync(cancellationToken);
            var invalidCategoryIds = request.CategoryIds.Except(existingCategoryIds).ToList();
            if (invalidCategoryIds.Any())
            {
                throw new BadRequestException($"Invalid Category IDs: {string.Join(", ", invalidCategoryIds)}");
            }

            // 3. Tag ID'lerini kontrol et
            var existingTagIds = await _tagRepository.GetAll()
                                        .Where(t => request.TagIds.Contains(t.Id))
                                        .Select(t => t.Id)
                                        .ToListAsync(cancellationToken);
            var invalidTagIds = request.TagIds.Except(existingTagIds).ToList();
            if (invalidTagIds.Any())
            {
                throw new BadRequestException($"Invalid Tag IDs: {string.Join(", ", invalidTagIds)}");
            }

            // 4. Blog bilgilerini güncelle (AuthorId hariç)
            blog.Title = request.Title;
            blog.Content = request.Content;
            blog.PublishedDate = request.PublishedDate;
            blog.IsPublished = request.IsPublished;
            blog.ImageUrl = request.ImageUrl;

            // 5. Kategori ilişkilerini güncelle
            blog.BlogCategories.Clear(); // Mevcut tüm kategori ilişkilerini temizle
            foreach (var categoryId in request.CategoryIds)
            {
                blog.BlogCategories.Add(new BlogCategory { BlogId = blog.Id, CategoryId = categoryId });
            }

            // 6. Tag ilişkilerini güncelle
            var currentTagIds = blog.Tags.Select(t => t.Id).ToList();
            // Silinecekler
            var tagsToRemove = blog.Tags.Where(t => !request.TagIds.Contains(t.Id)).ToList();
            foreach (var tagToRemove in tagsToRemove)
            {
                blog.Tags.Remove(tagToRemove);
            }
            // Eklenecekler
            var tagIdsToAdd = request.TagIds.Except(currentTagIds).ToList();
            foreach (var tagId in tagIdsToAdd)
            {
                var tag = await _tagRepository.GetByIdAsync(tagId);
                if (tag != null)
                {
                    blog.Tags.Add(tag);
                }
            }

            // 7. Blog'u güncelle (SaveChangesAsync gerekli DEĞİL, Repository içinde yapılıyor)
            await _blogRepository.UpdateAsync(blog);

            // IRequestHandler<TRequest> olduğu için bir şey dönmemiz GEREKMİYOR
            // Ancak MediatR 12+ Task dönmeyi bekler, Task<Unit> değilse.
            // Eğer hata alırsanız, aşağıdaki satırı ekleyin veya Handle metodunu Task<Unit> yapın.
            // return Unit.Value;

        }
    }
}
