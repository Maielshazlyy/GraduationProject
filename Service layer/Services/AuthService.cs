using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Constants;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Google.Apis.Auth;
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
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(UserManager<User> userManager, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        
        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.BusinessId))
                throw new Exception("BusinessId is required for agent registration.");

            var business = await _unitOfWork.Businesses.GetByIdAsync(model.BusinessId);
            if (business == null)
                throw new Exception($"Business with id '{model.BusinessId}' not found.");

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                throw new Exception($"Email '{model.Email}' is already registered.");

            var user = model.ToEntity();

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            return GenerateJwtToken(user);
        }

        public async Task<AuthResponseDTO> RegisterAdminAsync(RegisterBootstrapDTO model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                throw new Exception($"Email '{model.Email}' is already registered.");

            var user = model.ToEntity();
            user.Role = Roles.Admin;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            return GenerateJwtToken(user);
        }

        public async Task<AuthResponseDTO> RegisterOwnerAsync(RegisterBootstrapDTO model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                throw new Exception($"Email '{model.Email}' is already registered.");

            var user = model.ToEntity();
            user.Role = Roles.Owner;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            return GenerateJwtToken(user);
        }

        /// <summary>
        /// Creates a Business + Owner account in one shot.
        /// Solves the chicken-and-egg problem: no BusinessId required from the caller.
        /// </summary>
        public async Task<AuthResponseDTO> RegisterWithBusinessAsync(RegisterWithBusinessDTO model)
        {
            // 1. Guard: email must be unique
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                throw new Exception($"Email '{model.Email}' is already registered.");

            // 2. Create the Business first (generates its own Id)
            var business = new Business
            {
                Id          = Guid.NewGuid().ToString(),
                BusinessId  = Guid.NewGuid().ToString(),
                Name        = model.BusinessName.Trim(),
                Type        = model.BusinessType ?? string.Empty,
                IsActive    = true,
                IsVerified  = false,
                CreatedAt   = DateTime.UtcNow,
            };

            await _unitOfWork.Businesses.AddAsync(business);
            await _unitOfWork.CompleteAsync();          // persist so the FK exists

            // 3. Create the Owner user linked to that Business
            var user = new User
            {
                UserName   = model.Email,
                Email      = model.Email,
                FullName   = model.FullName,
                PhoneNumber = model.PhoneNumber,
                BusinessId = business.Id,
                Role       = Roles.Owner,
                CreatedAt  = DateTime.UtcNow,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                // Roll back the orphan business we just created
                _unitOfWork.Businesses.Delete(business);
                await _unitOfWork.CompleteAsync();

                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

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
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty), 
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role ?? "User"),
            };
            
            // Add BusinessId only if it exists
            if (!string.IsNullOrEmpty(user.BusinessId))
            {
                authClaims.Add(new Claim("BusinessId", user.BusinessId));
            }

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
        public async Task<AuthResponseDTO> GoogleLoginAsync(string googleToken)
        {
            // 1. التحقق من التوكن
            GoogleJsonWebSignature.Payload payload;
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _configuration["Google:ClientId"] }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(googleToken, settings);
            }
            catch
            {
                throw new Exception("Invalid Google Token");
            }

            // 2. البحث عن المستخدم أو إنشاؤه
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    FullName = payload.Name,
                    Role = "Owner",
                    BusinessId = "1", // قيمة مؤقتة
                    CreatedAt = DateTime.UtcNow
                };
                await _userManager.CreateAsync(user, "GooglePass_123!");
            }

            // 3. إصدار التوكن الخاص بنا
            return GenerateJwtToken(user);
        }
    }

}

