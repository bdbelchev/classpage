using System.Collections.Generic;

#nullable disable

namespace ClassPage.Models
{
    public partial class Student
    {
        public Student()
        {
            Grades = new HashSet<Grade>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int ClassId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public virtual Class Class { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
