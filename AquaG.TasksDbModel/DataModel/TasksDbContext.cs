using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

/*
Install-Package Microsoft.EntityFrameworkCore.Tools
    
Update-Database

[ForeignKey("CompanyInfoKey")] 
 
Remove-Migration
Get-Migration
Update-Help
Get-Help Update-Database -Online

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

https://docs.microsoft.com/ru-ru/aspnet/core/data/ef-mvc/complex-data-model?view=aspnetcore-3.1
https://docs.microsoft.com/en-us/ef/core/modeling/entity-properties?tabs=data-annotations%2Cwithout-nrt#included-and-%20%D0%B8%D1%81%D0%BA%D0%BB%D1%8E%D1%87%D0%B5%D0%BD%D0%B8%D1%8F-%D1%81%D0%B2%D0%BE%D0%B9%D1%81%D1%82%D0%B2%D0%B0

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
        public DbSet<TaskNote> TaskNotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=TaskManaderDB;Trusted_Connection=True;";
            Console.WriteLine(connectionString);
            builder.UseSqlServer(connectionString);
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<User>().HasIndex(u => u.NormalizedEmail).IsUnique();
        //}
    }
}
