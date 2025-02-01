using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.DTOs
{
    public class FileDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; }
        public DateTime UploadedDate { get; set; }
        //İsteğe bağlı: İndirme linki
         public string DownloadLink { get; set; }
    }
}
