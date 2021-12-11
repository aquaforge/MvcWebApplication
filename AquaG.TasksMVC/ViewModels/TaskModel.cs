using AquaG.TasksDbModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace AquaG.TasksMVC.ViewModels
{
    public class TaskModel
    {

        [Required] public int Id { get; set; } = 0;

        [Required] [MaxLength(250)] public string Caption { get; set; }

        [MaxLength(2000)] public string Description { get; set; }

        public DateTime? CreationDate { get; set; } = DateTime.Now;

        public DateTime? LastModified { get; set; } = DateTime.Now;


        [Required] public bool IsDeleted { get; set; } = false;

        public Project Project { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required] public bool IsNotifyNeeded { get; set; } = false;

        [Required] public ItemImportance Priority { get; set; } = ItemImportance.Normal;

        [Required] public bool IsCompleted { get; set; } = false;

        [Required] public bool IsOneAction { get; set; } = false;

        [Required] public int OrderId { get; set; } = 0;

    }
}
