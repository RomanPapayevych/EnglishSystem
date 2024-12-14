using EnglishSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnglishSystem.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<EnglishLevel> EnglishLevel { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Homework> Homework { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Group>()
            .HasOne(g => g.Teacher)
            .WithMany() // Вчитель може викладати кільком групам
            .HasForeignKey(g => g.TeacherId)
            .OnDelete(DeleteBehavior.Restrict); // Щоб уникнути каскадного видалення

            builder.Entity<Group>()
            .HasMany(g => g.Students)
            .WithOne(s => s.Group)
            .HasForeignKey(s => s.GroupId)
            .OnDelete(DeleteBehavior.SetNull); // Якщо групу видалено, студентам видаляється посилання на неї
        }
    }
}
