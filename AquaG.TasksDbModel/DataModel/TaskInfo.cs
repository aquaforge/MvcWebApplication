using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace AquaG.TasksDbModel
{
    public class TaskInfo : BaseTableNext
    {

        [Required] public string UserId { get; set; }
        [Required] public User User { get; set; }

        public int? ProjectId { get; set; }
        public Project Project { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required] public bool IsCompleted { get; set; } = false;
    }
}
