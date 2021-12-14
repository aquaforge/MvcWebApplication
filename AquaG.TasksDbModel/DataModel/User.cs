using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AquaG.TasksDbModel
{
    public class User : IdentityUser
    {
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastAccess { get; set; } = DateTime.Now;

        public ICollection<Project> Projects { get; set; }
        public ICollection<TaskInfo> TaskInfos { get; set; }

        //public int Compare(object x, object y)
        //{
        //    return (new CaseInsensitiveComparer()).Compare((x as User).Id, (y as User).Id);
        //}

        //public int CompareTo(object obj)
        //{
        //    if (obj == null) throw new ArgumentException($"Object is not a {this.GetType().Name}");
        //    return this.Id.CompareTo((obj as User).Id);
        //}

        //public static bool operator ==(User u1, User u2)
        //{
        //    if (u1 == null && u2 == null) return true;
        //    if (u1 == null) return u2.Equals(u1);
        //    return u1.Equals(u2);
        //}

        //public static bool operator !=(User u1, User u2)
        //{
        //    return !(u1 == u2);
        //}

        //public override bool Equals(object obj)
        //{
        //    if (obj == null) return false;
        //    if (obj is not User) return false;
        //    return Id == (obj as User).Id;
        //}

        //public override int GetHashCode()
        //{
        //    return this.Id.GetHashCode();
        //}

    }
}
