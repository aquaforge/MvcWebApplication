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

        [Required] public int UserID { get; set; }
        public User User { get; set; }

        public int? ProjectID { get; set; }

        public Project Project { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required] public ItemImportance Priority { get; set; } = ItemImportance.Normal;

        [Required] public bool IsCompleted { get; set; } = false;

        [Required] public bool IsOneAction { get; set; } = false;

        [Required] public int OrderId { get; set; } = 0;

        public ICollection<TaskNote> Notes { get; set; }


        public TaskInfo() { }
        public TaskInfo(int userID, int? projectID, string caption, string description, DateTime? startDate, DateTime? endDate, ItemImportance priority = ItemImportance.Normal) : base(caption, description)
        {
            UserID = userID;
            ProjectID = projectID;
            StartDate = startDate;
            EndDate = endDate;
            Priority = priority;
        }
    }
}
