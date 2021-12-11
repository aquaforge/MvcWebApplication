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
        public IEnumerable<ProjectModel> Projects { get; set; }
        public IEnumerable<TaskModel> Tasks { get; set; }
    }
}
