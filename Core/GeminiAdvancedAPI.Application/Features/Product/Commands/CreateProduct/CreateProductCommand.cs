﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Product.Commands.CreateProduct
{
	public record CreateProductCommand(string Name, string Description, decimal Price, int Stock) : IRequest<Guid>;

}
