using GeminiAdvancedAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GeminiAdvancedAPI.Application.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GeminiAdvancedAPI.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GeminiAdvancedAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, ITokenService tokenService, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // İsteğe bağlı: Kullanıcıya rol atama
                    await _userManager.AddToRoleAsync(user, "User");

                    return Ok(new { UserId = user.Id });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var roles = await _userManager.GetRolesAsync(user);

                    // Access Token oluştur
                    var accessToken = await _tokenService.CreateToken(user, roles);

                    // Refresh Token oluştur
                    var refreshToken = Guid.NewGuid().ToString();
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); // Burada sabit bir değer atadık.
                    await _userManager.UpdateAsync(user);

                    return Ok(new
                    {
                        Token = accessToken,
                        RefreshToken = refreshToken
                    });
                }
                else if (result.IsLockedOut)
                {
                    return Unauthorized("User account locked out.");
                }
                else
                {
                    return Unauthorized("Invalid login attempt.");
                }
            }
            return BadRequest(ModelState);
        }

        private string GenerateJwtToken(AppUser user, IList<string> roles)
        {
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName) // Eğer JWT'de username kullanmak isterseniz
    };

            // Kullanıcının rollerini claim olarak ekle
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

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

        [Authorize] // Sadece giriş yapmış kullanıcılar erişebilir
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            // User.Identity.Name, kullanıcının email veya username bilgisini içerir.
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            // AutoMapper ile UserDto'ya map'leme (ileride eklenecek)
            // var userDto = _mapper.Map<UserDto>(user);
            // return Ok(userDto);

            // Şimdilik, sadece basit bir mesaj dönelim:
            return Ok(new { Message = $"Kullanıcı bilgileri getirildi: {user.FirstName} {user.LastName}" });
        }

        [Authorize] // Sadece giriş yapmış kullanıcılar erişebilir
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [Authorize] // Sadece giriş yapmış kullanıcılar erişebilir
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logout successful" });
        }

        // Admin işlemleri (Rol yönetimi)
        [Authorize(Roles = "Admin")] // Sadece "Admin" rolündekiler erişebilir
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name cannot be empty.");
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                // Rol yoksa oluştur
                var role = new AppRole { Name = roleName };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return Ok(new { Message = $"Role '{roleName}' created successfully." });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }

            return BadRequest($"Role '{roleName}' already exists.");
        }

        [Authorize(Roles = "Admin")] // Sadece "Admin" rolündekiler erişebilir
        [HttpPost("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"User with email '{email}' not found.");
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return NotFound($"Role '{roleName}' not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new { Message = $"Role '{roleName}' assigned to user '{email}' successfully." });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid refresh token.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            // Yeni bir access token oluştur
            var newAccessToken = await _tokenService.CreateToken(user, roles);

            // Yeni bir refresh token oluştur (isteğe bağlı)
            var newRefreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
