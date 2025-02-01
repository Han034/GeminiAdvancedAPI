using AutoMapper;
using GeminiAdvancedAPI.Application.DTOs;
using GeminiAdvancedAPI.Application.Features.Product.Commands.CreateProduct;
using GeminiAdvancedAPI.Application.Features.Product.Dtos;
using GeminiAdvancedAPI.Domain.Entities;
using GeminiAdvancedAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using GeminiAdvancedAPI.Application.Features.Product.Commands.UpdateProduct;

namespace GeminiAdvancedAPI.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<UpdateProductCommand, Product>();
            CreateMap<AppUser, UserDto>().ReverseMap();

            //AutoMapper needs a way to create an instance of IUrlHelper. Therefore we need to use a factory method.
            CreateMap<FileEntity, FileDto>()
             .ForMember(dest => dest.DownloadLink, opt => opt.MapFrom<FileUrlResolver>());

        }
    }
}
