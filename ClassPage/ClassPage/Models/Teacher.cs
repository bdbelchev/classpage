using System.Collections.Generic;

#nullable disable

namespace ClassPage.Models
{
    public class Teacher
    {
        public Teacher()
        {
            ClassesTeachers = new HashSet<ClassesTeacher>();
            Grades = new HashSet<Grade>();
            TeachersSubjects = new HashSet<TeachersSubject>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public virtual Class Class { get; set; }
        public virtual ICollection<ClassesTeacher> ClassesTeachers { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<TeachersSubject> TeachersSubjects { get; set; }
    }
}
