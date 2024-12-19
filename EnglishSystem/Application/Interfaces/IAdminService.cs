using EnglishSystem.Application.Common;
using EnglishSystem.Application.DTOs;
using EnglishSystem.Domain.Entities;

namespace EnglishSystem.Application.Interfaces
{
    public interface IAdminService
    {
        Task<List<EnglishLevel>> GetAllLevelsAsync();
        Task<OperationResult> CreateLevelAsync(string name);
        Task<OperationResult> DeleteLevelAsync(int levelId);
        Task<EnglishLevel> GetLevelByIdAsync(int levelId);
        Task<OperationResult> CreateGroupAsync(CreateGroupDTO model);
        Task<OperationResult> DeleteGroupAsync(int groupId);
        Task<List<GroupDTO>> GetAllGroupsAsync();
        //Task<OperationResult> CreateScheduleAsync(CreateScheduleDTO model);
        //Task<IEnumerable<Schedule>> GetScheduleAsync();
    }
}
