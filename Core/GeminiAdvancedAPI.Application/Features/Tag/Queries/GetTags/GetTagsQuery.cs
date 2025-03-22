using GeminiAdvancedAPI.Application.Features.Tag.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Tag.Queries.GetTags
{
    public record GetTagsQuery : IRequest<List<TagDto>>; // TagDto listesi dönecek

}
