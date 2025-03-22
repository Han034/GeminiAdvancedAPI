using GeminiAdvancedAPI.Application.Exceptions;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using GeminiAdvancedAPI.Domain.Entities.Blog;
using MediatR;
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

        public UpdateBlogCommandHandler(IBlogRepository blogRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }
        public async Task<Unit> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
        {

            var blog = await _blogRepository.GetByIdAsync(request.Id);

            if (blog == null)
            {
                throw new NotFoundException(nameof(Blog), request.Id); // Daha açıklayıcı exception
            }

            // Blog bilgilerini güncelle
            blog.Title = request.Title;
            blog.Content = request.Content;
            blog.PublishedDate = request.PublishedDate;
            blog.IsPublished = request.IsPublished;
            blog.ImageUrl = request.ImageUrl;

            // Kategori ilişkilerini güncelle

            // Mevcut kategorileri temizle
            blog.BlogCategories.Clear();

            //Yeni kategorileri ekle
            foreach (var categoryId in request.CategoryIds)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category != null)
                {
                    blog.BlogCategories.Add(new BlogCategory { BlogId = blog.Id, CategoryId = categoryId });
                }

            }

            // Tag ilişkilerini güncelle
            // Önce blog'a ait tag'leri getir
            var currentTags = blog.Tags.ToList();

            //Silinmesi gereken tag'leri bul ve sil
            var tagsToRemove = currentTags.Where(x => !request.TagIds.Contains(x.Id)).ToList();
            foreach (var tagToRemove in tagsToRemove)
            {
                blog.Tags.Remove(tagToRemove);
            }

            //Eklenmesi gereken tag'leri bul ve ekle
            var tagsToAdd = request.TagIds.Where(x => !currentTags.Select(x => x.Id).Contains(x)).ToList();
            foreach (var tagId in tagsToAdd)
            {
                var tag = await _tagRepository.GetByIdAsync(tagId);
                if (tag != null)
                {
                    blog.Tags.Add(tag);
                }

            }

            await _blogRepository.UpdateAsync(blog);
            return Unit.Value; // Başarılı olduğunu belirtmek için
        }

        Task IRequestHandler<UpdateBlogCommand>.Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
