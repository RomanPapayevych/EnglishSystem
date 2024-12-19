using EnglishSystem.Application.Common;
using EnglishSystem.Application.DTOs;
using EnglishSystem.Application.Interfaces;
using EnglishSystem.Domain.Entities;
using EnglishSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EnglishSystem.Application.Services
{
    public class AuthService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }
        public async Task<OperationResult> AssignRoleToUserAsync(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "User not found"
                };
            }
            var roleExists = await _roleManager.RoleExistsAsync(newRole);
            if (!roleExists)
            {
                var role = new ApplicationRole { Name = newRole };
                var createRoleResult = await _roleManager.CreateAsync(role);

                if (!createRoleResult.Succeeded)
                {
                    return new OperationResult
                    {
                        Succeeded = false,
                        Message = "Failed to create role",
                        Errors = createRoleResult.Errors.Select(e => e.Description).ToList()
                    };
                }
            }
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Contains(newRole))
            {
                return new OperationResult
                {
                    Succeeded = true,
                    Message = $"User already has the {newRole} role"
                };
            }
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Failed to remove current roles",
                    Errors = removeResult.Errors.Select(e => e.Description).ToList()
                };
            }
            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addResult.Succeeded)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Failed to add new role",
                    Errors = addResult.Errors.Select(e => e.Description).ToList()
                };
            }
            return new OperationResult
            {
                Succeeded = true,
                Message = $"Role changed to {newRole} successfully"
            };
        }

        public async Task<OperationResult> DeleteUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "User not found"
                };
            }
            else
            {
                await _userManager.DeleteAsync(user);
                return new OperationResult
                {
                    Succeeded = true,
                    Message = "User deleted"
                };
            }
        }

        public string GenerateToken(LoginDTO loginDTO)
        {
            var user = _userManager.FindByEmailAsync(loginDTO.Email!).Result;
            var roles = _userManager.GetRolesAsync(user!).Result;
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, loginDTO.Email!),
                new Claim(ClaimTypes.NameIdentifier, user!.Id.ToString())
            };
            foreach (var role in roles) 
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var securityToken = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddMinutes(60),
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value
                );
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public bool isUniqueUser(string name)
        {
            var unique = _context.Users.FirstOrDefaultAsync(u => u.UserName == name);
            if (unique == null)
            {
                return true;
            }
            return false;
        }

        public async Task<OperationResult> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email!);
            if (user == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Invalid email or password"
                };
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password!, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Invalid email or password",
                    Data = user
                };
            }
            return new OperationResult
            {
                Succeeded = true,
                Message = "Login successful",
                Data = user
            };
        }

        public async Task<OperationResult> RegisterAsync(RegisterDTO registerDTO)
        {
            var existUser = await _userManager.FindByEmailAsync(registerDTO.Email!);
            if (existUser != null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "User already exists",
                };
            }
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName
            };
            IdentityResult result = await _userManager.CreateAsync(applicationUser, registerDTO.Password!);
            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(applicationUser);
                var check = await _userManager.FindByEmailAsync(applicationUser.Email!);
                if(check!.Email == "administratorsite@gmail.com")
                {
                    var role = await _roleManager.CreateAsync(new ApplicationRole(){ Name = "Admin"});
                    await _userManager.AddToRoleAsync(check, "Admin");
                }
                else
                {
                    await _roleManager.CreateAsync(new ApplicationRole() { Name = "User" });
                    await _userManager.AddToRoleAsync(check, "User");
                }
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                return new OperationResult
                {
                    Succeeded = true,
                    Message = "Registration successful",
                    Data = result,
                    Errors = new List<string>()
                };
            }
            else
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Registration failed",
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

        }
        public async Task<List<UserWithRolesDTO>> GetUsersWithRoles()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new List<UserWithRolesDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userRoles.Add(new UserWithRolesDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }
            return userRoles;
        }
    }
}
