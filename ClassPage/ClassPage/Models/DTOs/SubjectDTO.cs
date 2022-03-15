using System.ComponentModel.DataAnnotations;

namespace ClassPage.Models.DTOs
{
    public class SubjectDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(45)]
        public string SubjectName { get; set; }
    }
}
