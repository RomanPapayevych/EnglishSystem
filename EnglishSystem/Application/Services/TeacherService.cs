using EnglishSystem.Application.Common;
using EnglishSystem.Application.Interfaces;
using EnglishSystem.Domain.Entities;
using EnglishSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EnglishSystem.Application.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminService _adminService;
        public TeacherService(ApplicationDbContext context, IAdminService adminService)
        {
            _context = context;
            _adminService = adminService;
        }

        public async Task<OperationResult> AssignTeacherAsync(int groupId, int teacherId)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Group not found",
                    Errors = new List<string>()
                };
            }
            var teacher = await _context.Users.FirstOrDefaultAsync(u => u.Id == teacherId);
            if (teacher == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Teacher not found",
                    Errors = new List<string>()
                };
            }    
            group.TeacherId = teacher.Id;
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
            return new OperationResult
            {
                Succeeded = true,
                Message = "Teacher choosed group successfully"
            };
        }
        public async Task<IEnumerable<Group>> TeacherGroupsAsync(int teacherId)
        {
            return await _context.Groups.Where(g => g.TeacherId == teacherId).ToListAsync();
        }
        public async Task<OperationResult> RemoveTeacherFromGroup(int groupId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Group not found",
                    Errors = new List<string>()
                };
            }
            if (group.TeacherId == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "This group does not have an assigned teacher.",
                    Errors = new List<string>()
                };
            }
            group.TeacherId = null;
            group.Teacher = null;
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
            return new OperationResult
            {
                Succeeded = true,
                Message = "Teacher has been successfully removed from the group.",
            };
        }

        public async Task<List<ApplicationUser>> GetStudentsByGroupId(int groupId)
        {
            var group = await _context.Groups.Include(g => g.Students).FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
            {
                throw new Exception("Group not found");
            }
            return group.Students!.ToList();
        }

        public async Task<Lesson> AddLessonToScheduleAsync(int scheduleId, DateTime date, string? topic, string? description)
        {
            var schedule = await _context.Schedules.Include(s => s.Lessons).FirstOrDefaultAsync(s => s.Id == scheduleId);
            if (schedule == null)
            {
                throw new Exception("Schedule not found");
            }
            var lesson = new Lesson
            {
                LessonDateTime = date,
                Topic = topic,
                Description = description,
                ScheduleId = scheduleId
            };
            schedule.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }

        public async Task<Homework> AddHomeworkToLessonAsync(int lessonId, string content)
        {
            var lesson = await _context.Lessons.Include(l => l.HomeworkAssignments).FirstOrDefaultAsync(l => l.Id == lessonId);
            if (lesson == null)
            {
                throw new Exception("Lesson not found");
            }
            var homework = new Homework
            {
                Content = content,
                CreatedAt = DateTime.Now,
                Status = Domain.Enum.HomeworkStatus.Active,
                LessonId = lessonId
            };
            lesson.HomeworkAssignments.Add(homework);
            await _context.SaveChangesAsync();
            return homework;
        }

        public async Task<OperationResult> DeleteLessonAsync(int lessonId)
        {
            var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.Id == lessonId);
            if(lesson == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Lesson not found",
                    Errors = new List<string>()
                };
            }
            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return new OperationResult
            {
                Succeeded = true,
                Message = "Lesson deleted successfully"
            };
        }
    }
}
