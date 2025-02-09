using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Interfaces
{
    public interface IQRCodeService
    {
        byte[] GenerateQrCode(string text);
    }
}
