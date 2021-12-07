using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace AquaG.TasksDbModel
{
    public class TaskNote : BaseTable
    {
        [Required] public int TaskInfoID { get; set; }
        public TaskInfo TaskInfo { get; set; }

        public TaskNote() { }
        public TaskNote(int taskInfoID, string caption, string description) : base(caption, description)
        {
            TaskInfoID = taskInfoID;
        }
    }
}
