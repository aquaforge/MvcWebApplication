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

        public int? ParentId { get; set; }
        public Project Parent { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<TaskInfo> TaskInfo { get; set; }

        [Required] public int OrderId { get; set; } = 0;
        [Required] public int SubLevelNo { get; set; } = 0;


        public Project() { }
        public Project(User user, string caption, string description, Project parent) : base(caption, description)
        {
            User = user;
            ParentId = parent.Id;
            SubLevelNo = (parent == null) ? 0 : parent.SubLevelNo + 1;
        }
    }
}
