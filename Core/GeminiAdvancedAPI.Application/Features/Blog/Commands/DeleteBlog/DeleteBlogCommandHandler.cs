using GeminiAdvancedAPI.Application.Exceptions;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Blog.Commands.DeleteBlog
{
    public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommand>
    {
        private readonly IBlogRepository _blogRepository;

        public DeleteBlogCommandHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<Unit> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
        {
            var blog = await _blogRepository.GetByIdAsync(request.Id);

            if (blog == null)
            {
                throw new NotFoundException(nameof(Blog), request.Id); // Daha açıklayıcı exception
            }
            await _blogRepository.DeleteAsync(request.Id);
            return Unit.Value;
        }

        Task IRequestHandler<DeleteBlogCommand>.Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
