using System;
using System.Collections.Generic;
using System.Web;
using System.Data.Entity;

namespace UniversityCommittee.Models
{
    public class UniversityContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>().HasMany(c => c.Users)
                .WithMany(s => s.Subjects)
                .Map(t => t.MapLeftKey("SubjectId")
                .MapRightKey("UserId")
                .ToTable("SubjectUser"));
        }
    }
}