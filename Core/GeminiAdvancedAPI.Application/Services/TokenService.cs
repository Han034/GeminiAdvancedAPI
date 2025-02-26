using GeminiAdvancedAPI.Application.DTOs;
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
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory; // IUserClaimsPrincipalFactory'i enjekte ediyoruz


        public TokenService(IOptions<JwtSettings> jwtSettings, UserManager<AppUser> userManager, IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
        }

        public async Task<string> CreateToken(AppUser user, IList<string> roles)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName) // veya user.Email
        };

            // Kullanıcının rollerini claim olarak ekle
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Kullanıcının claim'lerini almak için IUserClaimsPrincipalFactory kullanın
            var principal = await _claimsPrincipalFactory.CreateAsync(user);
            claims.AddRange(principal.Claims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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
