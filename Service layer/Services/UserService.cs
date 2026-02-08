using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain_layer.Constants;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.AspNetCore.Identity;
using Service_layer.DTOS.User;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IBusinessRepository _businessRepository;

        public UserService(UserManager<User> userManager, IBusinessRepository businessRepository)
        {
            _userManager = userManager;
            _businessRepository = businessRepository;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return _userManager.Users.ToList();
        }

        public async Task<IEnumerable<User>> GetByBusinessIdAsync(string businessId)
        {
            return _userManager.Users.Where(u => u.BusinessId == businessId).ToList();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User?> UpdateAsync(string id, UserUpdateDTO dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            user.FullName = dto.FullName;
            if (!string.IsNullOrEmpty(dto.Email))
            {
                user.Email = dto.Email;
                user.UserName = dto.Email; // UserName must match Email in Identity
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return user;
        }

        public async Task<bool> AssignRoleAsync(string userId, string newRole)
        {
            // Validate role
            if (newRole != Roles.Admin && newRole != Roles.Owner && newRole != Roles.Agent && newRole != Roles.User)
                throw new ArgumentException($"Invalid role: {newRole}");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.Role = newRole;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}

