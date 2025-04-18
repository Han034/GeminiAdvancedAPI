﻿using GeminiAdvancedAPI.Application.DTOs;
using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace GeminiAdvancedAPI.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<AppUser> _userManager;
        //private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory; // IUserClaimsPrincipalFactory'i enjekte ediyoruz


        public TokenService(IOptions<JwtSettings> jwtSettings, UserManager<AppUser> userManager, IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            //_claimsPrincipalFactory = claimsPrincipalFactory;
        }

        public async Task<string> CreateToken(AppUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName) // veya user.Email
                // NameIdentifier, Name, Role MANUEL olarak EKLENMEYECEK!
            };

            // Kullanıcının rollerini claim olarak ekle
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //3.IUserClaimsPrincipalFactory'den gelen *diğer* claim'leri al
            //var principal = await _claimsPrincipalFactory.CreateAsync(user);
            //var factoryClaims = principal.Claims;
            //(Belki Duplicate Önleme Gerekebilir - Şimdilik Yorumda Kalsın)
            // claims.AddRange(principal.Claims.Where(c => !claims.Any(existing => existing.Type == c.Type)));
            //claims.AddRange(factoryClaims.Where(fc => !claims.Any(c => c.Type == fc.Type)));

            // Veritabanındaki özel claim'leri UserManager ile al ve ekle
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expires = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

                    var token = new JwtSecurityToken(
                        _jwtSettings.Issuer,
                        _jwtSettings.Audience,
                        claims, // Doğru claim listesi
                        expires: expires,
                        signingCredentials: creds
                    );

                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
        //var claims = new List<Claim>
        //{
        //    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //     new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //     new Claim(ClaimTypes.Name, user.UserName) // veya user.Email

        //    // MANUEL OLARAK NameIdentifier, Name, Role EKLEMEYİN!
        //};

        //        // Kullanıcının rollerini claim olarak ekle (BU KISIM KALABİLİR VEYA IUserClaimsPrincipalFactory'den de alabilirsiniz)
        //        foreach (var role in roles)
        //        {
        //            claims.Add(new Claim(ClaimTypes.Role, role));
        //        }

        //        // IUserClaimsPrincipalFactory'den gelen claim'leri al
        //        var principal = await _claimsPrincipalFactory.CreateAsync(user);
        //        // Sadece mevcut olmayanları ekle (Duplicate önlemek için)
        //        claims.AddRange(principal.Claims.Where(c => !claims.Any(existing => existing.Type == c.Type && existing.Value == c.Value)));


        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //        var expires = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

        //        var token = new JwtSecurityToken(
        //            _jwtSettings.Issuer,
        //            _jwtSettings.Audience,
        //            claims, // Artık doğru claim'ler
        //            expires: expires,
        //            signingCredentials: creds
        //        );

        //        return new JwtSecurityTokenHandler().WriteToken(token);
        //    }
        //========================================================================

        public int GetRefreshTokenExpiryDays()
        {
            return _jwtSettings.RefreshTokenExpirationDays;
        }
        //public string GenerateJwtToken(AppUser user, IList<string> roles)
        //{
        //    var claims = new List<Claim>
        //{
        //    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //    new Claim(ClaimTypes.Name, user.UserName) // veya user.Email
        //};

        //    // Kullanıcının rollerini claim olarak ekle
        //    foreach (var role in roles)
        //    {
        //        claims.Add(new Claim(ClaimTypes.Role, role));
        //    }

        //    // Kullanıcının veritabanındaki claim'lerini çek ve ekle
        //    var userClaims = _dbContext.UserClaims.Where(uc => uc.UserId == user.Id).ToList();
        //    foreach (var userClaim in userClaims)
        //    {
        //        claims.Add(new Claim(userClaim.ClaimType, userClaim.ClaimValue));
        //    }

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var expires = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

        //    var token = new JwtSecurityToken(
        //        _jwtSettings.Issuer,
        //        _jwtSettings.Audience,
        //        claims,
        //        expires: expires,
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
