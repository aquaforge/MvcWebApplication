using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AquaG.TasksDbModel
{
    public class BaseTableNext :BaseTable
    {
        [Required] public DateTime CreationDate { get; set; } = DateTime.Now;
        [Required] public DateTime LastModidied { get; set; } = DateTime.Now;

    }
}
