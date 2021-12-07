using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;

/*
Install-Package Microsoft.EntityFrameworkCore.Tools
Add-Migration InitialCreate
Update-Database

[ForeignKey("CompanyInfoKey")] 
 
Remove-Migration
Get-Migration

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

https://docs.microsoft.com/ru-ru/aspnet/core/data/ef-mvc/complex-data-model?view=aspnetcore-3.1
https://docs.microsoft.com/en-us/ef/core/modeling/entity-properties?tabs=data-annotations%2Cwithout-nrt#included-and-%20%D0%B8%D1%81%D0%BA%D0%BB%D1%8E%D1%87%D0%B5%D0%BD%D0%B8%D1%8F-%D1%81%D0%B2%D0%BE%D0%B9%D1%81%D1%82%D0%B2%D0%B0

 */



namespace AquaG.TasksDbModel
{




    public class TasksDbContext : DbContext
    {

        public static string GetDefaultDbConnectionString()
        {
            string connectionString = "";// ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            if (string.IsNullOrEmpty(connectionString)) 
                connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=TaskManaderDB;Trusted_Connection=True;";
            return connectionString;
        }

        public TasksDbContext() : base() { }

        public TasksDbContext(DbContextOptions<TasksDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskInfo> TaskInfos { get; set; }
        public DbSet<TaskNote> TaskNotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        //	=> builder.UseSqlite(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        {
            string connectionString = GetDefaultDbConnectionString();
            connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=TaskManaderDB;Trusted_Connection=True;";

            Console.WriteLine(connectionString);
            builder.UseSqlServer(connectionString);
        }
    }
}
