using AquaG.TasksDbModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace AquaG.TasksMVC.ViewModels
{
    public class TaskModel :BaseTable
    {
        public int? ProjectId { get; set; }

        [Display(Name = "Дата начала")]
        [DisplayFormat(DataFormatString = DateTimeHtmlExtention.HtmlDateTimeFormatString, ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Дата окончания")]
        [DisplayFormat(DataFormatString = DateTimeHtmlExtention.HtmlDateTimeFormatString, ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Required] public bool IsCompleted { get; set; } = false;

    }
}
