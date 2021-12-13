using AquaG.TasksDbModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace AquaG.TasksMVC.ViewModels
{
    public class TaskModel :BaseTable
    {
        public int? ProjectId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required] public bool IsCompleted { get; set; } = false;

    }
}
