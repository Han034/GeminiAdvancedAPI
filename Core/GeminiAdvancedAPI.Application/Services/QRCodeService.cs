using GeminiAdvancedAPI.Application.Interfaces;
using QRCoder; // QRCoder için using ekleyin
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;        // MemoryStream için

namespace GeminiAdvancedAPI.Application.Services
{
    public class QRCodeService : IQRCodeService
    {
        public byte[] GenerateQrCode(string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

            // QRCode sınıfı yerine PngByteQRCode kullanın
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qrCode.GetGraphic(20); // PixelPerModule değeri

            return qrCodeBytes;
        }
    }
}
