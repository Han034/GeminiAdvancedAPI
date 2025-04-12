using GeminiAdvancedAPI.Application.Exceptions;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using GeminiAdvancedAPI.Domain.Entities.Blog;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace GeminiAdvancedAPI.Application.Features.Blog.Commands.CreateBlog
{
    public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, Guid>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CreateBlogCommandHandler> _logger; // Logging ekleyelim


        public CreateBlogCommandHandler(IBlogRepository blogRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository, IHttpContextAccessor httpContextAccessor,
        ILogger<CreateBlogCommandHandler> logger)
        {
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _httpContextAccessor = httpContextAccessor; // Atama yapıldı
                    _logger = logger; // Logger'ı ata

        }

        public async Task<Guid> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
        {
            // 1. Giriş yapmış kullanıcının ID'sini al
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid authorId))
            {
                throw new UnauthorizedAccessException("User is not authenticated or User ID is invalid.");
            }

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

            // 4. Yeni Blog entity'sini oluştur
            var blog = new Domain.Entities.Blog.Blog
            {
                Title = request.Title,
                Content = request.Content,
                AuthorId = authorId, // Giriş yapmış kullanıcının ID'si
                PublishedDate = request.PublishedDate,
                IsPublished = request.IsPublished,
                ImageUrl = request.ImageUrl,
                BlogCategories = new List<BlogCategory>(), // Boş olarak başlat
                Tags = new List<Domain.Entities.Blog.Tag>() // Boş olarak başlat
            };

            // 5. Kategorileri ekle (BlogCategories ara tablosu üzerinden)
            foreach (var categoryId in request.CategoryIds)
            {
                blog.BlogCategories.Add(new BlogCategory { CategoryId = categoryId, Blog = blog });
            }

            // 6. Tag'leri ekle (Tags koleksiyonuna direkt ekleme - EF Core ara tabloyu yönetir)
            foreach (var tagId in request.TagIds)
            {
                var tag = await _tagRepository.GetByIdAsync(tagId);
                if (tag != null) // Her ihtimale karşı (validasyon yaptık ama yine de)
                {
                    blog.Tags.Add(tag);
                }
            }

            // 7. Blog'u veritabanına ekle
            await _blogRepository.AddAsync(blog);

            // 8. Yeni blog'un ID'sini dön
            return blog.Id;
        }
    }
}
