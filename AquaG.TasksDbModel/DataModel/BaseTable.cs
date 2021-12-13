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

        public BaseTable() { }
        public BaseTable(string caption, string description)
        {
            Caption = caption;
            Description = description;
        }
    }
}
