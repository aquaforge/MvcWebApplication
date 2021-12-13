using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaG.TasksDbModel
{
    public class Project : BaseTable
    {
        [Required] public User User { get; set; }

        public ICollection<TaskInfo> TaskInfos { get; set; }
    }
}
