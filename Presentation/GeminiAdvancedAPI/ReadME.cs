using GeminiAdvancedAPI.Application.Exceptions;
using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Application.Mappings;
using GeminiAdvancedAPI.Application.Services;
using GeminiAdvancedAPI.Controllers;
using GeminiAdvancedAPI.Domain.Entities.Identity;
using GeminiAdvancedAPI.Domain.Entities;
using GeminiAdvancedAPI.Middleware;
using GeminiAdvancedAPI.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Serilog;
using StackExchange.Redis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System;

namespace GeminiAdvancedAPI
{
    public class ReadME
    {
        /*
    Şu Ana Kadar Tamamlananlar:

    Temel Proje Yapısı:

    .NET Core Web API projesi oluşturuldu.
    Onion Architecture prensiplerine uygun katmanlı mimari (Core, Infrastructure, Presentation) kuruldu.
    Gerekli NuGet paketleri yüklendi (EF Core, MediatR, AutoMapper, FluentValidation, Serilog, QRCoder, ASP.NET Core Identity, JWT Bearer, MailKit, System.Drawing.Common vs.).
    Domain Katmanı:

    Product, BaseEntity, AppUser, AppRole, Cart, CartItem, FileEntity, AppUserClaim entity'leri oluşturuldu.
    Infrastructure Katmanı:

    ApplicationDbContext oluşturuldu ve Identity ile entegre edildi.
    Entity Framework Core konfigürasyonu yapıldı.
    Migration'lar oluşturuldu ve veritabanı güncellendi.
    IProductRepository, ProductRepository, ICartRepository, CartRepository(Redis ile) implemente edildi.
    IUnitOfWork ve UnitOfWork (artık kullanılmıyor, Redis'e geçildi).
    IEmailService ve EmailService (SMTP ile basit e-posta gönderimi).
    IQRCodeService ve QRCodeService(QRCoder ile).
    ITokenService ve TokenService.
    FileUrlResolver.
    Application Katmanı:

    CQRS pattern'i uygulandı (MediatR kullanılarak).
    Product işlemleri için Command, Query ve Handler'lar (Create, Update, Delete, Get, GetList, GetProductsPaged).
    Cart işlemleri için Command, Query ve Handler'lar (AddProduct, RemoveProduct, UpdateQuantity, ClearCart, GetCart, GetCartItemCount).
    ForgotPassword, ResetPassword işlemleri için altyapı.
    AutoMapper profilleri (MappingProfile).
    FluentValidation validator'ları (bazı DTO'lar ve Command'ler için).
    ErrorDetails modeli ve ApiException, NotFoundException gibi özel exception sınıfları.
    DTO'lar (ProductDto, CartDto, CartItemDto, UserDto, RegisterModel, LoginModel, ChangePasswordModel, FileUploadDto, ForgotPasswordDto, ResetPasswordDto vs.).


    Presentation (Web API) Katmanı:

    ProductController (ürün işlemleri için).
    UserController(kullanıcı işlemleri için).
    FileController(dosya yükleme/indirme/silme işlemleri için).
    CartController(sepet işlemleri için).
    Global Exception Handling(ExceptionMiddleware).
    JWT Authentication(giriş yapmış kullanıcılar için token oluşturma, doğrulama, refresh token).
    Swagger/OpenAPI entegrasyonu.
    Serilog ve Seq ile loglama.
    In-Memory Caching (ürün listesi için).
    Pagination(ürün listesi için).
    Anonymous kullanıcılar için cookie-based sepet.
    Sepet birleştirme (login olurken).
    E-posta ile şifre sıfırlama (SMTP ile).
    QR kod oluşturma.
    Kullanıcının kendi yüklediği dosyaları görmesi/silmesi.
    Profil bilgilerini getirme ve güncelleme.
    Basit bir unit test örneği.
         
    
    */
    }
}
