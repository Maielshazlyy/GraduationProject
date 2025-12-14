using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service_layer.DTOS.Auth;
using Service_layer.Mapping;
using Service_layer.ServicesInterfaces;

namespace Service_layer.Services
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration; // لقراءة الـ Secret Key من appsettings
        public AuthService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        
        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO model)
        {
            // 1. استخدام الـ Mapping لتحويل البيانات
            var user = model.ToEntity();

            // 2. إنشاء المستخدم وتشفير الباسورد تلقائياً
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            // 3. (اختياري) إضافة الرول في جدول الأدوار لو مفعلة
            // await _userManager.AddToRoleAsync(user, model.Role);

            // 4. إنشاء التوكن وإرجاع النتيجة
            return GenerateJwtToken(user);
        }

     
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO model)
        {
           
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                throw new Exception("Invalid Email or Password");
            }

            return GenerateJwtToken(user);
        }

        private AuthResponseDTO GenerateJwtToken(User user)
        {
            // 1. تحديد المعلومات اللي هتكون جوه التوكن (Claims)
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id), 
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("BusinessId", user.BusinessId) 
            };

            // 2. مفتاح التشفير (لازم يكون نفس الموجود في appsettings)
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            // 3. إعدادات التوكن (المدة، التشفير، إلخ)
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddDays(3), // مدة الصلاحية 3 أيام
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            // 4. تجهيز الرد النهائي
            return new AuthResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                BusinessId = user.BusinessId
            };
        }
    }

}

