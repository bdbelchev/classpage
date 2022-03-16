using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClassPage.Models.DTOs
{
    public class TeacherDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(45)]
        public string FirstName { get; set; }
        [StringLength(45)]
        public string MiddleName { get; set; }
        [Required]
        [StringLength(45)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [RegularExpression(@"[0-9]{9}")]
        public string Phone { get; set; }
        public int? MainClassId { get; set; }

        public virtual ICollection<int> ClassIds { get; set; }
        public virtual ICollection<int> SubjectIds { get; set; }
    }
}
