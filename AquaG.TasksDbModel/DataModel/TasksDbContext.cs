using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

/*
Install-Package Microsoft.EntityFrameworkCore.Tools
    


[ForeignKey("CompanyInfoKey")] 
 
Add-Migration Initial
Script-Migration Initial
Update-Database

Remove-Migration
Get-Migration
Update-Help
Get-Help Update-Database -Online

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

db.Database.ExecuteSqlRaw(
    @"CREATE VIEW View_BlogPostCounts AS
        SELECT b.Name, Count(p.PostId) as PostCount
        FROM Blogs b
        JOIN Posts p on p.BlogId = b.BlogId
        GROUP BY b.Name");

    modelBuilder
        .Entity<BlogPostsCount>(
            eb =>
            {
                eb.HasNoKey();
                eb.ToView("View_BlogPostCounts");
                eb.Property(v => v.BlogName).HasColumnName("Name");
            });

 */
namespace AquaG.TasksDbModel
{
    public class TasksDbContext : IdentityDbContext//<User, Role, int>
    {
        public TasksDbContext() : base()
        {
            //Database.EnsureCreated();
        }

        public TasksDbContext(DbContextOptions<TasksDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskInfo> TaskInfos { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder builder)
        //{
        //    string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=TaskManaderDB;Trusted_Connection=True;";
        //    Console.WriteLine(connectionString);
        //    builder.UseSqlServer(connectionString);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasOne(p => p.User).WithMany(u => u.Projects).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TaskInfo>().HasOne(t => t.Project).WithMany(p => p.TaskInfos).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TaskInfo>().HasOne(t => t.User).WithMany(u => u.TaskInfos).OnDelete(DeleteBehavior.Restrict);


            //modelBuilder.Entity<User>().HasIndex(u => u.NormalizedEmail).IsUnique();
            modelBuilder.Entity<Project>().ToTable("Projects", t => t.IsTemporal());
            modelBuilder.Entity<TaskInfo>().ToTable("TaskInfos", t => t.IsTemporal());

            base.OnModelCreating(modelBuilder);
        }

    }
}
