using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AquaG.TasksDbModel
{
    public class User : IdentityUser//<int>
    {
        [Required] public DateTime CreationDate { get; set; } = DateTime.Now;

        [Required] public DateTime LastModified { get; set; } = DateTime.Now;

        [Required] public bool IsDeleted { get; set; } = false;

        public ICollection<Project> Projects { get; set; }
        public ICollection<TaskInfo> Tasks { get; set; }

    }
}
