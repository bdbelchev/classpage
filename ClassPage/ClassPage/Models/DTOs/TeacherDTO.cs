using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassPage.Models.DTOs
{
    public class TeacherDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? MainClassId { get; set; }

        public virtual ICollection<int> ClassIds { get; set; }
        public virtual ICollection<int> SubjectIds { get; set; }
    }
}
