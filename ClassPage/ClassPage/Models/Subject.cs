using System.Collections.Generic;

#nullable disable

namespace ClassPage.Models
{
    public class Subject
    {
        public Subject()
        {
            Grades = new HashSet<Grade>();
            TeachersSubjects = new HashSet<TeachersSubject>();
        }

        public int Id { get; set; }
        public string SubjectName { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<TeachersSubject> TeachersSubjects { get; set; }
    }
}
