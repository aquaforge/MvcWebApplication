﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AquaG.TasksDbModel;

namespace AquaG.TasksMVC.ViewModels
{
    public class ProjectModel : BaseTable
    {
        public int NumberOfTasks { get; set; } = -1;
    }
}
