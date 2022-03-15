using System.Collections.Generic;

#nullable disable

namespace ClassPage.Models
{
    public partial class Class
    {
        public Class()
        {
            ClassesTeachers = new HashSet<ClassesTeacher>();
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }
        public string ClassName { get; set; }
        public int MainTeacherId { get; set; }

        public virtual Teacher MainTeacher { get; set; }
        public virtual ICollection<ClassesTeacher> ClassesTeachers { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
