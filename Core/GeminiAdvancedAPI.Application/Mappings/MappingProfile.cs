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
using GeminiAdvancedAPI.Application.DTOs.Identity;
using GeminiAdvancedAPI.Domain.Entities.Blog;
using GeminiAdvancedAPI.Application.Features.Tag.DTOs;

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
            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

            //AutoMapper needs a way to create an instance of IUrlHelper. Therefore we need to use a factory method.
            CreateMap<FileEntity, FileDto>()
             .ForMember(dest => dest.DownloadLink, opt => opt.MapFrom<FileUrlResolver>());

            // Kaynaktan -> Hedefe
            CreateMap<AppUser, UserProfileDto>();
            CreateMap<UpdateUserProfileDto, AppUser>()
            .ForMember(dest => dest.ProfilePictureUrl, opt => opt.Ignore()); // Güncelleme sırasında ProfilePictureUrl'i şimdilik ignore et.

            // Hedefden -> Kaynağa (eğer gerekliyse)
            CreateMap<UserProfileDto, AppUser>();

            CreateMap<Category, CategoryDto>(); // Category -> CategoryDto
                                                // Eğer Category'den CreateCategoryCommand'a veya UpdateCategoryCommand'a map'leme yapmanız gerekirse:
                                                // CreateMap<Category, CreateCategoryCommand>();
                                                // CreateMap<Category, UpdateCategoryCommand>();
            CreateMap<Tag, TagDto>(); // Tag -> TagDto
                                      // Eğer Tag'den CreateTagCommand veya UpdateTagCommand'a map'leme yapmanız gerekirse:
                                      // CreateMap<Tag, CreateTagCommand>();
                                      // CreateMap<Tag, UpdateTagCommand>();
        }
    }
}
