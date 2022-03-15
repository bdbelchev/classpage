using System.ComponentModel.DataAnnotations;

namespace ClassPage.Models.DTOs
{
    public class StudentDTO
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
        public int ClassId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [RegularExpression(@"[0-9]{9}")]
        public string Phone { get; set; }
    }
}
