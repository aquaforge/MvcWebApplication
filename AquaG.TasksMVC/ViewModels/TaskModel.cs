using AquaG.TasksDbModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace AquaG.TasksMVC.ViewModels
{
    public class TaskModel : BaseTable
    {
        public int? ProjectId { get; set; }

        [Display(Name = "Дата начала")] public DateTime? StartDate { get; set; }

        [Display(Name = "Дата окончания")] public DateTime? EndDate { get; set; }

        [Required] public bool IsCompleted { get; set; } = false;

    }
}
