﻿using GeminiAdvancedAPI.Application.Features.Product.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Product.Queries.GetProduct
{
	public record GetProductQuery(Guid Id) : IRequest<ProductDto>;
}