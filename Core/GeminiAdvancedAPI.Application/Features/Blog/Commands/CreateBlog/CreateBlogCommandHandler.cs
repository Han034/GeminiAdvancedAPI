using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using GeminiAdvancedAPI.Domain.Entities.Blog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Blog.Commands.CreateBlog
{
    public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, Guid>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;


        public CreateBlogCommandHandler(IBlogRepository blogRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        public async Task<Guid> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
        {
            var blog = new Domain.Entities.Blog.Blog
            {
                Title = request.Title,
                Content = request.Content,
                AuthorId = request.AuthorId,
                PublishedDate = request.PublishedDate,
                IsPublished = request.IsPublished,
                ImageUrl = request.ImageUrl,
                BlogCategories = new List<BlogCategory>(), // Boş olarak başla, sonra doldur
                Tags = new List<Domain.Entities.Blog.Tag>() // Boş olarak başla, sonra doldur
            };


            // Kategori ekleme
            foreach (var categoryId in request.CategoryIds)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category != null)
                {
                    blog.BlogCategories.Add(new BlogCategory { CategoryId = categoryId, Blog = blog });
                }
                // else { // İsteğe bağlı: Kategori bulunamazsa hata fırlat
                //     throw new NotFoundException(nameof(Category), categoryId);
                // }
            }

            // Tag ekleme
            foreach (var tagId in request.TagIds)
            {
                var tag = await _tagRepository.GetByIdAsync(tagId);
                if (tag != null)
                {
                    blog.Tags.Add(tag); // BlogTag ara tablosu EF Core tarafından otomatik yönetilecek
                }
                // else { // İsteğe bağlı: Tag bulunamazsa hata fırlat
                //     throw new NotFoundException(nameof(Tag), tagId);
                // }
            }


            await _blogRepository.AddAsync(blog);
            return blog.Id;
        }
    }
}
