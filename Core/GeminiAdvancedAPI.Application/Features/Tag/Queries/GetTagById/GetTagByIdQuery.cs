using GeminiAdvancedAPI.Application.Features.Tag.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Tag.Queries.GetTagById
{
    public record GetTagByIdQuery(int Id) : IRequest<TagDto>; // TagDto dönecek

}
