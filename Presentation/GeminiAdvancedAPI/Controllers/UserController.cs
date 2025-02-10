using GeminiAdvancedAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GeminiAdvancedAPI.Application.DTOs;
using GeminiAdvancedAPI.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AutoMapper;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;

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
        private readonly IMapper _mapper; // IMapper field'ı
        private readonly IEmailService _emailService; // Ekledik
        private readonly ICartRepository _cartRepository; // Sepet işlemleri için

        public UserController(ICartRepository cartRepository, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, ITokenService tokenService, IOptions<JwtSettings> jwtSettings, IMapper mapper, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
            _emailService = emailService;
            _cartRepository = cartRepository; // Dependency Injection ile alıyoruz

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

                    // Refresh Token oluştur ve ata
                    user.RefreshToken = Guid.NewGuid().ToString();
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays); // _jwtSettings'e erişiminiz olduğundan emin olun
                    await _userManager.UpdateAsync(user); // Kullanıcıyı güncelle

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var roles = await _userManager.GetRolesAsync(user);
                var accessToken = _tokenService.GenerateJwtToken(user, roles);

                // Cookie'de sepet ID'si varsa ve kullanıcı giriş yaptıysa, sepetleri birleştir
                if (Request.Cookies.TryGetValue("MyShoppingCart", out string anonymousCartId) && !string.IsNullOrEmpty(user.Id.ToString()))
                {
                    await MergeCartsAsync(user.Id.ToString(), anonymousCartId);
                    // Anonim sepet ID'sini içeren cookie'yi sil
                    Response.Cookies.Delete("MyShoppingCart");
                }

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

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
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
            var newAccessToken = _tokenService.GenerateJwtToken(user, roles);

            // Refresh token'ın kullanım ömrünü uzat
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = newAccessToken,
                RefreshToken = request.RefreshToken // Mevcut refresh token'ı dön
            });
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Kullanıcı bulunamadı hatası dönmek yerine, işlem başarılı gibi davranmak daha güvenlidir.
                // Bu, olası saldırganların sistemde kayıtlı e-posta adreslerini öğrenmesini engeller.
                return Ok();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "User", new { userId = user.Id, token = token }, protocol: HttpContext.Request.Scheme);

            // E-posta gönderme
            //await _emailService.SendEmailAsync(model.Email, "Şifre Sıfırlama", $"Şifrenizi sıfırlamak için lütfen aşağıdaki linke tıklayın: <a href='{callbackUrl}'>Şifre Sıfırlama</a>"); //Eski kod
            await _emailService.SendResetPasswordEmailAsync(model.Email, callbackUrl);
            return Ok();
        }
        [HttpGet("ResetPassword")]
        public IActionResult ResetPassword(string userId, string token) //Bu sayfa View'da olmalı.
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("Invalid token."); //Token boş olamaz.
            }
            var model = new ResetPasswordDto
            {
                Token = token
            };
            return Ok(model);
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //Model valid değilse client'e gönder
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                //Kullanıcı bulunamadığında direk hata vermek yerine, şifre sıfırlama maili gönderilmiş gibi davran.
                return Ok();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        private async Task MergeCartsAsync(string userId, string anonymousCartId)
        {
            //Kullanıcının Redis'te kayıtlı sepeti var mı?
            var userCart = await _cartRepository.GetByUserIdAsync(userId);
            //Anonim kullanıcının Redis'te kayıtlı sepeti var mı?
            var anonymousCart = await _cartRepository.GetByUserIdAsync(anonymousCartId);

            if (anonymousCart != null)
            {
                if (userCart == null)
                {
                    // Kullanıcının sepeti yoksa, anonim sepeti kullanıcının sepeti yap
                    anonymousCart.UserId = userId;
                    await _cartRepository.AddOrUpdateCartAsync(anonymousCart);
                }
                else
                {
                    // Kullanıcının sepeti varsa, anonim sepetteki ürünleri kullanıcının sepetine ekle (veya birleştir)
                    foreach (var anonymousItem in anonymousCart.CartItems)
                    {
                        var existingItem = userCart.CartItems.FirstOrDefault(ci => ci.ProductId == anonymousItem.ProductId);
                        if (existingItem == null)
                        {
                            userCart.CartItems.Add(anonymousItem);
                        }
                        else
                        {
                            existingItem.Quantity += anonymousItem.Quantity;
                        }
                    }
                    await _cartRepository.AddOrUpdateCartAsync(userCart);
                }

                // Anonim sepeti sil
                await _cartRepository.DeleteCartAsync(anonymousCartId);
            }
        }
    }
}
