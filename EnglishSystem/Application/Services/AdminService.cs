using Azure.Messaging;
using EnglishSystem.Application.Common;
using EnglishSystem.Application.DTOs;
using EnglishSystem.Application.Interfaces;
using EnglishSystem.Domain.Entities;
using EnglishSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace EnglishSystem.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }
       
        public async Task<OperationResult> CreateGroupAsync(CreateGroupDTO model)
        {
            var englishLevel = await _context.EnglishLevel.FindAsync(model.EnglishLevelId);
            if (englishLevel == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Invalid English level ID",
                    Errors = new List<string>()
                };
            }   
            if(model.DaysOfWeek.Count == 0 || model.DaysOfWeek.Count > 7)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Invalid schedule days.",
                    Errors = new List<string>()
                };
            }
            var schedule = new Schedule
            {
                DaysOfWeek = model.DaysOfWeek,
                StartTime = model.StartTime,
                EndTime = model.EndTime
            };
            var group = new Group
            {
                Name = model.Name,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                EnglishLevel = englishLevel,
                Schedule = schedule,
            };
            //group.Schedule = schedule;
            _context.Schedules.Add(schedule);
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return new OperationResult
            {
                Succeeded = true,
                Message = "Group created successfully",
                Data = group
            };
        }
        public async Task<OperationResult> DeleteGroupAsync(int groupId)
        {
            var deleteGroup = await _context.Groups.FirstOrDefaultAsync(d => d.Id == groupId);
            if (deleteGroup == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Id not found",
                    Errors = new List<string>()
                };
            }
            _context.Groups.Remove(deleteGroup);
            await _context.SaveChangesAsync();
            return new OperationResult
            {
                Succeeded = true,
                Message = "Group deleted successfully",
                Data = deleteGroup
            };
        }
        public async Task<List<GroupDTO>> GetAllGroupsAsync()
        {
            var groups = await _context.Groups.Include(g => g.Schedule).Include(g => g.EnglishLevel).Include(g => g.Teacher).ToListAsync();
            var result = groups.Select(group => new GroupDTO
            {
                Id = group.Id,
                Name = group.Name!,
                StartTime = group.StartTime,
                EndTime = group.EndTime,
                EnglishLevelId = group.EnglishLevelId,
                EnglishLevel = group.EnglishLevel?.Level,
                Teacher = group.Teacher != null ? new { group.Teacher.Id, group.Teacher.FirstName, group.Teacher.LastName } : null,
                DaysOfWeek = group.Schedule.DaysOfWeek.Select(day => day.ToString()).ToList()
            }).ToList();

            return result;
            //return await _context.Groups.ToListAsync();
        }
        public async Task<OperationResult> CreateLevelAsync(string name)
        {
            var checkLevel = await _context.EnglishLevel.FirstOrDefaultAsync(c => c.Level == name);
            if (checkLevel != null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Level is already created"
                };
            }
            var newLevel = new EnglishLevel { Level = name };
            await _context.EnglishLevel.AddAsync(newLevel);
            await _context.SaveChangesAsync();
            return new OperationResult
            {
                Succeeded = true,
                Message = "Level created successfully",
                Data = newLevel
            };
        }
        public async Task<OperationResult> DeleteLevelAsync(int levelId)
        {
            var level = await _context.EnglishLevel.FindAsync(levelId);
            if (level == null)
            {
                return new OperationResult
                {
                    Succeeded = false,  
                    Message = "Level not found",
                    Errors = new List<string>()
                };
            } 
            _context.EnglishLevel.Remove(level);
            await _context.SaveChangesAsync();
            return new OperationResult
            {
                Succeeded = true,
                Message = "Level deleted"
            };
        }
        public async Task<List<EnglishLevel>> GetAllLevelsAsync()
        {
            return await _context.EnglishLevel.ToListAsync();
        }
        public async Task<EnglishLevel> GetLevelByIdAsync(int levelId)
        {
            return (await _context.EnglishLevel.FindAsync(levelId))!;
        }
    }
}
