using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AquaG.TasksDbModel
{
    public class BaseTable
    {
        [Required] public int Id { get; set; } = 0;

        [Required] [MaxLength(250)] public string Caption { get; set; }

        [MaxLength(2000)] public string Description { get; set; }

        [Required] public DateTime CreationDate { get; set; } = DateTime.Now;

        [Required] public DateTime LastModified { get; set; } = DateTime.Now;


        [Required] public bool IsDeleted { get; set; } = false;

        public BaseTable() { }
        public BaseTable(string caption, string description)
        {
            Caption = caption;
            Description = description;
        }

        // protected void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<BaseTable>()
        //        .Property(t => t.CreationDate)
        //        .HasDefaultValueSql("GETDATE()");


    }
}
