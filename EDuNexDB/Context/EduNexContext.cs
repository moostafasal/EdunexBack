using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduNexDB.Context
{
    public class EduNexContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Level> Levels { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<StudentExam> StudentExam { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<StudentsAnswersSubmissions> StudentsAnswersSubmissions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        //public DbSet<City> cities { get; set; }

        //public DbSet<Governorate> governorates { get; set; }
        public EduNexContext(DbContextOptions<EduNexContext> options) : base(options)
        {

        }


        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entityEntry in entities)
            {
                var now = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = now;
                }

                ((BaseEntity)entityEntry.Entity).UpdatedAt = now;
            }

            return base.SaveChanges();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
              new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
              new IdentityRole { Id = "2", Name = "Student", NormalizedName = "STUDENT" },
              new IdentityRole { Id = "3", Name = "Teacher", NormalizedName = "TEACHER" }


          );
            modelBuilder.Entity<Level>().HasData(
                   new Level { Id = 1, LevelName = "Level one" },
                   new Level { Id = 2, LevelName = "Level two" },
                    new Level { Id = 3, LevelName = "Level three" }

               );








            //configure identity

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles").HasKey(p => new { p.UserId, p.RoleId });
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins").HasKey(p => p.UserId);
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens").HasKey(p => p.UserId);
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<Teacher>().ToTable("Teachers");
            modelBuilder.Entity<Student>().ToTable("Students");


            modelBuilder.Entity<Lecture>()
                .Property(l => l.Price)
                .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<Transaction>()
       .Property(t => t.Amount)
       .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Wallet>()
       .Property(w => w.Balance)
       .HasColumnType("decimal(18, 2)");

            //configure Examination models relationships  
            modelBuilder.Entity<StudentExam>()
           .HasKey(se => new { se.StudentId, se.ExamId });
            modelBuilder.Entity<StudentExam>()
                  .HasOne(se => se.Student)
                  .WithMany(s => s.StudentExams)
                  .HasForeignKey(se => se.StudentId)
                  .OnDelete(DeleteBehavior.NoAction); // Specify delete behavior

            modelBuilder.Entity<StudentExam>()
                .HasOne(se => se.Exam)
                .WithMany(e => e.StudentExams)
                .HasForeignKey(se => se.ExamId)
                .OnDelete(DeleteBehavior.NoAction); // Specify delete behavior


            modelBuilder.Entity<StudentsAnswersSubmissions>()
       .HasKey(s => s.Id);

            modelBuilder.Entity<StudentsAnswersSubmissions>()
                .HasOne(s => s.Exam)
                .WithMany()
                .HasForeignKey(s => s.ExamId)
                .OnDelete(DeleteBehavior.Restrict); // Disable cascade delete for Exam relationship

            modelBuilder.Entity<StudentsAnswersSubmissions>()
                .HasOne(s => s.Student)
                .WithMany()
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Restrict); // Disable cascade delete for Exam relationship


            modelBuilder.Entity<StudentsAnswersSubmissions>()
                .HasOne(s => s.Question)
                .WithMany()
                .HasForeignKey(s => s.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); // Disable cascade delete for Exam relationship


            modelBuilder.Entity<StudentsAnswersSubmissions>()
                .HasOne(s => s.Answer)
                .WithMany()
                .HasForeignKey(s => s.AnswerId)
                .OnDelete(DeleteBehavior.Restrict); // Disable cascade delete for Exam relationship

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.gender)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);



        }
    }


}

