using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AquaG.TasksDbModel;

namespace AquaG.TasksMVC.ViewModels
{
    public class ProjectDropListModel
    {

        public int? Id { get; set; } = 0;

        [Required] [MaxLength(250)] public string Caption { get; set; }

        public ProjectDropListModel(int? id, string caption)
        {
            Id = id;
            Caption = caption;
        }
    }
}
