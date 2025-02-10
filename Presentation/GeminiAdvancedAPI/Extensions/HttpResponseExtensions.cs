using GeminiAdvancedAPI.Application.DTOs.PagedResult;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GeminiAdvancedAPI.Extensions
{
    public static class HttpResponseExtensions
    {
        public static void AddPaginationHeader<T>(this HttpResponse response, PagedResult<T> pagedResult, IUrlHelper urlHelper, string actionName, object routeValues = null)
        {
            var paginationMetadata = new
            {
                totalCount = pagedResult.TotalCount,
                pageSize = pagedResult.PageSize,
                currentPage = pagedResult.PageNumber,
                totalPages = pagedResult.TotalPages
            };

            response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            // Link header (RFC 5988)
            var links = new List<string>();
            if (pagedResult.PageNumber > 1)
            {
                links.Add($"<{urlHelper.ActionLink(actionName, null, new { pageNumber = pagedResult.PageNumber - 1, pageSize = pagedResult.PageSize }, response.HttpContext.Request.Scheme)}>; rel=\"prev\"");
            }
            if (pagedResult.PageNumber < pagedResult.TotalPages)
            {
                links.Add($"<{urlHelper.ActionLink(actionName, null, new { pageNumber = pagedResult.PageNumber + 1, pageSize = pagedResult.PageSize }, response.HttpContext.Request.Scheme)}>; rel=\"next\"");
            }
            links.Add($"<{urlHelper.ActionLink(actionName, null, new { pageNumber = 1, pageSize = pagedResult.PageSize }, response.HttpContext.Request.Scheme)}>; rel=\"first\"");
            links.Add($"<{urlHelper.ActionLink(actionName, null, new { pageNumber = pagedResult.TotalPages, pageSize = pagedResult.PageSize }, response.HttpContext.Request.Scheme)}>; rel=\"last\"");

            if (links.Any())
            {
                response.Headers.Add("Link", string.Join(", ", links));
            }
        }
    }
}
