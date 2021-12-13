using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaG.TasksMVC.ViewModels
{
    public class UserAllViewModel
    {
        public List<ProjectModel> Projects { get; set; }
        public List<TaskModel> Tasks { get; set; }
    }
}
