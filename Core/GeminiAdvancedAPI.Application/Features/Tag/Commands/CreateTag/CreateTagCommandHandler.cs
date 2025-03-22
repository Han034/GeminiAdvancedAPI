using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Tag.Commands.CreateTag
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, int>
    {
        private readonly ITagRepository _tagRepository;

        public CreateTagCommandHandler(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }
        public async Task<int> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = new Domain.Entities.Blog.Tag
            {
                Name = request.Name,
            };
            await _tagRepository.AddAsync(tag);
            //await _unitOfWork.SaveChangesAsync(cancellationToken); // DEĞİŞİKLİKLERİ KAYDET
            return tag.Id;
        }
    }
}
