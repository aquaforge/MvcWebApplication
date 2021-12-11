using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaG.TasksMVC.ViewModels
{
    public class ProjectModel
    {
        public int? Id { get; set; } = 0;

        [Required] [MaxLength(250)] public string Caption { get; set; }

        [MaxLength(2000)] public string Description { get; set; }

        public DateTime? CreationDate { get; set; } = DateTime.Now;

        public DateTime? LastModified { get; set; } = DateTime.Now;

        public bool? IsActive { get; set; } = true;

        public bool? IsDeleted { get; set; } = false;

        public int NumberOfTasks { get; set; } = -1;
    }
}
