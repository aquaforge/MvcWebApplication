﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AquaG.TasksDbModel
{
    public class User : IdentityUser//<int>
    {
        public ICollection<Project> Projects { get; set; }
        public ICollection<TaskInfo> TaskInfos { get; set; }

    }
}
