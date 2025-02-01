using GeminiAdvancedAPI.Application.DTOs;
using GeminiAdvancedAPI.Domain.Entities;
using GeminiAdvancedAPI.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace GeminiAdvancedAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<FileController> _logger; // ILogger ekleyin

        public FileController(IWebHostEnvironment hostingEnvironment, ApplicationDbContext dbContext, ILogger<FileController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _dbContext = dbContext;
            _logger = logger; // Logger'ı field'a atayın
        }

        [HttpPost("Upload")]
        [Authorize] // Sadece giriş yapmış kullanıcılar dosya yükleyebilsin
        //[Authorize] // İsteğe bağlı: Sadece giriş yapmış kullanıcılar dosya yükleyebilsin
        public async Task<IActionResult> Upload([FromForm] FileUploadDto model)
        {
            if (model.File == null || model.File.Length == 0)
                return BadRequest("File is not selected.");

            // İzin verilen dosya uzantıları
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            // Maksimum dosya boyutu (örneğin, 5 MB)
            var maxFileSize = 5 * 1024 * 1024;

            var fileExtension = Path.GetExtension(model.File.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
                return BadRequest("Invalid file extension.");

            if (model.File.Length > maxFileSize)
                return BadRequest("File size exceeds the limit.");

            try
            {
                // Dosyayı sunucuda bir klasöre kaydedin
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = ContentDispositionHeaderValue.Parse(model.File.ContentDisposition).FileName.Trim('"');

                // SEO uyumlu dosya adı oluştur
                var seoFriendlyFileName = GenerateSeoFriendlyFileName(Path.GetFileNameWithoutExtension(fileName)) + fileExtension;

                var filePath = Path.Combine(uploadsFolder, seoFriendlyFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }

                // Dosya bilgilerini veritabanına kaydet
                var fileEntity = new FileEntity
                {
                    Id = Guid.NewGuid(),
                    FileName = seoFriendlyFileName,
                    FilePath = Path.Combine("uploads", seoFriendlyFileName), // Veritabanında sadece uploads klasöründen sonraki kısmı saklayın
                    FileSize = model.File.Length,
                    ContentType = model.File.ContentType,
                    UploadedBy = User.Identity.Name, // Giriş yapmış kullanıcının adını alın (eğer gerekiyorsa)
                    ///*
                    /// Burada deneme amaçlı "Admin" olarak atadık. Authorize attribute ekleyerek giriş yapmış kullanıcı adını alabilirsiniz.
                    /// 
                    /// */
                    //UploadedBy = "Admin", // Geçici olarak "Admin" atayın
                    UploadedDate = DateTime.UtcNow
                };
                _dbContext.Files.Add(fileEntity);
                await _dbContext.SaveChangesAsync();

                return Ok(new { FileName = seoFriendlyFileName, FileId = fileEntity.Id }); // Artık FileId'yi de dönüyoruz
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                _logger.LogError(ex, "Dosya yükleme hatası"); // Serilog eklediyseniz bu şekilde loglayabilirsiniz
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string GenerateSeoFriendlyFileName(string fileName)
        {
            // Dosya adını küçük harfe çevir
            fileName = fileName.ToLowerInvariant();

            // Geçersiz karakterleri kaldır
            fileName = Regex.Replace(fileName, @"[^a-z0-9\s-]", "");

            // Boşlukları ve ardışık boşlukları tire ile değiştir
            fileName = Regex.Replace(fileName, @"\s+", " ").Trim();
            fileName = Regex.Replace(fileName, @"\s", "-");

            // Ardışık tireleri tek tire ile değiştir
            fileName = Regex.Replace(fileName, @"-+", "-");

            return fileName;
        }
    }
}
