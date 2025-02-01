using AutoMapper;
using GeminiAdvancedAPI.Application.DTOs;
using GeminiAdvancedAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace GeminiAdvancedAPI.Application.Mappings
{
    public class FileUrlResolver : IValueResolver<FileEntity, FileDto, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileUrlResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(FileEntity source, FileDto destination, string destMember, ResolutionContext context)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return null; // veya uygun bir varsayılan değer
            }

            // ActionContext oluştur
            var actionContext = new ActionContext(
                _httpContextAccessor.HttpContext,
                _httpContextAccessor.HttpContext.GetRouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
            );

            // IUrlHelper oluştur
            var urlHelper = new UrlHelper(actionContext);

            // URL'yi oluştur
            var downloadUrl = urlHelper.Action("Download", "File", new { fileId = source.Id }, _httpContextAccessor.HttpContext.Request.Scheme);

            return downloadUrl;
        }
    }
}
