﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Cart.Commands.ClearCart
{
    public record ClearCartCommand(string UserId) : IRequest;
}
