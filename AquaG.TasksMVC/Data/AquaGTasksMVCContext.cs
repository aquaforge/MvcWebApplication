using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AquaG.TasksDbModel;

namespace AquaG.TasksMVC.Data
{
    public class AquaGTasksMVCContext : DbContext
    {
        public AquaGTasksMVCContext (DbContextOptions<AquaGTasksMVCContext> options)
            : base(options)
        {
        }

        public DbSet<AquaG.TasksDbModel.ProjectModel> ProjectModel { get; set; }
    }
}
