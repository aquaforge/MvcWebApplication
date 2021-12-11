using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace AquaG.TasksDbModel
{
    public class TaskInfo : BaseTable
    {

        [Required] public User User { get; set; }

        public int? ProjectID { get; set; }

        public Project Project { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required] public bool IsNotifyNeeded { get; set; } = false;

        [Required] public ItemPriority Priority { get; set; } = ItemPriority.Normal;

        [Required] public bool IsCompleted { get; set; } = false;

        [Required] public bool IsOneAction { get; set; } = false;

        [Required] public int OrderId { get; set; } = 0;
    }
}
