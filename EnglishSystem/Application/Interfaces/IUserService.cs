using EnglishSystem.Application.Common;
using EnglishSystem.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EnglishSystem.Application.Interfaces
{
    public interface IUserService
    {
        Task<OperationResult> RegisterAsync(RegisterDTO registerDTO);
        Task<OperationResult> LoginAsync(LoginDTO loginDTO);
        bool isUniqueUser(string name);
        string GenerateToken(LoginDTO loginDTO);
        Task<OperationResult> DeleteUser(string name);
    }
}
