using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaG.TasksDbModel
{
    public class User : BaseTable
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required] [MaxLength(50)] [MinLength(3)] public string Password { get; set; }

        [Required] public Guid UserGuid { get; set; } = Guid.NewGuid();

        public ICollection<Project> Projects { get; set; }
        public ICollection<TaskInfo> Tasks { get; set; }

        public User() { }

        public User(string email, string password, string caption, string description) : base(caption, description)
        {
            Email = email;
            Password = password;
            Email = email;
        }
    }
}
