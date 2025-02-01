using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.DTOs
{
    public class FileUploadDto
    {
        public IFormFile File { get; set; }
        // İsteğe bağlı: Dosya ile ilgili ek bilgiler (başlık, açıklama vb.)
        // public string Title { get; set; }
        // public string Description { get; set; }
    }
}
